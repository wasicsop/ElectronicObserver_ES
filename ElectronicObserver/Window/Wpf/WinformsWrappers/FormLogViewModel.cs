using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormLogViewModel : WinformsHostViewModel
	{
		public FormLogViewModel() : base("Log", "FormLog")
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormLog(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}