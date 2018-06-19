using System;
using Newtonsoft.Json;
using System.IO;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using SimpleTCP;
using System.Collections.Generic;

namespace OpenRCT2_Twitch_Bot
{
    class Program
    {
        public static Config config;
        public static Dictionary<string, string> ChannelToIP;
        public static SimpleTCP.SimpleTcpServer server = new SimpleTcpServer();
        static void Main(string[] args)
        {
            //Startup
            config = new Config();
            
            try
            {
                if (File.Exists("./config.json"))
                {
                    config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("./config.json"));
                } else
                {
                    GenerateNewConfig();
                }
            } catch
            {
                GenerateNewConfig();
            }
            ConnectionCredentials credentials = new ConnectionCredentials(config.username, config.oauth);

            
            server.Start(6500);
            server.DataReceived += Server_DataReceived;
            server.ClientConnected += Server_ClientConnected;
            server.ClientDisconnected += Server_ClientDisconnected;
            Console.WriteLine("Server is ready for connections.");
            Console.ReadKey();
            
            //From here we listen for connecting OpenRCT2 instances.
            //TwitchClient client;
        }

        private static void Server_ClientDisconnected(object sender, System.Net.Sockets.TcpClient e)
        {
            Console.WriteLine("Client Disconnected.");
        }

        private static void Server_ClientConnected(object sender, System.Net.Sockets.TcpClient e)
        {
            Console.WriteLine("Client Connected.");
            System.Threading.Thread.Sleep(500);
        }

        private static void Server_DataReceived(object sender, Message e)
        {
            Console.WriteLine(e.MessageString);
        }

        private static void GenerateNewConfig()
        {
            config.username = "default";
            config.oauth = "oauth:default";
            config.connectionid = "default";
            File.WriteAllText("./config.json", JsonConvert.SerializeObject(config, Formatting.Indented));
            Console.WriteLine("New Config Generated.");
            Environment.Exit(1);
        }
    }
    class Config
    {
        public string username;
        public string oauth;
        public string connectionid;
    }
}
