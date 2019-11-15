using System;

namespace Trips.Models
{
    public class CoordinateModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }

        public override string ToString() => $"{Latitude},{Longitude}";
    }
}
