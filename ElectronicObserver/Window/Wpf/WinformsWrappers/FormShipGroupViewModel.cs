using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormShipGroupViewModel : WinformsHostViewModel
	{
		public FormShipGroupViewModel() : base("Group", "FormShipGroup",
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormShipGroup))
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormShipGroup(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}