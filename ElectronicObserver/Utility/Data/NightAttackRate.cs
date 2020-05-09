using System;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Utility.Data
{
	public static class NightAttackRate
	{
		public static double GetNightAttackRate(this IShipData ship, Enum attack, IFleetData fleet)
		{
			if (attack is NightAttackKind.DoubleShelling) return 0.99;

			int attackMod = AttackMod(attack);
			if (attackMod == 0) return 0;

			double luckLevelPart = ship.LuckTotal switch
			{
				int luck when luck < 50 => 15 + luck + 0.75 * Math.Sqrt(ship.Level),
				int luck => 65 + Math.Sqrt(luck - 50) + 0.8 * Math.Sqrt(ship.Level)
			};

			double baseRate = luckLevelPart + FlagshipBonus(fleet, ship) + ship.HPBonus() + ship.SkilledLookoutsBonus();

			return Math.Floor(baseRate) / attackMod;
		}

		private static double FlagshipBonus(IFleetData fleet, IShipData ship) => fleet.MembersInstance
				.FirstOrDefault()?.MasterShip.ShipId switch
			{
				ShipId id when id == ship.MasterShip.ShipId => 15,
				_ => 0
			};

		private static int HPBonus(this IShipData ship) => ship.HPRate switch
		{
			_ when ship.HPRate > 0.5 => 0,
			_ when ship.HPRate > 0.25 => 18,
			_ => 0,
		};

		private static int SkilledLookoutsBonus(this IShipData ship) => ship.HasSkilledLookouts() switch
		{
			true => 5,
			false => 0
		};

		private static int AttackMod(Enum attack) => attack switch
		{
			NightAttackKind.CutinTorpedoTorpedo => 122,
			NightAttackKind.CutinMainMain => 140,
			NightAttackKind.CutinMainSub => 130,
			NightAttackKind.CutinMainTorpedo => 115,

			NightAttackKind.CutinTorpedoRadar => 130,
			NightAttackKind.CutinTorpedoPicket => 150,

			CvnciKind.FighterFighterAttacker => 105,
			CvnciKind.FighterAttacker => 115,
			CvnciKind.Phototube => 115,
			CvnciKind.FighterOtherOther => 125,

			// todo: are we sure this is the same as normal TCI?
			NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment => 122,
			NightTorpedoCutinKind.LateModelTorpedo2 => 122,

			_ => 0
		};
	}
}