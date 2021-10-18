using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

[Union(0, typeof(BossKillTaskModel))]
[Union(1, typeof(ExpeditionTaskModel))]
public interface IQuestTask
{
	int Progress { get; set; }
	int Count { get; }
}
