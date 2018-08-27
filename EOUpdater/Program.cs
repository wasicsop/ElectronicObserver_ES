using Codeplex.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;

namespace EOUpdater
{
	internal class Program
	{
		private static readonly string AppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"ElectronicObserver");
	    private const string UpdateUrl = @"http://raw.githubusercontent.com/silfumus/ryuukitsune.github.io/master/Translations/en-US/update.json";

	    private static void Main(string[] args)
		{
			var wait = true;
		    var restart = false;

			foreach (var argument in args)
			{
			    switch (argument)
			    {
			        case "--nowait":
			            wait = false;
			            break;
			        case "--restart":
			            restart = true;
			            break;
			        default:
                        Console.WriteLine(argument + "is not a valid argument.");
			            break;
                }
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
                var tempFile = AppDataFolder + @"\latest.zip";
                var destPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);

                GetUpdateInfo(UpdateUrl, out var downloadUrl, out var downloadHash);
				GetUpdateFile(downloadUrl, tempFile, downloadHash);
                Extract(tempFile, destPath);
            }
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

		    if (!restart) return;
		    var appPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"ElectronicObserver.exe");
            Process.Start(appPath);
		}

		private static void Extract(string zipPath, string extractPath)
		{
			var localPath = new Uri(extractPath).LocalPath;
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Read))
			{
				foreach (var file in archive.Entries)
				{
				    Console.WriteLine("Start extracting...");
                    var fullname = file.FullName.Replace(@"ElectronicObserver/", "");
					var completeFileName = Path.Combine(localPath, fullname);
					var directory = Path.GetDirectoryName(completeFileName);

					if (directory != null && !Directory.Exists(directory))
						Directory.CreateDirectory(directory);

					if (file.Name == "DynamicJson.dll" || file.Name == "EOUpdater.exe" || file.Name == "") continue;
					file.ExtractToFile(completeFileName, true);
				    Console.WriteLine("Extracting finished.");
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

		private static void GetUpdateInfo(string url, out string downloadUrl, out string downloadHash)
		{
			using (var client = new WebClient())
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var updateData = client.OpenRead(url);
				var json = DynamicJson.Parse(updateData);

				Console.WriteLine("== Update Data==");
				Console.WriteLine(json.bld_date);
				Console.WriteLine(json.ver);
				Console.WriteLine(json.note.Replace("<br>", "\r\n"));
				Console.WriteLine(json.url);
				Console.WriteLine("Hash: " + json.hash);
				Console.WriteLine("==========\r\n");

			    downloadUrl = json.url;
			    downloadHash = json.hash;
			}
		}

		private static void GetUpdateFile(string downloadUrl, string tempFile, string downloadHash)
		{
			if (!File.Exists(tempFile) || GetFileHash(tempFile) != downloadHash)
                DownloadUpdate(downloadUrl, tempFile);
		    Console.WriteLine("File: latest.zip SHA-256: " + GetFileHash(tempFile));
		}

		private static void DownloadUpdate(string url, string tempFile)
		{
			try
			{
				using (var client = new WebClient())
				{
					Console.WriteLine("Download starting...");
					client.DownloadFile(url, tempFile);
					if (File.Exists(tempFile))
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
