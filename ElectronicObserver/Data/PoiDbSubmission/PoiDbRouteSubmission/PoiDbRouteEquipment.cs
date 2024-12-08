using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class PoiDbRouteEquipment
{
	[JsonPropertyName("api_id")]
	public required int ApiId { get; init; }

	[JsonPropertyName("api_slotitem_id")]
	public required int ApiSlotitemId { get; init; }

	[JsonPropertyName("api_locked")]
	public required int ApiLocked { get; init; }

	[JsonPropertyName("api_level")]
	public required int ApiLevel { get; init; }

	[JsonPropertyName("api_alv")]
	public required int ApiAlv { get; init; }
}
