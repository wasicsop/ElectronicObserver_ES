using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_map;

public class air_raid : APIBase
{
	public override void OnResponseReceived(dynamic data)
	{
		KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_req_map/air_raid";
}
