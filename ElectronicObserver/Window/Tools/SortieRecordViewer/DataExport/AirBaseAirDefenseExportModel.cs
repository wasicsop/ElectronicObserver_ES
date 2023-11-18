using System;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;
// all required
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record AirBaseAirDefenseExportModel
{
	public DateTime Date { get; init; }
	public string World { get; init; }
	public string Square { get; init; }
	public string PlayerFormation { get; init; }
	public string EnemyFormation { get; init; }
	public string Engagement { get; init; }
	public string AirBaseDamage { get; init; }
	public int PlayerAircraft { get; init; }
	public int PlayerAircraftLost { get; init; }
	public int EnemyAircraft { get; init; }
	public int EnemyAircraftLost { get; init; }
	public string? AirState { get; init; }
	public string? PlayerContact { get; init; }
	public string? EnemyContact { get; init; }
	public AirBaseExportModel AirBase1 { get; init; }
	public AirBaseExportModel AirBase2 { get; init; }
	public AirBaseExportModel AirBase3 { get; init; }
	public AirBaseAirDefenseShipExportModel EnemyShip1 { get; init; }
	public AirBaseAirDefenseShipExportModel EnemyShip2 { get; init; }
	public AirBaseAirDefenseShipExportModel EnemyShip3 { get; init; }
	public AirBaseAirDefenseShipExportModel EnemyShip4 { get; init; }
	public AirBaseAirDefenseShipExportModel EnemyShip5 { get; init; }
	public AirBaseAirDefenseShipExportModel EnemyShip6 { get; init; }

	public int PlayerTorpedoFlags1 { get; init; }
	public int PlayerTorpedoFlags2 { get; init; }
	public int PlayerTorpedoFlags3 { get; init; }

	public int EnemyTorpedoFlags1 { get; init; }
	public int EnemyTorpedoFlags2 { get; init; }
	public int EnemyTorpedoFlags3 { get; init; }
	public int EnemyTorpedoFlags4 { get; init; }
	public int EnemyTorpedoFlags5 { get; init; }
	public int EnemyTorpedoFlags6 { get; init; }

	public int PlayerBomberFlags1 { get; init; }
	public int PlayerBomberFlags2 { get; init; }
	public int PlayerBomberFlags3 { get; init; }

	public int EnemyBomberFlags1 { get; init; }
	public int EnemyBomberFlags2 { get; init; }
	public int EnemyBomberFlags3 { get; init; }
	public int EnemyBomberFlags4 { get; init; }
	public int EnemyBomberFlags5 { get; init; }
	public int EnemyBomberFlags6 { get; init; }

	public int PlayerHitFlags1 { get; init; }
	public int PlayerHitFlags2 { get; init; }
	public int PlayerHitFlags3 { get; init; }

	public int EnemyHitFlags1 { get; init; }
	public int EnemyHitFlags2 { get; init; }
	public int EnemyHitFlags3 { get; init; }
	public int EnemyHitFlags4 { get; init; }
	public int EnemyHitFlags5 { get; init; }
	public int EnemyHitFlags6 { get; init; }

	public double PlayerDamage1 { get; init; }
	public double PlayerDamage2 { get; init; }
	public double PlayerDamage3 { get; init; }

	public double EnemyDamage1 { get; init; }
	public double EnemyDamage2 { get; init; }
	public double EnemyDamage3 { get; init; }
	public double EnemyDamage4 { get; init; }
	public double EnemyDamage5 { get; init; }
	public double EnemyDamage6 { get; init; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
