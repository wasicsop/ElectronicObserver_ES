using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.AvalonDockTesting;
using ElectronicObserver.Window.Wpf.BrowserHost;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;

namespace ElectronicObserver
{
	public class ViewTemplateSelector : DataTemplateSelector
	{
		public DataTemplate? FleetTemplate { get; set; }
		public DataTemplate? BrowserHostTemplate { get; set; }
		public DataTemplate? LogViewTemplate { get; set; }

		public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
		{
			FleetViewModel => FleetTemplate,
			BrowserHostViewModel => BrowserHostTemplate,

			LogViewModel => LogViewTemplate,

			_ => base.SelectTemplate(item, container)
		};
	}
}