using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormJsonViewModel : WinformsHostViewModel
	{
		public FormJsonViewModel() : base("JSON", "FormJson",
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormJson))
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormJson(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}