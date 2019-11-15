using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Trips.Models;
using Trips.Services.Interfaces;

namespace Trips.Services
{
    public class LocationService : ILocationService
    {
        private ICommand _changedCommand;

        public LocationService()
        {

        }

        public async Task StartListeningAsyc(ICommand changedCommand)
        {
            _changedCommand = changedCommand;
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, false);
            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
        }

        public async Task StopListeningAsyc()
        {
            if (CrossGeolocator.Current.IsListening)
            {
                await CrossGeolocator.Current.StopListeningAsync();
                CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;
                _changedCommand = null;
            }
        }

        private void Current_PositionChanged(object sender, PositionEventArgs e)
        {
            _changedCommand?.Execute(new CoordinateModel
            {
                Latitude = e?.Position?.Latitude ?? 0,
                Longitude = e?.Position?.Longitude ?? 0,
                Speed = e?.Position?.Speed ?? 0
            });
        }
    }
}
