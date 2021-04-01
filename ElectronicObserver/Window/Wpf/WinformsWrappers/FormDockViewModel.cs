using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormDockViewModel : WinformsHostViewModel
	{
		public FormDockViewModel() : base("Dock", "FormDock")
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormDock(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}