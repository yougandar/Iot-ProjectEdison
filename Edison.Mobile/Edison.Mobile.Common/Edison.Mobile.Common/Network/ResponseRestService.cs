using System.Collections.Generic;
using System.Threading.Tasks;
using Edison.Core.Common.Models;
using Edison.Mobile.Common.Auth;
using Edison.Mobile.Common.Logging;
using RestSharp;

namespace Edison.Mobile.Common.Network
{
    public class ResponseRestService : BaseRestService
    {
        public ResponseRestService(AuthService authService, ILogger logger, string baseUrl) : base(authService, logger, baseUrl) { }

        public async Task<IEnumerable<ResponseLightModel>> GetResponses()
        {
            var request = PrepareRequest("Responses", Method.GET);
            var queryResult = await client.ExecuteGetTaskAsync<IEnumerable<ResponseLightModel>>(request);

            if (queryResult.IsSuccessful)
            {
                return queryResult.Data;
            }

            logger.Log($"Error getting responses. Response Status: {queryResult.ResponseStatus}, Error Message: {queryResult.ErrorMessage}");

            return null;
        }
    }
}
