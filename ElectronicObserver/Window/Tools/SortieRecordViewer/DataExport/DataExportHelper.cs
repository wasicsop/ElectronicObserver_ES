using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElectronicObserver.Common.ContentDialogs.ExportProgress;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Extensions;
using DayAttack = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase.DayAttack;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public class DataExportHelper
{
	private ElectronicObserverContext Db { get; }
	private ToolService ToolService { get; }

	public DataExportHelper(ElectronicObserverContext db, ToolService toolService)
	{
		Db = db;
		ToolService = toolService;
	}

	public async Task<List<DayShellingExportModel>> DayShelling(
		ObservableCollection<SortieRecordViewModel> sorties,
		ExportProgressViewModel exportProgress,
		CancellationToken cancellationToken = default)
	{
		exportProgress.Total = sorties.Count;

		foreach (SortieRecordViewModel sortieRecord in sorties)
		{
			await sortieRecord.Model.EnsureApiFilesLoaded(Db, cancellationToken);
		}

		List<DayShellingExportModel> dayShellingData = new();

		foreach (SortieRecordViewModel sortieRecord in sorties)
		{
			SortieDetailViewModel? sortieDetail = ToolService.GenerateSortieDetailViewModel(sortieRecord);
			int? admiralLevel = await sortieRecord.Model.GetAdmiralLevel(Db, cancellationToken);

			if (sortieDetail is null) continue;

			bool isFirstNode = true;

			foreach (BattleNode node in sortieDetail.Nodes.OfType<BattleNode>())
			{
				PhaseInitial? initial = node.FirstBattle.Phases.OfType<PhaseInitial>().FirstOrDefault();
				PhaseSearching? searching = node.FirstBattle.Phases.OfType<PhaseSearching>().FirstOrDefault();
				PhaseAirBattle? airBattle = node.FirstBattle.Phases.OfType<PhaseAirBattle>().FirstOrDefault();
				IFleetData? playerFleet = initial?.FleetsAfterPhase?.Fleet;

				if (initial is null) continue;
				if (searching is null) continue;
				if (airBattle is null) continue;
				if (playerFleet is null) continue;

				foreach (PhaseShelling shelling in node.FirstBattle.Phases.OfType<PhaseShelling>())
				{
					foreach (PhaseShellingAttackViewModel attackDisplay in shelling.AttackDisplays)
					{
						IFleetData? attackerFleet = searching.FleetsBeforePhase?.GetFleet(attackDisplay.AttackerIndex);

						if (attackerFleet is null) continue;

						foreach ((DayAttack attack, int attackIndex) in attackDisplay.Attacks.Select((a, i) => (a, i)))
						{
							dayShellingData.Add(new()
							{
								No = dayShellingData.Count + 1,
								Date = sortieDetail.StartTime!.Value.ToLocalTime(),
								World = KCDatabase.Instance.MapInfo[sortieDetail.World * 10 + sortieDetail.Map]?.NameEN ?? "",
								Square = SquareString(sortieDetail, node),
								Sortie = (node.IsBoss, isFirstNode) switch
								{
									(true, true) => $"{CsvExportResources.Sortie}&{CsvExportResources.BossNode}",
									(_, true) => CsvExportResources.Sortie,
									(true, _) => CsvExportResources.BossNode,
									_ => "",
								},
								Rank = WinRank(node.BattleResult?.WinRank),
								EnemyFleet = node.BattleResult?.EnemyFleetName,
								AdmiralLevel = admiralLevel,
								PlayerFormation = Constants.GetFormation(searching.PlayerFormationType),
								EnemyFormation = Constants.GetFormation(searching.EnemyFormationType),
								PlayerSearch = GetSearchingResult(searching.PlayerDetectionType),
								EnemySearch = GetSearchingResult(searching.EnemyDetectionType),
								AirState = Constants.GetAirSuperiority(airBattle.AirState),
								Engagement = Constants.GetEngagementForm(searching.EngagementType),
								PlayerContact = airBattle.TouchAircraftFriend,
								EnemyContact = airBattle.TouchAircraftEnemy,
								PlayerFlare = null,
								EnemyFlare = null,
								BattleType = CsvExportResources.ShellingBattle,
								ShipName1 = attackerFleet.MembersInstance.Skip(0).FirstOrDefault()?.Name,
								ShipName2 = attackerFleet.MembersInstance.Skip(1).FirstOrDefault()?.Name,
								ShipName3 = attackerFleet.MembersInstance.Skip(2).FirstOrDefault()?.Name,
								ShipName4 = attackerFleet.MembersInstance.Skip(3).FirstOrDefault()?.Name,
								ShipName5 = attackerFleet.MembersInstance.Skip(4).FirstOrDefault()?.Name,
								ShipName6 = attackerFleet.MembersInstance.Skip(5).FirstOrDefault()?.Name,
								PlayerFleetType = GetPlayerFleet(initial.FleetsAfterPhase!, attackDisplay.AttackerIndex, attackDisplay.DefenderIndex),
								BattlePhase = GetPhaseString(shelling),
								AttackerSide = attackDisplay.AttackerIndex.FleetFlag switch
								{
									FleetFlag.Player => CsvExportResources.Player,
									_ => CsvExportResources.Enemy,
								},
								AttackType = CsvDayAttackKind(attack.AttackKind),
								AttackIndex = attackIndex,
								DisplayedEquipment1 = attackDisplay.DisplayEquipment.Skip(0).FirstOrDefault()?.NameEN,
								DisplayedEquipment2 = attackDisplay.DisplayEquipment.Skip(1).FirstOrDefault()?.NameEN,
								DisplayedEquipment3 = attackDisplay.DisplayEquipment.Skip(2).FirstOrDefault()?.NameEN,
								HitType = (int)attack.CriticalFlag,
								Damage = attack.Damage,
								Protected = attack.GuardsFlagship switch
								{
									true => 1,
									_ => 0,
								},
								Attacker = MakeShip(attack.Attacker, attackDisplay.AttackerIndex, attackDisplay.AttackerHpBeforeAttack),
								Defender = MakeShip(attack.Defender, attackDisplay.DefenderIndex, attackDisplay.DefenderHpBeforeAttacks[attackIndex]),
								FleetType = Constants.GetCombinedFleet(playerFleet.FleetType),
								EnemyFleetType = GetEnemyFleetType(initial.IsEnemyCombinedFleet),
							});
						}
					}
				}

				isFirstNode = false;
			}

			exportProgress.Progress++;
		}

		return dayShellingData;
	}

	private static ShipExportModel MakeShip(IShipData ship, BattleIndex index, int hpBeforeAttack) => new()
	{
		Index = index.Index + 1,
		Id = ship.ShipID,
		Name = ship.Name,
		ShipType = ship.MasterShip.ShipType.Display(),
		Condition = NullForAbyssals(ship.Condition, ship),
		HpCurrent = hpBeforeAttack,
		HpMax = ship.HPMax,
		DamageState = GetDamageState(hpBeforeAttack, ship.HPMax),
		FuelCurrent = NullForAbyssals(ship.Fuel, ship),
		FuelMax = NullForAbyssals(ship.FuelMax, ship),
		AmmoCurrent = NullForAbyssals(ship.Ammo, ship),
		AmmoMax = NullForAbyssals(ship.AmmoMax, ship),
		Level = ship.Level,
		Speed = Constants.GetSpeed(ship.Speed),
		Firepower = ship.FirepowerTotal,
		Torpedo = ship.TorpedoTotal,
		AntiAir = ship.AATotal,
		Armor = ship.ArmorTotal,
		Evasion = ship.EvasionTotal,
		AntiSubmarine = ship.ASWTotal,
		Search = ship.LOSTotal,
		Luck = ship.LuckTotal,
		Range = Constants.GetRange(ship.Range),
		Equipment1 = MakeEquipment(ship, 0),
		Equipment2 = MakeEquipment(ship, 1),
		Equipment3 = MakeEquipment(ship, 2),
		Equipment4 = MakeEquipment(ship, 3),
		Equipment5 = MakeEquipment(ship, 4),
		Equipment6 = MakeEquipment(ship, 5),
	};

	private static EquipmentExportModel MakeEquipment(IShipData ship, int index) => new()
	{
		Name = ship.AllSlotInstance.Skip(index).FirstOrDefault()?.Name,
		Level = ship.AllSlotInstance.Skip(index).FirstOrDefault()?.Level,
		AircraftLevel = ship.AllSlotInstance.Skip(index).FirstOrDefault()?.AircraftLevel,
		Aircraft = ship.Aircraft.Skip(index).FirstOrDefault(),
	};

	private static string SquareString(SortieDetailViewModel sortieDetail, SortieNode node) =>
		$"{CsvExportResources.Map}:{sortieDetail.World}-{sortieDetail.Map} {CsvExportResources.Cell}:{node.Cell}";

	private static string WinRank(string? rank) => rank switch
	{
		"SS" => "完全勝利!!S",
		"S" => "勝利S",
		"A" => "勝利A",
		"B" => "戦術的勝利B",
		"C" => "戦術的敗北C",
		"D" => "敗北D",
		"E" => "敗北E",

		_ => "",
	};

	private static string GetSearchingResult(DetectionType id) => id switch
	{
		DetectionType.Success => "発見!",
		DetectionType.SuccessNoReturn => "発見!索敵機未帰還機あり",
		DetectionType.NoReturn => "発見できず…索敵機未帰還機あり",
		DetectionType.Failure => "発見できず…",
		DetectionType.SuccessNoPlane => "発見!(索敵機なし)",
		DetectionType.FailureNoPlane => "なし",
		_ => $"不明({id})",
	};

	private static string GetPlayerFleet(BattleFleets fleets, BattleIndex attackerIndex, BattleIndex defenderIndex)
	{
		BattleIndex index = attackerIndex.FleetFlag switch
		{
			FleetFlag.Player => attackerIndex,
			_ => defenderIndex,
		};

		IFleetData? fleet = fleets.GetFleet(index);

		if (fleet is null) throw new NotImplementedException();

		if (fleet == fleets.EscortFleet) return "連合第2艦隊";
		if (fleets.EscortFleet is null) return "通常艦隊";
		if (fleet == fleets.Fleet) return "連合第1艦隊";

		throw new NotImplementedException();
	}

	private static int CsvDayAttackKind(DayAttackKind attack) => attack switch
	{
		< DayAttackKind.Shelling => (int)attack,
		_ => 0,
	};

	private static string GetDamageState(int hpCurrent, int hpMax) => ((double)hpCurrent / hpMax) switch
	{
		<= 0 => "轟沈",
		> 0.75 => "小破未満",

		double hpRate => Constants.GetDamageState(hpRate),
	};

	private static string GetPhaseString(PhaseBase phase) => phase switch
	{
		PhaseShelling { DayShellingPhase: DayShellingPhase.First } => "1",
		PhaseShelling { DayShellingPhase: DayShellingPhase.Second } => "2",
		PhaseShelling { DayShellingPhase: DayShellingPhase.Third } => "3",

		_ => phase.Title,
	};

	private static string GetEnemyFleetType(bool isCombined) => isCombined switch
	{
		true => CsvExportResources.CombinedFleet,
		_ => ConstantsRes.NormalFleet,
	};

	private static int? NullForAbyssals(int? value, IShipData ship) => ship.MasterShip.IsAbyssalShip switch
	{
		true => null,
		_ => value,
	};
}
