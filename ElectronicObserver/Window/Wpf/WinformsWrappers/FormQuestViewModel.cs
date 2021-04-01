using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormQuestViewModel : WinformsHostViewModel
	{
		public FormQuestViewModel() : base("Quests", "FormQuest")
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormQuest(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}