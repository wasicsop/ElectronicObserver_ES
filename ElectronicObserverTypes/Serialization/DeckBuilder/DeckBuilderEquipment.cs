using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.DeckBuilder;

public class DeckBuilderEquipment
{
	[JsonPropertyName("id")] public EquipmentId Id { get; set; }
	[JsonPropertyName("rf")] public int Level { get; set; }
	[JsonPropertyName("mas")] public int? AircraftLevel { get; set; }
}