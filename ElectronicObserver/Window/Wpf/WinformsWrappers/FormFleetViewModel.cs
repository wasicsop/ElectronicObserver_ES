using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormFleetViewModel : WinformsHostViewModel
	{
		public FormFleetViewModel(int fleetId) : base($"#{fleetId}", $"FormFleet{fleetId}",
			ImageSourceIcons.GetIcon(IconContent.FormFleet))
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormFleet(null!, fleetId, SetIcon) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}

		private void SetIcon(IconContent icon)
		{
			IconSource = ImageSourceIcons.GetIcon(icon);
		}
	}
}