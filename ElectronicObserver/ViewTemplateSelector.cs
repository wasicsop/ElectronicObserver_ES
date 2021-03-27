using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.AvalonDockTesting;
using ElectronicObserver.Window.Wpf.BrowserHost;

namespace ElectronicObserver
{
	public class ViewTemplateSelector : DataTemplateSelector
	{
		public DataTemplate? BrowserHostTemplate { get; set; }
		public DataTemplate? LogViewTemplate { get; set; }

		public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
		{
			BrowserHostViewModel => BrowserHostTemplate,

			LogViewModel => LogViewTemplate,

			_ => base.SelectTemplate(item, container)
		};
	}
}