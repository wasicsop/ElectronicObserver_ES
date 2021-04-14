using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Wpf.Battle;
using ElectronicObserver.Window.Wpf.Compass;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using ElectronicObserver.Window.Wpf.FleetOverview;
using ElectronicObserver.Window.Wpf.Headquarters;
using ElectronicObserver.Window.Wpf.ShipGroup.ViewModels;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver
{
	public class ViewTemplateSelector : DataTemplateSelector
	{
		public DataTemplate? FleetTemplate { get; set; }
		public DataTemplate? FleetOverviewTemplate { get; set; }
		public DataTemplate? ShipGroupTemplate { get; set; }
		public DataTemplate? HeadquartersTemplate { get; set; }
		public DataTemplate? CompassTemplate { get; set; }
		public DataTemplate? BattleTemplate { get; set; }


		public DataTemplate? WinformsHostTemplate { get; set; }

		public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
		{
			FleetViewModel => FleetTemplate,
			FleetOverviewViewModel => FleetOverviewTemplate,
			ShipGroupViewModel => ShipGroupTemplate,
			HeadquartersViewModel => HeadquartersTemplate,
			CompassViewModel => CompassTemplate,
			BattleViewModel => BattleTemplate,

			WinformsHostViewModel => WinformsHostTemplate,

			_ => base.SelectTemplate(item, container)
		};
	}
}