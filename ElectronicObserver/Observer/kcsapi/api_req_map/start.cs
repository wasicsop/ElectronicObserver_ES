﻿using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_map
{

	public class start : APIBase
	{

		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);

			base.OnResponseReceived((object)data);


			// 表示順の関係上、UIの更新をしてからデータを更新する
			if (KCDatabase.Instance.Battle.Compass.EventID == 3)
			{
				next.EmulateWhirlpool();
			}

		}


		public override bool IsRequestSupported => true;

		public override void OnRequestReceived(Dictionary<string, string> data)
		{

			KCDatabase.Instance.Fleet.LoadFromRequest(APIName, data);

			int deckID = int.Parse(data["api_deck_id"]);
			int maparea = int.Parse(data["api_maparea_id"]);
			int mapinfo = int.Parse(data["api_mapinfo_no"]);

			Utility.Logger.Add(2, string.Format("#{0} 「{1}」 has sortied to 「{2}-{3} {4}」.", deckID, KCDatabase.Instance.Fleet[deckID].Name, maparea, mapinfo, KCDatabase.Instance.MapInfo[maparea * 10 + mapinfo].Name));

			base.OnRequestReceived(data);
		}


		public override string APIName => "api_req_map/start";
	}

}
