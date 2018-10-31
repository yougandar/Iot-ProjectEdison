using Edison.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Edison.Core.Common.Models
{
    public class ConsumerProperties
    {
        public ChatReportType RequestType { get; set; }
        public Geolocation Geolocation { get; set; }
        public string DeviceType { get; set; }
    }
}
