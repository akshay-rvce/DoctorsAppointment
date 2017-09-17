using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Device.Location;

namespace DoctorsAppointment
{
    public class Distance
    {
        public double distance(double sLatitude,double sLongitude,double eLatitude, double eLongitude)
        {
            var sCoord = new GeoCoordinate(sLatitude, sLongitude);
            var eCoord = new GeoCoordinate(eLatitude, eLongitude);

            return sCoord.GetDistanceTo(eCoord);
        }
    }
}