using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserverTypes;
using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[MessagePackObject]
public class ShipConditionModel : ObservableObject, ICondition
{
	[Key(0)] public ShipId Id { get; set; } = ShipId.Kamikaze;
	[Key(1)] public bool MustBeFlagship { get; set; }
	[Key(2)] public RemodelComparisonType RemodelComparisonType { get; set; } = RemodelComparisonType.Any;
}