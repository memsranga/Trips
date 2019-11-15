using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Trips.Models;

namespace Trips.ViewModels
{
    public class TripsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public TripsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            NewTripCommand = new DelegateCommand(NewTripCommandHandler);
            TripSelectedCommand = new DelegateCommand<TripModel>(TripSelectedCommandHandler);
        }

        private void TripSelectedCommandHandler(TripModel selectedTrip)
        {
            if (selectedTrip == null)
            {
                return;
            }
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

        public ObservableCollection<TripModel> Trips { get; } = new ObservableCollection<TripModel>();

        public ICommand NewTripCommand { get; }
        public ICommand TripSelectedCommand { get; }
    }
}
