﻿using Carrotcord_API.Carrotcord.Stuff.Commands;
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
        public Action<CommandContext> resultContext;
        public Action<Exception, Message> onError;
        public Action<string, CommandContext> onCommandCanceled;
        public List<string> args = new List<string>();
        /// <summary>
        /// Use when you need an exact amount of arguments
        /// Will render <see cref="minArgs"/> and <see cref="maxArgs"/> useless.
        /// </summary>
        public int? requiredArgsExact;
        /// <summary>
        /// Use when you need a minimum amount of arguments, but you allow more than the minimum.
        /// Can't be used along with <see cref="requiredArgsExact"/>
        /// </summary>
        public int? minArgs;
        /// <summary>
        /// Use to declare a maximum amount of arguments.
        /// Can't be used along with <see cref="requiredArgsExact"/>
        /// </summary>
        public int? maxArgs;
        public bool catchError = true;

        public List<CommandRequirement> requirements;

        /// <summary>
        /// Use when you want to initialize the variables by object reference
        /// </summary>
        public Command(){}

        [Obsolete("Depracated. Will work but use Command(string name, Action<CommandContext> context) or Command(string name, Action<Message, List<string>> result, Action<Exception, Message> onError) instead")]
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

        /// <summary>
        /// Creates a command
        /// </summary>
        /// <param name="name">name of the command</param>
        /// <param name="context"></param>
        public Command(string name, Action<CommandContext> context)
        {
            this.name = name;
            resultContext = context;
        }

        public Command(string name, List<CommandRequirement> requirements, Action<CommandContext> context)
        {
            this.name = name;
            resultContext = context;
            this.requirements = requirements;
        }

        internal void Cancel(string reason, CommandContext context)
        {
            onCommandCanceled?.Invoke(reason, context);
        }

        internal bool isValid()
        {
            if (name == null || (result == null && resultContext==null)) return false;
            else return true;
        }

        internal void execute(Message message)
        {

            List<string> args;
            try
            {
                args = message.content.Substring(Bot.current.prefix.Length + name.Length + 1).Split(' ').ToList();
                if (result != null) return;
                CommandContext context = new CommandContext(Channel.getChannel(message.channelID), message.author, message, message.content.Substring(Bot.current.prefix.Length + name.Length + 1).Split(' ').ToList<string>(), this);
                if (requiredArgsExact.HasValue && args.Count != requiredArgsExact)
                {
                    Cancel($"Amount of arguments does not match the required amount of {requiredArgsExact}", context);
                    return;
                }
                if(minArgs.HasValue && args.Count<minArgs)
                {
                    Cancel($"Too little arguments given. Given: {args.Count} Minimum required: {minArgs}", context);
                    return;
                }
                if(maxArgs.HasValue && args.Count>maxArgs)
                {
                    Cancel($"Too many arguments given. Given: {args.Count} Maximum: {maxArgs}", context);
                    return;
                }
            }
            catch(ArgumentOutOfRangeException)
            {
                args = new List<string>();
            }

            if(requirements!=null)
            {
                foreach(CommandRequirement requirement in requirements)
                {
                    if (!requirement.checkCondition(new CommandContext(Channel.getChannel(message.channelID), message.author, message, args, this)))
                    {
                        Cancel(requirement.reason, new CommandContext(Channel.getChannel(message.channelID), message.author, message, args, this));
                        return;
                    }
                }
            }
            if (result != null)
            {
                try {
                    //Console.WriteLine(message.content.Substring(Bot.current.prefix.Length + name.Length + 1));
                    message.content.Substring(Bot.current.prefix.Length + name.Length + 1);
                    if(catchError)
                    {
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
                    }else result.Invoke(message, message.content.Substring(Bot.current.prefix.Length + name.Length + 1).Split(' ').ToList<string>());
                } catch(ArgumentOutOfRangeException)
                {
                    if(catchError)
                    {
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
                    }else result.Invoke(message, new List<string>());
                }
            }else if(resultContext!=null)
            {
                try
                {
                    message.content.Substring(Bot.current.prefix.Length + name.Length + 1);
                    if(catchError)
                    {
                        try
                        {
                            resultContext.Invoke(new CommandContext(Channel.getChannel(message.channelID), message.author, message, message.content.Substring(Bot.current.prefix.Length + name.Length + 1).Split(' ').ToList<string>(), this));
                        }
                        catch (Exception ex)
                        {
                            if (onError != null)
                            {
                                onError.Invoke(ex, message);
                            }
                            else
                            {
                                DiscordEmbed embed = new DiscordEmbed()
                                {
                                    title = ex.GetType().ToString(),
                                    description = ex.Message
                                };
                                message.reply("Something borked. <:monkaSCD:444404955265236992>", embed);
                            }
                        }
                    }else resultContext.Invoke(new CommandContext(Channel.getChannel(message.channelID), message.author, message, message.content.Substring(Bot.current.prefix.Length + name.Length + 1).Split(' ').ToList<string>(), this));
                }
                catch(ArgumentOutOfRangeException)
                {
                    if(catchError)
                    {
                        try
                        {
                            resultContext.Invoke(new CommandContext(Channel.getChannel(message.channelID), message.author, message, new List<string>(), this));
                        }
                        catch (Exception ex)
                        {
                            if (onError != null)
                            {
                                onError.Invoke(ex, message);
                            }
                            else
                            {
                                DiscordEmbed embed = new DiscordEmbed()
                                {
                                    title = ex.GetType().ToString(),
                                    description = ex.Message
                                };
                                message.reply("Something borked. <:monkaSCD:444404955265236992>", embed);
                            }
                        }
                    }else resultContext.Invoke(new CommandContext(Channel.getChannel(message.channelID), message.author, message, new List<string>(), this));
                }
            }
        }
    }
}
