using System.Collections.ObjectModel;
using ElectronicObserverTypes;
using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[MessagePackObject]
public class AllowedShipTypesConditionModel : ObservableObject, ICondition
{
	[Key(0)] public ObservableCollection<ShipTypes> Types { get; set; } = new() { ShipTypes.Destroyer };
}
