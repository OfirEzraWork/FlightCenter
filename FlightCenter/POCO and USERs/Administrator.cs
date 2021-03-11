using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.POCO
{
    public class Administrator : IPoco, IUser
    {
        public int ID { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public Administrator()
        {
        }

        public Administrator(int iD, string username, string password)
        {
            ID = iD;
            Username = username;
            Password = password;
        }

        public override bool Equals(object obj)
        {
            Administrator administrator = obj as Administrator;
            if (administrator == null)
            {
                return false;
            }
            return this == administrator;
        }

        public override int GetHashCode()
        {
            return ID;
        }
        public static bool operator ==(Administrator a, Administrator b)
        {
            if (ReferenceEquals(a,null) & ReferenceEquals(b, null))
            {
                return true;
            }
            else if (ReferenceEquals(a, null) | ReferenceEquals(b, null))
            {
                return false;
            }
            else
            {
                return a.ID == b.ID;
            }
        }
        public static bool operator !=(Administrator a, Administrator b)
        {
            return !(a == b);
        }
    }
}
