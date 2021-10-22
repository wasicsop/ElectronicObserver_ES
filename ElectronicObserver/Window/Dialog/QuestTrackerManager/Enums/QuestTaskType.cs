using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

public enum QuestTaskType
{
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "QuestTaskType_BossKill")]
	BossKill,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "QuestTaskType_Expedition")]
	Expedition,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "QuestTaskType_BattleNodeId")]
	BattleNodeId,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "QuestTaskType_7_2_1")]
	World7Map2Boss1,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "QuestTaskType_7_2_2")]
	World7Map2Boss2,
}
