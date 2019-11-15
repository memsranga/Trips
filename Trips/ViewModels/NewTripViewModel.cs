using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
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
        private readonly IPageDialogService _pageDialogService;
        private CoordinateModel _currentLocation;

        public NewTripViewModel(INavigationService navigationService, ILocationService locationService, IDeviceService deviceService, IPageDialogService pageDialogService)
        {
            _navigationService = navigationService;
            _locationService = locationService;
            _deviceService = deviceService;
            _pageDialogService = pageDialogService;
            LocationChangedCommand = new DelegateCommand<CoordinateModel>(LocationChangedCommandHandler);
            BackCommand = new DelegateCommand(BackCommandHandler);
            EndTripCommand = new DelegateCommand(EndTripCommandHandler);
        }

        private async void EndTripCommandHandler()
        {
            var option = await _pageDialogService.DisplayActionSheetAsync("End Trip", "", "Delete", "Save");
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
            });
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            try
            {
                if (parameters.GetNavigationMode() == NavigationMode.New)
                {
                    await _locationService.StartListeningAsyc(LocationChangedCommand);
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

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
