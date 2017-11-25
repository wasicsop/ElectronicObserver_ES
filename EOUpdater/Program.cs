using Codeplex.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;

namespace EOUpdater
{
	class Program
	{
		private static string _downloadUrl = string.Empty;
		private static string _downloadHash = string.Empty;

		private static readonly string AppDataFolder =
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\ElectronicObserver";

		private static readonly string DestPath =
			Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);

		private static readonly string UpdateFile = AppDataFolder + @"\latest.zip";
		private static readonly string UpdateUrl =
			"http://raw.githubusercontent.com/silfumus/ryuukitsune.github.io/master/Translations/en-US/update.json";

		static void Main(string[] args)
		{
			var wait = true;
			foreach (var argument in args)
			{
				if (argument == "--nowait")
					wait = false;
			}

			if (wait)
			{
				foreach (var process in Process.GetProcessesByName("ElectronicObserver"))
				{
					Console.WriteLine("Waiting Electronic Observer to exit...");
					process.WaitForExit();
				}
				foreach (var process in Process.GetProcessesByName("EOBrowser"))
				{
					Console.WriteLine("Waiting EOBrowser to exit...");
					process.WaitForExit();
				}
			}

			if (!Directory.Exists(AppDataFolder))
				Directory.CreateDirectory(AppDataFolder);

			try
			{
				GetUpdateInfo();
				GetUpdateFile();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Environment.Exit(0);
			}
			
			Console.WriteLine("Start extracting...");
			Extract(UpdateFile, DestPath);
			Console.WriteLine("Extracting finished.");

			foreach (var argument in args)
			{
				if (argument != "--restart") continue;
				var path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
				Process.Start(Path.GetDirectoryName(path) + @"\ElectronicObserver.exe");
			}
		}

		private static void Extract(string zipPath, string extractPath)
		{
			var localPath = new Uri(extractPath).LocalPath;
			using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Read))
			{
				foreach (var file in archive.Entries)
				{
					var fullname = file.FullName.Replace(@"ElectronicObserver/", "");
					var completeFileName = Path.Combine(localPath, fullname);
					var directory = Path.GetDirectoryName(completeFileName);

					if (!Directory.Exists(directory))
						Directory.CreateDirectory(directory);

					if (file.Name == "DynamicJson.dll")
					{
						
					}
					else if(file.Name != "")
					{
						file.ExtractToFile(completeFileName, true);
					}
				}
			}
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

		private static void GetUpdateInfo()
		{
			using (var client = new WebClient())
			{
				var updateData = client.OpenRead(UpdateUrl);
				var json = DynamicJson.Parse(updateData);

				Console.WriteLine("== Update Data==");
				Console.WriteLine(json.bld_date);
				Console.WriteLine(json.ver);
				Console.WriteLine(json.note.Replace("<br>", "\r\n"));
				Console.WriteLine(json.url);
				Console.WriteLine("Hash: " + json.hash);
				Console.WriteLine("==========\r\n");

				_downloadUrl = json.url;
				_downloadHash = json.hash;
			}
		}

		private static void GetUpdateFile()
		{
			if (!File.Exists(UpdateFile))
			{
				Console.WriteLine(UpdateFile + " does not exists.");

				DownloadUpdate(_downloadUrl);
				var fileHash = GetFileHash(UpdateFile);
				Console.WriteLine("File: latest.zip");
				Console.WriteLine("SHA-256: " + fileHash);

			}
			else if (GetFileHash(UpdateFile) != _downloadHash)
			{
				Console.WriteLine("File hash does not match.");
				File.Delete(UpdateFile);
				DownloadUpdate(_downloadUrl);
			}

			Console.WriteLine("File hash matched.");
		}

		private static void DownloadUpdate(string url)
		{
			try
			{
				using (var client = new WebClient())
				{
					Console.WriteLine("Download starting...");
					client.DownloadFile(url, UpdateFile);
					if (File.Exists(UpdateFile))
					{
						Console.WriteLine("Download successful.\r\n");
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

		}
	}
}
