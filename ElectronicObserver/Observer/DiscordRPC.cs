using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer
{
    class DiscordRPC
    {
        #region Singleton

        private static readonly DiscordRPC instance = new DiscordRPC();

        public static DiscordRPC Instance => instance;

        #endregion

        private DiscordRpcClient client;

        private int count;

        public DiscordFormat data { get; set; }

        private DiscordRPC()
        {
            // Store the client id somewhere
            client = new DiscordRpcClient("391369077991538698");

            //Set the logger
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            //Connect to the RPC
            client.Initialize();

            StartRPCUpdate();
        }

        private void SetActivity()
        {
            if (!Utility.Configuration.Config.Control.EnableDiscordRPC) return;

            string state = "";

            if (data.bot != null)
            {
                state = data.bot[++count % data.bot.Count];
            }

            //Set the rich presence
            //Call this as many times as you want and anywhere in your code.
            client.SetPresence(new RichPresence()
            {
                Details = data.top,
                State = state,
                Assets = new Assets()
                {
                    LargeImageKey = "kc_logo_512x512",
                    LargeImageText = data.large,
                    SmallImageText = data.small,
                }
            });
        }

        private void StartRPCUpdate()
        {
            data = new DiscordFormat()
            {
                bot = new List<string>(),
                top = "Loading Integration...",
                large = "Kantai Collection",
                small = "Idle"
            };

            data.bot.Add("Rank data not loaded");

            SetActivity();

            Task task = Task.Run(async () => {
                for (; ; )
                {
                    await Task.Delay(15000);
                    SetActivity();
                }
            });
        }

        public class DiscordFormat
        {
            public string top { get; set; }
            public List<string> bot { get; set; }
            public string large { get; set; }
            public string small { get; set; }
            public string timestamp { get; set; }
        }
    }
}
