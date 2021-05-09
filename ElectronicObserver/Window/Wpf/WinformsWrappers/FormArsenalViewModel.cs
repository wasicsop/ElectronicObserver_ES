using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormArsenalViewModel : WinformsHostViewModel
	{
		public FormArsenalViewModel() : base("Arsenal", "FormArsenal",
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormArsenal))
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormArsenal(null!) {TopLevel = false};

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}