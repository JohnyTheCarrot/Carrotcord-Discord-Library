using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.API
{
    public class CarrotcordLogger
    {

        public enum LogSource
        {
            //9
            WEBSOCKET = (12-9),
            REST = (12-4),
            BOT = (12-3),
            EVENT = (12-5),
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
            Console.WriteLine($"[{DateTime.Now}] [CLIENT] [{getFixedLogSource(LogSource.ERRORHANDLER)}]: {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void log(LogSource logSource, string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[{DateTime.Now}] [CLIENT] [{getFixedLogSource(logSource)}]: {message}");
        }

        public static void LogServer(LogSource logSource, string message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[{DateTime.Now}] [SERVER] [{getFixedLogSource(logSource)}]: {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
