using System.Runtime.Serialization;

namespace EduQuest.Features.Courses
{
    [Serializable]
    public class InvalidCourseStatusException : Exception
    {
        public InvalidCourseStatusException()
        {
        }

        public InvalidCourseStatusException(string? message) : base(message)
        {
        }

        public InvalidCourseStatusException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidCourseStatusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}