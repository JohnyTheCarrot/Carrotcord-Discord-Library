using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class EmbedAuthor
    {
        public string name;
        public string url;
        public string icon_url;
        string proxied_icon_url;

        internal object toJSON()
        {
            return new { name = this.name, url = this.url, icon_url = this.icon_url };
        }
    }
}
