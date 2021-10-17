using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[MessagePackObject]
public class GroupConditionModel : ObservableObject, ICondition
{
	[Key(0)] public Operator GroupOperator { get; set; } = Operator.And;
	[Key(1)] public ObservableCollection<ICondition> Conditions { get; set; } = new();
}