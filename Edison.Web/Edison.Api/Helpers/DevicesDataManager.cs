using Edison.Core.Common.Models;
using System.Threading.Tasks;
using AutoMapper;
using System;
using Edison.Common.DAO;
using Edison.Common.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Edison.Api.Config;
using Microsoft.Azure.Documents;
using System.Net;

namespace Edison.Api.Helpers
{
    public class DevicesDataManager
    {
        private ICosmosDBRepository<DeviceDAO> _repoDevices;
        private IMapper _mapper;

        public DevicesDataManager(IMapper mapper,
            ICosmosDBRepository<DeviceDAO> repoDevices)
        {
            _mapper = mapper;
            _repoDevices = repoDevices;
        }

        public async Task<DeviceModel> GetDevice(Guid deviceId)
        {
            DeviceDAO deviceEntity = await _repoDevices.GetItemAsync(deviceId);
            return _mapper.Map<DeviceModel>(deviceEntity);
        }

        public async Task<IEnumerable<DeviceMapModel>> GetDevicesForMap()
        {
            IEnumerable<DeviceDAO> devices = await _repoDevices.GetItemsAsync(
                p => p.Enabled && p.Sensor,
                p => new DeviceDAO()
                {
                    Id = p.Id,
                    DeviceType = p.DeviceType,
                    Geolocation = p.Geolocation,
                    LastAccessTime = p.LastAccessTime
                }
                );

            return _mapper.Map<IEnumerable<DeviceMapModel>>(devices);
        }

        public async Task<IEnumerable<DeviceModel>> GetDevices()
        {
            IEnumerable<DeviceDAO> devices = await _repoDevices.GetItemsAsync(p => p.Enabled && p.Sensor);
            return _mapper.Map<IEnumerable<DeviceModel>>(devices);
        }

        public async Task<IEnumerable<Guid>> GetDevicesInRadius(DeviceGeolocationModel deviceGeolocationObj)
        {
            IEnumerable<DeviceDAO> devices = await _repoDevices.GetItemsAsync(
               p => p.Enabled && ((deviceGeolocationObj.FetchSensors && p.Sensor) || (!deviceGeolocationObj.FetchSensors && !p.Sensor)),
               p => new DeviceDAO()
               {
                   Id = p.Id,
                   Geolocation = p.Geolocation
               }
               );

            List<Guid> output = new List<Guid>();
            GeolocationDAOObject daoGeocodeCenterPoint = _mapper.Map<GeolocationDAOObject>(deviceGeolocationObj.ResponseEpicenterLocation);
            foreach (DeviceDAO deviceObj in devices)
                if (RadiusHelper.IsWithinRadius(deviceObj.Geolocation, daoGeocodeCenterPoint, deviceGeolocationObj.Radius))
                    output.Add(new Guid(deviceObj.Id));
            return output;
        }

        public async Task<DeviceModel> CreateOrUpdateDevice(DeviceTwinModel deviceTwinObj)
        {
            if (deviceTwinObj.DeviceId == Guid.Empty)
                throw new Exception($"No device found that matches DeviceId: {deviceTwinObj.DeviceId}");

            DeviceDAO deviceDAO = await _repoDevices.GetItemAsync(deviceTwinObj.DeviceId);

            //Create
            if (deviceDAO == null)
                return await CreateDevice(deviceTwinObj);

            //Update
            if(deviceTwinObj.Properties?.Desired != null)
                deviceDAO.Desired = deviceTwinObj.Properties.Desired;
            if (deviceTwinObj.Properties?.Reported != null)
                deviceDAO.Reported = deviceTwinObj.Properties.Reported;
            if (deviceTwinObj.Tags != null)
            {
                deviceDAO.DeviceType = deviceTwinObj.Tags.DeviceType;
                deviceDAO.Enabled = deviceTwinObj.Tags.Enabled;
                deviceDAO.Custom = deviceTwinObj.Tags.Custom;
                deviceDAO.Name = deviceTwinObj.Tags.Name;
                deviceDAO.Location1 = deviceTwinObj.Tags.Location1;
                deviceDAO.Location2 = deviceTwinObj.Tags.Location2;
                deviceDAO.Location3 = deviceTwinObj.Tags.Location3;
                deviceDAO.Sensor = deviceTwinObj.Tags.Sensor;
                deviceDAO.Geolocation = _mapper.Map<GeolocationDAOObject>(deviceTwinObj.Tags.Geolocation);
            }

            try
            {
                await _repoDevices.UpdateItemAsync(deviceDAO);
            }
            catch (DocumentClientException e)
            {
                //Update concurrency issue, retrying
                if (e.StatusCode == HttpStatusCode.PreconditionFailed)
                    return await CreateOrUpdateDevice(deviceTwinObj);
                throw e;
            }

            return _mapper.Map<DeviceModel>(deviceDAO);
        }

        public async Task<DeviceMobileModel> CreateOrUpdateDevice(DeviceMobileModel deviceMobile)
        {
            if (deviceMobile.DeviceId == Guid.Empty  && string.IsNullOrEmpty(deviceMobile.MobileId))
                throw new Exception($"Invalid DeviceId and/or MobileId");

            DeviceDAO deviceDAO = null;

            if (deviceMobile.DeviceId != Guid.Empty)
                deviceDAO = await _repoDevices.GetItemAsync(deviceMobile.DeviceId);
            else if (!string.IsNullOrEmpty(deviceMobile.MobileId))
                deviceDAO = await _repoDevices.GetItemAsync(
                    d => (string)d.Custom["MobileId"] == deviceMobile.MobileId);


            if (deviceDAO == null)
            //Create
            {
                deviceDAO = await CreateDevice(deviceMobile);
            }   
            else
            //Update
            {
                deviceDAO.Custom = deviceMobile.Custom;
                try
                {
                    await _repoDevices.UpdateItemAsync(deviceDAO);
                }
                catch (DocumentClientException e)
                {
                    //Update concurrency issue, retrying
                    if (e.StatusCode == HttpStatusCode.PreconditionFailed)
                        return await CreateOrUpdateDevice(deviceMobile);
                    throw e;
                }
            }

            return _mapper.Map<DeviceMobileModel>(deviceDAO);
        }

        public async Task<DeviceModel> CreateDevice(DeviceTwinModel deviceTwinObj)
        {
            //If device doesn't exist, throw exception
            DeviceDAO deviceEntity = _mapper.Map<DeviceDAO>(deviceTwinObj);
            deviceEntity.Id = await _repoDevices.CreateItemAsync(deviceEntity);
            if (_repoDevices.IsDocumentKeyNull(deviceEntity))
                throw new Exception($"An error occured when creating a new device: {deviceTwinObj.DeviceId}");

            return _mapper.Map<DeviceModel>(deviceEntity);
        }

        public async Task<DeviceDAO> CreateDevice(DeviceMobileModel deviceMobileObj)
        {
            //If device doesn't exist, throw exception
            DeviceDAO deviceEntity = _mapper.Map<DeviceDAO>(deviceMobileObj);
            deviceEntity.Id = null;
            deviceEntity.Id = await _repoDevices.CreateItemAsync(deviceEntity);
            if (_repoDevices.IsDocumentKeyNull(deviceEntity))
                throw new Exception($"An error occured when creating a new device: {deviceMobileObj.DeviceId}");

            return deviceEntity;
        }

        public async Task<bool> UpdateHeartbeat(Guid deviceId)
        {
            if (deviceId == Guid.Empty)
                throw new Exception($"No device found that matches DeviceId: {deviceId}");

            DeviceDAO deviceDAO = await _repoDevices.GetItemAsync(deviceId);

            string etag = deviceDAO.ETag;
            deviceDAO.LastAccessTime = DateTime.UtcNow;

            try
            {
                return await _repoDevices.UpdateItemAsync(deviceDAO);
            }
            catch (DocumentClientException e)
            {
                //Update concurrency issue, retrying
                if (e.StatusCode == HttpStatusCode.PreconditionFailed)
                    return await UpdateHeartbeat(deviceId);
                throw e;
            }
        }

        public async Task<DeviceModel> UpdateGeolocation(DeviceGeolocationUpdateModel updateGeolocationObj)
        {
            if (updateGeolocationObj.DeviceId == Guid.Empty)
                throw new Exception($"No device found that matches DeviceId: {updateGeolocationObj.DeviceId}");

            DeviceDAO deviceDAO = await _repoDevices.GetItemAsync(updateGeolocationObj.DeviceId);

            string etag = deviceDAO.ETag;
            deviceDAO.LastAccessTime = DateTime.UtcNow;
            deviceDAO.Geolocation = _mapper.Map<GeolocationDAOObject>(updateGeolocationObj.Geolocation);

            try
            {
                var result = await _repoDevices.UpdateItemAsync(deviceDAO);
                if (result)
                    return _mapper.Map<DeviceModel>(deviceDAO);
                throw new Exception($"Error while updating device geolocation: {updateGeolocationObj.DeviceId}.");

            }
            catch (DocumentClientException e)
            {
                //Update concurrency issue, retrying
                if (e.StatusCode == HttpStatusCode.PreconditionFailed)
                    return await UpdateGeolocation(updateGeolocationObj);
                throw e;
            }
        }

        public async Task<bool> DeleteDevice(Guid deviceId)
        {
            if (await _repoDevices.GetItemAsync(deviceId) != null)
                return await _repoDevices.DeleteItemAsync(deviceId);
            return true;
        }

        public async Task<bool> DeleteDevice(string registrationId)
        {
            DeviceDAO device = await _repoDevices.GetItemAsync(d => (string)d.Custom["RegistrationId"] == registrationId);
            if (device != null)
                return await _repoDevices.DeleteItemAsync(device.Id);
            return true;
        }
    }
}
