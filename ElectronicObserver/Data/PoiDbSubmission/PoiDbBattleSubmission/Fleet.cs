using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;

public class Fleet
{
	[JsonPropertyName("LBAC")]
	public required List<PoiAirBase> Lbac { get; init; }

	[JsonPropertyName("escort")]
	public required List<JsonNode>? Escort { get; init; }

	[JsonPropertyName("main")]
	public required List<JsonNode> Main { get; init; }

	/// <summary>
	/// They don't need support data currently.
	/// </summary>
	[JsonPropertyName("support")]
	public List<JsonNode>? Support { get; init; }

	[JsonPropertyName("type")]
	public required FleetType Type { get; init; }
}
