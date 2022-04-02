using System.Collections.Generic;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public record MapNode(string Letter, List<int>? Ids = null)
{
	public override string ToString()
	{
		// special case for *
		if (Ids is null) return Letter;
		if (Utility.Configuration.Config.UI.UseOriginalNodeId) return Letter;

		return $"{Letter} ({string.Join(',', Ids)})";
	}
}