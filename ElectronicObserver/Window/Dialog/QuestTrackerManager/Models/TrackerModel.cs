using System.Collections.ObjectModel;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;
using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;

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
}
