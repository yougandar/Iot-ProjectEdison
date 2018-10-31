using Edison.Core.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Edison.Core.Common.Models
{
    public class ConversationModel
    {
        public Guid ConversationId { get; set; }
        public ChatReportType ReportType { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public List<ConversationLogModel> ConversationLogs { get; set; }
        [JsonConverter(typeof(EpochDateTimeConverter))]
        public DateTime CreationDate { get; set; }
        [JsonConverter(typeof(EpochDateTimeConverter))]
        public DateTime UpdateDate { get; set; }
        [JsonConverter(typeof(EpochDateTimeConverter))]
        public DateTime? EndDate { get; set; }
        public string ETag { get; set; }
    }
}
