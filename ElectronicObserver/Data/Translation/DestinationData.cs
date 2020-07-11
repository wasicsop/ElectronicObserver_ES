using DynaJson;
using ElectronicObserver.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ElectronicObserver.Data.Translation
{
	public class DestinationData : TranslationBase
	{
		private string DefaultFilePath = TranslationManager.WorkingFolder + @"\destination.json";

		private Dictionary<string, string> DestinationList;

		/// <summary>
		/// Destination Node Letter ID (e.g: 1-1-A)
		/// </summary>
		public string DisplayID(int map, int area, int destination)
		{
			var dest = map.ToString() + "-" + area.ToString() + "-" + destination.ToString();
			return IsConverted(dest) ? DestinationList[dest] : destination.ToString();
		}
		public bool IsConverted(string dest)
			=> Configuration.Config.UI.UseOriginalNodeId == false && DestinationList.ContainsKey(dest);

		public override void Initialize()
		{
			DestinationList = LoadDictionary(DefaultFilePath);
		}

		public DestinationData()
		{
			Initialize();
		}

		public Dictionary<string, string> LoadDictionary(string path)
		{
			var dict = new Dictionary<string, string>();

			var json = Load(path);
			if (json == null) return dict;

			foreach (KeyValuePair<string, object> map in json)
			{
				if (map.Key == "version") continue;
				var destinations = JsonObject.Parse(map.Value.ToString());
				foreach (KeyValuePair<string, dynamic> dest in destinations)
				{
					string area = map.Key.Remove(0, 6);
					string destination = $"{area}-{dest.Key}";
					string displayID = dest.Value[1];
					dict.TryAdd(destination, displayID);
				}
			}
			return dict;
		}
	}
}
