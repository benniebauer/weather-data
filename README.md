# weather-data
## Weather Data Generator

**Author:** Bernhard Baier BB 8===8

**Desc:** Weather Data Generator code for CBA. Create a toy simulation of the environment:

## Variables Affecting Weather
- atmosphere
- topography
- geography
- oceanography

that evolves over time. 

## Inputs 
measurements at various:
- locations: Data sourced from file cities_data.csv
- times: Random incrementatal times. Max increment found in App.config

## Outputs
- Location 
- Position (comma separate triple of latitude, longitude, elevation)
- Local Time 
- Conditions 
- Temperature 
- Pressure 
- Humidity

### Data Sources
- **Locations**: https://www.australiantownslist.com/ - au-towns.csv


## Installation
 - Ensure directory C:\Users\Public exists. 
 - If not, create it by running: _mkdir C:\Users\Public_
      
 - Dowload the source code as a zip file _weather-data-master.zip_
 - Unzip the source code to a folder on the C: drive of the local workstation <root_dir>
 
## Execution
 - Go to _<root_dir>\weather-data-master\WeatherSimulation\ConsoleApp1\bin\Debug_.
 - Double click _WeatherSimulation.exe_ to run the program.
 - Output file will be _C:\Users\Public\weather_report_YYYYmmdd_hhMMss.txt_.
 
