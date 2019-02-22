using System;
using System.Runtime.Serialization;

namespace QGate.Eaf.Domain.Exceptions
{
    public class EafException : Exception
    {
        public EafException()
        {
        }

        public EafException(string message) : base(message)
        {
        }

        public EafException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EafException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
