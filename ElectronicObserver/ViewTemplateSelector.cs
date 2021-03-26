using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.AvalonDockTesting;

namespace ElectronicObserver
{
	public class ViewTemplateSelector : DataTemplateSelector
	{
		public DataTemplate? LogViewTemplate { get; set; }

		public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
		{
			LogViewModel => LogViewTemplate,

			_ => base.SelectTemplate(item, container)
		};
	}
}