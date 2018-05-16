using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff
{
    public class Command
    {
        public string name;
        public Action<Message, List<string>> result;
        public Action<Exception, Message> onError;
        public List<string> args = new List<string>();

        public Command(string name, Action<Message, List<string>> result)
        {
            this.name = name;
            this.result = result;
        }

        public Command(string name, Action<Message, List<string>> result, Action<Exception, Message> onError)
        {
            this.name = name;
            this.result = result;
            this.onError = onError;
        }

        internal void execute(Message message)
        {
            if (result != null)
            {
                try {
                    //Console.WriteLine(message.content.Substring(Bot.current.prefix.Length + name.Length + 1));
                    Console.WriteLine(message.content.Substring(Bot.current.prefix.Length + name.Length + 1));
                    try
                    {
                        result.Invoke(message, message.content.Substring(Bot.current.prefix.Length + name.Length + 1).Split(' ').ToList<string>());
                    }catch(Exception ex)
                    {
                        if(onError!=null)
                        {
                            onError.Invoke(ex, message);
                        }else
                        {
                            DiscordEmbed embed = new DiscordEmbed()
                            {
                                title = ex.GetType().ToString(),
                                description = ex.Message
                            };
                            message.reply("Something borked. <:monkaSCD:444404955265236992>", embed);
                        }
                    }
                } catch(ArgumentOutOfRangeException)
                {
                    Console.WriteLine("hey");
                    try
                    {
                        result.Invoke(message, new List<string>());
                    }catch(Exception ex)
                    {
                        if (onError!=null) onError.Invoke(ex, message);
                        else
                        {
                            DiscordEmbed embed = new DiscordEmbed()
                            {
                                title = ex.GetType().ToString(),
                                description = $"**Type:**: {ex.StackTrace}\n**Message:** {ex.Message}"
                            };
                            message.reply("Something borked. <:monkaSCD:444404955265236992>", embed);
                        }
                    }
                }
            }
        }
    }
}
