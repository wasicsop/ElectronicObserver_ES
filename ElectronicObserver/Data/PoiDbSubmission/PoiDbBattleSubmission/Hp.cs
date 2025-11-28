using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;

public class Hp
{
	[JsonPropertyName("api_defeat_count")]
	public required int? DefeatCount { get; init; }

	[JsonPropertyName("api_now_maphp")]
	public required int? ApiNowMaphp { get; init; }
}
