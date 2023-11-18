using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;

public class EmptyNode(IKCDatabase kcDatabase, int world, int map, int cell, int eventId, int eventKind)
	: SortieNode(kcDatabase, world, map, cell, eventId, eventKind);
