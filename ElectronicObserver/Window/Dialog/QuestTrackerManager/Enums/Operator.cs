using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

public enum Operator
{
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "Operator_And")]
	And,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "Operator_Or")]
	Or
}