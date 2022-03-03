using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

[ObservableObject]
public partial class QuestTrackerManagerViewModel : QuestTrackerManagerBase
{
	public QuestTrackerManagerTranslationViewModel Translation { get; }

	public IEnumerable<QuestModel> Quests { get; }

	public QuestModel? Quest { get; set; }

	public QuestTrackerManagerViewModel()
	{
		Translation = App.Current.Services.GetService<QuestTrackerManagerTranslationViewModel>()!;

		Quests = KCDatabase.Instance.Quest.Quests.Values
			.Select(q => new QuestModel(q.ID));

		SubscribeToApis();
	}

	[ICommand]
	private void CopyTrackersToClipboard()
	{
		List<TrackerModel> trackers = Trackers.Select(t => t.Model).ToList();
		byte[] data = MessagePackSerializer.Serialize(trackers);
		Clipboard.SetDataObject(MessagePackSerializer.ConvertToJson(data));
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

		KCDatabase.Instance.Quest.OnQuestUpdated();
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

		KCDatabase.Instance.Quest.OnQuestUpdated();
	}

	[ICommand]
	private void RemoveTracker(TrackerViewModel? tracker)
	{
		if (tracker is null) return;

		Trackers.Remove(tracker);

		KCDatabase.Instance.Quest.OnQuestUpdated();
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
			byte[] progressBytes = MessagePackSerializer.Serialize(progresses, DateTimeOptions);
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
			string json = File.ReadAllText(ProgressPath);
			byte[] data = MessagePackSerializer.ConvertFromJson(json);
			QuestProgressRecord progress = MessagePackSerializer.Deserialize<QuestProgressRecord>(data, DateTimeOptions);
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
