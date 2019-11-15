using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;

namespace Trips.Models
{
    public class TripModel : BindableBase
    {
        private string _name;
        private DateTimeOffset _startTime;
        private DateTimeOffset _endTime;
        private IList<CoordinateModel> _route;

        public string Id { get; } = Guid.NewGuid().ToString();
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public DateTimeOffset StartTime
        {
            get => _startTime;
            set
            {
                SetProperty(ref _startTime, value);
                RaisePropertyChanged(nameof(Duration));
            }
        }

        public DateTimeOffset EndTime
        {
            get => _endTime;
            set
            {
                SetProperty(ref _endTime, value);
                RaisePropertyChanged(nameof(Duration));
            }
        }

        public IList<CoordinateModel> Route
        {
            get => _route;
            set
            {
                _route = value;
                RaisePropertyChanged(nameof(StaticImageUrl));
                RaisePropertyChanged(nameof(AverageSpeed));
                RaisePropertyChanged(nameof(ApproxCenter));
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return EndTime - StartTime;
            }
        }

        public string StaticImageUrl
        {
            get
            {
                return $"{Constants.GoogleMapsStaticBaseUrl}&{Constants.GoogleMapsStaticStyle}|{string.Join("|", Route)}";
            }
        }

        public double AverageSpeed
        {
            get
            {
                return Route.Average(x => x.Speed);
            }
        }

        public CoordinateModel ApproxCenter
        {
            get
            {
                return new CoordinateModel
                {
                    Latitude = Route.Average(x => x.Latitude),
                    Longitude = Route.Average(x => x.Longitude)
                };
            }
        }
    }
}
