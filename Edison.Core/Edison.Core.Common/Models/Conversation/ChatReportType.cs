using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Edison.Core.Common.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChatReportType
    {
        Unknown,
        Report,
        Emergency
    }
}
