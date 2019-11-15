using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Trips.Models;
using Trips.Services.Interfaces;

namespace Trips.ViewModels
{
    public class NewTripViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ILocationService _locationService;
        private readonly IDeviceService _deviceService;
        private readonly IUserDialogs _userDialogs;
        private CoordinateModel _currentLocation;
        private DateTimeOffset _startTime;
        private Timer _timer;
        private double _currentSpeed;

        public NewTripViewModel(INavigationService navigationService, ILocationService locationService, IDeviceService deviceService, IUserDialogs userDialogs)
        {
            _navigationService = navigationService;
            _locationService = locationService;
            _deviceService = deviceService;
            _userDialogs = userDialogs;
            LocationChangedCommand = new DelegateCommand<CoordinateModel>(LocationChangedCommandHandler);
            BackCommand = new DelegateCommand(BackCommandHandler);
            EndTripCommand = new DelegateCommand(EndTripCommandHandler);
        }

        private async void EndTripCommandHandler()
        {
            EndTime = DateTimeOffset.UtcNow;
            var titleResult = await _userDialogs.PromptAsync("Enter the Trip Name", "Trip Name", "Save", "Discard", "Name", inputType: InputType.Name);
            var navParams = new NavigationParameters();
            if (titleResult.Ok)
            {
                var newTrip = new TripModel
                {
                    Name = titleResult.Text,
                    StartTime = StartTime,
                    EndTime = EndTime,
                    Route = Route.ToList()
                };
                navParams.Add("NewTripDetails", newTrip);
            }

            await _navigationService.GoBackAsync(navParams, useModalNavigation: true);
        }

        private async void BackCommandHandler()
        {
            await _navigationService.GoBackAsync();
        }

        private void LocationChangedCommandHandler(CoordinateModel location)
        {
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                CurrentLocation = location;
                CurrentSpeed = location.Speed;
            });
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            try
            {
                if (parameters.GetNavigationMode() == NavigationMode.New)
                {
                    StartTime = DateTimeOffset.UtcNow;
                    await _locationService.StartListeningAsyc(LocationChangedCommand);
                    _timer = new Timer(1000);
                    _timer.Elapsed += (s, ea) =>
                    {
                        _deviceService.BeginInvokeOnMainThread(() =>
                        {
                            RaisePropertyChanged(nameof(Duration));
                        });
                    };
                    _timer.Start();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            try
            {
                await _locationService.StopListeningAsyc();
                _timer.Stop();
                _timer.Dispose();
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

        public double CurrentSpeed
        {
            get => _currentSpeed;
            set => SetProperty(ref _currentSpeed, value);
        }

        public TimeSpan Duration => DateTimeOffset.UtcNow - StartTime;
        public DateTimeOffset EndTime { get; set; }

        public CoordinateModel CurrentLocation
        {
            get => _currentLocation;
            set => SetProperty(ref _currentLocation, value);
        }

        public ObservableCollection<CoordinateModel> Route { get; } = new ObservableCollection<CoordinateModel>();

        public ICommand LocationChangedCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand EndTripCommand { get; }
    }
}
