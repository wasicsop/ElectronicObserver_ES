using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

[MessagePackObject]
public class ExpeditionTaskModel : ObservableObject, IQuestTask
{
	[Key(0)] public ExpeditionModel Expedition { get; set; } = new(1);
	[Key(1)] public int Count { get; set; }
	[IgnoreMember] public int Progress { get; set; }
}
