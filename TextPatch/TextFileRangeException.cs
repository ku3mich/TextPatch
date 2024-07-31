using System.Runtime.Serialization;

namespace TextPatch
{
    [Serializable]
    public class TextFileRangeException : Exception
    {
        public TextFileRangeException()
        {
        }

        public TextFileRangeException(string? message) : base(message)
        {
        }

        public TextFileRangeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TextFileRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
