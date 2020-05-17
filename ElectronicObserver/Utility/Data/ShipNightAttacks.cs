using System;
using System.Linq;
using System.Collections.Generic;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Utility.Data
{
	public static class ShipNightAttacks
	{
		public static IEnumerable<Enum> GetNightAttacks(this IShipData ship)
		{
			IEnumerable<Enum> nightAttacks = new List<Enum>();

			if (ship.MasterShip.IsSubmarine)
			{
				// sub TCI or normal TCI
				IEnumerable<Enum> specialAttack = SubmarineNightSpecialAttacks.Cast<Enum>()
					.Concat(SurfaceShipNightSpecialAttacks.Cast<Enum>())
					.FirstOrDefault(ship.CanDo!) switch
					{
						Enum attack => new List<Enum> { attack },
						_ => Enumerable.Empty<Enum>()
					};

				nightAttacks = nightAttacks
					.Concat(specialAttack)
					.Concat(NightTorpedoAttacks.Cast<Enum>());
			}

			if (ship.IsSurfaceShip())
			{
				if (ship.IsDestroyer())
				{
					nightAttacks = nightAttacks.Concat(DestroyerSpecialAttacks.Cast<Enum>());
				}

				IEnumerable<Enum> specialAttack =
					SurfaceShipNightSpecialAttacks.Cast<Enum>().FirstOrDefault(ship.CanDo!) switch
					{
						NightAttackKind attack => new List<Enum> {attack},
						_ => Enumerable.Empty<Enum>()
					};

				nightAttacks = nightAttacks
					.Concat(specialAttack)
					.Concat(SurfaceShipNightNormalAttacks.Cast<Enum>());
			}

			if (ship.IsNightCarrier())
			{
				nightAttacks = nightAttacks
					.Concat(CarrierNightSpecialAttacks.Cast<Enum>())
					.Concat(CarrierNightNormalAttacks.Cast<Enum>());
			}
			else if (ship.IsSpecialNightCarrier() || ship.IsArkRoyal() && ship.HasSwordfish())
			{
				nightAttacks = nightAttacks
					.Concat(new List<Enum> { NightAttackKind.DoubleShelling })
					.Concat(new List<Enum> { NightAttackKind.Shelling });
			}

			return nightAttacks.Where(ship.CanDo!);
		}

		private static List<NightAttackKind> DestroyerSpecialAttacks => new List<NightAttackKind>
		{
			NightAttackKind.CutinTorpedoRadar,
			NightAttackKind.CutinTorpedoPicket,
		};

		private static List<NightAttackKind> SurfaceShipNightSpecialAttacks => new List<NightAttackKind>
		{
			NightAttackKind.CutinTorpedoTorpedo,
			NightAttackKind.CutinMainMain,
			NightAttackKind.CutinMainSub,
			NightAttackKind.CutinMainTorpedo,
			NightAttackKind.DoubleShelling,
		};

		private static List<CvnciKind> CarrierNightSpecialAttacks => new List<CvnciKind>
		{
			CvnciKind.FighterFighterAttacker,
			CvnciKind.FighterAttacker,
			CvnciKind.Phototube,
			CvnciKind.FighterOtherOther
		};

		private static List<NightAttackKind> SurfaceShipNightNormalAttacks => new List<NightAttackKind>
		{
			NightAttackKind.Shelling
		};

		private static List<NightAttackKind> NightTorpedoAttacks => new List<NightAttackKind>
		{
			NightAttackKind.Torpedo
		};

		private static List<NightAttackKind> CarrierNightNormalAttacks => new List<NightAttackKind>
		{
			NightAttackKind.AirAttack
		};

		private static List<NightTorpedoCutinKind> SubmarineNightSpecialAttacks = new List<NightTorpedoCutinKind>
		{
			NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment,
			NightTorpedoCutinKind.LateModelTorpedo2,
		};

		private static bool CanDo(this IShipData ship, Enum attack) => attack switch
		{
			NightAttackKind.CutinTorpedoTorpedo => ship.HasTorpedo(2),
			NightAttackKind.CutinMainMain => ship.HasMainGun(3),
			NightAttackKind.CutinMainSub => ship.HasMainGun(2) && ship.HasSecondaryGun(),
			NightAttackKind.CutinMainTorpedo => ship.HasMainGun() && ship.HasSecondaryGun(),
			NightAttackKind.DoubleShelling => ship.MainGunCount() + ship.SecondaryGunCount() >= 2,

			NightAttackKind.CutinTorpedoRadar => ship.HasMainGun() && ship.HasTorpedo() && ship.HasRadar(),
			NightAttackKind.CutinTorpedoPicket => ship.HasTorpedo() && ship.HasSkilledLookouts() && ship.HasRadar(),

			CvnciKind.FighterFighterAttacker => ship.HasNightFighter(2) && ship.HasNightAttacker(),
			CvnciKind.FighterAttacker => ship.IsNightCarrier() && ship.HasNightFighter() && ship.HasNightAttacker(),
			CvnciKind.Phototube => ship.HasNightPhototoubePlane() &&
			                       (ship.HasNightFighter() || ship.HasNightAttacker()),
			CvnciKind.FighterOtherOther => ship.HasNightFighter() && ship.HasNightAircraft(3),

			NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment => ship.HasLateModelTorp() &&
			                                                            ship.HasSubmarineEquipment(),
			NightTorpedoCutinKind.LateModelTorpedo2 => ship.HasLateModelTorp(2),

			NightAttackKind.Shelling => true,
			NightAttackKind.Torpedo => true,
			NightAttackKind.AirAttack => ship.HasNightAircraft(),

			_ => false
		};
	}
}