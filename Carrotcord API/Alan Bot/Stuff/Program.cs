using Alan_Bot.Commands;
using Alan_Bot.Stuff;
using Carrotcord_API;
using Carrotcord_API.Carrotcord.API;
using Carrotcord_API.Carrotcord.Stuff;
using Carrotcord_API.Carrotcord.Stuff.Commands;
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
            bot = new Bot(BotInfo.Token);
            bot.ClientReadyEvent += Bot_ClientReadyEvent;
            bot.MessageCreatedEvent += Bot_MessageCreatedEvent;

            bot.RegisterCommand(new Command("simulateinvalidsession", new Action<Message, List<string>>((message, args) => {
                bot.INVALID_SESSIONS_EVENT("{ \"haha\": \"yes\"}");
            })));

            bot.RegisterCommand(new Command("botinfo", new Action<CommandContext>(context => {
                DiscordEmbed embed = new DiscordEmbed() {
                    title = "Bot Info",
                    description = "test",
                    fields =
                    {
                        new EmbedField("Owner", "JohnyTheCarrot#0001"),
                        new EmbedField("Session ID", "["+bot.session_id +"]"),
                        new EmbedField("Sequence", ""+bot.sequence)
                    }
                };
                context.message.reply("Henlo? ", embed);  
            })));

            bot.RegisterCommand(new Command("ban", new Action<Message, List<string>>((message, args) => {
                if(args.Count==0)
                {
                    message.reply("You're gonna need to give in a user ID");
                    return;
                }
            })));

            bot.RegisterCommand(new Command("haspermission", new Action<Message, List<string>>((message, args) => {
                if(args.Count==0) {
                    message.reply("Need more arguments");
                    return;
                }
                try
                {
                    GuildUser user = message.Guild.getMember(message.author.ID);
                    message.reply($"User has {args[0]}: {user.hasPermission(args[0])}");
                }
                catch(ArgumentException)
                {
                    message.reply("Failed to parse permission");
                }
                /**if (Enum.TryParse(args[0], out GuildPermission.Permission permission))
                {
                    GuildUser user = message.Guild.getMember(message.author.ID);
                    message.reply($"User has {args[0]}: {user.hasPermission(permission)}");
                }
                else message.reply("Failed to parse permission");*/
                
            })));

            //bot.RegisterCommand(new userinfo());

            Command commandContextTest = new Command("test", new List<CommandRequirement>() { new RequireChannelID(448181570252308480), new RequireGuildPermission(GuildPermission.Permission.ADMINISTRATOR) }, new Action<CommandContext>(context => {
                context.message.reply("hey");
            }));

            commandContextTest.onCommandCanceled = new Action<string, CommandContext>((reason, context) => {
                context.message.reply(reason);
            });

            bot.RegisterCommand(commandContextTest);

            bot.RegisterCommand(new Command("userinfo", new Action<Message, List<string>>((message, args) => {

                GuildUser user = message.Guild.getMember(message.author.ID);
                if (message.Guild == null)
                {
                    message.reply("guild is null my dude");
                    return;
                }
                if (user == null)
                {
                    message.reply("null my dude");
                    return;
                }
                if (!user.hasPermission(GuildPermission.Permission.MANAGE_MESSAGES))
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
                object search = null;
                if (Int64.TryParse(args[0], out long result))
                {
                    search = result;
                }
                else search = args[0];
                GuildUser guildUser = message.Guild.getMember(search);
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

            /**bot.RegisterCommand(new Command("userinfo", new Action<Message, List<string>>((message, args) => {
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


            })));*/
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
