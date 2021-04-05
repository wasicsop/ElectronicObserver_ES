using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using ElectronicObserver.Window.Wpf.ShipGroup.ViewModels;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver
{
	public class ViewTemplateSelector : DataTemplateSelector
	{
		public DataTemplate? FleetTemplate { get; set; }
		public DataTemplate? ShipGroupTemplate { get; set; }


		public DataTemplate? WinformsHostTemplate { get; set; }

		public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
		{
			FleetViewModel => FleetTemplate,
			ShipGroupViewModel => ShipGroupTemplate,

			WinformsHostViewModel => WinformsHostTemplate,

			_ => base.SelectTemplate(item, container)
		};
	}
}