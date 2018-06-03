# Carrotcord-Discord-Library
A Discord library made by JohnyTheCarrot
# WORK IN PROGRESS
This library is barely usable in it's current state.
# HOW-TO

### Creating your bot

The following will login and initialize your bot.

```csharp
class Program
{

	public static void Main(string[] args)
		=> new Program().MainAsync().GetAwaiter().GetResult();
		
	public static Bot bot;
	private string token = "INSERT TOKEN HERE";
	
	public async Task MainAsync()
	{
		bot = new Bot(token);
		await Task.Delay(-1);
	}

}
```

### Creating your first commands

Now that we have our bot set up, we'd like to add some commands. Let's make the classic ping pong command.

```csharp

class Program
{

	public static void Main(string[] args)
		=> new Program().MainAsync().GetAwaiter().GetResult();
		
	public static Bot bot;
	private string token = "INSERT TOKEN HERE";
	
	public async Task MainAsync()
	{
		bot = new Bot(token);
		
		//NEW
		bot.RegisterCommand(new Command("ping", new Action<CommandContext>(context => {
			context.message.reply("pong");
		})));
		//NEW
		
		await Task.Delay(-1);
	}

}

```

# DOCS

## Commands
###

## ARGUMENTS

```csharp

            Bot bot = new Bot(token);
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
                minArgs = 1, //minimum arguments required for the command
                maxArgs = 4, //maximum arguments allowed in the command
                //when the command gets canceled because of whatever reason, the following will fire
                //when the argument count is less than minArgs or greater than maxArgs, it will also cancel the command and fire the following
                onCommandCanceled = new Action<string, CommandContext>((reason, context) => {
                    context.message.reply(reason);
                })
            });
```
