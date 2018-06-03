using Carrotcord_API.Carrotcord.API;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class Message
    {
        public User author;
        public string content;
        public long ID, channelID, guildID = 0;
        public bool pinned;
        private Guild guild;
        public Guild Guild { get { return guild; } internal set { guild = value; } }

        public static Message getMessage(long channel, long ID)
        {
            CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, $"channels/{channel}/messages/{ID}");
            dynamic data = JSONDeserializeAndHandleErrors.DeserializeJSON(RestApiClient.GET($"channels/{channel}/messages/{ID}"));
            Message message = new Message();
            message.ID = data.id;
            message.channelID = data.channel_id;
            message.author = User.fromData(data.author);
            message.pinned = data.pinned;
            message.Guild = Guild.getGuild(message.guildID);
            message.content = Convert.ToString(data.content);
            return message;
        }

        internal static Message fromJSON(dynamic data)
        {
            Message message = new Message();
            message.author = User.fromData(data.author);
            message.content = Convert.ToString(data.content);
            message.ID = Convert.ToInt64(data.id);
            message.channelID = Convert.ToInt64(data.channel_id);
            message.guildID = Convert.ToInt64(data.guild_id);
            message.pinned = Convert.ToBoolean(data.pinned);
            message.Guild = Guild.getGuild(message.guildID);
            return message;
        }

        public void rickroll(string message)
        {
            if (Bot.current != null)
            {
                RestApiClient.POSTDiscordMessage($"channels/{channelID}/messages", $"https://www.youtube.com/watch?v=dQw4w9WgXcQ {message}");
            }
        }

        public static List<Channel> getDMs()
        {
            dynamic data = JSONDeserializeAndHandleErrors.DeserializeJSON(RestApiClient.GET("users/@me/channels"));
            if (data.Length <= 0) return null;
            List<Channel> channels = new List<Channel>();
            for(int i = 0; i < data.Length; i++)
            {
                channels.Add(Channel.fromData(data[i]));
            }
            return channels;
        }

        public Channel createDM()
        {
            IRestResponse restResponse = RestApiClient.POST("users/@me/channels", new { recipient_id = $"{author.ID}" });
            Channel c = Channel.fromData(JSONDeserializeAndHandleErrors.DeserializeJSON(restResponse));
            reply(""+c.recipients[0]);
            return c;
        }

        public void reply(string message)
        {
            if(Bot.current!=null)
            {
                CarrotcordLogger.logVerbose(RestApiClient.POSTDiscordMessage($"channels/{channelID}/messages", message).Content);
            }
        }

        public void reply(string message, DiscordEmbed embed)
        {
            if (Bot.current != null)
            {
                embed.checkLimits();
                RestApiClient.POST($"channels/{channelID}/messages", getJSON(message, embed.toJSON()));
            }
        }

        public static void edit(long channelID, long ID, string content)
        {
            JSONDeserializeAndHandleErrors.DeserializeJSON(RestApiClient.PATCH($"channels/{channelID}/messages/{ID}", new { content = content }));
        }

        public static void edit(long channelID, long ID, string content, DiscordEmbed embed)
        {
            JSONDeserializeAndHandleErrors.DeserializeJSON(RestApiClient.PATCH($"channels/{channelID}/messages/{ID}", new { content = content, embed = embed.toJSON() }));
        }

        public void edit(string _content)
        {
            JSONDeserializeAndHandleErrors.DeserializeJSON(RestApiClient.PATCH($"channels/{channelID}/messages/{ID}", new { content=_content }));
        }

        public void edit(string content, DiscordEmbed embed)
        {
            JSONDeserializeAndHandleErrors.DeserializeJSON(RestApiClient.PATCH($"channels/{channelID}/messages/{ID}", new { content = content, embed=embed.toJSON() }));
        }

        internal object getJSON(string _content, object _embed)
        {
            return new { content=_content, embed=_embed };
        }

        public void delete()
        {
            RestApiClient.DELET_THIS($"channels/{channelID}/messages/{ID}");
        }

        public void deleteOwnReaction(Emote emote)
        {
            RestApiClient.DELET_THIS($"channels/{channelID}/messages/{ID}/reactions/{emote.ToString()}/@me");
        }

        public void deleteAllReactions()
        {
            RestApiClient.DELET_THIS($"channels/{channelID}/messages/{ID}/reactions");
        }

        public void deleteReaction(Emote emote, User user)
        {
            RestApiClient.DELET_THIS($"channels/{channelID}/messages/{ID}/reactions/{emote.ToString()}/{user.ID}");
        }

        public void addReaction(Emote emote)
        {
            //433261288668266507
            RestApiClient.PUT($"channels/{channelID}/messages/{ID}/reactions/{emote.ToString()}/@me");
        }

    }
}
