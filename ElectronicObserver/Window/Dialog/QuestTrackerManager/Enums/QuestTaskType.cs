using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

public enum QuestTaskType
{
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "QuestTaskType_BossKill")]
	BossKill,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "QuestTaskType_Expedition")]
	Expedition,
}
