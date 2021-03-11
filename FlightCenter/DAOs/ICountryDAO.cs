using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.DAOs
{
    public interface ICountryDAO : IBasicDB<Country>
    {
        Country GetCountryByName(string name);
    }
}
