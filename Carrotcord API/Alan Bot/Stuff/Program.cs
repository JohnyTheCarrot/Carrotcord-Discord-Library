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

            /**bot.RegisterCommand(new Command("disconnectandresume", new Action<Message, List<string>>((message, args) => {
                bot.Disconnect();
            })));*/

            bot.RegisterCommand(new Command("bb", new Action<CommandContext>(context => {

            })) { });

            bot.RegisterCommand(new Command("countwords", new Action<CommandContext>(context =>
            {
                long channelID = Convert.ToInt64(context.args[0]);
                long ID = Convert.ToInt64(context.args[1]);
                Message msg = Message.getMessage(channelID, ID);
                int wordCount = msg.content.Split(' ').Length;
                context.message.reply($"There are {wordCount} words in that message.");
                bot.UpdateStatus(StatusType.PLAYING, "");
            }))
            {
                catchError = false,
                minArgs = 2,
                onCommandCanceled = new Action<string, CommandContext>((reason, context) => {
                    context.message.reply(reason);
                })
            });

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


            bot.RegisterCommand(new Command("404", new Action<CommandContext>(context => {
                bot.trigger404();
            })));

            bot.RegisterCommand(new Command("ping", new Action<CommandContext>(context => {
                context.message.reply("pong");
                context.message.reply("pong2");
                context.message.reply("pong3");
            })));

            bot.RegisterCommand(new Command("argtest", new Action<CommandContext>(context =>
            {
                string args = "";
                foreach(string arg in context.args)
                {
                    args += arg + "\n";
                }
                DiscordEmbed embed = new DiscordEmbed()
                {
                    title = "arguments",
                    description = args
                };
                context.message.reply("", embed);
            }))
            {
                minArgs = 1,
                maxArgs = 4,
                onCommandCanceled = new Action<string, CommandContext>((reason, context) => {
                    context.message.reply(reason);
                })
            });


            bot.RegisterCommand(new Command("message", new Action<CommandContext>(context =>
            {
                DiscordEmbed embed = new DiscordEmbed()
                {
                    title = "Quote",
                    description = Message.getMessage(context.message.channelID, context.message.ID).content,
                    author = new EmbedAuthor()
                    {
                        name = context.author.username
                    }
                };
                context.message.reply("", embed);
            }))
            { catchError = false });

            bot.RegisterCommand(new Command("ping", new Action<CommandContext>(context => { context.message.reply("pong"); context.message.reply("bop"); })) { catchError = false });

            /**bot.RegisterCommand(new Command("haspermission", new Action<Message, List<string>>((message, args) => {
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
                /**
            })));*/

            bot.RegisterCommand(new userinfo());

            Command commandContextTest = new Command("test", new List<CommandRequirement>() { new RequireChannelID(448181570252308480), new RequireGuildPermission(GuildPermission.Permission.ADMINISTRATOR) }, new Action<CommandContext>(context => {
                context.message.reply("hey");
            }));

            commandContextTest.onCommandCanceled = new Action<string, CommandContext>((reason, context) => {
                context.message.reply(reason);
            });

            bot.RegisterCommand(commandContextTest);

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


