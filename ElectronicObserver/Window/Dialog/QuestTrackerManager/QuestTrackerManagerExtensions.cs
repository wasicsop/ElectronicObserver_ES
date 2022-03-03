using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public static class QuestTrackerManagerExtensions
{
	public static IEnumerable<TrackerModel> SortTrackers(this IEnumerable<TrackerModel> trackers)
	{
		trackers = trackers.OrderBy(t => t.Quest.Id).ToList();

		foreach (TrackerModel tracker in trackers)
		{
			List<IQuestTask?> tasks = tracker.Tasks.ToList();
			tasks.Sort(new TaskComparer());
			tracker.Tasks = new(tasks);
		}

		return trackers;
	}
	public static int TaskOrder(this IQuestTask? task) => task switch
	{
		BossKillTaskModel => 0,
		BattleNodeIdTaskModel => 0,
		NodeReachTaskModel => 2,
		MapFirstClearTaskModel => 3,

		ExerciseTaskModel => 4,

		ExpeditionTaskModel => 5,

		EquipmentScrapTaskModel => 6,
		EquipmentCategoryScrapTaskModel => 7,
		EquipmentCardTypeScrapTaskModel => 8,
		EquipmentIconTypeScrapTaskModel => 9,

		_ => 9999
	};
}
