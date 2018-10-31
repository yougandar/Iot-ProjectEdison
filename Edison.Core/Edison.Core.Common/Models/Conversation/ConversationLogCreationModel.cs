using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Edison.Core.Common.Models
{
    public class ConversationLogCreationModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public ChatReportType ReportType { get; set; }
        public ConversationLogModel Message { get; set; }
    }
}
