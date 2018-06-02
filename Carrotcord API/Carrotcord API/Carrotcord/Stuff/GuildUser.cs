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

        public bool hasPermission(GuildPermission.Permission permission)
        {
            foreach(Role role in roles)
            {
                foreach(GuildPermission.Permission perm in role.permissions)
                {
                    if (perm == permission) return true;
                }
            }
            return false;
        }

        public bool hasPermission(string permission)
        {
            if (Enum.TryParse(permission, out GuildPermission.Permission perm))
            {
                return hasPermission(perm);
            }
            throw new ArgumentException($"Could not parse {permission} as a permission");
        }

        public bool hasPermissions(List<GuildPermission.Permission> permissions)
        {
            int foundPermissions = 0;
            foreach (Role role in roles)
            {
                foreach (GuildPermission.Permission perm in role.permissions)
                {
                    foreach(GuildPermission.Permission perms in permissions)
                    {
                        if (perm == perms) foundPermissions++;
                        if (foundPermissions == permissions.Count) return true;
                    }
                }
            }
            return false;
        }

        internal static GuildUser FromData(dynamic data, Guild guild)
        {
            GuildUser user = new GuildUser();
            user.user = User.fromData(data.user);
            if(user.nick!=null) user.nick = data.nick;
            user.ID = data.user.id;
            user.guild = guild;
            user.username = data.user.username;
            user.discriminator = data.user.discriminator;
            user.discriminatorString = User.fixDiscrim(user.discriminator);
            //TODO: roles;
            if(data.roles!=null)
            {
                for(int i = 0; i < data.roles.Count; i++)
                {
                    //CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, ""+data.roles[i]);
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
