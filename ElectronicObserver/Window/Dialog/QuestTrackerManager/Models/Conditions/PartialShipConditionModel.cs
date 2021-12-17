using System.Collections.ObjectModel;
using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[MessagePackObject]
public class PartialShipConditionModel : ObservableObject, ICondition
{
	[Key(0)] public ObservableCollection<ShipConditionModel> Conditions { get; set; } = new();
	[Key(1)] public int Count { get; set; }
}
