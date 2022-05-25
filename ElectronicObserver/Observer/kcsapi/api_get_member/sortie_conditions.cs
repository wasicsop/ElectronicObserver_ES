namespace ElectronicObserver.Observer.kcsapi.api_get_member;

public class sortie_conditions : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{
		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_get_member/sortie_conditions";
}
