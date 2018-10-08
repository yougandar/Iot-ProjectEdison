using System;
using Edison.Mobile.Common.Auth;
using Edison.Mobile.Common.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace Edison.Mobile.Common.Network
{
    public class BaseRestService
    {
        protected readonly AuthService authService;
        protected readonly RestClient client;
        protected readonly ILogger logger;

        public BaseRestService(AuthService authService, ILogger logger, string baseUrl)
        {
            this.logger = logger;
            this.authService = authService;

            client = new RestClient(baseUrl);
        }

        protected RestRequest PrepareRequest(string endpoint, Method method, object requestBody = null)
        {
            try
            {
                var token = authService.AuthenticationResult.AccessToken;
                var request = new RestRequest(endpoint, method) { RequestFormat = DataFormat.Json };

                request.AddHeader("Authorization", $"Bearer {token}");

                if (method == Method.POST && requestBody != null)
                {
                    request.AddParameter("application/json", JsonConvert.SerializeObject(requestBody), ParameterType.RequestBody);
                }

                return request;
            }
            catch (Exception ex)
            {
                logger.Log(ex, "Error preparing REST request");
                return new RestRequest();
            }
        }
    }
}
