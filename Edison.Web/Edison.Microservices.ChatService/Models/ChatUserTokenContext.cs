using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edison.ChatService.Models
{
    public class ChatUserTokenContext
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
        [JsonProperty(PropertyName = "expiresIn")]
        public int? ExpiresIn { get; set; }
        [JsonProperty(PropertyName = "conversationId")]
        public string ConversationId { get; set; }
        [JsonProperty(PropertyName = "userContext")]
        public ChatUserContext UserContext { get; set; }
    }
}
