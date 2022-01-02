using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[Union(0, typeof(GroupConditionModel))]
[Union(1, typeof(ShipConditionModel))]
[Union(2, typeof(ShipTypeConditionModel))]
[Union(3, typeof(PartialShipConditionModel))]
[Union(4, typeof(AllowedShipTypesConditionModel))]
[Union(5, typeof(ShipPositionConditionModel))]
public interface ICondition
{
}
