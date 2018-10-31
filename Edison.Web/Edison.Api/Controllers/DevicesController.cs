using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Edison.Api.Helpers;
using Edison.Common.Interfaces;
using Edison.Common.Messages;
using Edison.Common.Messages.Interfaces;
using Edison.Core.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edison.Api.Controllers
{
    [Authorize(AuthenticationSchemes = "AzureAd,B2CWeb")]
    [Route("api/Devices")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly DevicesDataManager _devicesDataManager;
        private readonly IMassTransitServiceBus _serviceBus;

        public DevicesController(DevicesDataManager eventDataManager, IMassTransitServiceBus serviceBus)
        {
            _devicesDataManager = eventDataManager;
            _serviceBus = serviceBus;
        }

        [HttpGet("{deviceId}")]
        [Produces(typeof(DeviceModel))]
        public async Task<IActionResult> GetDevice(Guid deviceId)
        {
            DeviceModel events = await _devicesDataManager.GetDevice(deviceId);
            return Ok(events);
        }

        [HttpGet("Map")]
        [Produces(typeof(IEnumerable<DeviceMapModel>))]
        public async Task<IActionResult> GetDevicesForMap()
        {
            IEnumerable<DeviceMapModel> devices = await _devicesDataManager.GetDevicesForMap();
            return Ok(devices);
        }

        [HttpPost("Radius")]
        [Produces(typeof(IEnumerable<Guid>))]
        public async Task<IActionResult> GetDevicesInRadius([FromBody] DeviceGeolocationModel deviceGeolocationObj)
        {
            IEnumerable<Guid> deviceIds = await _devicesDataManager.GetDevicesInRadius(deviceGeolocationObj);
            return Ok(deviceIds);
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<DeviceModel>))]
        public async Task<IActionResult> GetDevices()
        {
            IEnumerable<DeviceModel> devices = await _devicesDataManager.GetDevices();
            return Ok(devices);
        }

        [HttpPost]
        [Produces(typeof(DeviceModel))]
        public async Task<IActionResult> CreateOrUpdateDevice([FromBody]DeviceTwinModel deviceTwinObj)
        {
            var result = await _devicesDataManager.CreateOrUpdateDevice(deviceTwinObj);
            return Ok(result);
        }

        [HttpPut("Heartbeat")]
        [Produces(typeof(HttpStatusCode))]
        public async Task<IActionResult> UpdateHeartbeat([FromBody]Guid deviceId)
        {
            var result = await _devicesDataManager.UpdateHeartbeat(deviceId);
            if(result)
                return Ok(HttpStatusCode.OK);
            else
                return Ok(HttpStatusCode.InternalServerError);
        }

        [HttpPut("DeviceLocation")]
        [Produces(typeof(HttpStatusCode))]
        public async Task<IActionResult> UpdateGeolocation([FromBody]DeviceGeolocationUpdateModel updateGeolocationObj)
        {
            var result = await _devicesDataManager.UpdateGeolocation(updateGeolocationObj);

            if (result != null)
            {
                //We need to notify the frontend about the change, but it doesn't fit in the device workflow as phones aren't actual iot devices.
                await _serviceBus.BusAccess.Publish(new DeviceUIUpdateRequestedEvent()
                {
                    CorrelationId = updateGeolocationObj.DeviceId,
                    DeviceUI = new DeviceUIModel()
                    {
                        DeviceId = updateGeolocationObj.DeviceId,
                        Device = result,
                        UpdateType = "UpdateDevice"
                    }
                });
                return Ok(HttpStatusCode.OK);
            }
            return Ok(HttpStatusCode.InternalServerError);
        }

        [HttpDelete]
        [Produces(typeof(HttpStatusCode))]
        public async Task<IActionResult> DeleteDevice(Guid deviceId)
        {
            var result = await _devicesDataManager.DeleteDevice(deviceId);
            return Ok(result);
        }
    }
}
