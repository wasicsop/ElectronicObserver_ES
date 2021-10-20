using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public partial class QuestTrackerManagerViewModel : ObservableObject
{
	public QuestTrackerManagerTranslationViewModel Translation { get; }

	public IEnumerable<QuestModel> Quests { get; }
	public ObservableCollection<TrackerViewModel> Trackers { get; } = new();

	public QuestModel? Quest { get; set; }

	private DateTime LastQuestListUpdate { get; set; } = new(2000, 1, 1);

	public QuestTrackerManagerViewModel()
	{
		Translation = App.Current.Services.GetService<QuestTrackerManagerTranslationViewModel>()!;

		Quests = KCDatabase.Instance.Quest.Quests.Values
			.Select(q => new QuestModel(q.ID));

		SubscribeToApis();
	}

	private void SubscribeToApis()
	{
		var ao = APIObserver.Instance;

		ao.APIList["api_get_member/questlist"].ResponseReceived += QuestUpdated;

		ao.APIList["api_req_sortie/battleresult"].ResponseReceived += BattleFinished;
		ao.APIList["api_req_combined_battle/battleresult"].ResponseReceived += BattleFinished;

		ao.APIList["api_req_mission/result"].ResponseReceived += ExpeditionCompleted;
	}

	private void QuestUpdated(string apiname, dynamic data)
	{
		QuestManager quests = KCDatabase.Instance.Quest;

		bool ShouldQuestReset(QuestModel quest) => quest.ResetType switch
		{
			QuestResetType.Daily => DateTimeHelper.IsCrossedDailyQuestReset(LastQuestListUpdate),
			QuestResetType.Weekly => DateTimeHelper.IsCrossedWeeklyQuestReset(LastQuestListUpdate),
			QuestResetType.Monthly => DateTimeHelper.IsCrossedMonthlyQuestReset(LastQuestListUpdate),
			QuestResetType.Quarterly => DateTimeHelper.IsCrossedQuarterlyQuestReset(LastQuestListUpdate),

			QuestResetType.January => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 1),
			QuestResetType.February => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 2),
			QuestResetType.March => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 3),
			QuestResetType.April => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 4),
			QuestResetType.May => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 5),
			QuestResetType.June => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 6),
			QuestResetType.July => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 7),
			QuestResetType.August => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 8),
			QuestResetType.September => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 9),
			QuestResetType.October => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 10),
			QuestResetType.November => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 11),
			QuestResetType.December => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 12),

			_ => false
		};

		//消えている・達成済みの任務の進捗情報を削除
		if (!quests.IsLoadCompleted) return;

		IEnumerable<TrackerViewModel> trackersToReset = Trackers
			.Where(t => !quests.Quests.ContainsKey(t.QuestId) || ShouldQuestReset(t.Model.Quest));

		foreach (TrackerViewModel tracker in trackersToReset)
		{
			foreach (IQuestTask task in tracker.Model.Tasks)
			{
				task.Progress = 0;
			}
		}

		LastQuestListUpdate = DateTime.Now;
	}

	private void BattleFinished(string apiname, dynamic data)
	{
		var bm = KCDatabase.Instance.Battle;
		var battle = bm.SecondBattle ?? bm.FirstBattle;
		var hps = battle.ResultHPs;
		var fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (hps is null) return;
		if (bm.Compass.EventID != 5) return;
		if (fleet is null) return;

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(fleet, bm.Result.Rank, bm.Compass.MapAreaID, bm.Compass.MapInfoID);
		}

		// p.Increment(bm.Result.Rank, bm.Compass.MapAreaID * 10 + bm.Compass.MapInfoID, bm.Compass.EventID == 5);

	}

	void ExpeditionCompleted(string apiname, dynamic data)
	{
		// 遠征失敗
		if ((int)data.api_clear_result == 0) return;

		FleetData? fleet = KCDatabase.Instance.Fleet.Fleets.Values
			.FirstOrDefault(f => f.Members.Contains((int)data.api_ship_id[1]));

		if (fleet == null) return;

		int areaId = fleet.ExpeditionDestination;

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(fleet, areaId);
		}
	}

	[ICommand]
	private void CopyTrackersToClipboard()
	{
		List<TrackerModel> trackers = Trackers.Select(t => t.Model).ToList();
		byte[] data = MessagePackSerializer.Serialize(trackers);
		Clipboard.SetText(MessagePackSerializer.ConvertToJson(data));
	}

	[ICommand]
	private void LoadTrackerFromClipboard()
	{
		try
		{
			string json = Clipboard.GetText();
			byte[] data = MessagePackSerializer.ConvertFromJson(json);
			List<TrackerModel> trackers = MessagePackSerializer.Deserialize<List<TrackerModel>>(data);
			MergeTrackers(trackers);
		}
		catch
		{
			// ignored
		}
	}

	private void MergeTrackers(List<TrackerModel> trackers)
	{
		IEnumerable<int> existingQuestIds = Trackers.Select(t => t.QuestId);
		List<TrackerModel> clashingTrackers = trackers.Where(t => existingQuestIds.Contains(t.Quest.Id)).ToList();

		foreach (TrackerModel tracker in trackers.Where(t => !existingQuestIds.Contains(t.Quest.Id)))
		{
			Trackers.Add(new TrackerViewModel(tracker));
		}

		if (clashingTrackers.Any())
		{
			string warning = string.Format(Translation.TrackerOverwrite, string.Join("\n", Trackers
				.Where(t => clashingTrackers.Select(x => x.Quest.Id).Contains(t.QuestId))
				.Select(t => t.Model.Quest.Display)));

			if (MessageBox.Show(warning, Translation.Warning, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				foreach (TrackerModel tracker in clashingTrackers)
				{
					Trackers.Remove(Trackers.First(t => t.QuestId == tracker.Quest.Id));
					Trackers.Add(new TrackerViewModel(tracker));
				}
			}
		}
	}

	[ICommand]
	private void AddTracker()
	{
		if (Quest is null) return;
		if (Trackers.Select(t => t.QuestId).Contains(Quest.Id)) return;

		TrackerViewModel tracker = new(new TrackerModel(Quest))
		{
			ShowDetails = true
		};

		Trackers.Add(tracker);
	}

	[ICommand]
	private void RemoveTracker(TrackerViewModel? tracker)
	{
		if (tracker is null) return;

		Trackers.Remove(tracker);
	}

	public void Save()
	{
		SaveExistingTrackers();
		SaveProgress();
	}

	public void Load()
	{
		LoadExistingTrackers();
		LoadProgress();
	}

	private string CustomTrackerPath => Path.Combine("Record", "QuestTrackers.json");
	private string ProgressPath => Path.Combine("Record", "QuestProgress.json");

	private void SaveExistingTrackers()
	{
		try
		{
			IEnumerable<TrackerModel> trackers = Trackers.Select(t => t.Model);
			byte[] trackerBytes = MessagePackSerializer.Serialize(trackers);
			File.WriteAllText(CustomTrackerPath, MessagePackSerializer.ConvertToJson(trackerBytes));
		}
		catch
		{
		}
	}

	private void LoadExistingTrackers()
	{
		if (!File.Exists(CustomTrackerPath)) return;

		try
		{
			byte[] data = MessagePackSerializer.ConvertFromJson(File.ReadAllText(CustomTrackerPath));
			List<TrackerModel> trackers = MessagePackSerializer.Deserialize<List<TrackerModel>>(data);
			MergeTrackers(trackers);
		}
		catch
		{

		}
	}

	private void SaveProgress()
	{
		try
		{
			QuestProgressRecord progresses = new(LastQuestListUpdate, 
				Trackers.Select(t => new QuestTrackerProgressRecord(t.QuestId, t.GetProgress())));
			byte[] progressBytes = MessagePackSerializer.Serialize(progresses);
			File.WriteAllText(ProgressPath, MessagePackSerializer.ConvertToJson(progressBytes));
		}
		catch
		{
		}
	}

	private void LoadProgress()
	{
		if (!File.Exists(ProgressPath)) return;

		try
		{
			byte[] data = MessagePackSerializer.ConvertFromJson(File.ReadAllText(ProgressPath));
			QuestProgressRecord progress = MessagePackSerializer.Deserialize<QuestProgressRecord>(data);
			foreach ((int questId, IEnumerable<int> progresses) in progress.TrackerProgresses)
			{
				Trackers.FirstOrDefault(t => t.QuestId == questId)?.SetProgress(progresses);
			}

			LastQuestListUpdate = progress.LastQuestListUpdate;
		}
		catch
		{

		}
	}
}
