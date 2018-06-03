using Carrotcord_API.Carrotcord.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.API
{
    public class CarrotcordLogger
    {

        public static event CarrotcordLoggerLogEvent LogEvent;

        public static CarrotcordLogger current { get; internal set;}

        public static bool doLogVerbose = false;

        public enum LogSource
        {
            //9
            WEBSOCKET = (12-9),
            REST = (12-4),
            BOT = (12-3),
            EVENT = (12-5),
            CHAT = (12-4),
            VERBOSE = (12-7),
            ERRORHANDLER = 0 //12
        }

        public enum Side
        {
            CLIENT,
            SERVER
        }

        private static string getFixedLogSource(LogSource logSource)
        {
            string toreturn = "" + logSource;
            for(int i = 0; i < (int)logSource; i++)
            {
                toreturn += " ";
            }
            return toreturn;
        }

        /// <summary>
        /// When shit goes south, use this. It will display the log message in red.
        /// </summary>
        /// <param name="logSource"></param>
        /// <param name="message"></param>
        public static void logBork(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now}] [CLIENT] [X] [{getFixedLogSource(LogSource.ERRORHANDLER)}]: {message}");
            Console.ForegroundColor = ConsoleColor.White;
            if (current == null) current = new CarrotcordLogger();
            LogEvent?.Invoke(current, new CarrotcordLoggerLogEventArgs($"[{DateTime.Now}] [CLIENT] [X] [{getFixedLogSource(LogSource.ERRORHANDLER)}]: {message}"));
        }

        public static void logVerbose(string message)
        {
            if (!doLogVerbose) return;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now}] [CLIENT] [X] [{getFixedLogSource(LogSource.VERBOSE)}]: {message}");
            Console.ForegroundColor = ConsoleColor.White;
            if (current == null) current = new CarrotcordLogger();
            LogEvent?.Invoke(current, new CarrotcordLoggerLogEventArgs($"[{DateTime.Now}] [CLIENT] [X] [{getFixedLogSource(LogSource.VERBOSE)}]: {message}"));
        }

        /**public static void log(LogSource logSource, string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[{DateTime.Now}] [CLIENT] [{getFixedLogSource(logSource)}]: {message}");
        }*/

        public static void log(LogSource logSource, object message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[{DateTime.Now}] [CLIENT] [{Bot.current?.sequence}] [{getFixedLogSource(logSource)}]: {message}");
            if(current==null) current = new CarrotcordLogger();
            LogEvent?.Invoke(current, new CarrotcordLoggerLogEventArgs($"[{DateTime.Now}] [CLIENT] [{getFixedLogSource(logSource)}]: {message}"));
        }

        public static void LogClient(LogSource logSource, string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{DateTime.Now}] [SERVER] [{Bot.current?.sequence}] [{getFixedLogSource(logSource)}]: {message}");
            Console.ForegroundColor = ConsoleColor.White;
            if (current == null) current = new CarrotcordLogger();
            LogEvent?.Invoke(current, new CarrotcordLoggerLogEventArgs($"[{DateTime.Now}] [SERVER] [{getFixedLogSource(logSource)}]: {message}"));
        }

        public static void LogServer(LogSource logSource, string message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[{DateTime.Now}] [SERVER] [{Bot.current?.sequence}] [{getFixedLogSource(logSource)}]: {message}");
            Console.ForegroundColor = ConsoleColor.White;
            if (current == null) current = new CarrotcordLogger();
            LogEvent?.Invoke(current, new CarrotcordLoggerLogEventArgs($"[{DateTime.Now}] [SERVER] [{getFixedLogSource(logSource)}]: {message}"));
        }
    }
}
