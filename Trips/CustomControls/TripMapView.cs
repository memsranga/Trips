using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Trips.Models;
using Xamarin.Forms;

namespace Trips.CustomControls
{
    public class TripMapView : View
    {
        public static readonly BindableProperty CurrentLocationProperty = BindableProperty.Create(nameof(CurrentLocation), typeof(CoordinateModel), typeof(TripMapView), null, BindingMode.TwoWay);

        public static readonly BindableProperty RouteProperty = BindableProperty.Create(nameof(Route), typeof(IList<CoordinateModel>), typeof(TripMapView));

        public CoordinateModel CurrentLocation
        {
            set => SetValue(CurrentLocationProperty, value);
            get => (CoordinateModel)GetValue(CurrentLocationProperty);
        }

        public IList<CoordinateModel> Route
        {
            set => SetValue(RouteProperty, value);
            get => (IList<CoordinateModel>)GetValue(RouteProperty);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == CurrentLocationProperty.PropertyName)
            {
                Route?.Add(CurrentLocation);
            }
        }
    }
}
