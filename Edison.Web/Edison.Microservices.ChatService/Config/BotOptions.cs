using Edison.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edison.ChatService.Config
{
    public class BotOptions
    {
        public string MicrosoftAppId { get; set; }
        public string MicrosoftAppPassword { get; set; }
        public string BotSecret { get; set; }
        public string AdminChannel { get; set; }
        public List<string> Admins { get; set; }
    }
}
