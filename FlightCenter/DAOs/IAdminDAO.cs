using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.DAOs
{
    interface IAdminDAO : IBasicDB<Administrator>
    {
        Administrator GetAdministratorByUsername(string name);
    }
}
