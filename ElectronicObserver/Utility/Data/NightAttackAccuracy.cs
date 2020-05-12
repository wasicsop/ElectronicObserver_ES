using System;
using System.Linq;
using ElectronicObserver.Data;

namespace ElectronicObserver.Utility.Data
{
	public static class NightAttackAccuracy
	{
		public static double GetNightAttackAccuracy(this IShipData ship, Enum attack, IFleetData fleet)
		{
			int baseAccuracy = fleet.BaseAccuracy();
			double shipAccuracy = ship.Accuracy();
			double equipAccuracy = ship.AllSlotInstance
				.Where(e => e != null)
				.Sum(e => e.MasterEquipment.Accuracy);

			return (baseAccuracy + shipAccuracy + equipAccuracy)
			       * ship.ConditionMod()
			       * AttackKindMod(attack);
		}

		private static int BaseAccuracy(this IFleetData fleet) => 69;

		private static double AttackKindMod(Enum attack) => attack switch
		{
			NightAttackKind.CutinMainMain => 2,

			NightAttackKind.CutinTorpedoTorpedo => 1.65,

			NightAttackKind.CutinMainTorpedo => 1.5,
			NightAttackKind.CutinMainSub => 1.5,

			NightAttackKind.DoubleShelling => 1.1,

			NightAttackKind.NormalAttack => 1,

			// todo are we sure this is the same as TCI?
			NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment => 1.65,
			NightTorpedoCutinKind.LateModelTorpedo2 => 1.65,

			_ => 1
		};
	}
}