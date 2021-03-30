using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.AvalonDockTesting;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver
{
	public class ViewTemplateSelector : DataTemplateSelector
	{
		public DataTemplate? FleetTemplate { get; set; }
		public DataTemplate? WinformsHostTemplate { get; set; }
		public DataTemplate? LogViewTemplate { get; set; }

		public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
		{
			FleetViewModel => FleetTemplate,

			WinformsHostViewModel => WinformsHostTemplate,

			LogViewModel => LogViewTemplate,

			_ => base.SelectTemplate(item, container)
		};
	}
}