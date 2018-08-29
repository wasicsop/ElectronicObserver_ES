using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;
using Codeplex.Data;
using AppSettings = ElectronicObserver.Properties.Settings;

namespace ElectronicObserver.Utility
{
	internal class SoftwareUpdater
	{
		public static bool UpdateRestart = false;
		
		internal static readonly string AppDataFolder =
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\\ElectronicObserver";
		internal static readonly string TranslationFolder = AppDataFolder + "\\Translations";
		private static readonly Uri UpdateUrl =
			new Uri("https://raw.githubusercontent.com/silfumus/ryuukitsune.github.io/master/Translations/en-US/update.json");

		internal static string EqVer { get; set; } = "0.0.0";
		internal static string EqTypeVer { get; set; } = "0.0.0";
		internal static string ExpVer { get; set; } = "0.0.0";
		internal static string OpVer { get; set; } = "0.0.0";
		internal static string QuestVer { get; set; } = "0.0.0";
		internal static string ShipVer { get; set; } = "0.0.0";
		internal static string ShipTypeVer { get; set; } = "0.0.0";
		internal static string MaintDate { get; set; } = string.Empty;
		internal static int MaintState { get; set; }

		internal static string ZipUrl = string.Empty;
		internal static string DownloadHash = string.Empty;
		private static bool _isChecked;


		public static void UpdateSoftware()
		{
			if (!Directory.Exists(AppDataFolder))
				Directory.CreateDirectory(AppDataFolder);

			CheckVersion();
			DownloadUpdater();

			var updaterFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\EOUpdater.exe";
			if (!File.Exists(updaterFile))
			{
				Logger.Add(2, "Updater started. Close EO after it has finished downloading the update.");
			}
			var updater = new Process
			{
				StartInfo =
				{
					FileName = updaterFile,
					UseShellExecute = false,
					CreateNoWindow = false
		}
			};
			if (!UpdateRestart)
				updater.StartInfo.Arguments = "--restart";

			updater.Start();
			Logger.Add(2, "Updater started. Close EO after it has finished downloading the update.");
		}

		private static void DownloadUpdater()
		{
			try
			{
				using (var client = new WebClient())
				{
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
					var url = @"https://github.com/silfumus/ryuukitsune.github.io/raw/develop/Translations/en-US/EOUpdater.exe";
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

		internal static void CheckVersion()
		{
			if (_isChecked) return;
			try
			{
				using (var client = WebRequest.Create(UpdateUrl).GetResponse())
				{
					var updateData = client.GetResponseStream();
					var json = DynamicJson.Parse(updateData);

					ZipUrl = json.url;
					DownloadHash = json.hash;
					MaintDate = json.kancolle_mt;
					MaintState = (int)json.event_state;

					EqVer = json.tl_ver.equipment;
					EqTypeVer = json.tl_ver.equipment_type;
					ExpVer = json.tl_ver.expedition;
					OpVer = json.tl_ver.operation;
					QuestVer = json.tl_ver.quest;
					ShipVer = json.tl_ver.ship;
					ShipTypeVer = json.tl_ver.ship_type;

				}
			}
			catch (Exception e)
			{
				Logger.Add(3, "Failed to download update info. " + e);
			}

			_isChecked = true;
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

		internal static void DownloadTranslation(TranslationFile filename, string latestVersion)
		{
			var url = AppSettings.Default.EOTranslations.AbsoluteUri + "en-US";
			try
			{
				var r2 = WebRequest.Create(url + $"/{filename}.xml");
				using (var resp = r2.GetResponse())
				{
					var doc = XDocument.Load(resp.GetResponseStream());
					doc.Save(TranslationFolder + $"\\{filename}.xml");
				}
				Logger.Add(2, $"Updated {filename} translations to v{latestVersion}.");
			}
			catch (Exception e)
			{
				Logger.Add(3, $"Failed to download {filename}.xml. " + e.Message);
			}

		}
	}
}
