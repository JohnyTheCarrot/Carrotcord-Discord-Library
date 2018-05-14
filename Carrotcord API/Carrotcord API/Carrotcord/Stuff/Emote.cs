using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class Emote
    {
        public string name;
        public long ID;

        public Emote(string name, long ID)
        {
            this.name = name;
            this.ID = ID;
        }

        public override string ToString()
        {
            return $":{name}:{ID}";
        }

    }
}
