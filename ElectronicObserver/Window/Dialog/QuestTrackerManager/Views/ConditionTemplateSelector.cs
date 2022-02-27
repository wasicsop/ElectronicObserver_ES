using System;
using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public class ConditionTemplateSelector : DataTemplateSelector
{
	public DataTemplate? Group { get; set; }
	public DataTemplate? ShipType { get; set; }
	public DataTemplate? Ship { get; set; }
	public DataTemplate? PartialShip { get; set; }
	public DataTemplate? AllowedShipTypes { get; set; }
	public DataTemplate? ShipPosition { get; set; }
	public DataTemplate? ShipNationality { get; set; }
	public DataTemplate? Unknown { get; set; }

	public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
	{
		GroupConditionViewModel => Group,
		ShipTypeConditionViewModel => ShipType,
		ShipConditionViewModel => Ship,
		PartialShipConditionViewModel => PartialShip,
		AllowedShipTypesConditionViewModel => AllowedShipTypes,
		ShipPositionConditionViewModel => ShipPosition,
		ShipNationalityConditionViewModel => ShipNationality,

		_ => Unknown
	};
}
