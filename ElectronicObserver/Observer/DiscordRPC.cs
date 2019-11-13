using DiscordRPC;
using DiscordRPC.Logging;
using ElectronicObserver.Data;
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
		private int count;

		private DiscordRpcClient forcedClient;

		private List<string> clientIds;

		private DiscordRpcClient currentClient;
		private int currentClientIndex = -1;

		public DiscordFormat data { get; set; }

		private DiscordRPC()
		{

			if (!String.IsNullOrEmpty(Utility.Configuration.Config.Control.DiscordRPCApplicationId))
			{
				string clientID = Utility.Configuration.Config.Control.DiscordRPCApplicationId;
				// Store the client id somewhere
				forcedClient = new DiscordRpcClient(clientID);
			}
			else if (Utility.Configuration.Config.Control.UseFlagshipIconForRPC)
			{
				clientIds = new List<string>
				{
					"643912955913699363", // --- 1 -> 150
                    "643915499888967690", // --- 151 -> 300
                    "643915522127298579", // --- 301 -> 450
                    "643915540158611544", // --- 451 -> 600
                    "644074408389902336", // --- 601 -> 750
                    "644074452228898825", // --- 751 -> 900

                    "644074508306874389", // --- 1351 -> 1500
                };
			}
			else
			{
				// default application
				forcedClient = new DiscordRpcClient("391369077991538698");
			}

			StartRPCUpdate();
		}

		private void SetActivity()
		{
			if (!Utility.Configuration.Config.Control.EnableDiscordRPC) return;

			KCDatabase db = KCDatabase.Instance;

			int shipID = 0;
			if (db.Fleet.Fleets.Count > 0 && db.Fleet[1].MembersInstance.Count > 0) shipID = db.Fleet[1].MembersInstance[0].ShipID;

			string state = "";

			if (data.bot != null)
			{
				state = data.bot[++count % data.bot.Count];
			}

			if (forcedClient == null)
			{
				int clientId = Math.Abs((shipID - 1) / 150);
				if (currentClientIndex == clientId)
				{
					DiscordRpcClient client = currentClient;

					client.SetPresence(new RichPresence()
					{
						Details = data.top,
						State = state,
						Assets = new Assets()
						{
							LargeImageKey = shipID.ToString(),
							LargeImageText = data.large,
							SmallImageText = data.small,
						}
					});
				}
				else
				{
					DiscordRpcClient client = new DiscordRpcClient(clientIds[clientId]);
					client.OnReady += (sender, e) =>
					{
						client.SetPresence(new RichPresence()
						{
							Details = data.top,
							State = state,
							Assets = new Assets()
							{
								LargeImageKey = Utility.Configuration.Config.Control.UseFlagshipIconForRPC ? shipID.ToString() : "kc_logo_512x512",
								LargeImageText = data.large,
								SmallImageText = data.small,
							}
						});

						if (currentClient != null) currentClient.Dispose();
						currentClient = client;
						currentClientIndex = clientId;
					};

					client.Initialize();
				}
			}
			else
			{
				forcedClient.SetPresence(new RichPresence()
				{
					Details = data.top,
					State = state,
					Assets = new Assets()
					{
						LargeImageKey = shipID.ToString(),
						LargeImageText = data.large,
						SmallImageText = data.small,
					}
				});
			}
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

		public string MapInfo { get; set; }
	}
}
