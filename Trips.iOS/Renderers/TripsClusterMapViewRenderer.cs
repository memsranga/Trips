using System;
using System.ComponentModel;
using System.Linq;
using CoreLocation;
using Foundation;
using Google.Maps;
using Google.Maps.Utils;
using Trips.CustomControls;
using Trips.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TripsClusterMapView), typeof(TripsClusterMapViewRenderer))]
namespace Trips.iOS.Renderers
{
    public class TripsClusterMapViewRenderer : ViewRenderer<TripsClusterMapView, MapView>
    {
        private MapView _control;
        private GMUClusterManager _clusterManager;

        protected override void OnElementChanged(ElementChangedEventArgs<TripsClusterMapView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            if (e.OldElement != null)
            {
                var mapView = e.OldElement;
                mapView.Trips.CollectionChanged -= Trips_CollectionChanged;
            }

            var newMapView = e.NewElement;
            newMapView.Trips.CollectionChanged += Trips_CollectionChanged;
            if (_control == null)
            {
                _control = new MapView
                {
                    MapStyle = MapStyle.FromJson(Constants.MapStyleJson, null),
                };

                _control.MoveCamera(CameraUpdate.ZoomToZoom(0));
            }

            if (_clusterManager == null)
            {
                _clusterManager = new GMUClusterManager(_control,
                    new GMUNonHierarchicalDistanceBasedAlgorithm(),
                    new GMUDefaultClusterRenderer(_control, new GMUDefaultClusterIconGenerator()));
            }

            SetNativeControl(_control);
            GenerateClusters(newMapView);
        }

        private void GenerateClusters(TripsClusterMapView mapView)
        {
            if (mapView == null)
            {
                return;
            }

            if (_clusterManager.Delegate == null)
            {
                _clusterManager.SetDelegate(new ClusterDelegate(mapView), new MapDelegate());
            }

            _clusterManager.ClearItems();
            var random = new Random();

            mapView.Trips.ForEach(x => _clusterManager.AddItem(new ClusterMarker(x.Route.Average(y => y.Latitude), x.Route.Average(y => y.Longitude))
            {
                Id = x.Id,
                Title = x.Name
            }));
            _clusterManager.Cluster();
        }

        private void Trips_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GenerateClusters(Element as TripsClusterMapView);
        }
    }

    public class ClusterMarker : Marker, IGMUClusterItem
    {
        public string Id { get; set; }

        public ClusterMarker(double lat, double lon)
        {
            Position = new CLLocationCoordinate2D(lat, lon);
        }
    }

    public class MapDelegate : MapViewDelegate
    {

    }

    public class ClusterDelegate : GMUClusterManagerDelegate
    {
        private readonly TripsClusterMapView _clusterMapView;

        public ClusterDelegate(TripsClusterMapView clusterMapView)
        {
            _clusterMapView = clusterMapView;
        }

        [Export("clusterManager:didTapClusterItem:")]
        public override bool DidTapClusterItem(GMUClusterManager clusterManager, IGMUClusterItem clusterItem)
        {
            if (clusterItem is ClusterMarker clusterMarker)
            {
                _clusterMapView?.MarkerSelectedCommand?.Execute(clusterMarker?.Id);
            }

            return false;
        }
    }
}
