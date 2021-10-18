using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

public enum TrackableTaskType
{
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "TrackableTaskType_BossKill")]
	BossKill,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "TrackableTaskType_Expedition")]
	Expedition,
}
