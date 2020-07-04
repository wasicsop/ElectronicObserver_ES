using ElectronicObserver.Utility.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElectronicObserver.Data
{
	public class TsunDbRouting : TsunDbEntity
	{
		protected override string Url => "routing";

		#region Json Properties
		[JsonProperty("sortiedFleet")]
		private int SortiedFleet;

		[JsonProperty("fleetType")]
		private int FleetType;

		[JsonProperty("nodeInfo")]
		private TsunDbNodeInfo NodeInfo;

		[JsonProperty("map")]
		private string Map;

		[JsonProperty("hqLvl")]
		private int HqLvl;

		[JsonProperty("cleared")]
		private int Cleared;

		[JsonProperty("edgeID")]
		private List<int> EdgeID = new List<int>();

		[JsonProperty("nextRoute")]
		private int NextRoute;

		[JsonProperty("fleet1")]
		private List<TsunDbShipData> Fleet1 = new List<TsunDbShipData>();

		[JsonProperty("fleet2")]
		private List<TsunDbShipData> Fleet2 = new List<TsunDbShipData>();

		[JsonProperty("fleetSpeed")]
		private int FleetSpeed;

		[JsonProperty("los")]
		private double[] LOS = new double[4];

		[JsonProperty("fleetids")]
		private List<int> FleetIds = new List<int>();

		[JsonProperty("fleetlevel")]
		private int FleetLevel;

		[JsonProperty("fleetoneequips")]
		private List<int> FleetOneEquips = new List<int>();

		[JsonProperty("fleetoneexslots")]
		private List<int> FleetOneExSlots = new List<int>();

		[JsonProperty("fleetonetypes")]
		private List<int> FleetOneTypes = new List<int>();

		[JsonProperty("fleettwoequips")]
		private List<int> FleetTwoEquips = new List<int>();

		[JsonProperty("fleettwoexslots")]
		private List<int> FleetTwoExSlots = new List<int>();

		[JsonProperty("fleettwotypes")]
		private List<int> FleetTwoTypes = new List<int>();
		#endregion

		#region public methods
		/// <summary>
		/// Process Start node request
		/// </summary>
		/// <param name="api_data"></param>
		public void ProcessStart(dynamic api_data)
		{
			KCDatabase db = KCDatabase.Instance;

			this.SortiedFleet = db.Fleet.Fleets
				.Where(fleet => fleet.Value.IsInSortie)
				.Select(fleet => fleet.Value.FleetID)
				.Min();

			this.FleetType = db.Fleet.CombinedFlag;

			// --- Sets amount of nodes value in NodeInfo
			object[] cell_data = api_data["api_cell_data"];
			this.NodeInfo = new TsunDbNodeInfo(cell_data.Length);

			// --- Sets the map value
			this.Map = string.Format("{0}-{1}", db.Battle.Compass.MapAreaID, db.Battle.Compass.MapInfoID);

			// LBAS THINGS
			//this.processCellData(http); TODO

			this.ProcessNext(api_data);
		}

		/// <summary>
		/// Process next node request
		/// </summary>
		/// <param name="api_data"></param>
		public void ProcessNext(dynamic api_data)
		{
			CleanOnNext();

			KCDatabase db = KCDatabase.Instance;

			// --- Sets player's HQ level
			this.HqLvl = db.Admiral.Level;

			// Sets the map id
			/*const mapId = this.currentMap.join('');
			const mapData = this.mapInfo.find(i => i.api_id == mapId) || { };*/

			// --- Sets whether the map is cleared or not
			this.Cleared = db.Battle.Compass.MapInfo.IsCleared ? 1 : 0;

			// --- Charts the route array using edge ids as values
			this.EdgeID.Add((int)api_data.api_no);

			// --- All values related to node types
			this.NodeInfo.ProcessNext(api_data);

			// --- Checks whether the fleet has hit a dead end or not
			this.NextRoute = (int)api_data.api_next;

			// --- Fleet 1
			this.Fleet1 = this.PrepareFleet(db.Fleet[this.SortiedFleet]);

			// --- Fleet 2
			if (this.FleetType > 0 && this.SortiedFleet == 1)
			{
				this.Fleet2 = this.PrepareFleet(db.Fleet[2]);
			}

			foreach (var ship in this.Fleet1)
			{
				this.FleetIds.Add(ship.Id);
				this.FleetLevel += ship.Level;
				this.FleetOneEquips.AddRange(ship.Equip);
				this.FleetOneExSlots.Add(ship.Exslot);
				this.FleetOneTypes.Add(ship.Type);
			}


			if (this.Fleet2.Count > 0)
			{
				foreach (var ship in this.Fleet1) 
				{
					this.FleetIds.Add(ship.Id);
					this.FleetLevel += ship.Level;
					this.FleetTwoEquips.AddRange(ship.Equip);
					this.FleetTwoExSlots.Add(ship.Exslot);
					this.FleetTwoTypes.Add(ship.Type);
				}
			}
		}

		#endregion

		#region Private methods
		/// <summary>
		/// Clean all data before loading a node data
		/// </summary>
		private void CleanOnNext()
		{
			this.LOS = new double[] { 0, 0, 0, 0 };
			this.Fleet1 = new List<TsunDbShipData>();
			this.Fleet2 = new List<TsunDbShipData>();
			this.FleetSpeed = 20;
			this.FleetIds = new List<int>();
			this.FleetLevel = 0;
			this.FleetOneEquips = new List<int>();
			this.FleetOneExSlots = new List<int>();
			this.FleetOneTypes = new List<int>();
			this.FleetTwoEquips = new List<int>();
			this.FleetTwoExSlots = new List<int>();
			this.FleetTwoTypes = new List<int>();
		}

		/// <summary>
		/// Prepare fleet data
		/// </summary>
		/// <param name="fleetData"></param>
		/// <returns></returns>
		private List<TsunDbShipData> PrepareFleet(FleetData fleetData)
		{
			List<TsunDbShipData> shipsData = new List<TsunDbShipData>();

			var members = fleetData.MembersInstance.Where(s => s != null);

			this.FleetSpeed = members.Select(s => s.Speed).DefaultIfEmpty(20).Min();

			foreach (int weight in new int[4] { 1, 2, 3, 4 })
			{
				this.LOS[weight - 1] += Calculator.GetSearchingAbility_New33(fleetData, weight, this.HqLvl);
			}

			foreach (IShipData ship in fleetData.MembersInstance)
			{
				if (ship is null) continue;

				shipsData.Add(new TsunDbShipData(ship)
				{
					Flee = fleetData.EscapedShipList.Contains(ship.MasterID) ? 1 : 0
				});
			}

			return shipsData;
		}
		#endregion
	}
}
