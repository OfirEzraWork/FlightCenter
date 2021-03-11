using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter
{
    public class Customer : IPoco, IUser
    {
        public int ID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Address { get; private set; }
        public string PhoneNo { get; private set; }
        public string CreditCardNumber { get; private set; }
        public Customer()
        {
        }

        public Customer(int iD, string firstName, string lastName, string username, string password, string address, string phoneNo, string creditCardNumber)
        {
            ID = iD;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            Address = address;
            PhoneNo = phoneNo;
            CreditCardNumber = creditCardNumber;
        }

        public override bool Equals(object obj)
        {
            Customer customer = obj as Customer;
            if (customer == null)
            {
                return false;
            }
            return this == customer;
        }

        public override int GetHashCode()
        {
            return ID;
        }
        public static bool operator ==(Customer a, Customer b)
        {
            if (ReferenceEquals(a, null) & ReferenceEquals(b, null))
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
        public static bool operator !=(Customer a, Customer b)
        {
            return !(a == b);
        }
    }
}
