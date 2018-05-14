using Carrotcord_API.Carrotcord.API;
using System;

namespace Carrotcord_API.Carrotcord
{
    public class User
    {

        public string username;
        public string avatar;
        public int discriminator;
        /// <summary>
        /// turns for example 1 into 0001
        /// </summary>
        public string discriminatorString;
        public long ID;
        public bool bot;

        public string getAvatarURL()
        {
            return "https://cdn.discordapp.com/avatars/" + ID + "/" + avatar;
        }

        /// <summary>
        /// Returns the discriminator as shown in Discord. For example: 1 -> 0001
        /// </summary>
        /// <param name="discrim">discriminator</param>
        /// <returns></returns>
        public static string fixDiscrim(int discrim)
        {
            if (discrim < 10) return "000" + discrim;
            else if (discrim < 100) return "00" + discrim;
            else if (discrim < 1000) return "0" + discrim;
            return $"{discrim}";
        }

        public string ping()
        {
            return $"<@{ID}>";
        }

        public static void findUser(long ID)
        {
            Console.WriteLine(RestApiClient.PATCH("users/" + ID).Content);
        }

        internal static User fromData(dynamic data)
        {
            User user = new User();
            dynamic authorData = data;
            user.username = Convert.ToString(authorData.username);
            user.ID = Convert.ToInt64(authorData.id);
            user.discriminator = Convert.ToInt32(authorData.discriminator);
            user.avatar = Convert.ToString(authorData.avatar);
            user.bot = Convert.ToBoolean(authorData.bot);
            return user;
        }

        internal static string ToJSON(User user)
        {
            //"author":{ "username":"JohnyTheCarrot","id":"132819036282159104","discriminator":"0001","avatar":"a_6fa93287f72ae7ef6606860a97091655"}
            return $"{{ \"username\": {user.username}, \"id\": \"{user.ID}\", \"discriminator\":\"{user.discriminator}\", \"avatar\": \"{user.avatar}\" }}";
        }

        public static void getGuildMembers(long guildID)
        {
            //Console.WriteLine($"{{\"op\": 8, \"d\": {{\"guild_id\": \"{guildID}\", \"querry\": \"\", \"limit\": 50}}}}");
            //Bot.send($"{{\"op\":8,\"d\":{{\"guild_id\":\"{guildID}\",\"querry\":\"\",\"limit\":50}}}}");
            //Bot.send("{\"op\":8,\"d\":{\"guild_id\":\"392800140974358528\",\"querry\":\"\",\"limit\":3}}");
        }

    }
}
