using System.Windows.Forms.Integration;
using ElectronicObserver.ViewModels;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window.Wpf.WinformsHost
{
	public class WinformsHostViewModel : AnchorableViewModel
	{
		public DockContent? WinformsControl { get; set; }
		public WindowsFormsHost WindowsFormsHost { get; } = new();

		protected WinformsHostViewModel(string title) : this(title, title)
		{
			
		}

		protected WinformsHostViewModel(string title, string contentId) : base(title, contentId)
		{
			PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName is not nameof(WinformsControl)) return;
				if (WinformsControl is null) return;

				WindowsFormsHost.Child = WinformsControl;

				WinformsControl.BackColor = Utility.Configuration.Config.UI.BackColor;
				WinformsControl.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			};
		}
	}
}