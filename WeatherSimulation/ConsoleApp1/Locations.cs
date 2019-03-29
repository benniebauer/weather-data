using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherSimulation
{
    public class Locations
    {
        //Models a collection of locations
        private List<Location> _LocationList = new List<Location>();

        public bool AddLocation(string psName, double pdLatitude, double pdLongitude, double pdElevation)
        {
            try
            {
                Location newLocation = new Location(psName, pdLatitude, pdLongitude, pdElevation);
                _LocationList.Add(newLocation);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Count()
        {
            return _LocationList.Count();
        }

        public Location GetLocation(int pIndex)
        {
            return _LocationList[pIndex];
        }

        public string GetLocationsAsString(char pcDelimter)
        {
            string _sOutput = "";

            foreach (Location loc in _LocationList)
            {
                _sOutput = _sOutput + loc.Name + pcDelimter + loc.Latitude + pcDelimter + loc.Longitude + pcDelimter + loc.Elevation + Environment.NewLine;
            }
            return _sOutput;
        }

        //public class CarCollection : IEnumerable<Car>
        //{
        //    private List<Car> cars = new List<Car>();

        //    public IEnumerator<Car> GetEnumerator() { return this.cars.GetEnumerator(); }
    }

}
