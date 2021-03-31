using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormFleetPresetViewModel : WinformsHostViewModel
	{
		public FormFleetPresetViewModel() : base("Presets", "FormFleetPreset")
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormFleetPreset(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}