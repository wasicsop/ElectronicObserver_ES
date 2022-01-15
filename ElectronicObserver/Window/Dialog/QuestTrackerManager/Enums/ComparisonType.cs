using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

public enum ComparisonType
{
	[Display(Name = ">=")]
	GreaterOrEqual,
	[Display(Name = "=")]
	Equal,
	[Display(Name = "<=")]
	LessOrEqual
}
