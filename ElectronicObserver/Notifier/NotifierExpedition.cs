using System;
using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Notifier;

/// <summary>
/// 遠征帰投通知を扱います。
/// </summary>
public class NotifierExpedition : NotifierBase
{

	private Dictionary<int, bool> processedFlags;


	public NotifierExpedition()
		: base()
	{
		Initialize();
	}

	public NotifierExpedition(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
		: base(config)
	{
		Initialize();
	}


	private void Initialize()
	{
		DialogData.Title = NotifierRes.ExpeditionTitle;
		processedFlags = new Dictionary<int, bool>();
	}


	protected override void UpdateTimerTick()
	{

		foreach (var fleet in KCDatabase.Instance.Fleet.Fleets.Values)
		{

			if (!processedFlags.ContainsKey(fleet.FleetID))
				processedFlags.Add(fleet.FleetID, false);

			if (fleet.ExpeditionState != 0)
			{
				if (!processedFlags[fleet.FleetID] && (int)(fleet.ExpeditionTime - DateTime.Now).TotalMilliseconds <= AccelInterval)
				{

					processedFlags[fleet.FleetID] = true;
					Notify(fleet.FleetID, fleet.ExpeditionDestination);

				}

			}
			else
			{
				processedFlags[fleet.FleetID] = false;
			}

		}

	}

	public void Notify(int fleetID, int destination)
	{

		DialogData.Message = string.Format(NotifierRes.ExpeditionText,
			fleetID, KCDatabase.Instance.Fleet[fleetID].Name, KCDatabase.Instance.Mission[destination].DisplayID, KCDatabase.Instance.Mission[destination].NameEN);

		base.Notify();
	}
}
