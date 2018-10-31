using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Edison.ChatService.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChatUserRole
    {
        Unknown,
        Consumer,
        Admin
    }
}
