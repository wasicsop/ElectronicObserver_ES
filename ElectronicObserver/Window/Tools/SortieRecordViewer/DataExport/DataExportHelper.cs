using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElectronicObserver.Common.ContentDialogs.ExportProgress;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport.DayShellingExport;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

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

			foreach (BattleNode node in sortieDetail.Nodes.OfType<BattleNode>())
			{
				PhaseInitial? initial = node.FirstBattle.Phases.OfType<PhaseInitial>().FirstOrDefault();
				PhaseSearching? searching = node.FirstBattle.Phases.OfType<PhaseSearching>().FirstOrDefault();
				PhaseAirBattle? airBattle = node.FirstBattle.Phases.OfType<PhaseAirBattle>().FirstOrDefault();
				IFleetData? playerFleet = searching?.FleetsBeforePhase?.Fleet;
				IFleetData? enemyFleet = searching?.FleetsBeforePhase?.EnemyFleet;

				if (initial is null) continue;
				if (searching is null) continue;
				if (airBattle is null) continue;
				if (playerFleet is null) continue;
				if (enemyFleet is null) continue;

				foreach (PhaseShelling shelling in node.FirstBattle.Phases.OfType<PhaseShelling>())
				{
					foreach (PhaseShellingAttackViewModel attackDisplay in shelling.AttackDisplays)
					{
						foreach ((DayAttack attack, int attackIndex) in attackDisplay.Attacks.Select((a, i) => (a, i)))
						{
							dayShellingData.Add(new()
							{
								No = dayShellingData.Count + 1,
								Date = sortieDetail.StartTime!.Value,
								World = sortieDetail.World,
								Map = sortieDetail.Map,
								Cell = node.Cell,
								Sortie = node.IsBoss switch
								{
									true => CsvExportResources.BossNode,
									_ => CsvExportResources.NormalNode,
								},
								Rank = node.BattleResult?.WinRank,
								EnemyFleet = node.BattleResult?.EnemyFleetName,
								AdmiralLevel = admiralLevel,
								PlayerFormation = Constants.GetFormation(searching.PlayerFormationType),
								EnemyFormation = Constants.GetFormation(searching.EnemyFormationType),
								PlayerSearch = Constants.GetSearchingResult(searching.PlayerDetectionType),
								EnemySearch = Constants.GetSearchingResult(searching.EnemyDetectionType),
								AirState = Constants.GetAirSuperiority(airBattle.AirState),
								Engagement = Constants.GetEngagementForm(searching.EngagementType),
								PlayerContact = airBattle.TouchAircraftFriend,
								EnemyContact = airBattle.TouchAircraftEnemy,
								PlayerFlare = null,
								EnemyFlare = null,
								BattleType = CsvExportResources.ShellingBattle,
								ShipName1 = playerFleet.MembersInstance.Skip(0).FirstOrDefault()?.Name,
								ShipName2 = playerFleet.MembersInstance.Skip(1).FirstOrDefault()?.Name,
								ShipName3 = playerFleet.MembersInstance.Skip(2).FirstOrDefault()?.Name,
								ShipName4 = playerFleet.MembersInstance.Skip(3).FirstOrDefault()?.Name,
								ShipName5 = playerFleet.MembersInstance.Skip(4).FirstOrDefault()?.Name,
								ShipName6 = playerFleet.MembersInstance.Skip(5).FirstOrDefault()?.Name,
								PlayerFleetType = Constants.GetCombinedFleet(playerFleet.FleetType),
								BattlePhase = shelling.Title,
								AttackerSide = attackDisplay.AttackerIndex.FleetFlag switch
								{
									FleetFlag.Player => CsvExportResources.Player,
									_ => CsvExportResources.Enemy,
								},
								AttackType = (int)attack.AttackKind,
								AttackIndex = attackIndex,
								DisplayedEquipment1 = null, // this would need some research and doesn't feel useful
								DisplayedEquipment2 = null,
								DisplayedEquipment3 = null,
								HitType = (int)attack.CriticalFlag,
								Damage = attack.Damage,
								Protected = attack.GuardsFlagship switch
								{
									true => 1,
									_ => 0,
								},
								AttackerIndex = attackDisplay.AttackerIndex.Index + 1,
								AttackerId = attack.Attacker.ShipID,
								AttackerName = attack.Attacker.Name,
								AttackerShipType = attack.Attacker.MasterShip.ShipType.Display(),
								AttackerCondition = attack.Attacker.Condition,
								AttackerHpCurrent = attack.Attacker.HPCurrent,
								AttackerHpMax = attack.Attacker.HPMax,
								AttackerDamageState = Constants.GetDamageState(attack.Attacker.HPRate),
								AttackerFuelCurrent = attack.Attacker.Fuel,
								AttackerFuelMax = attack.Attacker.FuelMax,
								AttackerAmmoCurrent = attack.Attacker.Ammo,
								AttackerAmmoMax = attack.Attacker.AmmoMax,
								AttackerLevel = attack.Attacker.Level,
								AttackerSpeed = Constants.GetSpeed(attack.Attacker.Speed),
								AttackerFirepower = attack.Attacker.FirepowerTotal,
								AttackerTorpedo = attack.Attacker.TorpedoTotal,
								AttackerAntiAir = attack.Attacker.AATotal,
								AttackerArmor = attack.Attacker.ArmorTotal,
								AttackerEvasion = attack.Attacker.EvasionTotal,
								AttackerAntiSubmarine = attack.Attacker.ASWTotal,
								AttackerSearch = attack.Attacker.LOSTotal,
								AttackerLuck = attack.Attacker.LuckTotal,
								AttackerRange = Constants.GetRange(attack.Attacker.Range),

								AttackerEquipment1Name = attack.Attacker.AllSlotInstance.Skip(0).FirstOrDefault()?.Name,
								AttackerEquipment1Level = attack.Attacker.AllSlotInstance.Skip(0).FirstOrDefault()?.Level,
								AttackerEquipment1AircraftLevel = attack.Attacker.AllSlotInstance.Skip(0).FirstOrDefault()?.AircraftLevel,
								AttackerEquipment1Aircraft = attack.Attacker.Aircraft.Skip(0).FirstOrDefault(),

								AttackerEquipment2Name = attack.Attacker.AllSlotInstance.Skip(1).FirstOrDefault()?.Name,
								AttackerEquipment2Level = attack.Attacker.AllSlotInstance.Skip(1).FirstOrDefault()?.Level,
								AttackerEquipment2AircraftLevel = attack.Attacker.AllSlotInstance.Skip(1).FirstOrDefault()?.AircraftLevel,
								AttackerEquipment2Aircraft = attack.Attacker.Aircraft.Skip(1).FirstOrDefault(),

								AttackerEquipment3Name = attack.Attacker.AllSlotInstance.Skip(2).FirstOrDefault()?.Name,
								AttackerEquipment3Level = attack.Attacker.AllSlotInstance.Skip(2).FirstOrDefault()?.Level,
								AttackerEquipment3AircraftLevel = attack.Attacker.AllSlotInstance.Skip(2).FirstOrDefault()?.AircraftLevel,
								AttackerEquipment3Aircraft = attack.Attacker.Aircraft.Skip(2).FirstOrDefault(),

								AttackerEquipment4Name = attack.Attacker.AllSlotInstance.Skip(3).FirstOrDefault()?.Name,
								AttackerEquipment4Level = attack.Attacker.AllSlotInstance.Skip(3).FirstOrDefault()?.Level,
								AttackerEquipment4AircraftLevel = attack.Attacker.AllSlotInstance.Skip(3).FirstOrDefault()?.AircraftLevel,
								AttackerEquipment4Aircraft = attack.Attacker.Aircraft.Skip(3).FirstOrDefault(),

								AttackerEquipment5Name = attack.Attacker.AllSlotInstance.Skip(4).FirstOrDefault()?.Name,
								AttackerEquipment5Level = attack.Attacker.AllSlotInstance.Skip(4).FirstOrDefault()?.Level,
								AttackerEquipment5AircraftLevel = attack.Attacker.AllSlotInstance.Skip(4).FirstOrDefault()?.AircraftLevel,
								AttackerEquipment5Aircraft = attack.Attacker.Aircraft.Skip(4).FirstOrDefault(),

								AttackerEquipment6Name = attack.Attacker.AllSlotInstance.Skip(5).FirstOrDefault()?.Name,
								AttackerEquipment6Level = attack.Attacker.AllSlotInstance.Skip(5).FirstOrDefault()?.Level,
								AttackerEquipment6AircraftLevel = attack.Attacker.AllSlotInstance.Skip(5).FirstOrDefault()?.AircraftLevel,
								AttackerEquipment6Aircraft = attack.Attacker.Aircraft.Skip(5).FirstOrDefault(),

								DefenderIndex = attackDisplay.DefenderIndex.Index + 1,
								DefenderId = attack.Defender.ShipID,
								DefenderName = attack.Defender.Name,
								DefenderShipType = attack.Defender.MasterShip.ShipType.Display(),
								DefenderCondition = attack.Defender.Condition,
								DefenderHpCurrent = attack.Defender.HPCurrent,
								DefenderHpMax = attack.Defender.HPMax,
								DefenderDamageState = Constants.GetDamageState(attack.Defender.HPRate),
								DefenderFuelCurrent = attack.Defender.Fuel,
								DefenderFuelMax = attack.Defender.FuelMax,
								DefenderAmmoCurrent = attack.Defender.Ammo,
								DefenderAmmoMax = attack.Defender.AmmoMax,
								DefenderLevel = attack.Defender.Level,
								DefenderSpeed = Constants.GetSpeed(attack.Defender.Speed),
								DefenderFirepower = attack.Defender.FirepowerTotal,
								DefenderTorpedo = attack.Defender.TorpedoTotal,
								DefenderAntiAir = attack.Defender.AATotal,
								DefenderArmor = attack.Defender.ArmorTotal,
								DefenderEvasion = attack.Defender.EvasionTotal,
								DefenderAntiSubmarine = attack.Defender.ASWTotal,
								DefenderSearch = attack.Defender.LOSTotal,
								DefenderLuck = attack.Defender.LuckTotal,
								DefenderRange = Constants.GetRange(attack.Defender.Range),

								DefenderEquipment1Name = attack.Defender.AllSlotInstance.Skip(0).FirstOrDefault()?.Name,
								DefenderEquipment1Level = attack.Defender.AllSlotInstance.Skip(0).FirstOrDefault()?.Level,
								DefenderEquipment1AircraftLevel = attack.Defender.AllSlotInstance.Skip(0).FirstOrDefault()?.AircraftLevel,
								DefenderEquipment1Aircraft = attack.Defender.Aircraft.Skip(0).FirstOrDefault(),

								DefenderEquipment2Name = attack.Defender.AllSlotInstance.Skip(1).FirstOrDefault()?.Name,
								DefenderEquipment2Level = attack.Defender.AllSlotInstance.Skip(1).FirstOrDefault()?.Level,
								DefenderEquipment2AircraftLevel = attack.Defender.AllSlotInstance.Skip(1).FirstOrDefault()?.AircraftLevel,
								DefenderEquipment2Aircraft = attack.Defender.Aircraft.Skip(1).FirstOrDefault(),

								DefenderEquipment3Name = attack.Defender.AllSlotInstance.Skip(2).FirstOrDefault()?.Name,
								DefenderEquipment3Level = attack.Defender.AllSlotInstance.Skip(2).FirstOrDefault()?.Level,
								DefenderEquipment3AircraftLevel = attack.Defender.AllSlotInstance.Skip(2).FirstOrDefault()?.AircraftLevel,
								DefenderEquipment3Aircraft = attack.Defender.Aircraft.Skip(2).FirstOrDefault(),

								DefenderEquipment4Name = attack.Defender.AllSlotInstance.Skip(3).FirstOrDefault()?.Name,
								DefenderEquipment4Level = attack.Defender.AllSlotInstance.Skip(3).FirstOrDefault()?.Level,
								DefenderEquipment4AircraftLevel = attack.Defender.AllSlotInstance.Skip(3).FirstOrDefault()?.AircraftLevel,
								DefenderEquipment4Aircraft = attack.Defender.Aircraft.Skip(3).FirstOrDefault(),

								DefenderEquipment5Name = attack.Defender.AllSlotInstance.Skip(4).FirstOrDefault()?.Name,
								DefenderEquipment5Level = attack.Defender.AllSlotInstance.Skip(4).FirstOrDefault()?.Level,
								DefenderEquipment5AircraftLevel = attack.Defender.AllSlotInstance.Skip(4).FirstOrDefault()?.AircraftLevel,
								DefenderEquipment5Aircraft = attack.Defender.Aircraft.Skip(4).FirstOrDefault(),

								DefenderEquipment6Name = attack.Defender.AllSlotInstance.Skip(5).FirstOrDefault()?.Name,
								DefenderEquipment6Level = attack.Defender.AllSlotInstance.Skip(5).FirstOrDefault()?.Level,
								DefenderEquipment6AircraftLevel = attack.Defender.AllSlotInstance.Skip(5).FirstOrDefault()?.AircraftLevel,
								DefenderEquipment6Aircraft = attack.Defender.Aircraft.Skip(5).FirstOrDefault(),

								FleetType = attackDisplay.AttackerIndex.FleetFlag switch
								{
									FleetFlag.Player => Constants.GetCombinedFleet(playerFleet.FleetType),
									_ => GetEnemyFleetType(initial.IsEnemyCombinedFleet),
								},
								EnemyFleetType = attackDisplay.AttackerIndex.FleetFlag switch
								{
									FleetFlag.Player => GetEnemyFleetType(initial.IsEnemyCombinedFleet),
									_ => Constants.GetCombinedFleet(playerFleet.FleetType),
								},
							});
						}
					}
				}
			}

			exportProgress.Progress++;
		}

		return dayShellingData;
	}

	private static string GetEnemyFleetType(bool isCombined) => isCombined switch
	{
		true => CsvExportResources.CombinedFleet,
		_ => ConstantsRes.NormalFleet,
	};
}
