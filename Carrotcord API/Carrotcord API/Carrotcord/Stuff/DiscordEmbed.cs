using Carrotcord_API.Carrotcord.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class DiscordEmbed
    {
        public string title;
        public string type;
        public string description;
        public string url;
        public int color = 16711680;
        public EmbedFooter footer;
        public EmbedAuthor author;
        public List<EmbedField> fields = new List<EmbedField>();

        public override string ToString()
        {
            return $"{{\"title\":{title},\"type\":\"rich\",\"description\":\"{description}\"}}";
        }

        public void checkLimits()
        {
            if (title.Length > (int)FieldType.EMBED_TITLE) throw new InputTooLongException(FieldType.EMBED_TITLE, $"Embed title exceeds max length of {(int)FieldType.EMBED_TITLE}. Current length: {title.Length}");
            else if(description.Length>(int)FieldType.EMBED_DESCRIPTION) throw new InputTooLongException(FieldType.EMBED_TITLE, $"Embed description exceeds max length of {(int)FieldType.EMBED_DESCRIPTION}. Current length: {description.Length}");
        }

        internal object toJSON()
        {
            if(fields==null || fields.Count==0) return new { title = this.title, type = this.type, description = this.description, color = this.color, footer = footer?.toJSON(), author = author?.toJSON()};
            else return new { title = this.title, type = this.type, description = this.description, color = this.color, footer = footer?.toJSON(), author = author?.toJSON(), fields = fields };
        }
    }
}
