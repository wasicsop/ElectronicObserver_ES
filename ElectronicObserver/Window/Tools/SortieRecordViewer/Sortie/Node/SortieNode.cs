using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;

public abstract class SortieNode(IKCDatabase kcDatabase, int world, int map, int cell, int eventId, int eventKind)
{
	protected IKCDatabase KcDatabase { get; } = kcDatabase;

	public int World { get; } = world;
	public int Map { get; } = map;
	public int Cell { get; } = cell;
	public BattleBaseAirRaid? AirBaseRaid { get; private set; }
	public ApiOffshoreSupply? ApiOffshoreSupply { get; set; }

	public int ApiEventId { get; set; } = eventId;
	public int ApiEventKind { get; set; } = eventKind;

	public string DisplayCell => KCDatabase.Instance.Translation.Destination.DisplayId(World, Map, Cell);

	public void AddAirBaseRaid(BattleBaseAirRaid abRaid)
	{
		AirBaseRaid = abRaid;
	}

	public virtual void UpdateState(ApiGetMemberShipDeckResponse deck)
	{

	}
}
