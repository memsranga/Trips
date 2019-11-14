/*
 * Douglas Peucker Line Approximation for smoothening the path - https://www.codeproject.com/Articles/18936/A-C-Implementation-of-Douglas-Peucker-Line-Appro
 */
using System;
using System.Collections.Generic;
using Trips.Models;

namespace Trips.iOS.Helpers
{
    public class CurveApproxHelper
    {
        public static List<CoordinateModel> DouglasPeuckerReduction(List<CoordinateModel> points, double tolerance)
        {
            if (points == null || points.Count < 3)
                return points;

            var firstPoint = 0;
            var lastPoint = points.Count - 1;
            var pointIndexsToKeep = new List<int>();

            //Add the first and last index to the keepers
            pointIndexsToKeep.Add(firstPoint);
            pointIndexsToKeep.Add(lastPoint);

            //The first and the last point cannot be the same
            while (points[firstPoint].Equals(points[lastPoint]))
            {
                lastPoint--;
            }

            DouglasPeuckerReduction(points, firstPoint, lastPoint, tolerance, ref pointIndexsToKeep);

            var returnPoints = new List<CoordinateModel>();
            pointIndexsToKeep.Sort();
            foreach (var index in pointIndexsToKeep)
            {
                returnPoints.Add(points[index]);
            }

            return returnPoints;
        }

        private static void DouglasPeuckerReduction(List<CoordinateModel> points, int firstPoint, int lastPoint, double tolerance, ref List<int> pointIndexsToKeep)
        {
            var maxDistance = 0.0;
            var indexFarthest = 0;

            for (var index = firstPoint; index < lastPoint; index++)
            {
                var distance = PerpendicularDistance(points[firstPoint], points[lastPoint], points[index]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    indexFarthest = index;
                }
            }

            if (maxDistance > tolerance && indexFarthest != 0)
            {
                //Add the largest point that exceeds the tolerance
                pointIndexsToKeep.Add(indexFarthest);

                DouglasPeuckerReduction(points, firstPoint,
                indexFarthest, tolerance, ref pointIndexsToKeep);
                DouglasPeuckerReduction(points, indexFarthest,
                lastPoint, tolerance, ref pointIndexsToKeep);
            }
        }

        private static double PerpendicularDistance(CoordinateModel point1, CoordinateModel point2, CoordinateModel point)
        {

            var area = Math.Abs(.5 * (point1.Latitude * point2.Longitude + point2.Latitude *
            point.Longitude + point.Latitude * point1.Longitude - point2.Latitude * point1.Longitude - point.Latitude *
            point2.Longitude - point1.Latitude * point.Longitude));
            var bottom = Math.Sqrt(Math.Pow(point1.Latitude - point2.Latitude, 2) +
            Math.Pow(point1.Longitude - point2.Longitude, 2));
            var height = area / bottom * 2;

            return height;
        }
    }
}

