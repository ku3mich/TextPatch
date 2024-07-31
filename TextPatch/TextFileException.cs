using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TextPatch
{
    [Serializable]
    public class TextFileException : Exception
    {
        public TextFileException()
        {
        }

        public TextFileException(string? message) : base(message)
        {
        }

        public TextFileException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TextFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
