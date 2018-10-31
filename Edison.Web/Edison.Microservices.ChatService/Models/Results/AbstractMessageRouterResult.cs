using System;

namespace Edison.ChatService.Models.Results
{
    [Serializable]
    public abstract class AbstractMessageRouterResult
    {
        public string ErrorMessage
        {
            get;
            set;
        }

        public AbstractMessageRouterResult()
        {
            ErrorMessage = string.Empty;
        }

        public abstract string ToJson();
    }
}
