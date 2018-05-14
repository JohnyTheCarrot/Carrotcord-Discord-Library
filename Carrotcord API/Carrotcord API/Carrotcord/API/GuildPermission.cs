using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.API
{
    public class GuildPermission
    {
        [Flags]
        public enum Permission
        {
            CREATE_INSTANT_INVITE = 1,
            KICK_MEMBERS = 2,
            BAN_MEMBERS = 4,
            ADMINISTRATOR = 8,
            MANAGE_CHANNELS = 16,
            MANAGE_GUILD = 32,
            ADD_REACTIONS = 64, //7
            VIEW_AUDIT_LOG = 128,
            VIEW_CHANNELS = 1024,
            SEND_MESSAGES = 2048,
            SEND_TTS_MESSAGES = 4096,
            MANAGE_MESSAGES = 8192,
            EMBED_LINKS = 16384,
            ATTACH_FILES = 32768, //14 middle
            READ_MESSAGE_HISTORY = 65536,
            MENTION_EVERYONE = 131072,
            USE_EXTERNAL_EMOJIS = 262144,
            CONNECT = 1048576,
            SPEAK = 2097152,
            MUTE_MEMBERS = 4194304,
            DEAFEN_MEMBERS = 8388608, //7
            MOVE_MEMBERS = 16777216,
            USE_VOICE_ACTIVITY = 33554432,
            CHANGE_NICKNAME = 67108864,
            MANAGE_NICKNAMES = 134217728,
            MANAGE_ROLES = 268435456,
            MANAGE_WEBHOOKS = 536870912,
            MANAGE_EMOJIS = 1073741824
            /**
             * CREATE_INSTANT_INVITE = 0x00000001,
        KICK_MEMBERS = 0x00000002,
        BAN_MEMBERS = 0x00000004,
        ADMINISTRATOR = 0x00000008,
        MANAGE_CHANNELS = 0x00000010,
        MANAGE_GUILD = 0x00000020,
        ADD_REACTIONS = 0x00000040, //7
        VIEW_AUDIT_LOG = 0x00000080,
        VIEW_CHANNELS = 0x00000400,
        SEND_MESSAGES = 0x00000800,
        SEND_TTS_MESSAGES = 0x00001000,
        MANAGE_MESSAGES = 0x00002000,
        EMBED_LINKS = 0x00004000,
        ATTACH_FILES = 0x00008000, //14 middle
        READ_MESSAGE_HISTORY = 0x00010000,
        MENTION_EVERYONE = 0x00020000,
        USE_EXTERNAL_EMOJIS = 0x00040000,
        CONNECT = 0x00100000,
        SPEAK = 0x00200000,
        MUTE_MEMBERS = 0x00400000,
        DEAFEN_MEMBERS = 0x00800000, //7
        MOVE_MEMBERS = 0x01000000,
        USE_VAD = 0x02000000,
        CHANGE_NICKNAME = 0x04000000,
        MANAGE_NICKNAMES = 0x08000000,
        MANAGE_ROLES = 0x10000000,
        MANAGE_WEBHOOKS = 0x20000000,
        MANAGE_EMOJIS = 0x40000000
             * */
        }

        public static Permission calculatePermission(Permission[] permissions)
        {
            //Permission permission = permissions[0] | permissions[1] | permissions[2];
            Permission perm = permissions[0];
            for (int i = 0; i < permissions.Length; i++)
            {
                if (i != 0)
                {
                    perm = perm | permissions[i];
                }
            }
            return perm;
        }

        public static List<Permission> getPermissions(Permission value)
        {
            List<Permission> permissions = new List<Permission>();
            foreach (Permission perm in Enum.GetValues(typeof(Permission)))
            {
                if (value.HasFlag(perm)) permissions.Add(perm);
            }
            return permissions;
        }

        public static List<Permission> getPermissions(int value)
        {
            Permission parsedPerm = (Permission)value;
            List<Permission> permissions = new List<Permission>();
            foreach (Permission perm in Enum.GetValues(typeof(Permission)))
            {
                if (parsedPerm.HasFlag(perm)) permissions.Add(perm);
            }
            return permissions;
        }

        public static int TurnBitOn(int value, int bitToTurnOn)
        {
            return (value | bitToTurnOn);
        }
    }
}