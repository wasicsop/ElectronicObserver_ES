using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;

[MessagePackObject]
public record MapInfoModel([property: Key(0)] int AreaId, [property: Key(1)] int InfoId)
{
	[IgnoreMember] public string Display => $"{AreaId}-{InfoId}";
}