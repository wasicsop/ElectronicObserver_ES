namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public record AirBaseSquadronExportModel
{
	public string? Name { get; init; }
	public int? Level { get; init; }
	public int? AircraftLevel { get; init; }
	public string? Condition { get; init; }
	public int? Aircraft { get; init; }
}
