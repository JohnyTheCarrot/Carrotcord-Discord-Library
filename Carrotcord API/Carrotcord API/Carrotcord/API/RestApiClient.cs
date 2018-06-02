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

        public static IRestResponse GET(string dir)
        {
            client.BaseUrl = new Uri("https://discordapp.com/api/v6/");
            RestRequest request = new RestRequest(dir);
            request.AddHeader("Authorization", "Bot " + Bot.current.token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", User_Agent);
            IRestResponse response = client.Get(request);
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[GET] https://discordapp.com/api/v6/" + dir);
            //Console.WriteLine(response.Content);
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
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[POST] https://discordapp.com/api/v6/" + dir);
            return response;
        }

        public static IRestResponse POSTDiscordMessage(string dir, string body)
        {
            client.BaseUrl = new Uri("https://discordapp.com/api/v6/");
            RestRequest request = new RestRequest(dir);
            request.AddJsonBody(new { content = body });
            request.AddHeader("Authorization", "Bot " + Bot.current.token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", User_Agent);
            IRestResponse response = client.Post(request);
            foreach(Parameter header in response.Headers)
            {
                //CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, $"[POST] {header.Name} - {header.Value}");
            }
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[POST] https://discordapp.com/api/v6/" + dir);
            return response;
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
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[DELETE] https://discordapp.com/api/v6/" + dir);
            Console.WriteLine(response.Content);
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
            CarrotcordLogger.LogClient(CarrotcordLogger.LogSource.REST, "[PATCH] https://discordapp.com/api/v6/" + dir);
            return response;
        }

    }
}
