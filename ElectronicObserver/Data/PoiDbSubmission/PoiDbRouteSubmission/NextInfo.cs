using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class NextInfo
{
	[JsonPropertyName("api_maparea_id")]
	public required string World { get; init; }

	[JsonPropertyName("api_mapinfo_no")]
	public required string Map { get; init; }
}
