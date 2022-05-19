using System.Collections.Generic;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public class EventPhaseModel
{
	public string Name { get; set; } = "";
	public bool IsFinished { get; set; }
	public List<int> PhaseLockGroups { get; set; } = new();
	public List<int> PhaseShips { get; set; } = new();
}
