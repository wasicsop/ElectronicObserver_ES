using System.Linq;
using ElectronicObserverTypes;

namespace ElectronicObserver.Utility.Data
{
	public static class FleetDataExtensions
	{
		public static bool HasNightRecon(this IFleetData fleet) => fleet.MembersWithoutEscaped
			.Any(s => s?.HasNightRecon() ?? false);

		public static bool HasStarShell(this IFleetData fleet) => fleet.MembersWithoutEscaped
			.Any(s => s?.HasStarShell() ?? false);

		public static bool HasSearchlight(this IFleetData fleet) => fleet.MembersWithoutEscaped
			.Any(s => s?.HasSearchlight() ?? false);
	}
}