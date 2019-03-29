using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using Random.Org;

namespace WeatherSimulation
{
    static class WeatherGenerator
    {
        const double SYDNEY_LAT = -33.86714;

        //Functions used to generate Weather Data for a Location
        public static WeatherCondition GetRainOrSunny()
        {
            //Randomly decide whether Rain or Sunny. Would normally weight it towards being sunny but assume a 50/50 liklihood of each.
            RandomOrg _rndGenerator = new RandomOrg();
            int _wcIndex = _rndGenerator.Next(0, 1);
            return (WeatherCondition)_wcIndex;
        }

        public static AusSeason GetAusSeason(DateTime pDatetime)
        {
            //Return Seasonfor pDatetime. Could implement as a SQL stored proc or view
            if (new int[] { 12, 1, 2 }.Contains(pDatetime.Month))
                return AusSeason.Summer;
            else if (new int[] { 3, 4, 5 }.Contains(pDatetime.Month))
                return AusSeason.Autumn;
            else if (new int[] { 6, 7, 8 }.Contains(pDatetime.Month))
                return AusSeason.Winter;
            //else if (new int[] { 9, 10, 11 }.Contains(pDatetime.Month))
            else
                return AusSeason.Spring;
        }

        public static TimeOfDay GetTimeOfDay(DateTime pDatetime)
        {
            //Return Seasonfor pDatetime. Could implement as a SQL stored proc or view
            if (pDatetime.Hour >= 6 && pDatetime.Hour <= 11)
                return TimeOfDay.Morning;
            else if (pDatetime.Hour >= 12 && pDatetime.Hour <= 17)
                return TimeOfDay.Afternoon;
            else if (pDatetime.Hour >= 18 && pDatetime.Hour <= 23)
                return TimeOfDay.Evening;
            else
                return TimeOfDay.Night;
        }

        public static double GetSydneyAverageTemperatureForMonth(int piMonth)
        {
            //Return Hard Coded average temperature in Sydney for a month piMonth. 
            //For the simulation will be used as a base to calculate temperature for other locations
            //Data sourced from https://www.holiday-weather.com/sydney/averages/#chart-head-temperature
            switch (piMonth)
            {
                case 1:
                    return 23;
                case 2:
                    return 23;
                case 3:
                    return 22;
                case 4:
                    return 19;
                case 5:
                    return 16;
                case 6:
                    return 14;
                case 7:
                    return 13;
                case 8:
                    return 14;
                case 9:
                    return 16;
                case 10:
                    return 18;
                case 11:
                    return 20;
                default: //12
                    return 22;
            }
        }

        public static double ElevationTemperatureOffset(WeatherCondition pWeatherCondition, double pdElevation)
        {
            //Return Elevation Temperature Offset value based on pWeatherCondition and pdElevation
            //Formula sourced from: https://www.onthesnow.com/news/a/15157/does-elevation-affect-temperature:
            //Simplify to the following:
            // - If no snow or rain, temperature decreases by about 9.8°C per 1,000 meters up in elevation. 
            // - If snow or rain, temperature decreases by about 6°C per 1,000 meters up in elevation.
            double _offsetPer1000m;

            if (pWeatherCondition == WeatherCondition.Rain) //|| pWeatherCondition == WeatherCondition.Snow)
                _offsetPer1000m = -6.0;
            else //pWeatherCondition == WeatherCondition.Sunny
                _offsetPer1000m = -9.8;

            return (_offsetPer1000m / (-1000 / pdElevation));
        }

        public static double LatitudeTemperatureOffset(double pdLatitude)
        {
            //Return latitude Temperature Offset value based on pdLatitude 
            //Formula sourced from: https://www.onthesnow.com/news/a/15157/does-elevation-affect-temperature:
            //Temperatures Cool With Increasing Latitude
            //const double SYDNEY_LAT = -33.86714;

            //Simplistic, but take the approach that the difference in latitude increases or decreases the temperature by that amount
            return (pdLatitude  - SYDNEY_LAT);
        }

        public static double DeriveTemperature(Location pLocation, DateTime pDatetime, WeatherCondition pWeatherCondition)
        {
            //Return Temperature for pLocation at pDatetime. 
            //Start with Sydney's average temperature for the month in pDatetime and adjust based on elevation and latitude offsets.
            //NB: Could adjust temperature based on time of day, WeatherCondition etc, but leave for now
            //https://sciencing.com/latitude-affect-climate-4586935.html
            double _locationTemperature;
            RandomOrg _randomGenerator = new RandomOrg();

            AusSeason _season = GetAusSeason(pDatetime);
            TimeOfDay _tod = GetTimeOfDay(pDatetime);
            double _initTemperature = GetSydneyAverageTemperatureForMonth(pDatetime.Month);
            double _elevationOffset = ElevationTemperatureOffset(pWeatherCondition, pLocation.Elevation);
            double _latitudeOffset = LatitudeTemperatureOffset(pLocation.Latitude);

            _locationTemperature = _initTemperature + _latitudeOffset - _elevationOffset;

            //If Winter, raining, latitude < Sydney Latitude, and _locationTemperature < 10, randomly drop temperature by 10 degrees to ensure snow
            if (_season == AusSeason.Winter && pWeatherCondition == WeatherCondition.Rain && pLocation.Latitude < SYDNEY_LAT &&
                _locationTemperature < 10 && _locationTemperature > 0)
            {
                int _snowFlag = _randomGenerator.Next(0, 1);
                if (_snowFlag == 1) { _locationTemperature = _locationTemperature - 10; }
            }

            return _locationTemperature;
        }

        public static double DerivePressure(Location pLocation, double pdTemperature)
        {
            //Return Pressure for pLocation at pDatetime. 
            //https://sciencing.com/range-barometric-pressure-5505227.html
            //Standard air pressure 1013.25 HPa. The highest recorded was 1084 in Siberia; lowest was 870 in a typhoon in the Pacific Ocean.
            //Temperature and altitude affect barometric pressure.
            //Air pressure varies with altitude; it is always lower at high altitudes, regardless of weather.
            //Cool air is less dense than warm air. This results in lower air pressure.
            const double STD_PRESSURE_PA = 101325;
            double _locationPressure = 0;

            //Derive pressure from formula in https://socratic.org/questions/how-do-you-calculate-atmospheric-pressure-at-an-altitude
            _locationPressure = STD_PRESSURE_PA * Math.Pow((1 - 2.25577 * Math.Pow(10, -5) * pLocation.Elevation), 5.25588);

            //TODO: Adjust pressure based on temperature. Leave for now
            //_locationPressure = _locationPressure + - _temperatureOffset;
            return _locationPressure / 100; //Convert from Pa to HPa
        }

        public static double DeriveHumidity(Location pLocation, double pdTemperature, double pdPressure, WeatherCondition pWeatherCondition)
        {
            //Return Humidity for pLocation with temperature pdTemperature and pressure pdPressure. 
            //For this exercise, pick a random number between a range based on pWeatherCondition 
            double _locationHumidity = 0;
            RandomOrg _randomGenerator = new RandomOrg();

            if (pWeatherCondition == WeatherCondition.Rain)
            {
                _locationHumidity = _randomGenerator.Next(80, 100);
            }
            else if (pWeatherCondition == WeatherCondition.Snow)
            {
                _locationHumidity = _randomGenerator.Next(40, 79);
            }
            else //Sunny
            {
                _locationHumidity = _randomGenerator.Next(0, 39);
            }
            return _locationHumidity;
        }

        public static WeatherData GenerateLocationWeatherData(Location pLocation, DateTime pDatetime)
        {
            //Generate Weather Data for location pLocation at pdtDatetime
            WeatherData _wd = new WeatherData();
            WeatherCondition _weatherCondition = GetRainOrSunny();

            _wd.WeatherLocation = pLocation;
            _wd.LocalTime = pDatetime;
            _wd.Temperature = DeriveTemperature(pLocation, pDatetime, _weatherCondition);

            //Assume snowing if raining and temperature less that 0
            if (_wd.Temperature <= 0 && _weatherCondition == WeatherCondition.Rain)
            {
                _wd.Condition = WeatherCondition.Snow;
            }
            else
            {
                _wd.Condition = _weatherCondition;
            }

            _wd.Pressure = DerivePressure(pLocation, _wd.Temperature);
            _wd.Humidity = DeriveHumidity(pLocation, _wd.Temperature, _wd.Pressure, _wd.Condition);
            return _wd;
        }

    }
}