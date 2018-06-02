using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff.Commands
{
    public class RequireChannelID : CommandRequirement
    {
        public long ChannelID;
        public RequireChannelID(long ChannelID)
        {
            this.ChannelID = ChannelID;
            this.reason = "Can't do that in this channel";
        }

        public override bool checkCondition(CommandContext context)
        {
            return context.channel.ID == ChannelID;
        }

    }
}
