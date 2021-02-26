using System.Collections.Generic;
using KancolleProgress.ViewModels;

namespace KancolleProgress.Models
{
	public record ShipClassGroup(IEnumerable<ShipViewModel> Ships);
}