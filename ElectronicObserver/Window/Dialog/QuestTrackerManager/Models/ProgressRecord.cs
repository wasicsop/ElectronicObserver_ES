using System.Collections.Generic;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;

[MessagePackObject]
public record ProgressRecord([property: Key(0)] int QuestId, [property: Key(1)] IEnumerable<int> Progresses);
