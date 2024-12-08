using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class PoiDbRouteShip
{
	[JsonPropertyName("api_ship_id")]
	public required int ApiShipId { get; init; }

	[JsonPropertyName("api_lv")]
	public required int ApiLv { get; init; }

	[JsonPropertyName("api_sally_area")]
	public required int? ApiSallyArea { get; init; }

	[JsonPropertyName("api_soku")]
	public required int ApiSoku { get; init; }

	[JsonPropertyName("api_slotitem_ex")]
	public required int ApiSlotitemEx { get; init; }

	[JsonPropertyName("api_slotitem_level")]
	public required int ApiSlotitemLevel { get; init; }
}
