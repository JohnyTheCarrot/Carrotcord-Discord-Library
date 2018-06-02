using Carrotcord_API.Carrotcord.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff.Commands
{
    public class RequireGuildPermission : CommandRequirement
    {

        public GuildPermission.Permission permission;

        public RequireGuildPermission(GuildPermission.Permission perm)
        {
            permission = perm;
            reason = $"User needs {perm} permission";
        }

        public override bool checkCondition(CommandContext context)
        {
            if (context.channel.channelType == Channel.ChannelType.GUILD_TEXT)
            {
                //TODO fix
                return context.channel.getGuild().getMember(context.author).hasPermission(permission);
            }
            else return true;
        }

    }
}
