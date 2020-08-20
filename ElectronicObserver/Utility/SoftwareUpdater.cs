using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using DynaJson;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window;
using AppSettings = ElectronicObserver.Properties.Settings;

namespace ElectronicObserver.Utility
{
    internal class SoftwareUpdater
    {
        internal static string AppDataFolder =>
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\\ElectronicObserver";

        private static bool WaitForRestart { get; set; }
		public static UpdateData CurrentVersion { get; set; } = new UpdateData();
		public static UpdateData LatestVersion { get; set; } = new UpdateData();

		/// <summary>
		/// Perform software update in background 
		/// </summary>
		public static void UpdateSoftware()
        {
            if (WaitForRestart) return;

            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);

			var url = LatestVersion.AppDownloadUrl;
			if (url != string.Empty)
            {
	            try
	            {
		            Logger.Add(1, string.Format("Started downloading update. {0}", url));
		            DownloadUpdate(url);
		            Logger.Add(1, "Download finished.");
				}
	            catch
	            {
		            return;
	            }
            }

			try
			{
				DownloadUpdater();
			}
			catch
			{
				return;
			}

            var updaterFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\EOUpdater.exe";
            if (!File.Exists(updaterFile)) return;
            var updater = new Process
            {
	            StartInfo =
	            {
		            FileName = updaterFile,
		            UseShellExecute = false,
		            CreateNoWindow = false,
		            Arguments = "--restart"
	            }
            };
            updater.Start();
            Logger.Add(2, "Close Electronic Observer to complete the update process. It will restart automatically.");
            WaitForRestart = true;
        }

        public static async Task PeriodicUpdateCheckAsync(CancellationToken cancellationToken)
        {
			while (true)
            {
                // Check for update every 1 hour.
                await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
				await CheckUpdateAsync();
            }
        }

		/// <summary>
		/// Check for update data, but only update translation data.
		/// </summary>
		public static async Task CheckUpdateAsync()
		{
			try
            {
	            Directory.CreateDirectory(TranslationManager.WorkingFolder);

	            var updateFile = TranslationManager.WorkingFolder + $"\\update.json";
				Uri UpdateUrl = new Uri(string.Format("{0}/en-US/update.json", Configuration.Config.Control.UpdateURL));

				using var client = WebRequest.Create(UpdateUrl).GetResponse();
				var updateData = client.GetResponseStream();
				if (updateData != null)
				{
					var json = JsonObject.Parse(updateData);
					LatestVersion = ParseUpdate(json);
				}

				if (File.Exists(updateFile) == false && updateData != null)
				{
					using var file = File.Create(updateFile);
					updateData.CopyTo(file);
					CurrentVersion = new UpdateData();
				}
				else
				{
					var fileContent = File.ReadAllText(updateFile);
					CurrentVersion = ParseUpdate(JsonObject.Parse(fileContent));
				}

				/*if (Configuration.Config.Life.CheckUpdateInformation == true && SoftwareInformation.UpdateTime < LatestVersion.BuildDate)
				{
					FormMain.Instance.Update_Available(LatestVersion.AppVersion);
					UpdateSoftware();
				}*/

				var downloadList = new List<string>();
				var needReload = false;

				if (CurrentVersion.Equipment != LatestVersion.Equipment)
					downloadList.Add("equipment.json");
				if (CurrentVersion.Expedition != LatestVersion.Expedition)
					downloadList.Add("expedition.json");
				if (CurrentVersion.Destination != LatestVersion.Destination)
					downloadList.Add("destination.json");
				if (CurrentVersion.Operation != LatestVersion.Operation)
					downloadList.Add("operation.json");
				if (CurrentVersion.Quest != LatestVersion.Quest)
					downloadList.Add("quest.json");
				if (CurrentVersion.Ship != LatestVersion.Ship)
					downloadList.Add("ship.json");

				if (downloadList.Count > 0)
					needReload = true;
				downloadList.Add("update.json");

				var taskList = new List<Task>();
				var filenames = downloadList.ToArray();
				foreach (var filename in filenames)
				{
					taskList.Add(Task.Run(() => DownloadData(filename)));
				}

				await Task.WhenAll(taskList);
				if (needReload)
				{
					KCDatabase.Instance.Translation.Initialize();
					Logger.Add(2, "Translation files updated.");
				}

				CurrentVersion = LatestVersion;
			}
            catch (Exception e)
            {
                Logger.Add(3, "Failed to obtain update data. " + e);
            }
		}

		private static void DownloadUpdater()
        {
            try
            {
	            using var client = new WebClient();
	            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
	            var url = @"https://github.com/gre4bee/ryuukitsune.github.io/raw/master/Translations/en-US/EOUpdater.exe";
	            var updaterFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\EOUpdater.exe";

	            client.DownloadFile(url, updaterFile);
	            Logger.Add(1, "Updater download finished.");
            }
            catch (Exception e)
            {
                Logger.Add(3, "Failed to download updater. " + e);
                throw;
            }
        }

        private static void DownloadUpdate(string url)
        {
            try
            {
	            using var client = new WebClient();
	            string tempFile = AppDataFolder + @"\latest.zip"; ;
	            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
	            Console.WriteLine("Downloading update...");
	            client.DownloadFile(url, tempFile);
            }
            catch (Exception e)
            {
	            Logger.Add(3, "Failed to download EO. " + e.Message);
				throw;
            }
        }

		internal static UpdateData ParseUpdate(dynamic json)
		{
			var data = new UpdateData();
			try
			{
				DateTime buildDate = DateTimeHelper.CSVStringToTime(json.bld_date);
				var appVersion = (string)json.ver;
				var note = (string)json.note;
				var downloadUrl = (string)json.url;

				var eqVersion = (string)json.tl_ver.equipment;
				var expedVersion = (string)json.tl_ver.expedition;
				string destVersion = json.tl_ver.nodes.ToString();
				var opVersion = (string)json.tl_ver.operation;
				var questVersion = (string)json.tl_ver.quest;
				var shipVersion = (string)json.tl_ver.ship;

				DateTime maintenanceDate = DateTimeHelper.CSVStringToTime(json.kancolle_mt);
				var eventState = (MaintenanceState) (int)json.event_state;

				data = new UpdateData
				{
					BuildDate = buildDate,
					AppVersion = appVersion,
					Note = note,
					AppDownloadUrl = downloadUrl,
					Equipment = eqVersion,
					Expedition = expedVersion,
					Destination = destVersion,
					Operation = opVersion,
					Quest = questVersion,
					Ship = shipVersion,
					MaintenanceDate = maintenanceDate,
					EventState = eventState
				};
			}
			catch (Exception e)
			{
				Logger.Add(3, "Failed to parse update data: " + e.ToString());
			}
			return data;
		}

		internal static async Task DownloadData(string filename)
		{
			var path = TranslationManager.WorkingFolder + $"\\{filename}";
			var url = Configuration.Config.Control.UpdateURL.AbsoluteUri + "en-US/" + $"{filename}";
			try
            {
				using var client = new WebClient();
				await client.DownloadFileTaskAsync(new Uri(url), path);
				if (filename.Contains("update.json") == false)
					Logger.Add(1, $"File {filename} updated.");
			}
            catch (Exception e)
            {
                Logger.Add(3, $"Failed to update {filename} data. " + e.Message);
            }
        }
    }

	public class UpdateData
	{
		public DateTime BuildDate { get; set; }
		public string AppVersion { get; set; } = "0.0.0.0";
		public string Note { get; set; } = "";
		public string AppDownloadUrl { get; set; } = "";
		public string Equipment { get; set; } = "";
		public string Expedition { get; set; } = "";
		public string Destination { get; set; } = "";
		public string Operation { get; set; } = "";
		public string Quest { get; set; } = "";
		public string Ship { get; set; } = "";
		public DateTime MaintenanceDate { get; set; }

		/// <summary>
		/// 1=event start, 2=event end, 3=regular maintenance
		/// </summary>
		public MaintenanceState EventState { get; set; }
}
	public enum MaintenanceState { None = 0, EventStart = 1, EventEnd = 2, Regular = 3 };
}
