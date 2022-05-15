using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.DeckBuilder;

public class DeckBuilderShip
{
	[JsonPropertyName("id")] public ShipId Id { get; set; }
	[JsonPropertyName("lv")] public int Level { get; set; }
	[JsonPropertyName("items")] public DeckBuilderEquipmentList Equipment { get; set; } = new();
	[JsonPropertyName("luck")] public int Luck { get; set; }
}