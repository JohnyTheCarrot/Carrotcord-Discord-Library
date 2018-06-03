using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.API
{
    public enum ErrorCode
    {
        OK = 200,
        CREATED = 201,
        NO_CONTENT = 204,
        NOT_MODIFIED = 304,
        UNKNOWN_MEMBER = 10007,
        MISSING_ACCESS = 50001,
        MISSING_PERMISSIONS = 50013,
        //CLOSE
        /// <summary>
        /// We're not sure what went wrong. Try reconnecting?
        /// </summary>
        UNKNOWN_ERROR = 4000,
        /// <summary>
        /// You sent an invalid Gateway opcode or an invalid payload for an opcode. Don't do that!
        /// </summary>
        UNKNOWN_OPCODE = 4001,
        /// <summary>
        /// You sent an invalid payload to us. Don't do that!
        /// </summary>
        DECODE_ERROR = 4002,
        /// <summary>
        /// You sent us a payload prior to identifying.
        /// </summary>
        NOT_AUTHENTICATED = 4003,
        /// <summary>
        /// The account token sent with your identify payload is incorrect.
        /// </summary>
        AUTHENTICATION_FAILED = 4004,
        /// <summary>
        /// You sent more than one identify payload. Don't do that!
        /// </summary>
        ALREADY_AUTHORIZED = 4005,
        /// <summary>
        /// The sequence sent when resuming the session was invalid. Reconnect and start a new session.
        /// </summary>
        INVALID_SEQUENCE = 4007,
        /// <summary>
        /// Woah nelly! You're sending payloads to us too quickly. Slow it down!
        /// </summary>
        RATELIMIT = 4008,
        /// <summary>
        /// Your session timed out. Reconnect and start a new one.
        /// </summary>
        SESSION_TIMEOUT = 4009
    }
}
