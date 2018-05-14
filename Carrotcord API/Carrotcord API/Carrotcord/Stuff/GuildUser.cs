using Carrotcord_API.Carrotcord.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class GuildUser : User
    {
        public User user;
        public string nick;
        public List<Role> roles = new List<Role>();
        public string joined_at;
        public bool deaf;
        public bool mute;
        public Guild guild;

        internal static GuildUser FromData(dynamic data, Guild guild)
        {
            GuildUser user = new GuildUser();
            user.user = User.fromData(data.user);
            if(user.nick!=null) user.nick = data.nick;
            user.ID = data.user.id;
            user.guild = guild;
            user.discriminator = data.user.discriminator;
            user.discriminatorString = User.fixDiscrim(user.discriminator);
            //TODO: roles;
            if(data.roles!=null)
            {
                for(int i = 0; i < data.roles.Count; i++)
                {
                    CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, ""+data.roles[i]);
                    user.roles.Add(Role.getRole(Convert.ToInt64(data.roles[i])));
                }
            }
            user.joined_at = data.joined_at;
            user.deaf = data.deaf;
            user.mute = data.mute;
            return user;
        }

    }
}
