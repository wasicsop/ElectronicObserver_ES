using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Utility.Data
{
	public static class ShipAttacks
	{
		public static IEnumerable<Enum> GetDayAttacks(this IShipData ship)
		{
			IEnumerable<Enum> dayAttacks = new List<Enum>();

			if (ship.IsIseClassK2())
			{
				dayAttacks = dayAttacks.Concat(IseClassSpecialAttacks.Cast<Enum>());
			}

			if (ship.IsSurfaceShip())
			{
				dayAttacks = dayAttacks
					.Concat(SurfaceShipDaySpecialAttacks.Cast<Enum>())
					.Concat(SurfaceShipDayNormalAttacks.Cast<Enum>());
			}

			if (ship.MasterShip.IsAircraftCarrier)
			{
				dayAttacks = dayAttacks
					.Concat(CarrierDaySpecialAttacks.Cast<Enum>())
					.Concat(CarrierDayNormalAttacks.Cast<Enum>());
			}

			return dayAttacks.Where(ship.CanDo);
		}

		private static List<DayAttackKind> IseClassSpecialAttacks => new List<DayAttackKind>
		{
			DayAttackKind.SeaAirMultiAngle,
			DayAttackKind.ZuiunMultiAngle
		};

		private static List<DayAttackKind> SurfaceShipDaySpecialAttacks => new List<DayAttackKind>
		{
			DayAttackKind.CutinMainMain,
			DayAttackKind.CutinMainAP,
			DayAttackKind.CutinMainRadar,
			DayAttackKind.CutinMainSub,
			DayAttackKind.DoubleShelling
		};

		private static List<DayAttackKind> SurfaceShipDayNormalAttacks => new List<DayAttackKind>
		{
			DayAttackKind.Shelling
		};

		private static List<DayAttackKind> CarrierDayNormalAttacks => new List<DayAttackKind>
		{
			DayAttackKind.AirAttack
		};

		private static List<DayAirAttackCutinKind> CarrierDaySpecialAttacks => new List<DayAirAttackCutinKind>
		{
			DayAirAttackCutinKind.FighterBomberAttacker,
			DayAirAttackCutinKind.BomberBomberAttacker,
			DayAirAttackCutinKind.BomberAttacker
		};

		private static bool CanDo(this IShipData ship, Enum attack) => attack switch
		{
			DayAttackKind.SeaAirMultiAngle => ship.HasMainGun() && ship.HasSuisei(2),
			DayAttackKind.ZuiunMultiAngle => ship.HasMainGun() && ship.HasZuiun(2),
			
			DayAttackKind.CutinMainMain => ship.HasSeaplane() && ship.HasMainGun(2) && ship.HasApShell(),
			DayAttackKind.CutinMainAP => ship.HasSeaplane() && ship.HasMainGun() && ship.HasApShell(),
			DayAttackKind.CutinMainRadar => ship.HasSeaplane() && ship.HasMainGun() && ship.HasSecondaryGun() &&
			                                ship.HasRadar(),
			DayAttackKind.CutinMainSub => ship.HasSeaplane() && ship.HasMainGun() && ship.HasSecondaryGun(),
			DayAttackKind.DoubleShelling => ship.HasSeaplane() && ship.HasMainGun(2),

			DayAttackKind.Shelling => ship.IsSurfaceShip(),

			DayAirAttackCutinKind.FighterBomberAttacker => ship.HasFighter() && ship.HasBomber() && ship.HasAttacker(),
			DayAirAttackCutinKind.BomberBomberAttacker => ship.HasBomber(2) && ship.HasAttacker(),
			DayAirAttackCutinKind.BomberAttacker => ship.HasBomber() && ship.HasAttacker(),

			DayAttackKind.AirAttack => ship.HasBomber() || ship.HasAttacker(),

			_ => false
		};

		private static bool HasMainGun(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Count(e => e != null && e.MasterEquipment.IsMainGun) >= count;

		private static bool HasSecondaryGun(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SecondaryGun);

		private static bool HasApShell(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.APShell);

		private static bool HasSeaplane(this IShipData ship) => ship.AllSlotInstance
			.Zip(ship.Aircraft, (e, size) => (e, size))
			.Any(s => s.size > 0 && s.e != null && s.e.MasterEquipment.IsSeaplane());

		private static bool HasRadar(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e != null && e.MasterEquipment.IsRadar);

		private static bool HasSuisei(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Count(e => e.IsSuisei()) >= count;

		private static bool HasZuiun(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Count(e => e.IsZuiun()) >= count;

		private static bool HasFighter(this IShipData ship) => ship.AllSlotInstance
			.Zip(ship.Aircraft, (e, size) => (e, size))
			.Any(s => s.size > 0 && s.e != null && s.e.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedFighter);

		private static bool HasBomber(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Zip(ship.Aircraft, (e, size) => (e, size))
			.Count(s => s.size > 0 && s.e != null && s.e.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedBomber) 
		    >= count;

		public static bool HasAttacker(this IShipData ship) => ship.AllSlotInstance
			.Zip(ship.Aircraft, (e, size) => (e, size))
			.Any(s => s.size > 0 && s.e != null && s.e.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedTorpedo);

		private static bool IsSeaplane(this IEquipmentDataMaster equip) => equip.CategoryType switch
		{
			EquipmentTypes.SeaplaneRecon => true,
			EquipmentTypes.SeaplaneBomber => true,
			_ => false
		};

		private static bool IsSuisei(this IEquipmentData? equip) => equip?.EquipmentId switch
		{
			EquipmentId.CarrierBasedBomber_SuiseiModel22_634AirGroup => true,
			EquipmentId.CarrierBasedBomber_SuiseiModel22_634AirGroupSkilled => true,
			EquipmentId.CarrierBasedBomber_SuiseiModel12_634AirGroupwType3ClusterBombs => true,
			_ => false
		};

		private static bool IsZuiun(this IEquipmentData? equip) => equip?.EquipmentId switch
		{
			EquipmentId.SeaplaneBomber_Zuiun => true,
			EquipmentId.SeaplaneBomber_Zuiun_634AirGroup => true,
			EquipmentId.SeaplaneBomber_ZuiunModel12 => true,
			EquipmentId.SeaplaneBomber_ZuiunModel12_634AirGroup => true,
			EquipmentId.SeaplaneBomber_Zuiun_631AirGroup => true,
			EquipmentId.SeaplaneBomber_Zuiun_634AirGroupSkilled => true,
			EquipmentId.SeaplaneBomber_ZuiunKaiNi_634AirGroup => true,
			EquipmentId.SeaplaneBomber_ZuiunKaiNi_634AirGroupSkilled => true,
			_ => false
		};

		private static bool IsIseClassK2(this IShipData ship) => ship.MasterShip.ShipId switch
		{
			ShipId.IseKaiNi => true,
			ShipId.HyuugaKaiNi => true,
			_ => false
		};

		private static bool IsSurfaceShip(this IShipData ship) => ship.MasterShip.ShipType switch
		{
			ShipTypes.Escort => true,
			ShipTypes.Destroyer => true,
			ShipTypes.LightCruiser => true,
			ShipTypes.TorpedoCruiser => true,
			ShipTypes.HeavyCruiser => true,
			ShipTypes.AviationCruiser => true,
			ShipTypes.Battlecruiser => true,
			ShipTypes.Battleship => true,
			ShipTypes.AviationBattleship => true,
			ShipTypes.SuperDreadnoughts => true,
			ShipTypes.Transport => true,
			ShipTypes.SeaplaneTender => true,
			ShipTypes.AmphibiousAssaultShip => true,
			ShipTypes.RepairShip => true,
			ShipTypes.SubmarineTender => true,
			ShipTypes.TrainingCruiser => true,
			ShipTypes.FleetOiler => true,
			_ => false
		};
	}
}