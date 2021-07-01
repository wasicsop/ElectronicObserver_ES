using System;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Utility.Data
{
	public static class ShipDataExtensions
	{
		public static double Accuracy(this IShipData ship) =>
			2 * Math.Sqrt(ship.Level) + 1.5 * Math.Sqrt(ship.LuckTotal);

		public static int MainGunCount(this IShipData ship) => ship.AllSlotInstance
			.Count(e => e?.MasterEquipment.IsMainGun == true);

		public static bool HasMainGun(this IShipData ship, int count = 1) => ship.MainGunCount() >= count;

		public static int SecondaryGunCount(this IShipData ship) => ship.AllSlotInstance
			.Count(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SecondaryGun);

		public static bool HasSecondaryGun(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SecondaryGun);

		public static bool HasApShell(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.APShell);

		public static bool HasSeaplane(this IShipData ship) => ship.AllSlotInstance
			.Zip(ship.Aircraft, (e, size) => (e, size))
			.Any(s => s.size > 0 && s.e?.MasterEquipment.IsSeaplane() == true);

		public static bool HasRadar(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.IsRadar == true);

		public static bool HasSuisei634(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Count(e => e.IsSuisei634()) >= count;

		public static bool HasZuiun(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Count(e => e.IsZuiun()) >= count;

		public static bool HasFighter(this IShipData ship) => ship.AllSlotInstance
			.Zip(ship.Aircraft, (e, size) => (e, size))
			.Any(s => s.size > 0 && s.e?.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedFighter);

		public static bool HasBomber(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Zip(ship.Aircraft,(e, size) => (e, size))
			.Count(s => s.size > 0 && s.e?.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedBomber)
		    >= count;

		public static bool HasAttacker(this IShipData ship) => ship.AllSlotInstance
			.Zip(ship.Aircraft, (e, size) => (e, size))
			.Any(s => s.size > 0 && s.e?.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedTorpedo);

		public static bool HasTorpedo(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Count(e => e?.MasterEquipment.IsTorpedo == true)
		    >= count;

		public static bool HasSkilledLookouts(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SurfaceShipPersonnel);

		public static bool HasDestroyerSkilledLookouts(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.EquipmentId == EquipmentId.SurfaceShipPersonnel_TorpedoSquadronSkilledLookouts);

		public static bool HasDrum(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.TransportContainer);

		public static bool IsNightCarrier(this IShipData ship) =>
			ship.HasNightAviationPersonnel() ||
			ship.MasterShip.ShipId switch
			{
				ShipId.SaratogaMkII => true,
				ShipId.AkagiKaiNiE => true,
				_ => false
			};

		private static bool HasNightAviationPersonnel(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.IsNightAviationPersonnel == true);

		public static bool HasNightFighter(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Count(e => e?.MasterEquipment.IsNightFighter == true)
			>= count;

		public static bool HasNightAttacker(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.IsNightAttacker == true);

		public static bool HasNightAircraft(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Count(e => e?.MasterEquipment.IsNightAircraft == true || e?.IsNightCapableAircraft() == true)
		    >= count;

		public static bool HasNightPhototoubePlane(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.EquipmentId == EquipmentId.CarrierBasedBomber_SuiseiModel12_wType31PhotoelectricFuzeBombs);

		public static bool HasSwordfish(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.IsSwordfish ?? false);

		public static bool HasLateModelTorp(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Count(e => e?.MasterEquipment.IsLateModelTorpedo == true)
		    >= count;

		public static bool HasSubmarineEquipment(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SubmarineEquipment);

		public static bool HasNightRecon(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.EquipmentId switch
			{
				EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon => true,
				_ => false
			});

		public static bool HasStarShell(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.StarShell);

		public static bool HasSearchlight(this IShipData ship) => ship.AllSlotInstance
			.Any(e => e?.MasterEquipment.CategoryType switch
			{
				EquipmentTypes.Searchlight => true,
				EquipmentTypes.SearchlightLarge => true,
				_ => false
			});

		public static bool IsIseClassK2(this IShipData ship) => ship.MasterShip.ShipId switch
		{
			ShipId.IseKaiNi => true,
			ShipId.HyuugaKaiNi => true,
			_ => false
		};

		public static bool IsDestroyer(this IShipData ship) => ship.MasterShip.ShipType switch
		{
			ShipTypes.Destroyer => true,
			_ => false
		};

		public static bool IsSurfaceShip(this IShipData ship) => ship.MasterShip.ShipType switch
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

		public static bool IsSpecialNightCarrier(this IShipData ship) => ship.MasterShip.ShipId switch
		{
			ShipId.GrafZeppelin => true,
			ShipId.GrafZeppelinKai => true,
			ShipId.Saratoga => true,
			ShipId.TaiyouKaiNi => true,
			ShipId.ShinyouKaiNi => true,

			_ => false
		};

		public static bool IsArkRoyal(this IShipData ship) => ship.MasterShip.ShipId switch
		{
			ShipId.ArkRoyal => true,
			ShipId.ArkRoyalKai => true,

			_ => false
		};

		public static double GetHPDamageBonus(this IShipData ship) => ship.HPRate switch
		{
			_ when ship.HPRate <= 0.25 => 0.4,
			_ when ship.HPRate <= 0.5 => 0.7,
			_ => 1,
		};

		public static double GetLightCruiserDamageBonus(this IShipData ship)
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

		public static double GetItalianDamageBonus(this IShipData ship)
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
	}
}
