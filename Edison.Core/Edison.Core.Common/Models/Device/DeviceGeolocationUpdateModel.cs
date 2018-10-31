using System;
using System.Collections.Generic;

namespace Edison.Core.Common.Models
{
    public class DeviceGeolocationUpdateModel
    {
        public Guid DeviceId { get; set; }
        public Geolocation Geolocation { get; set; }     
    }
}
