using Edison.Core.Common;
using Edison.Core.Common.Models;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Edison.ChatService.Models
{
    [Serializable]
    public class ActivityMessageProperties
    {
        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }
        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }
        [JsonProperty(PropertyName = "reportType")]
        public ChatReportType ReportType { get; set; }
        [JsonConverter(typeof(EpochDateTimeConverter))]
        public DateTime Date { get; set; }

        public static ActivityMessageProperties FromChannelData(IActivity activity)
        {
            if (JsonConvert.DeserializeObject<ActivityMessageProperties>(activity.ChannelData?.ToString()) is ActivityMessageProperties activityProperties && activityProperties != null)
                return activityProperties;
            return null;
        }
    }
}
