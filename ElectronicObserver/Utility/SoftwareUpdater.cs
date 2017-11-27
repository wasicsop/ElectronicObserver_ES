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
		private static readonly string UpdateFile = AppDataFolder + @"\\latest.zip";
		private static readonly Uri UpdateUrl =
			new Uri("http://raw.githubusercontent.com/silfumus/ryuukitsune.github.io/master/Translations/en-US/update.json");

		internal static string EqVer { get; set; } = "0.0.0";
		internal static string EqTypeVer { get; set; } = "0.0.0";
		internal static string ExpVer { get; set; } = "0.0.0";
		internal static string OpVer { get; set; } = "0.0.0";
		internal static string QuestVer { get; set; } = "0.0.0";
		internal static string ShipVer { get; set; } = "0.0.0";
		internal static string ShipTypeVer { get; set; } = "0.0.0";

		internal static string ZipUrl = string.Empty;
		internal static string DownloadHash = string.Empty;
		private static bool _isChecked;


		public static void UpdateSoftware()
		{
			if (!Directory.Exists(AppDataFolder))
				Directory.CreateDirectory(AppDataFolder);

			CheckVersion();
			
			var downloadUrl = new Uri(ZipUrl);

			if (!File.Exists(UpdateFile))
				DownloadUpdate(downloadUrl);
			else if (GetFileHash(UpdateFile) != DownloadHash)
			{
				File.Delete(UpdateFile);
				DownloadUpdate(downloadUrl);
			}
			else
			{
				Logger.Add(2, "Close Electronic Observer to complete the update.");
			}

			var destPath =
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			UpdateUpdater(UpdateFile, destPath);

			var updater = new Process
			{
				StartInfo =
				{
					FileName = Application.StartupPath + @"\EOUpdater.exe",
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};
			if (!UpdateRestart)
				updater.StartInfo.Arguments = "--restart";

			updater.Start();
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

		private static string GetFileHash(string filename)
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

		private static void DownloadUpdate(Uri url)
		{
			try
			{
				using (var client = new WebClient())
				{
					Logger.Add(2, "Downloading new version of Electronic Observer...");
					client.DownloadFileCompleted += DownloadComplete;
					client.DownloadFileAsync(url, UpdateFile);
				}
			}
			catch (Exception e)
			{
				Logger.Add(3, "Failed to download update file." + e);
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

		private static void UpdateUpdater(string zipPath, string extractPath)
		{
			var localPath = new Uri(extractPath).LocalPath;
			using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Read))
			{
				foreach (var file in archive.Entries)
				{
					var fullname = file.FullName.Replace(@"ElectronicObserver/", "");
					var completeFileName = Path.Combine(localPath, fullname);

					if (file.Name != "EOUpdater.exe") continue;

					file.ExtractToFile(completeFileName, true);
				}
			}
		}

		private static void DownloadComplete(object sender, EventArgs e)
		{
			Logger.Add(2, "Download complete. Close Electronic Observer to complete the update.");
		}
	}
}
