using System.Collections.ObjectModel;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[MessagePackObject]
public class GroupConditionModel : ObservableObject, ICondition
{
	[Key(0)] public Operator GroupOperator { get; set; } = Operator.And;
	// can be null if the deserializer doesn't know about the condition type
	// for example an older version of EO not having a new condition
	[Key(1)] public ObservableCollection<ICondition?> Conditions { get; set; } = new();
}
