using System.Collections.Generic;

namespace KancolleProgress.Models
{
#if false
	public record ShipTypeGroup(string Label, IEnumerable<ShipClassGroup> ClassGroups);
#else
	public class ShipTypeGroup
	{
		public string Label { get; }
		public IEnumerable<ShipClassGroup> ClassGroups { get; }

		public ShipTypeGroup(string Label, IEnumerable<ShipClassGroup> ClassGroups)
		{
			this.Label = Label;
			this.ClassGroups = ClassGroups;
		}
	}
#endif
}