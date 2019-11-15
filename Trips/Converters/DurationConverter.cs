using System;
using System.Globalization;
using Xamarin.Forms;

namespace Trips.Converters
{
    public class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TimeSpan timeSpanValue))
            {
                return string.Empty;
            }

            var formattedString = "";
            if (timeSpanValue.Days > 0)
            {
                formattedString += $"{timeSpanValue.Days} day(s)";
            }

            if (timeSpanValue.Hours > 0)
            {
                formattedString += $" {timeSpanValue.Hours} hour(s)";
            }

            if (timeSpanValue.Minutes > 0)
            {
                formattedString += $" {timeSpanValue.Minutes} minute(s)";
            }

            if (timeSpanValue.Seconds > 0)
            {
                formattedString += $" {timeSpanValue.Seconds} second(s)";
            }

            return formattedString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
