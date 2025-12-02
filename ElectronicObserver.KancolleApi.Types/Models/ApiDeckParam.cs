namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiDeckParam
{
	/// <summary>
	/// Fleet air power
	/// </summary>
	[JsonPropertyName("api_seiku_value")]
	public int ApiSeikuValue { get; set; }

	/// <summary>
	/// Fleet transport point value on S rank
	/// </summary>
	[JsonPropertyName("api_tp_value")]
	public int ApiTpValue { get; set; }

	/// <summary>
	/// Fleet transport point value on S rank for special maps. Key is the area id and the map id (e.g. 612 for Fall 2025 E2)
	/// </summary>
	[JsonPropertyName("api_atp_value")]
	public Dictionary<string, int> ApiAdditionnalTpValue { get; set; } = [];
}
