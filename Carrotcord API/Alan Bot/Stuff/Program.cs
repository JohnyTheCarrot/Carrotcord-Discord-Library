using Carrotcord_API;
using Carrotcord_API.Carrotcord.API;
using Carrotcord_API.Carrotcord.Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alan_Bot
{
    class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public static Bot bot;

        public async Task MainAsync()
        {
            bot = new Bot("NDMwNzQ2NTA5NzE0MjU5OTg5.DdTQ2A.29HzIfoEyAxzK02qSJi4PE3uV_w");
            bot.ClientReadyEvent += Bot_ClientReadyEvent;
            bot.MessageCreatedEvent += Bot_MessageCreatedEvent;
            bot.RegisterCommand(new Command("userinfo", new Action<Message, List<string>>((message, args) => {
                GuildUser user = message.Guild.getMember(message.author.ID);
                if (user == null)
                {
                    message.reply("null my dude");
                    return;
                }
                bool cando = false;
                foreach (Role role in user.roles)
                {
                    if (role == null) message.reply("role is null");
                    else
                    {
                        foreach (GuildPermission.Permission perm in role.permissions)
                        {
                            Console.WriteLine("" + perm);
                            if (perm == GuildPermission.Permission.ADMINISTRATOR)
                            {
                                cando = true;
                                break;
                            }
                        }

                    }
                    if (cando) break;
                }
                if (!cando)
                {
                    message.reply("Nope, unauthorized");
                    return;
                }
                if (args.Count <= 0)
                {
                    message.reply("Please give in the proper arguments");
                    return;
                }
                string roleText = "";
                GuildUser guildUser = message.Guild.getMember(Convert.ToInt64(args[0]));
                foreach (Role role in guildUser.roles)
                {
                    roleText += role.name + "\n";
                }
                message.reply("", new DiscordEmbed()
                {
                    title = "User Info",
                    description = $"**Username:** {user.username}\n**Roles:** {roleText}"
                });


            })));
            await Task.Delay(-1);
        }

        private void Bot_MessageCreatedEvent(object sender, Carrotcord_API.Carrotcord.Events.MessageCreatedEventArgs e)
        {
            Console.WriteLine("ah yes");
        }

        private void Bot_ClientReadyEvent(object sender, Carrotcord_API.Carrotcord.Events.ClientReadyEventArgs e)
        {
            
        }
    }
}
