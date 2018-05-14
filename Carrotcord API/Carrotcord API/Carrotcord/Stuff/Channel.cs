using Carrotcord_API.Carrotcord.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class Channel
    {
        public long ID;
        public long guild_id;
        public long last_message_id;
        public List<User> recipients = new List<User>(); //DM
        public string icon; //DM
        public long owner_id; //DM
        public int type;
        public ChannelType channelType;
        public string name;
        public string topic;
        public bool nsfw;
        public int position;
        public long application_id; //DM, if creator is bot

        public enum ChannelType
        {
            GUILD_TEXT = 0,
            DM = 1,
            GUILD_VOICE = 2,
            GROUP_DM = 3,
            GUILD_CATEGORY = 4
        }

        public Guild getGuild()
        {
            if (getChannelType() == ChannelType.DM || getChannelType() == ChannelType.GROUP_DM) throw new InvalidOperationException("Cannot get guild of channel type " + getChannelType());
            else
            {
                return Guild.getGuild(guild_id);
            }
        }

        public static Channel getChannel(long ID)
        {
            //Console.WriteLine(RestApiClient.GET("channels/" + ID).Content);
            return fromData(JSONDeserializeAndHandleErrors.DeserializeJSON(RestApiClient.GET("channels/" + ID)));
        }

        public static Channel GetGuildChannels(long guild_id)
        {
            throw new NotImplementedException();
        }

        //[RequireChannelType(ChannelType.GUILD_TEXT)]
        public void sendMessage(string message)
        {
            if (getChannelType() != ChannelType.GUILD_CATEGORY && getChannelType() != ChannelType.GUILD_VOICE)
            {
                JSONDeserializeAndHandleErrors.DeserializeJSON(RestApiClient.POSTDiscordMessage($"channels/{ID}/messages", message));
            }
            else throw new InvalidOperationException("Cannot send message to channel of type " + getChannelType());
        }

        public List<Message> getPins()
        {
            List<Message> pins = new List<Message>();
            dynamic data = JSONDeserializeAndHandleErrors.DeserializeJSON(RestApiClient.GET($"channels/{ID}/pins"));
            for(int i = 0; i < data.Length; i++)
            {
                pins.Add(Message.fromJSON(data[i]));
            }
            return pins;
        }

        public static bool isGuildChannel(Channel channel)
        {
            if (channel.getChannelType() == Channel.ChannelType.DM || channel.getChannelType() == Channel.ChannelType.GROUP_DM) return false;
            else return true;
        }

        public bool isGuildChannel()
        {
            if (getChannelType() == ChannelType.DM || getChannelType() == ChannelType.GROUP_DM) return false;
            else return true;
        }

        internal static Channel fromData(dynamic data)
        {
            Channel channel = new Channel();
            channel.ID = Convert.ToInt64(data.id);
            channel.type = Convert.ToInt32(data.type);
            channel.name = data.name;
            if(data.position!=null) channel.position = data.position;
            channel.topic = data.topic;
            if(data.guild_id!=null) channel.guild_id = data.guild_id;
            if(data.recipients!=null)
            {
                for(int i = 0; i < data.recipients.Count; i++)
                {
                    channel.recipients.Add(User.fromData(data.recipients[i]));
                }
            }
            return channel;
        }

        public ChannelType getChannelType()
        {
            return (ChannelType)type;
        }

        public static ChannelType getChannelType(int data)
        {
            return (ChannelType)data;
        }

        internal static ChannelType getChannelType(dynamic data)
        {
            return Convert.ToInt32(data.type);
        } 

    }
}
