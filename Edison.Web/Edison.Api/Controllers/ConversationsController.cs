using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Edison.Api.Helpers;
using Edison.Core.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edison.Api.Controllers
{
    //[Authorize(AuthenticationSchemes = "Backend,B2CWeb")]
    [Route("api/Conversations")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly ConversationDataManager _conversationsDataManager;

        public ConversationsController(ConversationDataManager conversationsDataManager)
        {
            _conversationsDataManager = conversationsDataManager;
        }

        [HttpGet("ById/{conversationId}")]
        [Produces(typeof(ConversationModel))]
        public async Task<IActionResult> GetConversationById(Guid conversationId)
        {
            ConversationModel conversation = await _conversationsDataManager.GetConversationById(conversationId);
            return Ok(conversation);
        }

        [HttpGet("Active/{userId}")]
        [Produces(typeof(ConversationModel))]
        public async Task<IActionResult> GetActiveConversationFromUser(string userId)
        {
            ConversationModel conversation = await _conversationsDataManager.GetActiveConversationFromUser(userId);
            return Ok(conversation);
        }

        [HttpGet("{userId}")]
        [Produces(typeof(IEnumerable<ConversationModel>))]
        public async Task<IActionResult> GetConversationsFromUser(string userId)
        {
            IEnumerable<ConversationModel> conversations = await _conversationsDataManager.GetConversationsFromUser(userId);
            return Ok(conversations);
        }

        [HttpGet("Active")]
        [Produces(typeof(IEnumerable<ConversationModel>))]
        public async Task<IActionResult> GetActiveConversations()
        {
            IEnumerable<ConversationModel> conversations = await _conversationsDataManager.GetActiveConversations();
            return Ok(conversations);
        }

        [HttpPost]
        [Produces(typeof(ConversationModel))]
        public async Task<IActionResult> CreateOrUpdateConversation([FromBody]ConversationLogCreationModel conversationLogObj)
        {
            var result = await _conversationsDataManager.CreateOrUpdateConversation(conversationLogObj);
            return Ok(result);
        }


        [HttpPut("Close")]
        [Produces(typeof(ConversationModel))]
        public async Task<IActionResult> CloseConversation([FromBody]ConversationLogCloseModel conversationCloseObj)
        {
            var result = await _conversationsDataManager.CloseConversation(conversationCloseObj);
            return Ok(result);
        }
    }
}
