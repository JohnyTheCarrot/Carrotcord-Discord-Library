using System;
using System.Runtime.Serialization;

namespace Carrotcord_API.Carrotcord.Stuff
{
    [Serializable]
    internal class MissingAccessException50001 : Exception
    {
        public MissingAccessException50001()
        {
        }

        public MissingAccessException50001(string message) : base(message)
        {

        }

        public MissingAccessException50001(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingAccessException50001(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}