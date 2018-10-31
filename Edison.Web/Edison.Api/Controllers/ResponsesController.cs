using System;
using System.Collections.Generic;
using System.Linq;
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
    [ApiController]
    [Route("api/Responses")]
    public class ResponsesController : ControllerBase
    {
        private readonly ResponseDataManager _responseDataManager;
        private readonly IMassTransitServiceBus _serviceBus;

        public ResponsesController(ResponseDataManager responseDataManager, IMassTransitServiceBus serviceBusClient)
        {
            _responseDataManager = responseDataManager;
            _serviceBus = serviceBusClient;
        }
 
        [HttpGet("{responseId}")]
        [Produces(typeof(ResponseModel))]
        public async Task<IActionResult> GetResponseDetail(Guid responseId)
        {
            ResponseModel responseObj = await _responseDataManager.GetResponse(responseId);
            return Ok(responseObj);
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<ResponseLightModel>))]
        public async Task<IActionResult> GetResponses()
        {
            IEnumerable<ResponseLightModel> responseObjs = await _responseDataManager.GetResponses();
            return Ok(responseObjs);
        }

        [HttpPost("Radius")]
        [Produces(typeof(IEnumerable<ResponseModel>))]
        public async Task<IActionResult> GetResponsesFromPointRadius([FromBody]ResponseGeolocationModel responseGeolocationObj)
        {
            IEnumerable<ResponseModel> responseObjs = await _responseDataManager.GetResponsesFromPointRadius(responseGeolocationObj);
            return Ok(responseObjs);
        }

        [HttpPut("Safe")]
        [Produces(typeof(bool))]
        public async Task<IActionResult> SetSafeStatus([FromBody]ResponseSafeUpdateModel responseSafeUpdateObj)
        {
            bool result = await _responseDataManager.SetSafeStatus(responseSafeUpdateObj.UserId, responseSafeUpdateObj.IsSafe);
            return Ok(result);
        }

        [HttpPost]
        [Produces(typeof(ResponseModel))]
        public async Task<IActionResult> CreateResponse([FromBody]ResponseCreationModel responseObj)
        {
            var result = await _responseDataManager.CreateResponse(responseObj);
            IEventSagaReceiveResponseCreated newMessage = new EventSagaReceiveResponseCreated()
            {
                ResponseModel = result
            };
            await _serviceBus.BusAccess.Publish(newMessage);
            return Ok(result);
        }

        [HttpPut("Close")]
        [Produces(typeof(ResponseModel))]
        public async Task<IActionResult> CloseResponse(ResponseCloseModel responseObj)
        {
            var result = await _responseDataManager.CloseResponse(responseObj);
            IEventSagaReceiveResponseClosed newMessage = new EventSagaReceiveResponseClosed()
            {
                ResponseModel = result
            };
            await _serviceBus.BusAccess.Publish(newMessage);
            return Ok(result);
        }

        [HttpPut("AddEventClusters")]
        [Produces(typeof(ResponseModel))]
        public async Task<IActionResult> AddEventClusterIdsToResponse(ResponseEventClustersUpdateModel responseObj)
        {
            ResponseModel result = await _responseDataManager.AddEventClusterIdsToResponse(responseObj);
            return Ok(result);
        }

        [HttpDelete]
        [Produces(typeof(bool))]
        public async Task<IActionResult> DeleteResponse(Guid responseId)
        {
            var result = await _responseDataManager.DeleteResponse(responseId);
            return Ok(result);
        }

        [HttpPut("AddAction")]
        [Produces(typeof(ResponseModel))]
        public async Task<IActionResult> AddActionToResponse(ResponseAddActionPlanModel responseObj)
        {
            ResponseModel result = await _responseDataManager.AddActionToResponse(responseObj);
            if (result != null)
            {
                EventSagaReceiveResponseActionsUpdated newMessage = new EventSagaReceiveResponseActionsUpdated()
                {
                    Actions = responseObj.Actions,
                    ResponseId = responseObj.ResponseId,
                    Geolocation =  responseObj.Geolocation,
                    PrimaryRadius = responseObj.PrimaryRadius,
                    SecondaryRadius = responseObj.SecondaryRadius
                };
                await _serviceBus.BusAccess.Publish(newMessage);
            }

            return Ok(result);
        }
    }
}
