using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Carrotcord_API.Carrotcord.Stuff.Channel;

namespace Carrotcord_API.Carrotcord.API
{
    [AttributeUsage(AttributeTargets.Method)]
    class RequireChannelTypeAttribute : Attribute
    {

        private ChannelType channelType;

        public RequireChannelTypeAttribute(ChannelType type)
        {
            channelType = type;
        }

    }
}
