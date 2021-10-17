using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

public enum RemodelComparisonType
{
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "RemodelComparisonType_Any")]
	Any,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "RemodelComparisonType_AtLeast")]
	AtLeast,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "RemodelComparisonType_Exact")]
	Exact
}