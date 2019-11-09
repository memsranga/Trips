using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;

namespace Trips.ViewModels
{
    public class TripsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public TripsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            NewTripCommand = new DelegateCommand(NewTripCommandHandler);
        }

        private async void NewTripCommandHandler()
        {
            await _navigationService.NavigateAsync("NewTripView");
        }

        public ICommand NewTripCommand { get; }
    }
}
