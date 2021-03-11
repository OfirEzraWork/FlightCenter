using System;
using System.Runtime.Serialization;

namespace FlightCenter.Facades
{
    [Serializable]
    internal class CountryAlreadyExistsInDatabaseException : Exception
    {
        public CountryAlreadyExistsInDatabaseException()
        {
        }

        public CountryAlreadyExistsInDatabaseException(string message) : base(message)
        {
        }

        public CountryAlreadyExistsInDatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CountryAlreadyExistsInDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}