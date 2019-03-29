using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WeatherSimulation
{
    //Global user-defined Weather data types
    public enum AusSeason
    {
        Summer, //December to February
        Autumn, //March to May
        Winter, //June to August
        Spring //September to November
    }

    public enum TimeOfDay
    {
        Morning, //6:00am-11:59am
        Afternoon, //12:00pm-5:59pm
        Evening, //6:00pm-11:59pm
        Night //12:00am-5:59am
    }

    public enum WeatherCondition
    {
        Rain,
        Sunny,
        Snow
    }
}