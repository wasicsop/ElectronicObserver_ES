using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Tools.AirControlSimulator;
using ElectronicObserverTypes;

namespace ElectronicObserver.Services;

public class ToolService
{
	private DataSerializationService DataSerializationService { get; }

	public ToolService(DataSerializationService dataSerializationService)
	{
		DataSerializationService = dataSerializationService;
	}


	public void AirControlSimulator(AirControlSimulatorViewModel? viewModel = null)
	{
		viewModel ??= new();

		BaseAirCorpsSimulationContentDialog dialog = new(viewModel);

		if (dialog.ShowDialog(App.Current.MainWindow) is not true) return;

		AirControlSimulatorViewModel result = dialog.Result!;

		List<BaseAirCorpsData> bases = KCDatabase.Instance.BaseAirCorps.Values
			.Where(b => b.MapAreaID == result.AirBaseArea?.AreaId)
			.ToList();

		IEnumerable<IEquipmentData> equipment = KCDatabase.Instance.Equipments.Values
			.Where(e => result.AllEquipment || e.IsLocked);

		string airControlSimulatorData = DataSerializationService.AirControlSimulator
		(
			KCDatabase.Instance.Admiral.Level,
			result.Fleet1 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(0).FirstOrDefault() : null,
			result.Fleet2 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(1).FirstOrDefault() : null,
			result.Fleet3 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(2).FirstOrDefault() : null,
			result.Fleet4 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(3).FirstOrDefault() : null,
			bases.Skip(0).FirstOrDefault(),
			bases.Skip(1).FirstOrDefault(),
			bases.Skip(2).FirstOrDefault(),
			result.ShipData ? KCDatabase.Instance.Ships.Values : null,
			result.EquipmentData ? equipment : null,
			result.MaxAircraftLevelFleet,
			result.MaxAircraftLevelAirBase
		);

		string url = @$"https://noro6.github.io/kc-web#import:{airControlSimulatorData}";

		Window.FormBrowserHost.Instance.Browser.OpenAirControlSimulator(url);

		// todo: this doesn't work and I don't feel like making a workaround right now
		// https://stackoverflow.com/a/3114737
		if (result.SystemBrowser)
		{
			/*
				ProcessStartInfo psi = new()
				{
					FileName = url,
					UseShellExecute = true
				};

				Process.Start(psi);
				*/
		}
	}
}
