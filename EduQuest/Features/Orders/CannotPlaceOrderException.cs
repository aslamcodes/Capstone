using System.Runtime.Serialization;

namespace EduQuest.Features.Orders
{
    [Serializable]
    public class CannotPlaceOrderException : Exception
    {
        public CannotPlaceOrderException()
        {
        }

        public CannotPlaceOrderException(string? message) : base(message)
        {
        }

        public CannotPlaceOrderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CannotPlaceOrderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}