using Edison.Core.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Edison.ChatService.Models.DAO
{
    public class ConversationLogDAOObject
    {
        public string From { get; set; }
        public string Message { get; set; }
        [JsonConverter(typeof(EpochDateTimeConverter))]
        public DateTime Date { get; set; }
    }
}
