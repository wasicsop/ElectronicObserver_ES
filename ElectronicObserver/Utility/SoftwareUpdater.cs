using System;
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
        internal static readonly string AppDataFolder =
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\\ElectronicObserver";

		internal static string MaintDate { get; set; } = string.Empty;
        internal static int MaintState { get; set; }

        private static string UpdateFileUrl = string.Empty;
        private static bool isChecked;
        private static bool waitForRestart;

        public static void UpdateSoftware()
        {
            if (waitForRestart)
                return;
            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);

            UpdateCheck();

            if (UpdateFileUrl != string.Empty)
            {
                Logger.Add(1, string.Format("Started downloading update. {0}", UpdateFileUrl));
                DownloadUpdate(UpdateFileUrl);
                Logger.Add(1, "Download finished.");
            }

            DownloadUpdater();
            var updaterFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\EOUpdater.exe";
            if (!File.Exists(updaterFile)) return;
            var updater = new Process
            {
                StartInfo =
                {
                    FileName = updaterFile,
                    UseShellExecute = false,
                    CreateNoWindow = false
                }
            };
            updater.StartInfo.Arguments = "--restart";
            updater.Start();
            Logger.Add(2, "Close Electronic Observer to complete the update process. It will restart automatically.");
            waitForRestart = true;
        }

        public static async Task PeriodicUpdateCheckAsync(CancellationToken cancellationToken)
        {
	        if (!Configuration.Config.Life.CheckUpdateInformation) return;

			while (true)
            {
                // Check for update every 1 hour.
                await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
                UpdateCheck();
            }
        }

        public static void UpdateCheck()
		{
			if (isChecked) return;
			try
            {
				Uri UpdateUrl = new Uri(string.Format("{0}/en-US/update.json", Configuration.Config.Control.UpdateURL));
				using (var client = WebRequest.Create(UpdateUrl).GetResponse())
                {
                    var updateData = client.GetResponseStream();
                    var json = JsonObject.Parse(updateData);
                    DateTime date = DateTimeHelper.CSVStringToTime(json.bld_date);

                    if (SoftwareInformation.UpdateTime < date)
                    {
                        FormMain.Instance.Update_Available(json.ver);
                        UpdateSoftware();
                    }

                    UpdateFileUrl = json.url;
                    //DownloadHash = json.hash;
                    MaintDate = json.kancolle_mt;
                    MaintState = (int)json.event_state;

					foreach (string filename in Enum.GetNames(typeof(TranslationManager.TranslationFile)))
					{
						// Temporary workaround for phasing out nodes.json
						if (filename == "destination")
						{
							var str = "nodes";
							CheckDataVersion(filename, json["tl_ver"][str].ToString());
							continue;
						}
						CheckDataVersion(filename, (string)json["tl_ver"][filename]);
					}
				}

            }
            catch (Exception e)
            {
                Logger.Add(3, "Failed to obtain update data. " + e);
            }
			isChecked = true;
		}

		private static void DownloadUpdater()
        {
            try
            {
                using (var client = new WebClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var url = @"https://github.com/gre4bee/ryuukitsune.github.io/raw/develop/Translations/en-US/EOUpdater.exe";
                    var updaterFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\EOUpdater.exe";

                    client.DownloadFile(url, updaterFile);
                    Logger.Add(1, "Updater download finished.");
                }
            }
            catch (Exception e)
            {
                Logger.Add(3, "Failed to download updater. " + e);
            }
        }

        private static void DownloadUpdate(string url)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string tempFile = AppDataFolder + @"\latest.zip"; ;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    Console.WriteLine("Downloading update...");
                    client.DownloadFile(url, tempFile);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public static void CheckDataVersion(string filename, string latestVersion)
        {
			var needUpdate = false;
            var path = TranslationManager.WorkingFolder + $"\\{filename}.json";
			if (File.Exists(path) == false)
			{
				needUpdate = true;
			}
			else
			{
				try
				{
					using var sr = new StreamReader(path);
					var json = JsonObject.Parse(sr.ReadToEnd());
					var fileVersion = (string)json.version;
					Logger.Add(1, $"Checked {filename}.json file version (v{fileVersion}, latest: v{latestVersion})");
					if (fileVersion != latestVersion)
					{
						needUpdate = true;
					}
				}
				catch (Exception e)
				{
					Logger.Add(3, "Error checking translation data. " + e);
				}
			}
            if (needUpdate)
			{
				DownloadData(filename, path);
			}
        }

        private static string GetHash(string filename)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "");
                }
            }
        }


        internal static void DownloadData(string filename, string path)
		{
			var url = Configuration.Config.Control.UpdateURL.AbsoluteUri + "en-US/" + $"{filename}";
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(new Uri(url), path);
                    Logger.Add(2, $"File {filename} updated.");
                }
            }
            catch (Exception e)
            {
                Logger.Add(3, $"Failed to update {filename} data. " + e.Message);
            }
        }
    }
}
