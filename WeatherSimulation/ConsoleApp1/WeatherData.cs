using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherSimulation
{
    public class WeatherData
    {
        //Models Weather Data for a single location at a particular time
        private Location _weatherLocation;
        private DateTime _localDateTime;
        private WeatherCondition _sCondition;
        private double _dTemperature;
        private double _dPressure;
        private double _dHumidity;

        public WeatherData(Location pLocation, DateTime pdtLocalTime,
            WeatherCondition psCondition, double pdTemperature, double pdPressure, double pdHumidity)
        {
            _weatherLocation = pLocation;
            _localDateTime = pdtLocalTime;
            _sCondition = psCondition;
            _dTemperature = pdTemperature;
            _dPressure = pdPressure;
            _dHumidity = pdHumidity;
        }

        public WeatherData()
        {
            //Create new instance of class with no values populated
        }

        public Location WeatherLocation
        {
            get { return _weatherLocation; }
            set { _weatherLocation = value; }
        }

        public DateTime LocalTime
        {
            get { return _localDateTime; }
            set { _localDateTime = value; }
        }

        public WeatherCondition Condition
        {
            get { return _sCondition; }
            set { _sCondition = value; }
        }

        public double Temperature
        {
            get { return _dTemperature; }
            set { _dTemperature = value; }
        }

        public double Pressure
        {
            get { return _dPressure; }
            set { _dPressure = value; }
        }

        public double Humidity
        {
            get { return _dHumidity; }
            set { _dHumidity = value; }
        }

        public string Position(char pcDelimter)
        {
            //Output Weather Data Position in specified format; each column delimited by pcDelimter.
            //Position is a delimited triple containing latitude, longitude, and elevation in metres above sea level.
            string _sOutput = _weatherLocation.Latitude.ToString() + pcDelimter + _weatherLocation.Longitude.ToString() + pcDelimter + _weatherLocation.Elevation.ToString();
            return _sOutput;
        }
    }
}

