using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormBaseAirCorpsViewModel : WinformsHostViewModel
	{
		public FormBaseAirCorpsViewModel() : base("AB", "FormBaseAirCorps")
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormBaseAirCorps(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}