using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserverTypes.Serialization.DeckBuilder;
using ElectronicObserverTypes.Serialization.FleetAnalysis;

namespace ElectronicObserverTypes.Serialization.AirControlSimulator;

public class AirControlSimulatorData
{
	[JsonPropertyName("predeck")] public DeckBuilderData? Fleet { get; set; }
	[JsonPropertyName("ships")] public IEnumerable<FleetAnalysisShip>? Ships { get; set; }
	[JsonPropertyName("items")] public IEnumerable<FleetAnalysisEquipment>? Equipment { get; set; }
}
