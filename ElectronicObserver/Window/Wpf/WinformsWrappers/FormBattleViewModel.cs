using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormBattleViewModel : WinformsHostViewModel
	{
		public FormBattleViewModel() : base("Battle", "FormBattle")
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormBattle(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}