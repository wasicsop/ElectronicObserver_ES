namespace ElectronicObserver.Window.Tools.AirControlSimulator;

public record AirBaseArea(int AreaId, string? AreaName)
{
	public string Display => $"{AreaId}: {AreaName}";
}
