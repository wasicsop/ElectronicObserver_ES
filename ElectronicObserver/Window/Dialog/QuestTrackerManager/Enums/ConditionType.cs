using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

public enum ConditionType
{
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "ConditionType_Group")]
	Group,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "ConditionType_Ship")]
	Ship,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "ConditionType_ShipType")]
	ShipType
}
