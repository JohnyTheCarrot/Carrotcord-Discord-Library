using Carrotcord_API.Carrotcord.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class Role
    {
        //{"position":0,"permissions":104324161,"name":"@everyone","mentionable":false,"managed":false,"id":"392800140974358528","hoist":false,"color":0}
        public int position;
        public int permissionsInt;
        public List<GuildPermission.Permission> permissions = new List<GuildPermission.Permission>();
        public int color;
        public string name;
        public long ID;
        public bool hoist;
        public bool mentionable;
        public bool managed;
        public Guild guild;

        public Role() { }

        public static Role getRole(long ID)
        {
            foreach(Role role in Cache.cachedRoles.Keys)
            {
                CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, ""+role.ID);
                if (role.ID == ID) return role;
            }
            return null;
        }

        internal static Role fromJSON(dynamic data, Guild guild)
        {
            Role role = new Role();
            role.guild = guild;
            role.position = data.position;
            role.permissionsInt = data.permissions;
            role.permissions = GuildPermission.getPermissions(role.permissionsInt);
            role.name = data.name;
            role.mentionable = data.mentionable;
            role.managed = data.managed;
            role.ID = data.id;
            role.hoist = data.hoist;
            role.color = data.color;
            Cache.cachedRoles.Add(role, role.guild.ID);
            return role;
        }
    }
}
