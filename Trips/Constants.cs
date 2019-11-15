using System;
namespace Trips
{
    public class Constants
    {
        // TODO: Google Maps Api Key
        public const string GoogleMapsApiKey = "";
        public static readonly string GoogleMapsStaticBaseUrl = $"https://maps.googleapis.com/maps/api/staticmap?size=800x600&zoom=11&key={GoogleMapsApiKey}";
        public const string GoogleMapsStaticStyle = "path=color:0x0000ff|weight:5";
    }
}
