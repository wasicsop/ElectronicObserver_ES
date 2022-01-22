using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

[MessagePackObject]
public class ExerciseTaskModel : ObservableObject, IQuestTask
{
	[Key(0)] public BattleRank Rank { get; set; } = BattleRank.S;
	[Key(1)] public int Count { get; set; }
	[IgnoreMember] public int Progress { get; set; }
}
