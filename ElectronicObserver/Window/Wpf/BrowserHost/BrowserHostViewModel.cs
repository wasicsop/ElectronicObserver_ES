using System.Windows.Forms.Integration;
using ElectronicObserver.ViewModels;

namespace ElectronicObserver.Window.Wpf.BrowserHost
{
	public class BrowserHostViewModel : AnchorableViewModel
	{
		private FormBrowserHost BrowserForm { get; }

		public WindowsFormsHost WindowsFormsHost { get; }

		public BrowserHostViewModel() : base("Browser")
		{
			WindowsFormsHost = new();

			// todo remove parameter cause it's never used
			BrowserForm = new(null!);
			BrowserForm.TopLevel = false;

			WindowsFormsHost.Child = BrowserForm;
		}
	}
}