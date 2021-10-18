using System;
using DynaJson;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data;

public class TsunDbSubmissionManager : ResponseWrapper
{
	private TsunDbRouting RoutingSubmission = new TsunDbRouting();

	/// <summary>
	/// Response wrapper for getting API data
	/// </summary>
	/// <param name="apiname">apiname from battle</param>
	/// <param name="data">api_data or RawData</param>
	public override void LoadFromResponse(string apiname, dynamic data)
	{
		if (Configuration.Config.Control.SubmitDataToTsunDb != true) return;

		KCDatabase db = KCDatabase.Instance;

		try
		{
			JsonObject jData = (JsonObject)data;

			switch (apiname)
			{
				case "api_req_sortie/battleresult":
					if (db.Ships.Count < db.Admiral.MaxShipCount && db.Equipments.Count < db.Admiral.MaxEquipmentCount)
					{
						new ShipDrop(data).SendData();
						new ShipDropLoc(data).SendData();
					}
					break;
				case "api_req_map/start":
				{
					RoutingSubmission = jData.IsDefined("api_eventmap") ? new TsunDbEventRouting() : new TsunDbRouting();

					RoutingSubmission.ProcessStart(data);

					if (RoutingSubmission is TsunDbEventRouting routing)
					{
						routing.ProcessEvent(data);
					}

					RoutingSubmission.SendData();
					break;
				}
				case "api_req_map/next":
				{
					RoutingSubmission.ProcessNext(data);

					if (RoutingSubmission is TsunDbEventRouting routing)
					{
						routing.ProcessEvent(data);
					}

					RoutingSubmission.SendData();
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, "TsunDb Submission module");
		}
	}
}
