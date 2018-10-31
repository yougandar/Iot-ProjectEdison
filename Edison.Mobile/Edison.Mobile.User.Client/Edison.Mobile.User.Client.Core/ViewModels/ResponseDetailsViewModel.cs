using System;
using Edison.Core.Common.Models;
using Edison.Mobile.Common.Geolocation;
using Edison.Mobile.Common.Shared;

namespace Edison.Mobile.User.Client.Core.ViewModels
{
    public class ResponseDetailsViewModel : BaseViewModel
    {
        readonly ILocationService locationService;

        public ResponseModel Response { get; set; }

        public event EventHandler<LocationChangedEventArgs> OnLocationChanged;

        public ResponseDetailsViewModel(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();

            OnLocationChanged?.Invoke(this, new LocationChangedEventArgs
            {
                CurrentLocation = locationService.LastKnownLocation,
            });
        }

        public override void BindEventHandlers()
        {
            base.BindEventHandlers();

            locationService.OnLocationChanged += HandleOnLocationChanged;
        }

        public override void UnBindEventHandlers()
        {
            base.UnBindEventHandlers();

            locationService.OnLocationChanged -= HandleOnLocationChanged;
        }

        void HandleOnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            OnLocationChanged?.Invoke(this, e);
        }
    }
}
