using System.Runtime.Serialization;

namespace CustomerOrder.Exceptions
{
    [Serializable]
    public class DuplicateMailException : Exception
    {
        public DuplicateMailException()
        {
        }

        public DuplicateMailException(string? message) : base(message)
        {
        }

        public DuplicateMailException(string? message, Exception? innerException) : base(message, innerException)
        {
        }


        protected DuplicateMailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}