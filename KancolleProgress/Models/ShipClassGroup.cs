using System.Collections.Generic;
using KancolleProgress.ViewModels;

namespace KancolleProgress.Models
{
#if false
	public record ShipClassGroup(IEnumerable<ShipViewModel> Ships);
#else
	public class ShipClassGroup
	{
		public IEnumerable<ShipViewModel> Ships { get; }

		public ShipClassGroup(IEnumerable<ShipViewModel> Ships)
		{
			this.Ships = Ships;
		}
	}
#endif
}