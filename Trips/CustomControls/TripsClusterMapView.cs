using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Trips.Models;
using Xamarin.Forms;

namespace Trips.CustomControls
{
    public class TripsClusterMapView : View
    {
        public static readonly BindableProperty TripsProperty = BindableProperty.Create(nameof(Trips), typeof(ObservableCollection<TripModel>), typeof(TripsClusterMapView));
        public static readonly BindableProperty MarkerSelectedCommandProperty = BindableProperty.Create(nameof(MarkerSelectedCommand), typeof(ICommand), typeof(TripsClusterMapView));

        public ObservableCollection<TripModel> Trips
        {
            set => SetValue(TripsProperty, value);
            get => (ObservableCollection<TripModel>)GetValue(TripsProperty);
        }

        public ICommand MarkerSelectedCommand
        {
            set => SetValue(MarkerSelectedCommandProperty, value);
            get => (ICommand)GetValue(MarkerSelectedCommandProperty);
        }
    }
}
