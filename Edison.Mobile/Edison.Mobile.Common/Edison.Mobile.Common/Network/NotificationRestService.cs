using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Edison.Core.Common.Models;
using Edison.Mobile.Common.Auth;
using Edison.Mobile.Common.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace Edison.Mobile.Common.Network
{
    public class NotificationRestService : BaseRestService
    {
        public NotificationRestService(AuthService authService, ILogger logger, string baseUrl) : base(authService, logger, baseUrl) { }

        public async Task<bool> Register(DeviceRegistrationModel registrationModel)
        {
            var request = PrepareRequest("Notifications/Register", Method.POST, registrationModel);
            var queryResult = await client.ExecutePostTaskAsync(request);
            if (queryResult.IsSuccessful)
            {
                return true;
            }

            logger.Log($"Error registering device for notifications: {queryResult.ResponseStatus}, Error Message: {queryResult.ErrorMessage}");

            return false;
        }
    }
}
