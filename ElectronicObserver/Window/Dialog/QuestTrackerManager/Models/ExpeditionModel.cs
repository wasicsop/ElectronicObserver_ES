using ElectronicObserver.Data;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;

[MessagePackObject]
public record ExpeditionModel([property: Key(0)] int MissionId)
{
	[IgnoreMember] public string NameEN => KCDatabase.Instance.Mission[MissionId].NameEN;
	[IgnoreMember] public string DisplayId => KCDatabase.Instance.Mission[MissionId].DisplayID;
	[IgnoreMember] public string Display => $"{DisplayId}";
}