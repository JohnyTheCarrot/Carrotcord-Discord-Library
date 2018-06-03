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
        //internal int session_id = 0;
        public string session_id { get; internal set; }
        public int sequence { get; internal set; }
        private Connection connectionStatus;
        public bool verboseLogging = false;
        public Connection ConnectionStatus { get { return connectionStatus; } set{ connectionStatus = value; if(verboseLogging) CarrotcordLogger.logVerbose("ConnectionStatus set to " + value); } }

        //commands
        public Dictionary<string, Command> commands = new Dictionary<string, Command>();
        public string prefix = "!";
        private bool ratelimited = false;
        private bool retry = true;
        private Timer retryTimer;

        public enum Connection
        {
            LOGGING_IN,
            RESUMING, //DOING A RESUME
            EVERYTHING_IS_FINE, //:ablobsweats:
            OPCODE9, //OPCODE 9
            DISCONNECTED //DISCONNECTED
        }

        private bool helloHeartbeat = false;

        public enum ConnectType
        {
            NORMAL,
            RESUME
        }

        public event ClientReadyEventHandler ClientReadyEvent;
        public event MessageCreatedEventHandler MessageCreatedEvent;
        public event MessageDeletedEventHandler MessageDeletedEvent;

        public Bot(string _token)
        {
            ConnectionStatus = Connection.DISCONNECTED;
            token = _token;
            /**dynamic d = JsonConvert.DeserializeObject(RestApiClient.GETNOAUTH("gateway").Content);
            gateway = Convert.ToString(d.url);
            socket = new WebSocket($"{gateway}/?v={api_version}&encoding={encoding}&client_id=430746509714259989");
            CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, $"[CONNECTING] {gateway}, version: {api_version}, encoding: {encoding}");
            socket.ConnectAsync();
            socket.OnOpen += Socket_OnOpen;
            socket.OnClose += Socket_OnClose;
            socket.OnMessage += Socket_OnMessage;*/
            login(ConnectType.NORMAL);
            current = this;
        }

        private void login(ConnectType type)
        {
            if (socket!=null && socket.IsAlive) return;
            dynamic d = JsonConvert.DeserializeObject(RestApiClient.GETNOAUTH("gateway").Content);
            gateway = Convert.ToString(d.url);
            ConnectionStatus = Connection.LOGGING_IN;
            socket = new WebSocket($"{gateway}/?v={api_version}&encoding={encoding}&client_id=430746509714259989");
            CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, $"[CONNECTING] {gateway}, version: {api_version}, encoding: {encoding}");
            socket.ConnectAsync();
            if (type == ConnectType.NORMAL) socket.OnOpen += Socket_OnOpenNormal;
            else if (type == ConnectType.RESUME)
            {
                ConnectionStatus = Connection.RESUMING;
                retryTimer.Enabled = false;
                socket.OnOpen += Socket_OnOpenResume;
            }
            socket.OnClose += Socket_OnClose;
            socket.OnMessage += Socket_OnMessage;
        }

        private void Socket_OnOpenResume(object sender, EventArgs e)
        {
            //SEND_RESUME();
        }

        private void Socket_OnOpenNormal(object sender, EventArgs e)
        {
            CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, $"[CONNECTED] {gateway}/?v={api_version}&encoding={encoding}");
        }

        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        internal void SEND_RESUME()
        {
            //CarrotcordLogger.logBork("RESUMING");
            CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, $"RESUMING WITH SESSION ID {session_id} AT SEQUENCE {sequence}");
            socket.SendAsync($"{{ \"op\": {OPCodes.RESUME}, \"d\": {{\"token\": \"{token}\", \"session_id\": \"{session_id}\", \"seq\": {sequence}}}}}", new Action<bool>(completed => {
                //CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, "RESUME SENT");
            }));
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            /**if(retry)
            {
                retryTimer = new Timer(30000);
                retryTimer.AutoReset = true;
                retryTimer.Elapsed += RetryTimer_Elapsed;
                retryTimer.Start();
            }*/
            timer.Stop();
            CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, "DISCONNECTED");
        }

        private void RetryTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            login(ConnectType.RESUME);
        }

        public Bot RegisterCommand(Command command)
        {
            if (!command.isValid()) throw new NullReferenceException("Either the name or result variable has not been initialized!");
            foreach(string cmd in commands.Keys)
            {
                if(cmd==command.name)
                {
                    CarrotcordLogger.logBork($"There already is a command registered by the name of \"{command.name}\", skipping.");
                    return this;
                }
            }
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

        public User getBotOwner()
        {
            return ApplicationInfo.owner;
        }

        public void UpdateStatus(StatusType type, string name)
        {
            socket.Send($"{{\"op\": {OPCodes.STATUS_UPDATE}, \"d\": {{ \"game\": {{ \"name\": \"{name}\", \"type\": {type}}}, \"status\": \"online\", \"afk\": false, \"since\": null }}}}");
        }

        public void Disconnect()
        {
            //CarrotcordLogger.log(CarrotcordLogger.LogSource.ERRORHANDLER, "DISCONNECTED");
            if (socket.IsAlive) socket.CloseAsync();
            if (retry)
            {
                retryTimer = new Timer(5000);
                retryTimer.AutoReset = true;
                retryTimer.Elapsed += RetryTimer_Elapsed;
                retryTimer.Start();
            }
        }

        internal void OPCODE9_FAILED_RESUME()
        {
            //Disconnect();
            Timer timer = new Timer(new Random().Next(1, 5) * 1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public static void send(string message)
        {
            current.socket.Send(message);
            current.log(message);
        }

        protected void READY(dynamic data)
        {
            botUser = new SelfUser();
            botUser.bot = Convert.ToBoolean(data.d.user.bot);
            botUser.username = Convert.ToString(data.d.user.username);
            botUser.ID = Convert.ToInt64(data.d.user.id);
            botUser.avatar = Convert.ToString(data.d.user.avatar);
            session_id = Convert.ToString(data.d.session_id);
            ConnectionStatus = Connection.EVERYTHING_IS_FINE;
            ClientReadyEvent?.Invoke(this, new ClientReadyEventArgs(botUser));
        }

        protected void MESSAGE_CREATED(dynamic data)
        {
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

            Storage.cachedMessages.Add(message.ID, message);
            MessageCreatedEvent?.Invoke(this, new MessageCreatedEventArgs(message));
            if (message.author.bot) return;
            foreach (Command cmd in commands.Values) {
                if (message.content.StartsWith(prefix) && message.content.Substring(1).Split(' ')[0]==cmd.name)
                {
                    cmd.execute(message);
                    return;
                }
            }
        }

        protected void GUILD_CREATE(dynamic data)
        {
            Guild guild = Guild.fromJSON(data.d);
        }

        protected void MESSAGE_DELETED(dynamic data)
        {
            CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, data);
            if(Storage.cachedMessages.TryGetValue(Convert.ToInt64(data.d.id), out Message msg))
            {
                MessageDeletedEvent?.Invoke(this, new MessageDeletedEventArgs(msg));
            }else
            {
                MessageDeletedEvent?.Invoke(this, new MessageDeletedEventArgs(Convert.ToInt64(data.d.id), Convert.ToInt64(data.d.channel_id), Convert.ToInt64(data.d.guild_id)));
            }
        }

        private void HELLO(dynamic data)
        {
            CarrotcordLogger.LogServer(CarrotcordLogger.LogSource.WEBSOCKET, "[HELLO]");
            heartbeat_interval = Convert.ToInt32(data.d.heartbeat_interval);
            CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, $"Heartbeat interval set to {heartbeat_interval}.");
            if(timer==null || bootup==false || ConnectionStatus == Connection.RESUMING)
            {
                timer = new Timer(heartbeat_interval);
                timer.AutoReset = true;
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
                CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, "HEARTBEAT");
                socket.SendAsync($"{{\"op\":{OPCodes.HEARTBEAT}, \"d\": null}}", new Action<bool>(completed => {
                    helloHeartbeat = true;
                }));
            }
        }

        public void trigger404()
        {
            RestApiClient.GET("hahayes");
        }

        private void HELLO_AFTER_HEARTBEAT()
        {
            helloHeartbeat = false;
            if (ConnectionStatus == Connection.RESUMING)
            {
                SEND_RESUME();
                return;
            }
            if (!bootup)
            {
                IDENTIFY();
                bootup = true;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(socket.IsAlive)
            {
                socket.Send("{ \"op\": 1, \"d\": null }");
                CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, "[HEARTBEAT]");
            }
        }

        private void IDENTIFY()
        {
            string data = $"{{\"op\": 2, \"d\": {{ \"token\": \"{token}\", \"properties\": {{\"$os\": \"windows\", \"$browser\": \"carrotcord\", \"$device\": \"carrotcord\"}}, \"large_threshold\": 250, \"presence\": {{ \"game\": {{ \"name\": \"{game}\", \"type\": 0 }}, \"status\": \"online\", \"afk\": false, \"since\": null }} }} }}";
            CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, "[IDENTIFY]");
            socket.Send(data);
        }

        private void handleData(dynamic data)
        {
            string t = Convert.ToString(data.t);
            if(data.code!=null && data.code!="")
            {
                var code = Convert.ToInt32(data.code);
                CarrotcordLogger.logBork($"Error Code: {code}, {(ErrorCode)code}");
                return;
            }
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
            if(data.s!=null && Convert.ToInt32(data.s)!=null)
            {
                //session_id = Convert.ToInt32(data.s);
                sequence = Convert.ToInt32(data.s);
            }
            if(Convert.ToString(data.op)!=null)
            {
                //CarrotcordLogger.log(CarrotcordLogger.LogSource.WEBSOCKET, "OPCODE " + Convert.ToString(data.op));
            }
            switch(Convert.ToString(data.op))
            {
                case "9":
                    INVALID_SESSIONS_EVENT(data);
                    CarrotcordLogger.logBork("BIG BORK OPCODE 9");
                    break;
                case "10":
                    HELLO(data);
                    break;
                case "11":
                    CarrotcordLogger.LogServer(CarrotcordLogger.LogSource.WEBSOCKET, "<- HEARTBEAT RECEIVED");
                    if (helloHeartbeat) HELLO_AFTER_HEARTBEAT();
                    break;
            }
        }

        public void INVALID_SESSIONS_EVENT(dynamic data)
        {
            //ratelimited = true;
            CarrotcordLogger.logBork("[DISCONNECT] OPCODE 9 INVALID SESSION");
            /**CarrotcordLogger.logBork("[OPCODE 9 DATA]: ---------------------------");
            CarrotcordLogger.logBork(data);
            CarrotcordLogger.logBork("--------------------------------------------");*/
            Disconnect();
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
