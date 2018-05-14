using System;
using System.Runtime.Serialization;

namespace Carrotcord_API.Carrotcord.Stuff
{
    [Serializable]
    internal class UnknownMemberException10007 : Exception
    {
        public UnknownMemberException10007()
        {
        }

        public UnknownMemberException10007(string message) : base(message)
        {

        }

        public UnknownMemberException10007(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownMemberException10007(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}