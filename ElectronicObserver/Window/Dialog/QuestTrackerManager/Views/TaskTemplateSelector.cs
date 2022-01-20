using System;
using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public class TaskTemplateSelector : DataTemplateSelector
{
	public DataTemplate? BossKill { get; set; }
	public DataTemplate? Expedition { get; set; }
	public DataTemplate? BattleNodeId { get; set; }
	public DataTemplate? EquipmentScrap { get; set; }
	public DataTemplate? EquipmentCategoryScrap { get; set; }
	public DataTemplate? EquipmentCardTypeScrap { get; set; }
	public DataTemplate? EquipmentIconTypeScrap { get; set; }
	public DataTemplate? NodeReach { get; set; }
	public DataTemplate? MapFirstClear { get; set; }

	public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
	{
		BossKillTaskViewModel => BossKill,
		ExpeditionTask => Expedition,
		BattleNodeIdTaskViewModel => BattleNodeId,
		EquipmentScrapTaskViewModel => EquipmentScrap,
		EquipmentCategoryScrapTaskViewModel => EquipmentCategoryScrap,
		EquipmentCardTypeScrapTaskViewModel => EquipmentCardTypeScrap,
		EquipmentIconTypeScrapTaskViewModel => EquipmentIconTypeScrap,
		NodeReachTaskViewModel => NodeReach,
		MapFirstClearTaskViewModel => MapFirstClear,

		_ => throw new NotImplementedException(),
	};
}
