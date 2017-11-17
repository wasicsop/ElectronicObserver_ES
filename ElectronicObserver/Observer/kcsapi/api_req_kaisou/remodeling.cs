﻿using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou
{

	public class remodeling : APIBase
	{

		public override bool IsRequestSupported => true;
		public override bool IsResponseSupported => false;

		public override void OnRequestReceived(Dictionary<string, string> data)
		{

			int id = int.Parse(data["api_id"]);
			var ship = KCDatabase.Instance.Ships[id];
			Utility.Logger.Add(2, string.Format("{0} Lv. {1} has been succefully remodelled.", ship.MasterShip.RemodelAfterShip.NameWithClass, ship.Level));

			KCDatabase.Instance.Fleet.LoadFromRequest(APIName, data);

			base.OnRequestReceived(data);
		}

		public override string APIName => "api_req_kaisou/remodeling";
	}

}
