using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Mapinfo;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class PoiDbRouteSubmissionService(
	KCDatabase kcDatabase,
	string version,
	PoiHttpClient poiHttpClient,
	Action<Exception> logError)
{
	private KCDatabase KcDatabase { get; } = kcDatabase;
	private string Version { get; } = version;
	private PoiHttpClient PoiHttpClient { get; } = poiHttpClient;
	private Action<Exception> LogError { get; } = logError;

	private int? CellCount { get; set; }
	private List<int>? CellIds { get; set; }
	private Dictionary<int, int>? MapLevels { get; set; }
	private int? World { get; set; }
	private int? Map { get; set; }
	private FleetData? Fleet1 { get; set; }
	private FleetData? Fleet2 { get; set; }
	private List<int>? EscapeList { get; set; }

	public void ApiGetMember_MapInfo_ResponseReceived(string apiname, dynamic data)
	{
		string json = data.ToString();
		ApiGetMemberMapinfoResponse response = JsonSerializer.Deserialize<ApiGetMemberMapinfoResponse>(json)!;

		MapLevels = response.ApiMapInfo
			.Where(c => c.ApiEventmap is not null)
			.ToDictionary(i => i.ApiId, i => i.ApiEventmap!.ApiSelectedRank);
	}

	public void ApiReqMap_Start_ResponseReceived(string apiname, dynamic data)
	{
		string json = data.ToString();
		ApiReqMapStartResponse response = JsonSerializer.Deserialize<ApiReqMapStartResponse>(json)!;

		CellCount = response.ApiCellData.Count;
		World = response.ApiMapareaId;
		Map = response.ApiMapinfoNo;
		CellIds = response.ApiCellData
			.Where(c => c.ApiPassed is 1)
			.Select(c => c.ApiNo)
			.ToList();

		FleetData? fleet = KcDatabase.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (fleet is null) return;

		Fleet1 = fleet;
		EscapeList = [];

		bool isCombinedFleetSortie = fleet.FleetID is 1 &&
			KcDatabase.Fleet.CombinedFlag is not FleetType.Single;

		if (isCombinedFleetSortie)
		{
			FleetData escortFleet = KcDatabase.Fleet.Fleets[2];

			Fleet2 = escortFleet;
		}

		if (!CellIds.Contains(response.ApiNo))
		{
			CellIds.Add(response.ApiNo);
		}

		SubmitData();
	}

	public void ApiReqMap_NextOnResponseReceived(string apiname, dynamic data)
	{
		if (CellIds is null) return;

		string json = data.ToString();
		ApiReqMapNextResponse response = JsonSerializer.Deserialize<ApiReqMapNextResponse>(json)!;

		EscapeList = Fleet1?.EscapedShipList.ToList();

		if (EscapeList is not null && Fleet2 is not null)
		{
			EscapeList.AddRange(Fleet2.EscapedShipList);
		}

		if (!CellIds.Contains(response.ApiNo))
		{
			CellIds.Add(response.ApiNo);
		}

		SubmitData();
	}

	public void ApiPort_Port_ResponseReceived(string apiname, dynamic data)
	{
		if (CellIds is not null)
		{
			SubmitData();
		}

		ClearState();
	}

	private void ClearState()
	{
		CellCount = null;
		CellIds = null;
		MapLevels = null;
		World = null;
		Map = null;
		Fleet1 = null;
		Fleet2 = null;
		EscapeList = null;
	}

	[SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "<Pending>")]
	private void SubmitData()
	{
		if (Fleet1 is null) return;
		if (EscapeList is null) return;
		if (CellIds is null) return;
		if (MapLevels is null) return;
		if (CellCount is not int cellCount) return;
		if (World is not int world) return;
		if (Map is not int map) return;

		List<Dictionary<string, JsonNode?>> deck1 = Fleet1.MembersInstance!
			.OfType<ShipData>()
			.Select(Extensions.MakeShip)
			.Select(FilterShipValues)
			.ToList();

		List<List<object>> slot1 = Fleet1.MembersInstance!
			.OfType<ShipData>()
			.Select(s => s.AllSlotInstance
				.Select(e => e switch
				{
					null => (object)JsonValue.Create(-1),
					_ => FilterEquipmentValues(e.MakeEquipment()),
				})
				.ToList())
			.ToList();

		List<Dictionary<string, JsonNode?>>? deck2 = Fleet2?.MembersInstance!
			.OfType<ShipData>()
			.Select(Extensions.MakeShip)
			.Select(FilterShipValues)
			.ToList();

		List<List<object>>? slot2 = Fleet2?.MembersInstance!
			.OfType<ShipData>()
			.Select(s => s.AllSlotInstance
				.Select(e => e switch
				{
					null => (object)JsonValue.Create(-1),
					_ => FilterEquipmentValues(e.MakeEquipment()),
				})
				.ToList())
			.ToList();

		try
		{
			PoiDbRouteSubmissionData submissionData = new()
			{
				Deck1 = deck1,
				Deck2 = deck2,
				EscapeList = EscapeList,
				Slot1 = slot1,
				Slot2 = slot2,
				CellIds = CellIds,
				MapLevels = MapLevels,
				NextInfo = new()
				{
					World = world,
					Map = map,
				},
				AdmiralLevel = KcDatabase.Admiral.Level,
				LosValues = new()
				{
					SakuOne25 = null,
					SakuOne25a = null,
					SakuOne33x1 = Fleet1.GetSearchingAbility(1),
					SakuOne33x2 = Fleet1.GetSearchingAbility(2),
					SakuOne33x3 = Fleet1.GetSearchingAbility(3),
					SakuOne33x4 = Fleet1.GetSearchingAbility(4),
					SakuTwo25 = null,
					SakuTwo25a = null,
					SakuTwo33x1 = Fleet2?.GetSearchingAbility(1),
					SakuTwo33x2 = Fleet2?.GetSearchingAbility(2),
					SakuTwo33x3 = Fleet2?.GetSearchingAbility(3),
					SakuTwo33x4 = Fleet2?.GetSearchingAbility(4),
				},
				ApiCellData = cellCount,
				Version = Version,
			};

			Task.Run(async () =>
			{
				try
				{
					await PoiHttpClient.Route(submissionData);
				}
				catch (Exception e)
				{
					LogError(e);
				}
			});
		}
		catch (Exception e)
		{
			LogError(e);
			ClearState();
		}
	}

	private static Dictionary<string, JsonNode?> FilterShipValues(JsonNode node)
	{
		return node
			.AsObject()
			.Where(kvp => RelevantKey(kvp.Key))
			.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		// the last 2 keys seem to be extra data added by poi?
		static bool RelevantKey(string key) => key is
			"api_ship_id" or
			"api_lv" or
			"api_soku" or
			"api_slotitem_ex" or
			"api_slotitem_level";
	}

	private static Dictionary<string, JsonNode?> FilterEquipmentValues(JsonNode node)
	{
		return node
			.AsObject()
			.Where(kvp => RelevantKey(kvp.Key))
			.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		static bool RelevantKey(string key) => key is
			"api_id" or
			"api_slotitem_id" or
			"api_locked" or
			"api_level";
	}
}
