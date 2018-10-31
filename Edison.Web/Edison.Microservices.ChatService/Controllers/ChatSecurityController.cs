using Edison.ChatService.Config;
using Edison.ChatService.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Edison.ChatService.Models;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using System.Net;
using Edison.Core.Interfaces;
using Edison.Core.Common.Models;

namespace Edison.ChatService.Controllers
{
    [Authorize(AuthenticationSchemes = "B2CWeb")]
    [Route("Security")]
    [ApiController]
    public class ChatSecurityController : ControllerBase
    {
        private readonly BotOptions _config;
        private static IDirectLineRestService _directLineRestClient;
        private ILogger<ChatSecurityController> _logger;

        public ChatSecurityController(IOptions<BotOptions> config, IDirectLineRestService directLineRestClient, 
            ILogger<ChatSecurityController> logger)
        {
            _config = config.Value;
            _directLineRestClient = directLineRestClient;
            _logger = logger;
        }

        [HttpGet("GetToken")]
        public async Task<IActionResult> GetToken()
        {
            try
            {
                ChatUserContext userContext = ChatUserContext.FromClaims(User.Claims);
                userContext.SetUserRoleAgainstAdminList(_config.Admins);
                TokenConversationResult conversation = await _directLineRestClient.GenerateToken(new TokenConversationParameters()
                {
                    User = userContext
                });
                return Ok(new ChatUserTokenContext()
                {
                    ConversationId = conversation.ConversationId,
                    Token = conversation.Token,
                    ExpiresIn = conversation.ExpiresIn,
                    UserContext = userContext
                });

                //TODO: With conversationId, give information to cosmosdb?
            }
            catch(Exception e)
            {
                _logger.LogError($"ChatSecurityController: {e.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        
    }
}
