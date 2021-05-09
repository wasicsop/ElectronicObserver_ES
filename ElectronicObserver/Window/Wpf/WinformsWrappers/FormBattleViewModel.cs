using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormBattleViewModel : WinformsHostViewModel
	{
		public FormBattleViewModel() : base("Battle", "FormBattle",
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormBattle))
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormBattle(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}