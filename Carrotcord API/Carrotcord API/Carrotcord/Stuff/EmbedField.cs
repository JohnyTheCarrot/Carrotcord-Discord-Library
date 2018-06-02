using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class EmbedField
    {
        public string name, value;
        public bool inline;

        public EmbedField(string name, string value)
        {
            this.name = name;
            this.value = value;
            inline = false;
        }

        public EmbedField(string name, string value, bool inline)
        {
            this.name = name;
            this.value = value;
            this.inline = inline;
        }

        internal object ToJSON()
        {
            return new { name=name, value=value, inline=inline };
        }

    }
}
