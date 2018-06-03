using Carrotcord_API.Carrotcord.HTTP;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Carrotcord_API.Carrotcord.API
{
    public sealed class RestApiClient
    {

        static RestClient client = new RestClient();

        private static string User_Agent = "DiscordBot (https://www.youtube.com/watch?v=dQw4w9WgXcQ, v1.0.0)";

        public static int X_RateLimit_Limit_Post { get; internal set; } = 5;
        public static int X_RateLimit_Remaining_Post { get; internal set; } = 5;
        public static int X_RateLimit_Reset_Post { get; internal set; } = 0;

        public static IRestResponse GET(string dir)
        {
            client.BaseUrl = new Uri("https://discordapp.com/api/v6/");
            RestRequest request = new RestRequest(dir);
            request.AddHeader("Authorization", "Bot " + Bot.current.token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", User_Agent);
            IRestResponse response = client.Get(request);
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[GET] https://discordapp.com/api/v6/" + dir);
            Console.WriteLine(response.Content);
            foreach (Parameter header in response.Headers)
            {
                /**CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, $"[GET] {header.Name} - {header.Value}");
                if (header.Name == "X-RateLimit-Reset") Console.WriteLine($"{DateTime.Now} - {RatelimitHandler.FromUnixTime(Convert.ToInt64(header.Value))}");
                if (header.Name == "X-RateLimit-Post") Console.WriteLine("" + header.Value);*/
            }
            //CarrotcordLogger.log(CarrotcordLogger.LogSource.BOT, response.Content);
            return response;
        }

        public static IRestResponse GETNOAUTH(string dir)
        {
            client.BaseUrl = new Uri("https://discordapp.com/api/v6/");
            RestRequest request = new RestRequest(dir);
            IRestResponse response = client.Get(request);
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[GET] https://discordapp.com/api/v6/" + dir);
            return response;
        }

        public static IRestResponse POST(string dir, object body)
        {
            client.BaseUrl = new Uri("https://discordapp.com/api/v6/");
            RestRequest request = new RestRequest(dir);
            request.AddJsonBody(body);
            request.AddHeader("Authorization", "Bot " + Bot.current.token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", User_Agent);
            IRestResponse response = client.Post(request);
            checkForError(response);
            //Console.WriteLine(response.Content);
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[POST] https://discordapp.com/api/v6/" + dir);
            return response;
        }

        public static IRestResponse POSTDiscordMessage(string dir, string body)
        {
            if(X_RateLimit_Remaining_Post==0)
            {
                if (!(X_RateLimit_Reset_Post <= RatelimitHandler.ToUnixTime(DateTime.Now)))
                {
                    throw new RatelimitTriggeredException();
                }
            }
            client.BaseUrl = new Uri("https://discordapp.com/api/v6/");
            RestRequest request = new RestRequest(dir);
            request.AddJsonBody(new { content = body });
            request.AddHeader("Authorization", "Bot " + Bot.current.token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", User_Agent);
            IRestResponse response = client.Post(request);
            checkForError(response);
            if (X_RateLimit_Reset_Post != 0)
            {
                TimeSpan span = DateTime.Now.Subtract(RatelimitHandler.FromUnixTime(X_RateLimit_Reset_Post));
                Console.WriteLine("Compare: " + span.Seconds + " " + RatelimitHandler.ToUnixTime(DateTime.Now));
            }
            foreach (Parameter header in response.Headers)
            {
                //CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, $"[POST] {header.Name} - {header.Value}");
                if (header.Name == "X-RateLimit-Reset")
                {
                    X_RateLimit_Reset_Post = Convert.ToInt32(header.Value);
                    Console.WriteLine($"{DateTime.Now} - {RatelimitHandler.FromUnixTime(Convert.ToInt64(header.Value))} || {X_RateLimit_Reset_Post}");
                }
                if(header.Name=="X-RateLimit-Limit")
                {
                    X_RateLimit_Limit_Post = Convert.ToInt32(header.Value);
                }
                if(header.Name=="X-RateLimit-Remaining")
                {
                    Console.WriteLine("Remaining: "+header.Value);
                }
            }
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[POST] https://discordapp.com/api/v6/" + dir);
            return response;
        }

        private static void checkForError(IRestResponse response)
        {
            dynamic data = JsonConvert.DeserializeObject(response.Content);
            ErrorCode errorcode = (ErrorCode)Convert.ToInt32(data.code);
            Console.WriteLine(errorcode);
            if (errorcode != ErrorCode.OK)
            {
                var exception = JSONDeserializeAndHandleErrors.getExceptionFromErrorCode(errorcode);
                if (exception == null) return;
                CarrotcordLogger.logBork(exception.Message);
                if (exception != null) throw exception;
            }
        }

        public static IRestResponse PUT(string dir)
        {
            client.BaseUrl = new Uri("https://discordapp.com/api/v6/");
            RestRequest request = new RestRequest(dir);
            //request.AddBody(body);
            request.AddHeader("Authorization", "Bot " + Bot.current.token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", User_Agent);
            IRestResponse response = client.Put(request);
            Console.WriteLine(response.Content);
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[PUT] https://discordapp.com/api/v6/" + dir);
            return response;
        }

        public static IRestResponse DELET_THIS(string dir)
        {
            client.BaseUrl = new Uri("https://discordapp.com/api/v6/");
            RestRequest request = new RestRequest(dir);
            request.AddHeader("Authorization", "Bot " + Bot.current.token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", User_Agent);
            IRestResponse response = client.Delete(request);
            Console.WriteLine(response.Content);
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[DELETE] https://discordapp.com/api/v6/" + dir);
            //Console.WriteLine(response.Content);
            return response;
        }

        public static IRestResponse PATCH(string dir)
        {
            client.BaseUrl = new Uri("https://discordapp.com/api/v6/");
            RestRequest request = new RestRequest(dir);
            request.AddHeader("Authorization", "Bot " + Bot.current.token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", User_Agent);
            IRestResponse response = client.Patch(request);
            Console.WriteLine(response.Content);
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[PATCH] https://discordapp.com/api/v6/" + dir);
            Console.WriteLine(response.Content);
            return response;
        }

        public static IRestResponse PATCH(string dir, object body)
        {
            client.BaseUrl = new Uri("https://discordapp.com/api/v6/");
            RestRequest request = new RestRequest(dir);
            request.AddJsonBody(body);
            request.AddHeader("Authorization", "Bot " + Bot.current.token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", User_Agent);
            IRestResponse response = client.Patch(request);
            Console.WriteLine(response.Content);
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[PATCH] https://discordapp.com/api/v6/" + dir);
            return response;
        }

    }
}
