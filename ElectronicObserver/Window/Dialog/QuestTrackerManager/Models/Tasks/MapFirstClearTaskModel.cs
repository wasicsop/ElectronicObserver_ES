using System;
using CommunityToolkit.Mvvm.ComponentModel;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

[MessagePackObject]
public class MapFirstClearTaskModel : ObservableObject, IQuestTask
{
	[Key(0)] public MapInfoModel Map { get; set; } = new(1, 1);
	[Key(1)] public int Count { get; set; } = 1;
	[IgnoreMember] public int Progress { get; set; }

	public MapFirstClearTaskModel()
	{
		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(Progress) or nameof(Count))) return;

			Progress = Math.Clamp(Progress, 0, Count);
			Count = Math.Clamp(Count, 1, int.MaxValue);
		};
	}
}
