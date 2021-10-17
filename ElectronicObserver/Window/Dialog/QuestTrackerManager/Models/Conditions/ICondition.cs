using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[Union(0, typeof(GroupConditionModel))]
[Union(1, typeof(ShipConditionModel))]
[Union(2, typeof(ShipTypeConditionModel))]
public interface ICondition
{
}