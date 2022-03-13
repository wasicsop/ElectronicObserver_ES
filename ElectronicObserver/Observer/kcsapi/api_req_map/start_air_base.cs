using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_map;

public class start_air_base : APIBase
{
	public override string APIName => "api_req_map/start_air_base";

	public override void OnRequestReceived(Dictionary<string, string> data)
	{
		base.OnRequestReceived(data);

		KCDatabase db = KCDatabase.Instance;

		// --- Read strike points
		foreach (BaseAirCorpsData airCorpBase in db.BaseAirCorps.Values)
		{
			airCorpBase.LoadFromRequest(APIName, data);
		}
	}

	public override bool IsRequestSupported => true;
}
