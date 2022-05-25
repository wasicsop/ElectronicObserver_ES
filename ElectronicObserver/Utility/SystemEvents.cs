using System;

namespace ElectronicObserver.Utility;

/// <summary>
/// システムイベントを扱います。
/// </summary>
public static class SystemEvents
{

	public static bool UpdateTimerEnabled { get; set; }

	public static event Action UpdateTimerTick = delegate { };
	public static event Action SystemShuttingDown = delegate { };


	static SystemEvents()
	{
		UpdateTimerEnabled = true;
	}

	internal static void OnUpdateTimerTick()
	{
		if (UpdateTimerEnabled)
			UpdateTimerTick();
	}
	internal static void OnSystemShuttingDown() { SystemShuttingDown(); }

}
