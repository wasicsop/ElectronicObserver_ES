using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormDockViewModel : WinformsHostViewModel
	{
		public FormDockViewModel() : base("Dock", "FormDock",
			ImageSourceIcons.GetIcon(IconContent.FormDock))
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormDock(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}