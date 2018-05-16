using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    class Storage
    {
        public static Dictionary<long, Message> cachedMessages = new Dictionary<long, Message>();
        public static Dictionary<long, Guild> cachedGuilds = new Dictionary<long, Guild>();
    }
}
