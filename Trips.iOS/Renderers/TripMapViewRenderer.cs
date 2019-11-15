using System;
using System.ComponentModel;
using System.Linq;
using CoreLocation;
using Google.Maps;
using Trips.CustomControls;
using Trips.iOS.Helpers;
using Trips.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TripMapView), typeof(TripMapViewRenderer))]
namespace Trips.iOS.Renderers
{
    public class TripMapViewRenderer : ViewRenderer<TripMapView, MapView>
    {
        private MapView _control;
        private Marker _currentLocationMarker;
        private MutablePath _routePath;
        private Polyline _routePolyline;
        private static readonly UIKit.UIColor _pathColor = Color.FromHex("#1A237E").ToUIColor();

        protected override void OnElementChanged(ElementChangedEventArgs<TripMapView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null || e.OldElement != null)
            {
                return;
            }

            if (_control == null)
            {
                _control = new MapView
                {
                    MapStyle = MapStyle.FromJson(Constants.MapStyleJson, null),
                };

                _control.MoveCamera(CameraUpdate.ZoomToZoom(13));
            }

            SetNativeControl(_control);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var map = Element as TripMapView;
            if (e.PropertyName == TripMapView.CurrentLocationProperty.PropertyName)
            {
                UpdateCurrentLocation(map);
                UpdateRoute(map);
            }
        }

        private void UpdateRoute(TripMapView mapView)
        {
            if (mapView == null || mapView.Route == null || mapView.Route.Count == 0)
            {
                return;
            }

            if (_routePath == null)
            {
                _routePath = new MutablePath();
            }
            else
            {
                _routePath.RemoveAllCoordinates();
            }

            var curvePoints = CurveApproxHelper.DouglasPeuckerReduction(mapView.Route.ToList(), 0.00001);
            curvePoints.ForEach(x => _routePath.AddLatLon(x.Latitude, x.Longitude));

            if (mapView.CurrentLocation != null)
            {
                _routePath.AddLatLon(mapView.CurrentLocation.Latitude, mapView.CurrentLocation.Longitude);
            }

            if (_routePolyline != null)
            {
                _routePolyline.Map = null;
                _routePolyline.Dispose();
            }

            _routePolyline = new Polyline
            {
                Path = _routePath,
                StrokeWidth = 5f,
                StrokeColor = _pathColor
            };

            _routePolyline.Map = _control;

            _control.CameraTargetBounds = new CoordinateBounds(_routePath);
        }

        private void UpdateCurrentLocation(TripMapView mapView)
        {
            if (mapView == null || mapView.CurrentLocation == null)
            {
                return;
            }

            if (_currentLocationMarker == null)
            {
                _currentLocationMarker = new Marker()
                {
                    Map = _control,
                    Position = new CLLocationCoordinate2D(mapView.CurrentLocation.Latitude, mapView.CurrentLocation.Longitude),
                    Title = "Current Location",
                    Tappable = false,
                    IconView = new PulsatingMarker(),
                    GroundAnchor = new CoreGraphics.CGPoint(0.5, 0.5)
                };
            }

            _currentLocationMarker.Position = new CLLocationCoordinate2D(mapView.CurrentLocation.Latitude, mapView.CurrentLocation.Longitude);
        }
    }
}
