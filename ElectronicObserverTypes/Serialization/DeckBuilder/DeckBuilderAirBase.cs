using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.DeckBuilder;

public class DeckBuilderAirBase
{
	[JsonPropertyName("items")] public DeckBuilderAirBaseEquipmentList Equipment { get; set; } = new();
	[JsonPropertyName("mode")] public int Mode { get; set; }
}