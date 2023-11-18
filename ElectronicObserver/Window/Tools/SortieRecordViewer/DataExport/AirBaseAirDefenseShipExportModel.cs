namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public record AirBaseAirDefenseShipExportModel
{
	public int? Id { get; init; }
	public string? Name { get; init; }
	public int? Level { get; init; }
	public string? Hp { get; init; }
	public string? Equipment1Name { get; init; }
	public string? Equipment2Name { get; init; }
	public string? Equipment3Name { get; init; }
	public string? Equipment4Name { get; init; }
	public string? Equipment5Name { get; init; }
}
