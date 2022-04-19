using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;

[MessagePackObject]
public class TrackerModel : ObservableObject
{
	[Key(0)] public QuestModel Quest { get; set; }
	// can be null if the deserializer doesn't know about the task type
	// for example an older version of EO not having a new task
	[Key(1)] public ObservableCollection<IQuestTask?> Tasks { get; set; }
	[Key(2)] public GroupConditionModel Conditions { get; set; }

	public TrackerModel(QuestModel quest, ObservableCollection<IQuestTask?>? tasks = null,
		GroupConditionModel? conditions = null)
	{
		Quest = quest;
		Tasks = tasks ?? new();
		Conditions = conditions ?? new();
	}
}
