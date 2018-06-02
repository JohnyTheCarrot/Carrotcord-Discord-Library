using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class CommandContext
    {

        public Channel channel;
        public User author;
        public Message message;
        public List<String> args { get; internal set; }
        internal Command command;

        internal CommandContext(Channel channel, User author, Message message, List<String> args, Command command)
        {
            this.channel = channel;
            this.author = author;
            this.message = message;
            this.args = args;
            this.command = command;
        }

    }
}
