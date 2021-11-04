using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

[Union(0, typeof(BossKillTaskModel))]
[Union(1, typeof(ExpeditionTaskModel))]
[Union(2, typeof(BattleNodeIdTaskModel))]
[Union(3, typeof(EquipmentScrapTaskModel))]
public interface IQuestTask
{
	int Progress { get; set; }
	int Count { get; }
}
