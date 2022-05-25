using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_get_member;

public class slot_item : APIBase
{


	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase db = KCDatabase.Instance;


		db.Equipments.Clear();
		foreach (var elem in data)
		{

			var eq = new EquipmentData();
			eq.LoadFromResponse(APIName, elem);
			db.Equipments.Add(eq);

		}

		db.Battle.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_get_member/slot_item";
}
