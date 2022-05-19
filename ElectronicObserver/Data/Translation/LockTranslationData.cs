using System.Collections.Generic;

namespace ElectronicObserver.Data.Translation;

public class LockTranslationData : TranslationBase
{
	private string FilePath = DataAndTranslationManager.TranslationFolder + @"\Locks.json";

	private Dictionary<string, string> LockList { get; set; } = new();

	public LockTranslationData()
	{
		Initialize();
	}

	public override void Initialize()
	{
		LockList = Load<Dictionary<string, string>>(FilePath) ?? new();
	}

	public string Lock(string name)
	{
		if (Utility.Configuration.Config.UI.DisableOtherTranslations) return name;

		if (LockList.TryGetValue(name, out string? translatedName))
		{
			return translatedName;
		}
		
		return name;
	}
}
