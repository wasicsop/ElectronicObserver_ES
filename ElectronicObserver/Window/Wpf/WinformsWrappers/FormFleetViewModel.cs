using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormFleetViewModel : WinformsHostViewModel
	{
		public FormFleetViewModel(int fleetId) : base($"#{fleetId}", $"FormFleet{fleetId}")
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormFleet(null!, fleetId) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}