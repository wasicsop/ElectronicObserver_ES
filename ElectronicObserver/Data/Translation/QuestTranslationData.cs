using DynaJson;
using ElectronicObserver.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ElectronicObserver.Data.Translation
{
	public class QuestTranslationData : TranslationBase
	{
		private string DefaultFilePath = TranslationManager.WorkingFolder + @"\quest.json";

		private Dictionary<int, Quests> QuestList;

		public bool IsTranslated(int id) => Configuration.Config.UI.DisableOtherTranslations == false && QuestList.ContainsKey(id);
		public string Name(int id, string rawData) => IsTranslated(id) ? QuestList[id].Name : rawData;
		public string Description(int id, string rawData) => IsTranslated(id) ? QuestList[id].Description : rawData;
		public override void Initialize()
		{
			QuestList = LoadDictionary(DefaultFilePath);
		}

		public QuestTranslationData()
		{
			Initialize();
		}

		public Dictionary<int, Quests> LoadDictionary(string path)
		{
			var dict = new Dictionary<int, Quests>();

			var json = Load(path);
			if (json == null) return dict;

			for (int i = 101; i < 1000; i++)
			{
				if (json.IsDefined(i.ToString()))
				{
					var code = json[i.ToString()]["code"];
					var name = json[i.ToString()]["name"];
					var nameJP = json[i.ToString()]["name_jp"];
					var desc = json[i.ToString()]["desc"];
					var descJP = json[i.ToString()]["desc_jp"];
					var quest = new Quests(code, name, nameJP, desc, descJP);
					dict.TryAdd(i, quest);
				}
			}
			return dict;
		}
	}

	public class Quests
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string NameJP { get; set; }
		public string Description { get; set; }
		public string DescriptionJP { get; set; }
		public Quests (string code, string name, string nameJP, string description, string descriptionJP)
		{
			Code = code;
			Name = name;
			NameJP = nameJP;
			DescriptionJP = descriptionJP;
			Description = description;
		}
	}
	
}
