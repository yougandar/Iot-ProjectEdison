using Edison.ChatService.Helpers.Interfaces;
using Edison.ChatService.Models.Results;
using Edison.Core.Common.Models;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edison.ChatService.Helpers
{
    /// <summary>
    /// The routing data manager.
    /// </summary>
    [Serializable]
    public class RoutingDataManager
    {
        private readonly ILogger<RoutingDataManager> _logger;
        private readonly IRoutingDataStore _routingDataStore;

        public RoutingDataManager(IRoutingDataStore routingDataStore, ILogger<RoutingDataManager> logger)
        {
            _routingDataStore = routingDataStore;
            _logger = logger;
        }

        public async Task<bool> SaveConversationReference(ConversationReference conversation)
        {
            if (conversation == null)
            {
                throw new ArgumentNullException("The given conversation reference is null");
            }

            if (conversation.User == null)
            {
                throw new ArgumentNullException("User channel account in the conversation reference cannot be null");
            }

            return await _routingDataStore.AddConversationReference(conversation);
        }

        public async Task<IEnumerable<ConversationReference>> GetConversationsFromUser(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("The userId is null");
            }

            return await _routingDataStore.GetConversationsFromUser(userId);
        }

        public async Task<IEnumerable<ConversationReference>> GetConsumerConversations()
        {
            return await _routingDataStore.GetConversations(ChatUserRole.Consumer);
        }

        public async Task<IEnumerable<ConversationReference>> GetAdminConversations()
        {
            return await _routingDataStore.GetConversations(ChatUserRole.Admin);
        }

        public async Task<IEnumerable<ConversationReference>> GetConversations()
        {
            return await _routingDataStore.GetConversations();
        }

        public IEnumerable<ConversationReference> RemoveSelfConversation(IEnumerable<ConversationReference> conversations, ConversationReference self)
        {
            return conversations.Where(p => p.Conversation.Id != self.Conversation.Id);
        }
    }
}
