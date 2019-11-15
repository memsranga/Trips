using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Trips.Models;
using Trips.Services.Interfaces;
using Xamarin.Forms;

namespace Trips.ViewModels
{
    public class TripsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private bool _isMapView;
        private bool _isMapMode;
        private ImageSource _splitViewImage;

        public TripsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NewTripCommand = new DelegateCommand(NewTripCommandHandler);
            TripSelectedCommand = new DelegateCommand<TripModel>(TripSelectedCommandHandler);
            MarkerSelectedCommand = new DelegateCommand<string>(MarkerSelectedCommandHandler);
            ToggleViewModeCommand = new DelegateCommand(ToggleViewModeCommandHandler);
        }

        private void ToggleViewModeCommandHandler()
        {
            IsMapMode = !IsMapMode;
        }

        public ImageSource SplitViewImage
        {
            get => _splitViewImage;
            set => SetProperty(ref _splitViewImage, value);
        }

        private void MarkerSelectedCommandHandler(string tripId)
        {
            if (string.IsNullOrEmpty(tripId))
            {
                return;
            }
            var trip = Trips.FirstOrDefault(x => x.Id == tripId);
            if (trip == null)
            {
                return;
            }

            TripSelectedCommand.Execute(trip);
        }

        private async void TripSelectedCommandHandler(TripModel selectedTrip)
        {
            if (selectedTrip == null)
            {
                return;
            }

            var navParams = new NavigationParameters
            {
                { "TripDetails", selectedTrip }
            };
            await _navigationService.NavigateAsync("ViewTripView", navParams, useModalNavigation: true);
        }

        private async void NewTripCommandHandler()
        {
            await _navigationService.NavigateAsync("NewTripView", useModalNavigation: true);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() == NavigationMode.Back)
            {
                if (parameters.TryGetValue("NewTripDetails", out TripModel newTrip))
                {
                    Trips.Add(newTrip);
                    return;
                }
            }
        }

        public bool IsMapMode
        {
            get => _isMapMode;
            set => SetProperty(ref _isMapMode, value);
        }

        public ObservableCollection<TripModel> Trips { get; } = new ObservableCollection<TripModel>();

        public ICommand NewTripCommand { get; }
        public ICommand TripSelectedCommand { get; }
        public ICommand MarkerSelectedCommand { get; }
        public ICommand ToggleViewModeCommand { get; }
    }
}
