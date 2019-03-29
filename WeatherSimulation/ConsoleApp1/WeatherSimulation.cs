using System;
using System.Collections.Generic;
using System.Data;
using Random.Org;

namespace WeatherSimulation
{
    class WeatherSimulation
    {
        //Main program implementing the toy simulation of the environment
        static void Main(string[] args)
        {
            Locations _simLocations = new Locations();
            WeatherReport _simWeatherReport = new WeatherReport();
            RandomOrg _randomGenerator = new RandomOrg();

            string _sWeatherReportOutputFile = "";
            char _inDelimiter = ","[0];
            char _outDelimiter = "|"[0];
            char _outPosnDelimiter = ","[0];

            DateTime _startDateTime = DateTime.Now; //Would normally initialize from a config file or program input
            DateTime _prevDateTime; //Datetime of last reading
            DateTime _curDateTime; //Datetime of current reading

            try
            {
                //Get config values from App.config instead of hard coding values
                string _sLocFilename = System.Configuration.ConfigurationManager.AppSettings["cities_data_filename"];
                string _sWeatherReportFilename = System.Configuration.ConfigurationManager.AppSettings["weather_output_filename"];
                string _sWeatherReportFilenameTag = System.Configuration.ConfigurationManager.AppSettings["weather_output_filename_tag"];
                int _iMaxTimeIncrementSecs = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["max_time_increment_secs"]); //14 days = 60 * 60 *24 * 14 seconds
                int _iNbrSamples = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["nbr_weather_samples"]);

                InitializeEnvironment(_simLocations, _sLocFilename, _inDelimiter);
                string _sLocs = _simLocations.GetLocationsAsString(_inDelimiter);

                //Generate Weather Report for specified number of samples starting at _startDateTime
                _prevDateTime = _startDateTime;

                for (int i = 0; i < _iNbrSamples; i++)
                {
                    Console.WriteLine("Processing: " + i.ToString());
                    //Randomly select a location and time increment
                    int _currLocIndex = _randomGenerator.Next(0, _simLocations.Count() - 1);
                    int _iTimeIncrementSec = _randomGenerator.Next(1, _iMaxTimeIncrementSecs);
                    _curDateTime = _prevDateTime.AddSeconds(_iTimeIncrementSec);

                    //Generate Weather Data for location with index _currLocIndex at _currDateTime
                    WeatherData _currWD = WeatherGenerator.GenerateLocationWeatherData(_simLocations.GetLocation(_currLocIndex), _curDateTime);
                    _simWeatherReport.AddWeatherData(_currWD);

                    //Future reading will start from most recent sample datetime
                    _prevDateTime = _curDateTime;
                }

                //Write Weather Report to file.
                _sWeatherReportOutputFile = _sWeatherReportFilename.Replace(_sWeatherReportFilenameTag, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                //Console.Write(_sWeatherReportOutputFile);
                string _weatherReport = _simWeatherReport.GetWeatherReport(_outDelimiter, _outPosnDelimiter);
                FileIO.WriteTextToFile(_weatherReport, @_sWeatherReportOutputFile);
                //System.IO.File.WriteAllText(@_sWeatherReportOutputFile, _weatherReport);
            }
            catch (Exception _EX)
            {
                ErrorHander(_EX);
            }
        }

        static void InitializeEnvironment(Locations penvLocations, string psLocCSVFilename, char pcDelimiter)
        {
            //Init environment from data file psLocFilename
            //NB: Would typically be loaded from a Database
            DataTable _locDT = FileIO.DelimitedFileToDataTable(psLocCSVFilename, pcDelimiter);
            Console.WriteLine("Nbr Locations: " + _locDT.Rows.Count.ToString());

            foreach (DataRow row in _locDT.Rows)
            {
                penvLocations.AddLocation(
                    row["Name"].ToString(), Convert.ToDouble(row["Latitude"]), Convert.ToDouble(row["Longitude"]), Convert.ToDouble(row["Elevation"]));
            }
        }

        private static void ErrorHander(Exception pEX)
        {
            //Standard Error Handler for Class
            //Currently Outputs error message  but would typically be more detailed
            Console.Write("ERROR: {0} generating weather data.\r\nDetails: {1}", pEX.Message, pEX.StackTrace);
        }
    }
}
