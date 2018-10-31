using Edison.Core.Common;
using Newtonsoft.Json;
using System;

namespace Edison.ChatService.Repositories
{
    public interface IEntityChatDAO
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        string ETag { get; set; }
    }
}
