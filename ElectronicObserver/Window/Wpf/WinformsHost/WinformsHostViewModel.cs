using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using ElectronicObserver.ViewModels;


namespace ElectronicObserver.Window.Wpf.WinformsHost;

public class WinformsHostViewModel : AnchorableViewModel
{
	public Form? WinformsControl { get; set; }
	public WindowsFormsHost WindowsFormsHost { get; } = new();

	protected WinformsHostViewModel(string title) : this(title, title)
	{

	}

	protected WinformsHostViewModel(string title, string contentId, ImageSource? icon = null)
		: base(title, contentId, icon)
	{
		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(WinformsControl)) return;
			if (WinformsControl is null) return;

			WinformsControl.FormBorderStyle = FormBorderStyle.None;
			WinformsControl.TopLevel = false;
			WindowsFormsHost.Child = WinformsControl;

			WinformsControl.BackColor = Utility.Configuration.Config.UI.BackColor;
			WinformsControl.ForeColor = Utility.Configuration.Config.UI.ForeColor;
		};
	}
}
