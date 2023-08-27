using System.Runtime.Serialization;

namespace CustomerOrder.Exceptions
{
    [Serializable]
    internal class CustomerIdMissmatchException : Exception
    {
        public CustomerIdMissmatchException()
        {
        }

        public CustomerIdMissmatchException(string? message) : base(message)
        {
        }

        public CustomerIdMissmatchException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CustomerIdMissmatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}