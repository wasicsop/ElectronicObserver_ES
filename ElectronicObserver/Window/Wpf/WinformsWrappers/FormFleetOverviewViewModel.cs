using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormFleetOverviewViewModel : WinformsHostViewModel
	{
		public FormFleetOverviewViewModel() : base("Fleets", "FormFleets",
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormFleet))
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormFleetOverview(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}