using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
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

		ao.APIList["api_req_sortie/battleresult"].ResponseReceived += BattleFinished;
		ao.APIList["api_req_combined_battle/battleresult"].ResponseReceived += BattleFinished;
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

	private string CustomTrackerPath => Path.Combine("Settings", "QuestTrackers.json");
	private string ProgressPath => Path.Combine("Settings", "QuestProgress.json");

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
			IEnumerable<ProgressRecord> progresses = Trackers.Select(t => new ProgressRecord(t.Model.Quest.Id, t.Model.GetProgress()));
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
			IEnumerable<ProgressRecord> progresses = MessagePackSerializer.Deserialize<IEnumerable<ProgressRecord>>(data);
			foreach (ProgressRecord progress in progresses)
			{
				Trackers.FirstOrDefault(t => t.QuestId == progress.QuestId)
					?.Model.SetProgress(progress.Progresses);
			}
		}
		catch
		{

		}
	}
}
