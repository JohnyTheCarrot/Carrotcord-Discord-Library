using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class ApplicationInfo
    {
        public static long ID { get; internal set; }
        public static string name { get; internal set; }
        public static string icon_hash { get; internal set; }
        public static string description { get; internal set; }
        public static List<string> rpc_origins { get; internal set; }
        public static bool bot_public { get; internal set; }
        public static bool bot_require_code_grant { get; internal set; }
        public static User owner { get; internal set; }

        internal static void fromData(dynamic data)
        {
            
        }

    }
}
