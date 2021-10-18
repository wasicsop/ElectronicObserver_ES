using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers;

public class FormBaseAirCorpsViewModel : WinformsHostViewModel
{
	public FormBaseAirCorpsViewModel() : base("AB", "FormBaseAirCorps",
		ImageSourceIcons.GetIcon(IconContent.FormBaseAirCorps))
	{
		// todo remove parameter cause it's never used
		WinformsControl = new FormBaseAirCorps(null!) { TopLevel = false };

		WindowsFormsHost.Child = WinformsControl;
	}
}
