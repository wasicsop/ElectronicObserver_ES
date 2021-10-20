using System;
using System.Collections.Generic;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

[MessagePackObject]
public record QuestProgressRecord
(
	[property: Key(0)] DateTime LastQuestListUpdate,
	[property: Key(1)] IEnumerable<QuestTrackerProgressRecord> TrackerProgresses
);
