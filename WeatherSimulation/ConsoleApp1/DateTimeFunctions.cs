using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherSimulation
{
    static class DateTimeFunctions
    {
        //Generic Data and Time Functions
        public static DateTime DateTimeToUTC(DateTime pDateTime)
        {
            var _myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            DateTime _dateTime = DateTime.SpecifyKind(pDateTime, DateTimeKind.Unspecified);
            var _utcDateTime = TimeZoneInfo.ConvertTimeToUtc(_dateTime, _myTimeZone);
            return _utcDateTime;
        }


    }
}
