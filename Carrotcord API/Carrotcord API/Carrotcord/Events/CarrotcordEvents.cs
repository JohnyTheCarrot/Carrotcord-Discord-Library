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

    public delegate void CarrotcordLoggerLogEvent(object sender, CarrotcordLoggerLogEventArgs e);

    public class CarrotcordLoggerLogEventArgs : EventArgs
    {
        public string logmessage;

        public CarrotcordLoggerLogEventArgs(string logmessage)
        {
            this.logmessage = logmessage;
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

    public delegate void MessageDeletedEventHandler(object sender, MessageDeletedEventArgs e);

    /// <summary>
    /// Event args for the Message Deleted event
    /// </summary>
    public class MessageDeletedEventArgs : EventArgs
    {
        /// <summary>
        /// When the message is cached, (all messages received after bootup will be cached) this value will not be null.
        /// </summary>
        public Message message;
        /// <summary>
        /// Message ID, this value will never be null.
        /// </summary>
        public long message_id;
        /// <summary>
        /// Channel ID, this value will never be null.
        /// </summary>
        public long channel_id;
        /// <summary>
        /// Guild ID, this value will never be null.
        /// </summary>
        public long guild_id;

        public MessageDeletedEventArgs(Message message)
        {
            this.message = message;
            message_id = message.ID;
            channel_id = message.channelID;
            guild_id = message.guildID;
        }

        public MessageDeletedEventArgs(long ID, long channel, long guild)
        {
            message_id = ID;
            channel_id = channel;
            guild_id = guild;
        }

    }
}
