using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace EduQuest.Features.Payments
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class CannotMakePaymentException : Exception
    {
        public CannotMakePaymentException()
        {
        }

        public CannotMakePaymentException(string? message) : base(message)
        {
        }

        public CannotMakePaymentException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CannotMakePaymentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}