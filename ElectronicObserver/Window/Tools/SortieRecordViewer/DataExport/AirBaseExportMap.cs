using CsvHelper.Configuration;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class AirBaseExportMap : ClassMap<AirBaseExportModel>
{
	public AirBaseExportMap(string prefix)
	{
		Map(m => m.Hp).Name(CsvExportResources.Hp);
		References<AirBaseSquadronExportMap>(s => s.Squadron1).Prefix($"{prefix}{CsvExportResources.PrefixSquadron1}");
		References<AirBaseSquadronExportMap>(s => s.Squadron2).Prefix($"{prefix}{CsvExportResources.PrefixSquadron2}");
		References<AirBaseSquadronExportMap>(s => s.Squadron3).Prefix($"{prefix}{CsvExportResources.PrefixSquadron3}");
		References<AirBaseSquadronExportMap>(s => s.Squadron4).Prefix($"{prefix}{CsvExportResources.PrefixSquadron4}");
	}
}
