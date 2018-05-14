using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.API
{
    public class OPCodes
    {
        public static int DISPATCH = 0;
        public static int HEARTBEAT = 1;
        public static int IDENTIFY = 2;
        public static int STATUS_UPDATE = 3;
        public static int VOICE_STATE_UPDATE = 4;
        public static int VOICE_SERVER_PING = 5;
        public static int RESUME = 6;
        public static int RECONNECTED = 7;
        public static int REQUEST_GUILD_MEMBERS = 8;
        public static int INVALID_SESSION = 9;
        public static int HELLO = 10;
        public static int HEARTBEAT_ACK = 11;
    }
}
