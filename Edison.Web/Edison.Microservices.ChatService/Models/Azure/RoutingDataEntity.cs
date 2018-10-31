using Microsoft.WindowsAzure.Storage.Table;

namespace Edison.ChatService.Models.Azure
{
    public class RoutingDataEntity : TableEntity
    {
        public string Body { get; set; }
    }
}
