using Carrotcord_API.Carrotcord.API;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class Guild
    {
        public long ID;
        public long AFK_CHANNEL_ID;
        public long owner_id;
        public string name;
        public string icon;
        public string splash;
        public string region;
        public bool lazy;
        public bool large;
        public int member_count;
        public List<Channel> channels = new List<Channel>();
        public List<GuildUser> members = new List<GuildUser>();
        public List<Role> roles = new List<Role>();

        internal string ToString()
        {
            /**Dictionary<string, object> objects = new Dictionary<string, object>();
            objects.Add("id", ID);
            objects.Add("")
            return ConvertIntoJSON.Convert()*/
            Bot.current.log(ID);
            Bot.current.log(name);
            Bot.current.log(icon);
            Bot.current.log(region);
            Bot.current.log(AFK_CHANNEL_ID);
            Bot.current.log(owner_id);
            /**Bot.current.log(large);
            Bot.current.log(member_count);*/

            return "";
        }

        public void BanMember(User member)
        {
            if (member == null) throw new ArgumentNullException("Can't leave member as null, what were you thinking?");
            RestApiClient.PUT($"guilds/{ID}/bans/{member.ID}");
        }

        public void BanMember(GuildUser member, int delete_messages_days, string reason)
        {
            if (delete_messages_days < 0 || delete_messages_days > 7) throw new ArgumentException("Cannot delete messages older than six days. Or if you're silly and you're trying to break things, no, less than zero doesn't work smh.");
            if (String.IsNullOrEmpty(reason)) throw new ArgumentException("Can't leave reason empty.");
            if (member == null) throw new ArgumentNullException("Can't leave member as null, what were you thinking?");
            RestApiClient.PUT($"guilds/{ID}/bans/{member.ID}?delete-message-days={delete_messages_days}&reason={reason}");
        }

        public void BanMember(GuildUser member, string reason)
        {
            if (String.IsNullOrEmpty(reason)) throw new ArgumentException("Can't leave reason empty.");
            if (member == null) throw new ArgumentNullException("Can't leave member as null, what were you thinking?");
            RestApiClient.PUT($"guilds/{ID}/bans/{member.ID}?reason={reason}");
        }

        public static Guild getGuild(long ID)
        {
            /**IRestResponse response = RestApiClient.GET("guilds/" + ID);
            dynamic data = JsonConvert.DeserializeObject(response.Content);
            return fromJSON(data);*/
            if(Storage.cachedGuilds.TryGetValue(ID, out Guild value))
            {
                return value;
            }
            return null;
        }

        public GuildUser getMember(object member)
        {
            if(member is Int64) {
                //TODO: FIX
                //CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, "long: "+member);
                foreach (GuildUser m in members)
                {
                    //CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, "guilduser " + m.username + "("+m.ID+")");
                    if (m.ID == (long)member)
                    {
                        //CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, "MATCH");
                        return m;
                    }
                }
                return null;
            }
            if(member is User)
            {
                foreach (GuildUser m in members)
                {
                    if (m.user == (User)member) return m;

                }
                return null;
            }
            if(member is string)
            {
                CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, "" + member);
                foreach (GuildUser m in members)
                {
                    if (m.username == (string)member) return m;
                }
                return null;
            }
            return null;
        }

        /**public GuildUser getMember(long ID)
        {
            foreach(GuildUser member in members)
            {
                if (member.ID == ID) return member;
                
            }
            return null;
            /**IRestResponse response = RestApiClient.GET($"guilds/{this.ID}/members/{ID}");
            dynamic data = JsonConvert.DeserializeObject(response.Content);
            Console.WriteLine(data.code + " " + Convert.ToInt32(data.code));
            if (Convert.ToInt32(data.code) == 10007) throw new UnknownMemberException10007($"Unknown member \"{ID}\"");
            return GuildUser.fromData((dynamic)JsonConvert.DeserializeObject(response.Content));
        }*/

        internal static Guild fromJSON(dynamic data)
        {
            Guild guild = new Guild()
            {
                ID = Convert.ToInt64(data.id),
                name = Convert.ToString(data.name),
                icon = Convert.ToString(data.icon),
                splash = data.splash,
                region = data.region,
                owner_id = Convert.ToInt64(data.owner_id)   
            };

            if(data.roles!=null)
            {
                for(int i = 0; i < Convert.ToInt64(data.roles.Count); i++)
                {
                    guild.roles.Add(Role.fromJSON(data.roles[i], guild));
                }
            }

            if (data.members != null)
            {
                for (int i = 0; i < Convert.ToInt32(data.members.Count); i++)
                {
                    GuildUser user = GuildUser.FromData(data.members[i], guild);
                    guild.members.Add(user);
                    //CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, "guilduser init "+user.username);
                }
            }

            if(data.channels!=null)
            {
                for(int i = 0; i < Convert.ToInt32(data.channels.Count); i++)
                {
                    //Console.WriteLine(""+data.channels[i]);
                    Channel channel = Channel.fromData(data.channels[i]);
                    guild.channels.Add(channel);
                    //Console.WriteLine(channel.ID);
                }
            }

            Storage.cachedGuilds.Add(guild.ID, guild);
            
            return guild;
        }

    }
}
