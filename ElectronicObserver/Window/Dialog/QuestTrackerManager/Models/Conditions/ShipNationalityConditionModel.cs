using System.Collections.ObjectModel;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserverTypes;
using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[MessagePackObject]
public class ShipNationalityConditionModel : ObservableObject, ICondition
{
	[Key(0)] public ObservableCollection<ShipNationality> Nationalities { get; set; } = new();
	[Key(1)] public int Count { get; set; }
	[Key(2)] public bool MustBeFlagship { get; set; }
	[Key(3)] public ComparisonType ComparisonType { get; set; } = ComparisonType.GreaterOrEqual;
}
