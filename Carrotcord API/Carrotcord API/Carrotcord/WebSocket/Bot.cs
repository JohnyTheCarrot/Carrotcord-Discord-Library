using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using WebSocketSharp;
using Carrotcord_API.Carrotcord.Events;
using Carrotcord_API.Carrotcord.Users;
using Carrotcord_API.Carrotcord;
using Carrotcord_API.Carrotcord.Stuff;
using Carrotcord_API.Carrotcord.API;

namespace Carrotcord_API
{

    public class Bot
    {

        private WebSocket socket;
        private int heartbeat_interval;
        private Timer timer;
        public string token, game = "BEEP", status;
        private bool bootup = false;
        public SelfUser botUser;
        public static Bot current;

        internal string gateway;
        internal int api_version = 6;
        internal string encoding = "json";

        //commands
        public Dictionary<string, Command> commands = new Dictionary<string, Command>();
        public string prefix = "!";

        public event ClientReadyEventHandler ClientReadyEvent;
        public event MessageCreatedEventHandler MessageCreatedEvent;

        public Bot(string _token)
        {
            token = _token;
            dynamic d = JsonConvert.DeserializeObject(RestApiClient.GETNOAUTH("gateway").Content);
            gateway = Convert.ToString(d.url);
            socket = new WebSocket($"{gateway}/?v={api_version}&encoding={encoding}&client_id=430746509714259989");
            CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, $"[CONNECTING] {gateway}, version: {api_version}, encoding: {encoding}");
            socket.ConnectAsync();
            socket.OnOpen += Socket_OnOpen;
            socket.OnMessage += Socket_OnMessage;
            current = this;
        }

        public Bot RegisterCommand(Command command)
        {
            commands.Add(command.name, command);
            return this;
        }

        public Command getCommand(string name)
        {
            Command command;
            if (commands.TryGetValue(name, out command))
            {
                return command;
            }
            else return null;
        }

        public void UpdateStatus(string name)
        {
            socket.Send($"{{\"op\": {OPCodes.STATUS_UPDATE}, \"d\": {{ \"game\": {{ \"name\": \"{name}\", \"type\": 0 }}, \"status\": \"online\", \"afk\": false, \"since\": null }}}}");
        }

        private void Socket_OnOpen(object sender, EventArgs e)
        {
            CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, $"[CONNECTED] {gateway}/?v={api_version}&encoding={encoding}");
        }

        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        protected void READY(dynamic data)
        {
            botUser = new SelfUser();
            botUser.bot = Convert.ToBoolean(data.d.user.bot);
            botUser.username = Convert.ToString(data.d.user.username);
            botUser.ID = Convert.ToInt64(data.d.user.id);
            botUser.avatar = Convert.ToString(data.d.user.avatar);
            ClientReadyEvent(this, new ClientReadyEventArgs(botUser));
            //socket.Send($"{{\"op\":8,\"d\":{{\"guild_id\":\"430746669466910721\",\"querry\":\"\",\"limit\":50}}}}");
        }

        protected void MESSAGE_CREATED(dynamic data)
        {
            /**User author = new User();
            dynamic authorData = data.d.author;
            author.username = Convert.ToString(authorData.username);
            author.ID = Convert.ToInt64(authorData.ID);
            author.discriminator = Convert.ToInt32(authorData.discriminator);
            author.avatar = Convert.ToString(authorData.avatar);
            author.bot = Convert.ToBoolean(authorData.bot);*/
            User author = User.fromData(data.d.author);

            Message message = new Message();
            dynamic messageData = data.d;
            message.author = author;
            message.content = Convert.ToString(messageData.content);
            message.ID = Convert.ToInt64(messageData.id);
            message.channelID = Convert.ToInt64(messageData.channel_id);
            message.guildID = Convert.ToInt64(messageData.guild_id);
            message.Guild = Guild.getGuild(message.guildID);
            message.pinned = Convert.ToBoolean(messageData.pinned);

            //Storage.cachedMessages.Add(message.ID, message);

            MessageCreatedEvent(this, new MessageCreatedEventArgs(message));
            if (message.author.bot) return;
            foreach (Command cmd in commands.Values) {
                if (message.content.StartsWith(prefix) && message.content.Substring(1).Split(' ')[0]==cmd.name)
                {
                    cmd.execute(message);
                    return;
                }
            }
        }

        public static void send(string message)
        {
            current.socket.Send(message);
            current.log(message);
        }

        protected void GUILD_CREATE(dynamic data)
        {
            Guild guild = Guild.fromJSON(data.d);
            /**log(guild.name);
            foreach(Role role in guild.roles)
            {
                log($"{role.position}: {role.name}");
            }*/
        }

        protected void MESSAGE_DELETED(dynamic data)
        {
            
        }

        private void HELLO(dynamic data)
        {
            heartbeat_interval = Convert.ToInt32(data.d.heartbeat_interval);
            CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, $"Heartbeat interval set to {heartbeat_interval}.");
            if(timer==null)
            {
                timer = new Timer(heartbeat_interval);
                timer.AutoReset = true;
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
                //bool completed;
                var hello = new Payload()
                {
                    OP = OPCodes.HEARTBEAT,
                    type = null,
                    sequence = null,
                    data = null
                };
                socket.SendAsync($"{{\"op\":{OPCodes.HEARTBEAT}, \"d\": null}}", new Action<bool>(completed => {
                    if (!completed) return;
                    CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, "HEARTBEAT");
                    if (!bootup)
                    {
                        IDENTIFY();
                        bootup = true;
                    }
                }));
            }
        }

        /**private async Task SendAsync(int opcode, )
        {

        }*/

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            socket.Send("{ \"op\": 1, \"d\": null }");
            CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, "[HEARTBEAT]");
        }

        private void IDENTIFY()
        {
            string data = $"{{\"op\": 2, \"d\": {{ \"token\": \"{token}\", \"properties\": {{\"$os\": \"windows\", \"$browser\": \"carrotcord\", \"$device\": \"carrotcord\"}}, \"large_threshold\": 250, \"presence\": {{ \"game\": {{ \"name\": \"{game}\", \"type\": 0 }}, \"status\": \"online\", \"afk\": false, \"since\": null }} }} }}";
            //Console.WriteLine(data);
            CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, "[IDENTIFY]");
            socket.Send(data);
        }

        private void handleData(dynamic data)
        {
            string t = Convert.ToString(data.t);
            if (t!="null" && t!="")
            {
                CarrotcordLogger.LogServer(CarrotcordLogger.LogSource.EVENT, t);
                switch(t)
                {
                    case "READY":
                        READY(data);
                        break;
                    case "MESSAGE_CREATE":
                        MESSAGE_CREATED(data);
                        break;
                    case "MESSAGE_DELETE":
                        MESSAGE_DELETED(data);
                        break;
                    case "GUILD_CREATE":
                        GUILD_CREATE(data);
                        break;
                }
            }
            switch(Convert.ToString(data.op))
            {
                case "10":
                    HELLO(data);
                    break;
                case "11":
                    CarrotcordLogger.LogServer(CarrotcordLogger.LogSource.WEBSOCKET, "HEARTBEAT RECEIVED");
                    break;
            }
        }

        public void log(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void log(object message)
        {
            log("" + message);
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            //Gateway HELLO {"t":null,"5":null,"op":10,"d":{"heartbeat_interval":41250,"_trace"4"gateway-prd-main-6110b"]}} 
            dynamic dataJSON = JsonConvert.DeserializeObject(e.Data);
            handleData(dataJSON);
            
            //Console.WriteLine(e.Data);
        }

    }
}
