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

	public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
	{
		BossKillTaskViewModel => BossKill,
		ExpeditionTask => Expedition,
		BattleNodeIdTaskViewModel => BattleNodeId,
		EquipmentScrapTaskViewModel => EquipmentScrap,

		_ => throw new NotImplementedException(),
	};
}
