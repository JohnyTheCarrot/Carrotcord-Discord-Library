using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class EmbedFooter
    {
        public string text;
        public string icon;
        public string proxied_icon;

        internal object toJSON()
        {
            return new { text = this.text, icon_url = icon, proxied_icon_url = proxied_icon };
        } 
    }
}
