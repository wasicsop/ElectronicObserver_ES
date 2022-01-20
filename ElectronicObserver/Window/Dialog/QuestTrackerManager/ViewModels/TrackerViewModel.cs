using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels;

public partial class TrackerViewModel : ObservableObject
{
	public IEnumerable<QuestTaskType> TaskTypes { get; }
	public QuestTaskType TaskTypeType { get; set; } = QuestTaskType.BossKill;

	// when this was an IEnumerable, the call from FormQuest to ProgressDisplay caused
	// exponential NotifyPropertyChange events from the changed value (x2 each time)
	public List<IQuestTaskViewModel> Tasks { get; set; }
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

		TaskTypes = Enum.GetValues(typeof(QuestTaskType)).Cast<QuestTaskType>();
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

	private List<IQuestTaskViewModel> MakeTasks() => Model.Tasks.Select(t => t switch
	{
		BossKillTaskModel b => (IQuestTaskViewModel)new BossKillTaskViewModel(b),
		ExpeditionTaskModel e => new ExpeditionTask(e),
		BattleNodeIdTaskModel b => new BattleNodeIdTaskViewModel(b),
		EquipmentScrapTaskModel e => new EquipmentScrapTaskViewModel(e),
		EquipmentCategoryScrapTaskModel e => new EquipmentCategoryScrapTaskViewModel(e),
		EquipmentCardTypeScrapTaskModel e => new EquipmentCardTypeScrapTaskViewModel(e),
		EquipmentIconTypeScrapTaskModel e => new EquipmentIconTypeScrapTaskViewModel(e),
		NodeReachTaskModel n => new NodeReachTaskViewModel(n),
		MapFirstClearTaskModel m => new MapFirstClearTaskViewModel(m),
	}).ToList();

	[ICommand]
	private void AddTask()
	{
		Model.Tasks.Add(TaskTypeType switch
		{
			QuestTaskType.BossKill => new BossKillTaskModel(),
			QuestTaskType.Expedition => new ExpeditionTaskModel(),
			QuestTaskType.BattleNodeId => new BattleNodeIdTaskModel(),
			QuestTaskType.World7Map2Boss1 => new BattleNodeIdTaskModel
			{
				Map = new(7, 2),
				Name = "-1",
				NodeIds = new List<int> { 7 }
			},
			QuestTaskType.World7Map2Boss2 => new BattleNodeIdTaskModel
			{
				Map = new(7, 2),
				Name = "-2",
				NodeIds = new List<int> { 15 }
			},
			QuestTaskType.World7Map3Boss1 => new BattleNodeIdTaskModel
			{
				Map = new(7, 3),
				Name = "-1",
				NodeIds = new List<int> { 5, 8 },
			},
			QuestTaskType.World7Map3Boss2 => new BattleNodeIdTaskModel
			{
				Map = new(7, 3),
				Name = "-2",
				NodeIds = new List<int> { 18, 23, 24, 25 },
			},
			QuestTaskType.EquipmentScrap => new EquipmentScrapTaskModel(),
			QuestTaskType.EquipmentCategoryScrap => new EquipmentCategoryScrapTaskModel(),
			QuestTaskType.EquipmentCardTypeScrap => new EquipmentCardTypeScrapTaskModel(),
			QuestTaskType.EquipmentIconTypeScrap => new EquipmentIconTypeScrapTaskModel(),
			QuestTaskType.NodeReach => new NodeReachTaskModel(),
			QuestTaskType.MapFirstClear => new MapFirstClearTaskModel(),
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
		Clipboard.SetDataObject(MessagePackSerializer.ConvertToJson(data));
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

	public void Increment(IFleetData fleet, string resultRank, int compassMapAreaId, int compassMapInfoId, int nodeId)
	{
		if (!GroupConditions.ConditionMet(fleet)) return;

		foreach (BattleNodeIdTaskViewModel task in Tasks.OfType<BattleNodeIdTaskViewModel>())
		{
			task.Increment(FromString(resultRank), compassMapAreaId, compassMapInfoId, nodeId);
		}
	}

	public void Increment(IFleetData fleet, string resultRank, int compassMapAreaId, int compassMapInfoId)
	{
		if (!GroupConditions.ConditionMet(fleet)) return;

		foreach (BossKillTaskViewModel task in Tasks.OfType<BossKillTaskViewModel>())
		{
			task.Increment(FromString(resultRank), compassMapAreaId, compassMapInfoId);
		}
	}

	public void Increment(IFleetData fleet, int areaId)
	{
		if (!GroupConditions.ConditionMet(fleet)) return;

		foreach (ExpeditionTask task in Tasks.OfType<ExpeditionTask>())
		{
			task.Increment(areaId);
		}
	}

	public void Increment(IEnumerable<EquipmentId> ids)
	{
		foreach (EquipmentScrapTaskViewModel task in Tasks.OfType<EquipmentScrapTaskViewModel>())
		{
			task.Increment(ids);
		}
	}

	public void Increment(IEnumerable<EquipmentTypes> categories)
	{
		foreach (EquipmentCategoryScrapTaskViewModel task in Tasks.OfType<EquipmentCategoryScrapTaskViewModel>())
		{
			task.Increment(categories);
		}
	}

	public void Increment(IEnumerable<EquipmentCardType> cardTypes)
	{
		foreach (EquipmentCardTypeScrapTaskViewModel task in Tasks.OfType<EquipmentCardTypeScrapTaskViewModel>())
		{
			task.Increment(cardTypes);
		}
	}

	public void Increment(IEnumerable<EquipmentIconType> iconTypes)
	{
		foreach (EquipmentIconTypeScrapTaskViewModel task in Tasks.OfType<EquipmentIconTypeScrapTaskViewModel>())
		{
			task.Increment(iconTypes);
		}
	}

	public void Increment(IFleetData fleet, int compassMapAreaId, int compassMapInfoId, int nodeId)
	{
		if (!GroupConditions.ConditionMet(fleet)) return;

		foreach (NodeReachTaskViewModel task in Tasks.OfType<NodeReachTaskViewModel>())
		{
			task.Increment(compassMapAreaId, compassMapInfoId, nodeId);
		}
	}

	public void Increment(IFleetData fleet, int compassMapAreaId, int compassMapInfoId)
	{
		if (!GroupConditions.ConditionMet(fleet)) return;

		foreach (MapFirstClearTaskViewModel task in Tasks.OfType<MapFirstClearTaskViewModel>())
		{
			task.Increment(compassMapAreaId, compassMapInfoId);
		}
	}

	public void SetProgress(IEnumerable<int> progresses)
	{
		foreach ((IQuestTask task, int progress) in Model.Tasks.Zip(progresses, (t, p) => (t, p)))
		{
			task.Progress = progress;
		}
	}

	public IEnumerable<int> GetProgress()
	{
		return Model.Tasks.Select(t => t.Progress);
	}
}
