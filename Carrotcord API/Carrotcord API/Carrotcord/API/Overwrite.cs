using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.API
{
    public class Overwrite
    {
        public long ID;
        public string type; //either "role" or "member"
        public int allow;
        public int deny;
    }
}
