using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Trips.Models;
using Trips.Services.Interfaces;
using Xamarin.Forms.Internals;

namespace Trips.ViewModels
{
    public class ViewTripViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ILocationService _locationService;
        private readonly IDeviceService _deviceService;
        private CoordinateModel _currentLocation;
        private DateTimeOffset _startTime;
        private double _averageSpeed;
        private TimeSpan _duration;
        private DateTimeOffset _endTime;
        private string _name;

        public ViewTripViewModel(INavigationService navigationService, ILocationService locationService, IDeviceService deviceService)
        {
            _navigationService = navigationService;
            _locationService = locationService;
            _deviceService = deviceService;
            CloseCommand = new DelegateCommand(CloseCommandHandler);
        }

        private async void CloseCommandHandler()
        {
            await _navigationService.GoBackAsync();
        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            try
            {
                if (parameters.GetNavigationMode() == NavigationMode.New)
                {
                    if (parameters.TryGetValue("TripDetails", out TripModel trip))
                    {
                        Name = trip.Name;
                        StartTime = trip.StartTime;
                        EndTime = trip.EndTime;
                        Duration = trip.Duration;
                        AverageSpeed = trip.AverageSpeed;
                        trip.Route.ForEach(x => Route.Add(x));

                        CurrentLocation = new CoordinateModel
                        {
                            Latitude = trip.Route.Average(x => x.Latitude),
                            Longitude = trip.Route.Average(x => x.Longitude),
                        };
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public DateTimeOffset StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }

        public double AverageSpeed
        {
            get => _averageSpeed;
            set => SetProperty(ref _averageSpeed, value);
        }

        public TimeSpan Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        public DateTimeOffset EndTime
        {
            get => _endTime;
            set => SetProperty(ref _endTime, value);
        }

        public CoordinateModel CurrentLocation
        {
            get => _currentLocation;
            set => SetProperty(ref _currentLocation, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ObservableCollection<CoordinateModel> Route { get; } = new ObservableCollection<CoordinateModel>();

        public ICommand CloseCommand { get; }
    }
}
