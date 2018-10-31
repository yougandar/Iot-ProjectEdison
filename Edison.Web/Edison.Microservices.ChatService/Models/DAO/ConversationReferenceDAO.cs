using Edison.ChatService.Repositories;
using Newtonsoft.Json;

namespace Edison.ChatService.Models.DAO
{
    public class ConversationReferenceDAO : IEntityChatDAO
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }
        [JsonProperty(PropertyName = "botId")]
        public string BotId { get; set; }
        [JsonProperty(PropertyName = "botName")]
        public string BotName { get; set; }
        [JsonProperty(PropertyName = "conversationId")]
        public string ConversationId { get; set; }
        [JsonProperty(PropertyName = "channelId")]
        public string ChannelId { get; set; }
        [JsonProperty(PropertyName = "serviceUrl")]
        public string ServiceUrl { get; set; }
        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; set; }
        [JsonProperty(PropertyName = "type")]
        public ChatDataType DataType { get; set; }
    }
}
