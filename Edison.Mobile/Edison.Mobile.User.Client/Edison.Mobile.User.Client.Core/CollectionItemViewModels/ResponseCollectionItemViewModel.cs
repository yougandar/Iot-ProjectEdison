using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Autofac;
using Edison.Core.Common.Models;
using Edison.Mobile.Common.Ioc;
using Edison.Mobile.Common.Network;
using Edison.Mobile.Common.Shared;

namespace Edison.Mobile.User.Client.Core.CollectionItemViewModels
{
    public class ResponseCollectionItemViewModel : BaseCollectionItemViewModel
    {
        readonly Guid primaryEventClusterId;
        readonly IEnumerable<Guid> eventClusterIds;

        public event ViewNotification OnPrimaryEventClusterReceived;

        public ObservableRangeCollection<EventClusterModel> EventClusters { get; } = new ObservableRangeCollection<EventClusterModel>();
        public ObservableRangeCollection<EventModel> PrimaryEvents { get; } = new ObservableRangeCollection<EventModel>();

        public EventClusterModel PrimaryEventCluster { get; private set; }
        public Guid ResponseId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Geolocation Geolocation { get; private set; }

        public ResponseCollectionItemViewModel(ResponseLightModel responseLightModel)
        {
            Geolocation = responseLightModel.Geolocation;
            StartDate = responseLightModel.StartDate;
            EndDate = responseLightModel.EndDate;
            ResponseId = responseLightModel.ResponseId;
            primaryEventClusterId = responseLightModel.PrimaryEventClusterId;
            eventClusterIds = responseLightModel.EventClusterIds;
        }

        public async Task GetPrimaryEventCluster() 
        {
            var eventClusterRequestService = Container.Instance.Resolve<EventClusterRestService>();
            var eventCluster = await eventClusterRequestService.GetEventCluster(primaryEventClusterId.ToString());
            PrimaryEventCluster = eventCluster;

            OnPrimaryEventClusterReceived?.Invoke();
        }
    }
}
