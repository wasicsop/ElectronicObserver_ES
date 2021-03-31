using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormCompassViewModel : WinformsHostViewModel
	{
		public FormCompassViewModel() : base("Compass")
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormCompass(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}