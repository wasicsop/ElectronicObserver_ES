using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;
using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;

[MessagePackObject]
public class TrackerModel : ObservableObject
{
	[Key(0)] public QuestModel Quest { get; set; }
	[Key(1)] public ObservableCollection<IQuestTask> Tasks { get; set; }
	[Key(2)] public GroupConditionModel Conditions { get; set; }

	public TrackerModel(QuestModel quest, ObservableCollection<IQuestTask>? tasks = null,
		GroupConditionModel? conditions = null)
	{
		Quest = quest;
		Tasks = tasks ?? new();
		Conditions = conditions ?? new();
	}

	public void SetProgress(IEnumerable<int> progresses)
	{
		foreach ((IQuestTask task, int progress) in Tasks.Zip(progresses, (t, p) => (t, p)))
		{
			task.Progress = progress;
		}
	}

	public IEnumerable<int> GetProgress()
	{
		return Tasks.Select(t => t.Progress);
	}
}