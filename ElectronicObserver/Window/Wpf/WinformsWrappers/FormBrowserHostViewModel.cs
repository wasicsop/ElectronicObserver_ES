using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	// prefix with Form so it doesn't clash with the wpf versions
	public class FormBrowserHostViewModel : WinformsHostViewModel
	{
		public FormBrowserHostViewModel() : base("Browser", "FormBrowser",
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormBrowser))
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormBrowserHost(null!) {TopLevel = false};

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}