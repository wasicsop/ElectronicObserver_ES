using System;

namespace ElectronicObserver.Data
{
	public class TsunDbSubmissionManager : ResponseWrapper
	{
		/// <summary>
		/// Response wrapper for getting API data
		/// </summary>
		/// <param name="apiname">apiname from battle</param>
		/// <param name="data">api_data or RawData</param>
		public override void LoadFromResponse(string apiname, dynamic data)
		{
			KCDatabase db = KCDatabase.Instance;

			try
			{
				switch (apiname)
				{
					case "api_req_sortie/battleresult":
						if (db.Ships.Count < db.Admiral.MaxShipCount && db.Equipments.Count < db.Admiral.MaxEquipmentCount)
						{
							new ShipDrop(data).SendData();
							new ShipDropLoc(data).SendData();
						}
						break;
				}
			}
			catch (Exception ex)
			{
				Utility.ErrorReporter.SendErrorReport(ex, "TsunDb Submission module");
			}
		}
	}
}
