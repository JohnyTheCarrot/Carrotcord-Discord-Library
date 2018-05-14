using Carrotcord_API.Carrotcord.API;
using System;
using System.Runtime.Serialization;

namespace Carrotcord_API.Carrotcord.Stuff
{
    [Serializable]
    internal class InputTooLongException : Exception
    {
        public InputTooLongException()
        {
        }

        public InputTooLongException(FieldType type, string message) : base(message)
        {
        }

        public InputTooLongException(FieldType type, string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InputTooLongException(FieldType type, SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}