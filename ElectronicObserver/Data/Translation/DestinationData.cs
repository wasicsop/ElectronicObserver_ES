using DynaJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ElectronicObserver.Data.Translation
{
	public class DestinationData
	{
		public string DefaultFilePath = @"data\edges.json";
		//private readonly string DefaultFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"ElectronicObserver\Translations\nodes.json";

		/// <summary>
		/// Destination Node ID (e.g: 1-1-1)
		/// </summary>
		public string DestinationID { get; set; }

		/// <summary>
		/// Destination Node Letter ID (e.g: 1-1-A)
		/// </summary>
		public string DisplayID(int map, int area, int destination)
		{
			string id = $"{map}-{area}-{destination}";
			string displayID = destination.ToString();
			if (Dict.ContainsKey(id))
			{
				displayID = Dict[id];
			}
			Utility.Logger.Add(3, string.Format("{0}: {1} ", id, displayID));
			return displayID;
		}

		private Dictionary<string, string> Dict { get; set; }

		public DestinationData()
		{
			DestinationID = "";
			Dict = Load(DefaultFilePath);
		}

		private Dictionary<string, string> Load(string path)
		{
			var dict = new Dictionary<string, string>();
			try
			{
				using StreamReader sr = new StreamReader(path);
				var json = JsonObject.Parse(sr.ReadToEnd());
				foreach (KeyValuePair<string, object> world in json)
				{
					var destinations = JsonObject.Parse(world.Value.ToString());
					foreach (KeyValuePair<string, object> destination in destinations)
					{
						var arr = JsonObject.Parse(destination.Value.ToString());
						string destinationID = world.Key.ToString().Remove(0, 6) + "-" + destination.Key.ToString();
						string destinationDisplayID = arr[1].ToString();

						dict.Add(destinationID, destinationDisplayID);
						//Utility.Logger.Add(3, string.Format("{0}: {1} .", destinationID, destinationDisplayID));
					}
				}
			}
			catch (FileNotFoundException)
			{

				Utility.Logger.Add(3, string.Format("{0}: {1} does not exists.", GetType().Name, path));

			}
			catch (DirectoryNotFoundException)
			{

				Utility.Logger.Add(3, string.Format("{0}: {1} does not exists.", GetType().Name, path));

			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, " Failed to load " + GetType().Name);

			}
			return dict;
		}
	}
}
