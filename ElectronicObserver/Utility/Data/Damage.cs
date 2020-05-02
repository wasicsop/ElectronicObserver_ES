using System;

namespace ElectronicObserver.Utility.Data
{
	public static class Damage
	{
		public static double Cap(double damage, double cap)
		{
			if (damage < cap) return damage;

			return cap + Math.Sqrt(damage - cap);
		}
	}
}