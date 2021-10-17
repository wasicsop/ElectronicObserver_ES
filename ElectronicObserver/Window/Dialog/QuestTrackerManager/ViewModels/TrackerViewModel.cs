using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;
using ElectronicObserverTypes;
using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels;

public partial class TrackerViewModel : ObservableObject
{
	public IEnumerable<TrackableTaskType> TaskTypes { get; }
	public TrackableTaskType TaskTypeType { get; set; } = TrackableTaskType.BossKill;

	public IEnumerable<IQuestTaskViewModel> Tasks { get; set; }
	public GroupConditionViewModel GroupConditions { get; }

	public bool ShowDetails { get; set; }

	public TrackerModel Model { get; }

	public double Progress => Model.Tasks.Count switch
	{
		> 0 => Model.Tasks.Average(t => (double)t.Progress / t.Count),
		_ => 0
	};
	public string Display => string.Join(" \n", Tasks.Select(t => t.Display));

	public string? ProgressDisplay =>
		string.Join(" \n", Tasks.Select(t => t.Progress).Where(p => !string.IsNullOrEmpty(p))) switch
		{
			null or "" => null,
			string s => s
		};
	public string ClearCondition => string.Join(" \n", Tasks.Select(t => t.ClearCondition));

	public int QuestId => Model.Quest.Id;
	public string Title => Model.Quest.Name;
	public string? Description => Model.Quest.Description;
	public QuestCategory QuestCategory => Model.Quest.Category;
	public int State => Model.Quest.State;

	public TrackerViewModel(TrackerModel model)
	{
		Model = model;
		Tasks = MakeTasks();
		GroupConditions = new(Model.Conditions) { CanBeRemoved = false };

		TaskTypes = Enum.GetValues(typeof(TrackableTaskType)).Cast<TrackableTaskType>();
		Model.PropertyChanged += (_, e) =>
		{
			if (e.PropertyName is not nameof(Model.Quest.Id)) return;

			OnPropertyChanged(nameof(Title));
			OnPropertyChanged(nameof(Description));
		};

		Model.Tasks.CollectionChanged += (_, e) =>
		{
			Tasks = MakeTasks();
			KCDatabase.Instance.Quest.OnQuestUpdated();
		};
	}

	private IEnumerable<IQuestTaskViewModel> MakeTasks()
	{
		return Model.Tasks.Select(t => t switch
		{
			BossKillTaskModel b => (IQuestTaskViewModel)new BossKillTaskViewModel(b),
			ExpeditionTaskModel e => new ExpeditionTask(e),
		});
	}

	[ICommand]
	private void AddTask()
	{
		Model.Tasks.Add(TaskTypeType switch
		{
			TrackableTaskType.BossKill => new BossKillTaskModel(),
			TrackableTaskType.Expedition => new ExpeditionTaskModel(),
		});
	}

	[ICommand]
	private void RemoveTask(IQuestTask? task)
	{
		if (task is null) return;

		Model.Tasks.Remove(task);
	}

	[ICommand]
	private void CopyTracker()
	{
		List<TrackerModel> trackers = new() { Model };
		byte[] data = MessagePackSerializer.Serialize(trackers);
		Clipboard.SetText(MessagePackSerializer.ConvertToJson(data));
	}

	private BattleRank FromString(string rank) => rank.ToUpper() switch
	{
		"E" => BattleRank.E,
		"D" => BattleRank.D,
		"C" => BattleRank.C,
		"B" => BattleRank.B,
		"A" => BattleRank.A,
		"S" => BattleRank.S,
		"SS" => BattleRank.SS,
		_ => BattleRank.Any,
	};

	public void Increment(IFleetData fleet, string resultRank, int compassMapAreaId, int compassMapInfoId)
	{
		if (!GroupConditions.ConditionMet(fleet)) return;

		foreach (BossKillTaskViewModel task in Tasks.OfType<BossKillTaskViewModel>())
		{
			task.Increment(FromString(resultRank), compassMapAreaId, compassMapInfoId);
		}
	}
}