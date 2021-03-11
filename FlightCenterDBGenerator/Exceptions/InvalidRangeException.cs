using System;
using System.Runtime.Serialization;

namespace FlightCenterDBGenerator
{
    [Serializable]
    internal class InvalidRangeException : Exception
    {
        public InvalidRangeException()
        {
        }

        public InvalidRangeException(string message) : base(message)
        {
        }

        public InvalidRangeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}