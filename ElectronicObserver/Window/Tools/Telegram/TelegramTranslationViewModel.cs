using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.Telegram;

public class TelegramTranslationViewModel : TranslationBaseViewModel
{
	public string Title => TelegramResources.Title;

	public string Search => SortieRecordViewer.SortieRecordViewer.Search;
}
