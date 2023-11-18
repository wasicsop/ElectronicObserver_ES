namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public record AirBaseExportModel
{
	public string? Hp { get; init; }
	public AirBaseSquadronExportModel Squadron1 { get; init; }
	public AirBaseSquadronExportModel Squadron2 { get; init; }
	public AirBaseSquadronExportModel Squadron3 { get; init; }
	public AirBaseSquadronExportModel Squadron4 { get; init; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
