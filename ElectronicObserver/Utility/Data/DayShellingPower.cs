using System;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Utility.Data
{
	public static class DayShellingPower
	{
		public static int GetDayShellingPower(this IShipData ship, Enum attack, IFleetData fleet, EngagementType engagement = EngagementType.Parallel)
		{
			double basepower = ship.BaseDayShellingPower(fleet);

			basepower *= ship.GetHPDamageBonus() * GetEngagementFormDamageRate(engagement);
			basepower += ship.GetLightCruiserDamageBonus() + ship.GetItalianDamageBonus();

			basepower = Math.Floor(Damage.Cap(basepower, 180));

			basepower *= DayAttackKindDamageMod(attack);

			return (int)basepower;
		}

		private static double BaseDayShellingPower(this IShipData ship, IFleetData fleet) => ship switch
		{
			_ when ship.MasterShip.IsAircraftCarrier => ship.CarrierBasePower(fleet),
			_ when ship.MasterShip.ShipId == ShipId.HayasuiKai && ship.HasAttacker() => ship.CarrierBasePower(fleet),

			_ => ship.SurfaceShipBasePower(fleet)
		};

		private static double SurfaceShipBasePower(this IShipData ship, IFleetData fleet) =>
			ship.FirepowerTotal + ship.GetDayBattleEquipmentLevelBonus() +
			fleet.GetCombinedFleetShellingDamageBonus(ship) + 5;

		private static double CarrierBasePower(this IShipData ship, IFleetData fleet) =>
			Math.Floor((ship.FirepowerTotal + ship.TorpedoBase +
			            ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Torpedo ?? 0) +
			            Math.Floor(ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Bomber ?? 0) * 1.3) +
			            ship.GetDayBattleEquipmentLevelBonus() +
			            fleet.GetCombinedFleetShellingDamageBonus(ship)) * 1.5) + 55;

		/*private int CalculateShellingPower(int engagementForm = 1)
		{
			var attackKind = Calculator.GetDayAttackKind(AllSlotMaster.ToArray(), ShipID, -1);
			if (attackKind == DayAttackKind.AirAttack || attackKind == DayAttackKind.CutinAirAttack)
				return 0;


			double basepower = FirepowerTotal + GetDayBattleEquipmentLevelBonus() + GetCombinedFleetShellingDamageBonus() + 5;

			basepower *= GetHPDamageBonus() * GetEngagementFormDamageRate(engagementForm);

			basepower += GetLightCruiserDamageBonus() + GetItalianDamageBonus();

			// キャップ
			basepower = Math.Floor(CapDamage(basepower, 180));

			// 弾着観測射撃
			switch (attackKind)
			{
				case DayAttackKind.DoubleShelling:
				case DayAttackKind.CutinMainRadar:
					basepower *= 1.2;
					break;
				case DayAttackKind.CutinMainSub:
					basepower *= 1.1;
					break;
				case DayAttackKind.CutinMainAP:
					basepower *= 1.3;
					break;
				case DayAttackKind.CutinMainMain:
					basepower *= 1.5;
					break;
				case DayAttackKind.ZuiunMultiAngle:
					basepower *= 1.35;
					break;
				case DayAttackKind.SeaAirMultiAngle:
					basepower *= 1.3;
					break;
			}

			return (int)(basepower * GetAmmoDamageRate());
		}*/

		private static double GetDayBattleEquipmentLevelBonus(this IShipData ship) =>
			ship.AllSlotInstance.Sum(e => e.DayShellingBonus());

		private static double DayShellingBonus(this IEquipmentData? equip) => equip?.MasterEquipment.CategoryType switch
		{
			EquipmentTypes.MainGunSmall => Math.Sqrt(equip.Level),
			EquipmentTypes.MainGunMedium => Math.Sqrt(equip.Level),
			EquipmentTypes.APShell => Math.Sqrt(equip.Level),
			EquipmentTypes.AADirector => Math.Sqrt(equip.Level),
			EquipmentTypes.Searchlight => Math.Sqrt(equip.Level),
			EquipmentTypes.SearchlightLarge => Math.Sqrt(equip.Level),
			EquipmentTypes.AAGun => Math.Sqrt(equip.Level),
			EquipmentTypes.LandingCraft => Math.Sqrt(equip.Level),
			EquipmentTypes.SpecialAmphibiousTank => Math.Sqrt(equip.Level),

			EquipmentTypes.MainGunLarge => Math.Sqrt(equip.Level) * 1.5,
			EquipmentTypes.MainGunLarge2 => Math.Sqrt(equip.Level) * 1.5,

			EquipmentTypes.Sonar => Math.Sqrt(equip.Level) * 0.75,
			EquipmentTypes.SonarLarge => Math.Sqrt(equip.Level) * 0.75,
			EquipmentTypes.DepthCharge when equip.MasterEquipment.IsDepthChargeProjector => Math.Sqrt(equip.Level) * 0.75,

			EquipmentTypes.SecondaryGun => equip.EquipmentId switch
			{
				EquipmentId.SecondaryGun_12_7cmTwinHighangleGun => 0.2 * equip.Level,
				EquipmentId.SecondaryGun_8cmHighangleGun => 0.2 * equip.Level,
				EquipmentId.SecondaryGun_8cmHighangleGunKai_MachineGun => 0.2 * equip.Level,
				EquipmentId.SecondaryGun_10cmTwinHighangleGunKai_AdditionalMachineGuns => 0.2 * equip.Level,

				EquipmentId.SecondaryGun_15_5cmTripleSecondaryGun => 0.3 * equip.Level,
				EquipmentId.SecondaryGun_15_5cmTripleSecondaryGunKai => 0.3 * equip.Level,

				_ => Math.Sqrt(equip.Level)
			},

			EquipmentTypes.CarrierBasedTorpedo => 0.2 * equip.Level,

			_ => 0
		};

		private static double GetCombinedFleetShellingDamageBonus(this IFleetData fleet, IShipData ship) =>
			(fleet.FleetType, ship.Fleet) switch
			{
				(FleetType.Carrier, 1) => 2,
				(FleetType.Carrier, 2) => 10,

				(FleetType.Surface, 1) => 10,
				(FleetType.Surface, 2) => -5,

				(FleetType.Transport, 1) => -5,
				(FleetType.Transport, 2) => 10,

				_ => 0
			};

		private static double GetHPDamageBonus(this IShipData ship) => ship.HPRate switch
			{
				_ when ship.HPRate <= 0.25 => 0.4,
				_ when ship.HPRate <= 0.5 => 0.7,
				_ => 1,
			};

		private static double GetEngagementFormDamageRate(EngagementType form) => form switch
		{
			EngagementType.Parallel => 1.0,
			EngagementType.HeadOn => 0.8,
			EngagementType.TAdvantage => 1.2,
			EngagementType.TDisadvantage => 0.6,
			_ => 1.0
		};

		private static double GetLightCruiserDamageBonus(this IShipData ship)
		{
			if (ship.MasterShip.ShipType != ShipTypes.LightCruiser &&
			    ship.MasterShip.ShipType != ShipTypes.TorpedoCruiser &&
			    ship.MasterShip.ShipType != ShipTypes.TrainingCruiser)
			{
				return 0;
			}

			int single = ship.AllSlotInstance.Count(e => e?.EquipmentId switch
			{
				EquipmentId.MainGunMedium_14cmSingleGun => true,
				EquipmentId.SecondaryGun_15_2cmSingleGun => true,
				_ => false
			});

			int twin = ship.AllSlotInstance.Count(e => e?.EquipmentId switch
			{
				EquipmentId.MainGunMedium_14cmTwinGun => true,
				EquipmentId.MainGunMedium_15_2cmTwinGun => true,
				EquipmentId.MainGunMedium_15_2cmTwinGunKai => true,
				_ => false
			});

			return Math.Sqrt(twin) * 2.0 + Math.Sqrt(single);
		}

		private static double GetItalianDamageBonus(this IShipData ship)
		{
			// todo turn to switch expression once we get or patterns
			switch (ship.MasterShip.ShipId)
			{
				case ShipId.Zara:    
				case ShipId.ZaraKai: 
				case ShipId.ZaraDue: 
				case ShipId.Pola:    
				case ShipId.PolaKai: 
					return Math.Sqrt(ship.AllSlotInstance.Count(e => e?.EquipmentId == EquipmentId.MainGunMedium_203mm53TwinGun));

				default:
					return 0;
			}
		}

		private static double DayAttackKindDamageMod(Enum attackKind) => attackKind switch
		{
			DayAttackKind.DoubleShelling => 1.2,
			DayAttackKind.CutinMainRadar => 1.2,
			DayAttackKind.CutinMainSub => 1.1,
			DayAttackKind.CutinMainAP => 1.3,
			DayAttackKind.CutinMainMain => 1.5,

			DayAttackKind.ZuiunMultiAngle => 1.35,
			DayAttackKind.SeaAirMultiAngle => 1.3,

			DayAirAttackCutinKind.FighterBomberAttacker => 1.25,
			DayAirAttackCutinKind.BomberBomberAttacker => 1.20,
			DayAirAttackCutinKind.BomberAttacker => 1.15,

			_ => 1
		};
	}
}