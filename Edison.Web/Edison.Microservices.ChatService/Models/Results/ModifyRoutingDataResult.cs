using Newtonsoft.Json;

namespace Edison.ChatService.Models.Results
{
    public enum ModifyRoutingDataResultType
    {
        Added,
        AlreadyExists,
        Removed,
        IsBot,
        Error
    }

    public class ModifyRoutingDataResult : AbstractMessageRouterResult
    {
        public ModifyRoutingDataResultType Type
        {
            get;
            set;
        }

        public override string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
