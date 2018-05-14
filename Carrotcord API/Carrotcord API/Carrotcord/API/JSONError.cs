using System;
using System.Runtime.Serialization;

namespace Carrotcord_API.Carrotcord.API
{
    [Serializable]
    internal class JSONError : Exception
    {
        public JSONError()
        {
        }

        public JSONError(ErrorCode code, string message) : base(message)
        {
        }

        public JSONError(ErrorCode code, string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JSONError(ErrorCode code, SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}