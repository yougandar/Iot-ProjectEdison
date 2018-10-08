using System;
using CoreLocation;
using Edison.Core.Common.Models;
using MapKit;
using UIKit;

namespace Edison.Mobile.User.Client.iOS.Views
{
    public class ResponseMapView : UIView
    {
        readonly MKMapView mapView;
        readonly int regionSizeMeters = 500;

        Geolocation geolocation;
        public Geolocation Geolocation 
        {
            get => geolocation;
            set 
            {
                geolocation = value;
                if (geolocation != null)
                {
                    var location = new CLLocationCoordinate2D(geolocation.Latitude, geolocation.Longitude);
                    var viewRegion = MKCoordinateRegion.FromDistance(location, regionSizeMeters, regionSizeMeters);
                    var annotation = new MKPointAnnotation { Coordinate = location };
                    mapView.SetRegion(viewRegion, false);
                    mapView.AddAnnotation(annotation);
                }
            }
        }

        public ResponseMapView(Geolocation geolocation = null)
        {
            mapView = new MKMapView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                ShowsUserLocation = true,
                UserInteractionEnabled = false,
            };

            AddSubview(mapView);
            mapView.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
            mapView.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;
            mapView.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            mapView.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;

            Geolocation = geolocation;
        }
    }
}
