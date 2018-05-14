using Carrotcord_API.Carrotcord.Stuff;
using Carrotcord_API.Carrotcord.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Events
{
    public delegate void ClientReadyEventHandler(object sender, ClientReadyEventArgs e);

    public class ClientReadyEventArgs : EventArgs
    {
        public SelfUser BotUser;

        public ClientReadyEventArgs(SelfUser BotUser)
        {
            this.BotUser = BotUser;
        }
    }

    public delegate void MessageCreatedEventHandler(object sender, MessageCreatedEventArgs e);

    public class MessageCreatedEventArgs : EventArgs
    {
        public Message message;

        public MessageCreatedEventArgs(Message message)
        {
            this.message = message;
        }
    }
}
