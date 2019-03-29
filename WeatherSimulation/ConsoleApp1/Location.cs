using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherSimulation
{
    public class Location
    {
        //Models a single location
        private string _sName;
        private double _dLatitude;
        private double _dLongitude;
        private double _dElevation;

        public Location(string psName, double pdLatitude, double pdLongitude, double pdElevation)
        {
            _sName = psName;
            _dLatitude = pdLatitude;
            _dLongitude = pdLongitude;
            _dElevation = pdElevation;
        }

        public string Name
        {
            get { return _sName; }
            set { _sName = value; }
        }

        public double Latitude
        { 
            //Latitude : max/min +90 to -90: Could create user defined datatype
            get { return _dLatitude; }
            set { _dLatitude = value; }
        }

        public double Longitude
        {
            //Longitude : max/min +180 to -180: Could create user defined datatype
            get { return _dLongitude; }
            set { _dLongitude = value; }
        }

        public double Elevation
        {
            get { return _dElevation; }
            set { _dElevation = value; }
        }
    }
}
