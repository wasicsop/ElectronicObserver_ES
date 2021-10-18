using ElectronicObserver.Window;

namespace ElectronicObserver.ViewModels.Translations;

public class FormQuestTranslationViewModel : TranslationBaseViewModel
{
	public string Title => GeneralRes.Quest;

	public string MenuProgress_Reset => Properties.Window.FormQuest.MenuProgress_Reset.Replace("_", "__").Replace("&", "_");
	public string MenuMain_QuestFilter => Properties.Window.FormQuest.MenuMain_QuestFilter.Replace("_", "__").Replace("&", "_");
	public string MenuMain_GoogleQuest => Properties.Window.FormQuest.LookUpQuestOnWeb.Replace("_", "__").Replace("&", "_");
	public string MenuMain_KcwikiQuest => Properties.Window.FormQuest.MenuMain_KcwikiQuest.Replace("_", "__").Replace("&", "_");
	public string ManuMain_QuestTitle => Properties.Window.FormQuest.ManuMain_QuestTitle.Replace("_", "__").Replace("&", "_");
	public string ManuMain_QuestDescription => Properties.Window.FormQuest.ManuMain_QuestDescription.Replace("_", "__").Replace("&", "_");
	public string ManuMain_QuestTranslate => Properties.Window.FormQuest.ManuMain_QuestTranslate.Replace("_", "__").Replace("&", "_");
}
