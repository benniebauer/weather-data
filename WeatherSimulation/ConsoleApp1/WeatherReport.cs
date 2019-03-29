using System;
using System.Collections.Generic;

namespace WeatherSimulation
{
    public class WeatherReport
    {
        //Models a report of Weather Data for various locations at various times
        private List<WeatherData> _WeatherDataList = new List<WeatherData>();

        public bool AddWeatherData(WeatherData pWeatherData)
        {
            try
            {
                _WeatherDataList.Add(pWeatherData);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddWeatherData(
            Location pLocation, DateTime pdtLocalTime, WeatherCondition pCondition, 
            double pdTemperature, double pdPressure, double pdHumidity)
        {
            try
            {
                WeatherData newWeatherData = new WeatherData(pLocation, pdtLocalTime, pCondition, pdTemperature, pdPressure, pdHumidity);
                _WeatherDataList.Add(newWeatherData);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetWeatherReport(char pcDelimter, char pcPosnDelimter)
        {
            //Output Weather Report in specified format; each column delimted by pcDelimter.
            //Assume report not too large, else would write 1 element at a time.
            string _sOutput = "";

            foreach (WeatherData wd in _WeatherDataList)
            {
                //Add '+' in front of temperature if temperature is positive
                string _TempSign = "";
                if (wd.Temperature > 0) { _TempSign = "+"; }

                //Convert time to UTC time
                DateTime _utcLocalTime = DateTimeFunctions.DateTimeToUTC(wd.LocalTime);

                //Round Temperature and Pressure to 1 decimal place
                _sOutput = _sOutput + wd.WeatherLocation.Name + pcDelimter + wd.Position(pcPosnDelimter) + pcDelimter +
                    _utcLocalTime.ToString("yyyy-MM-ddTHH:mm:ssZ") + pcDelimter + wd.Condition + pcDelimter +
                    _TempSign + Math.Round(wd.Temperature, 1) + pcDelimter + Math.Round(wd.Pressure, 1) + pcDelimter + wd.Humidity + 
                    Environment.NewLine;
            }
            return _sOutput;
        }
    }
}
