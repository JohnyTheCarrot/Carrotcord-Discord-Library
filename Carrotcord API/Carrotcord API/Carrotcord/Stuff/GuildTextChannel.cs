using Carrotcord_API.Carrotcord.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class GuildTextChannel : Channel
    {
        internal static GuildTextChannel TextChannelFromData(dynamic data)
        {
            GuildTextChannel channel = new GuildTextChannel();
            channel.guild_id = data.guild_id;
            channel.ID = data.id;
            channel.position = data.position;
            channel.name = data.name;
            channel.topic = data.topic;
            channel.nsfw = data.nsfw;
            channel.last_message_id = data.last_message_id;
            channel.type = data.type;
            return channel;
        }

    }
}
