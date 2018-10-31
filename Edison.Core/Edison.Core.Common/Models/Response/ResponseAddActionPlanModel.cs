using Edison.Core.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Edison.Core.Common.Models
{
    public class ResponseAddActionPlanModel
    {
        public Guid ResponseId { get; set; }
        public double PrimaryRadius { get; set; }
        public double SecondaryRadius { get; set; }
        public Geolocation Geolocation { get; set; }
        public List<ActionModel> Actions { get; set; }
    }
}
