using System;
using System.Collections.Generic;
using ElectronicObserver.ReplayJSON;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using ElectronicObserver.Utility.Mathematics;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
	public class ReplayManager : ResponseWrapper
	{
		public static readonly string ReplayLogPath = "Replays";

		//TODO:fill in remaining values
		ReplayRoot Battle_replay = new ReplayRoot()
		{
			Id = 1,
			Support1 = 0,
			Support2 = 0,
			Combined = 0,
			Fleetnum = 1,
			Fleet1 = new List<Fleet1>(),
			Fleet2 = new List<Fleet2>(),
			Fleet3 = new List<Fleet3>(),
			Fleet4 = new List<Fleet4>(),
			Lbas = new List<Lba>(),
			Battles = new List<ReplayJSON.Battle>(1)
		};

		public Fleet1 Fleet1Data { get; set; }
		public Fleet2 Fleet2Data { get; set; }
		public Fleet3 Fleet3Data { get; set; }
		public Fleet4 Fleet4Data { get; set; }
		public Lba LbaData { get; set; }
		public Plane PlaneData { get; set; }
		public Range RangeData { get; set; }
		public dynamic DayData = null;
		public dynamic YasenData = null;

		/// <summary>
		///response wrapper for getting API data
		/// </summary>
		/// <param name="apiname">apiname from battle</param>
		/// <param name="data">api_data or RawData</param>
		public override void LoadFromResponse(string apiname, dynamic data)
		{
			KCDatabase db = KCDatabase.Instance;
			if (Battle_replay.Battles.Count < 1)
			{
				Battle_replay.Battles.Add(new ReplayJSON.Battle());
			}
			switch (apiname)
			{
				case "api_req_map/start":
				case "api_req_map/next":
					Battle_replay.Battles[0].Node = db.Battle.Compass.Destination;
					Battle_replay.Mapnum = db.Battle.Compass.MapInfoID;
					Battle_replay.World = db.Battle.Compass.MapAreaID;
					Battle_replay.Battles[0].SortieId = 1;
					Battle_replay.Battles[0].Yasen = new object();

					break;

				case "api_req_sortie/battle": // normal day 
					DayData = data;
					break;

				case "api_req_battle_midnight/battle": // normal night
					YasenData = data;
					break;

				case "api_req_battle_midnight/sp_midnight": // night node
					YasenData = data;
					break;

				case "api_req_sortie/airbattle": // single air raid
					DayData = data;
					break;

				case "api_req_sortie/ld_airbattle": // single air raid
					DayData = data;
					break;

				case "api_req_sortie/night_to_day": // single night to day
					DayData = data;

					break;

				case "api_req_sortie/ld_shooting": // single fleet radar ambush
					DayData = data;
					break;

				case "api_req_combined_battle/battle": //combined normal
					DayData = data;
					break;

				case "api_req_combined_battle/midnight_battle": // combined day to night					
					YasenData = data;
					break;

				case "api_req_combined_battle/sp_midnight":// combined
					YasenData = data;
					break;

				case "api_req_combined_battle/airbattle": // combined air exchange ?
					DayData = data;
					break;

				case "api_req_combined_battle/battle_water": // CTF TCF Combined battle
					DayData = data;
					break;

				case "api_req_combined_battle/ld_airbattle": // air raid
					DayData = data;
					break;

				case "api_req_combined_battle/ec_battle": // CTF enemy combined battle
					DayData = data;

					break;

				case "api_req_combined_battle/ec_midnight_battle": // combined night battle
					YasenData = data;
					break;

				case "api_req_combined_battle/ec_night_to_day": // enemy combined night to day
					DayData = data;
					break;

				case "api_req_combined_battle/each_battle": //STF combined vs combined
					DayData = data;
					break;

				case "api_req_combined_battle/each_battle_water": // STF combined
					DayData = data;

					break;

				case "api_req_combined_battle/ld_shooting": // combined radar ambush
					DayData = data;
					break;

				case "api_req_practice/battle": //pvp day
					DayData = data;
					break;

				case "api_req_practice/midnight_battle": // pvp night
					YasenData = data;
					break;

				case "api_req_sortie/battleresult":
				case "api_req_combined_battle/battleresult":
				case "api_req_practice/battle_result":
					Battle_replay.Battles[0].Drop = db.Battle.Result.DroppedShipID;
					Battle_replay.Time = DateTime.Now.Ticks / 1000;

					SaveReplay();
					break;

				case "api_port/port":
					Battle_replay.Combined = db.Fleet.CombinedFlag;
					break;
			}
		}

	

		private void GetNodeSupport()
		{
			KCDatabase db = KCDatabase.Instance;
			var fleets = db.Fleet.Fleets.Values.ToList();
			foreach(var fleet in fleets)
			{
				if(fleet.ExpeditionDestination == 33 || fleet.ExpeditionDestination == 301)
				{
					Battle_replay.Support1 = 1;
					Utility.Logger.Add(2, "Node support active");
				}
				else
				{
					Battle_replay.Support1 = 0;
					Utility.Logger.Add(2, "Node support  not active");
				}
			}
		}

		private void GetBossSupport()
		{
			KCDatabase db = KCDatabase.Instance;
			var fleets = db.Fleet.Fleets.Values.ToList();
			foreach (var fleet in fleets)
			{
				if (fleet.ExpeditionDestination == 34 || fleet.ExpeditionDestination == 302)
				{
					Battle_replay.Support2 = 1;
					Utility.Logger.Add(2, "Boss support active");
				}
				else
				{
					Battle_replay.Support2 = 0;
					Utility.Logger.Add(2, "Boss support not active");
				}
			}
		}

		private void GetLBAS()
		{
			Battle_replay.Lbas.Clear();
			KCDatabase db = KCDatabase.Instance;

			int[] bases = KCDatabase.Instance.BaseAirCorps.Keys.ToArray();
			int MatchedBases = 0; // add only if  bases match MapAreaID
			for (int baseIndex = 0; baseIndex < bases.Length; baseIndex++)
			{
				int baseID = bases[baseIndex];
				BaseAirCorpsData baseAirCorps = KCDatabase.Instance.BaseAirCorps[baseID];
				if (baseAirCorps.MapAreaID == db.Battle.Compass.MapAreaID )
				{
					Utility.Logger.Add(2, baseID.ToString());
					Battle_replay.Lbas.Add(new Lba());
					Battle_replay.Lbas[MatchedBases].Action = baseAirCorps.ActionKind;
					Battle_replay.Lbas[MatchedBases].Range = new Range();
					Battle_replay.Lbas[MatchedBases].Range.ApiBase = baseAirCorps.Base_Distance;
					Battle_replay.Lbas[MatchedBases].Range.ApiBonus = baseAirCorps.Bonus_Distance;
					Battle_replay.Lbas[MatchedBases].Planes = new List<Plane>(4);
					if (baseAirCorps == null)
						continue;

					for (int squadronIndex = 0; squadronIndex < 4; squadronIndex++)
					{
						var squadron = baseAirCorps.Squadrons[squadronIndex + 1];
						var eq = squadron.EquipmentInstance;
						if (eq != null)
						{
							Battle_replay.Lbas[MatchedBases].Planes.Add(new Plane());
							Battle_replay.Lbas[MatchedBases].Planes[squadronIndex].MstId = eq.EquipmentID;
							Battle_replay.Lbas[MatchedBases].Planes[squadronIndex].State = squadron.State;
							Battle_replay.Lbas[MatchedBases].Planes[squadronIndex].Count = squadron.AircraftCurrent;
							Battle_replay.Lbas[MatchedBases].Planes[squadronIndex].Morale = squadron.Condition;
							Utility.Logger.Add(2, eq.NameWithLevel.ToString());
						}
						else
						{
							Battle_replay.Lbas[MatchedBases].Planes.Add(new Plane());
							Utility.Logger.Add(2, "No equipment in lbas:");
						}
					}
					MatchedBases++;
				}
			}
		}

		public async void SaveReplay()
		{
			GetFleet1();
			GetNodeSupport();
			GetBossSupport();
			GetLBAS();
			JsonSerializerSettings KCjsonSerializer = new JsonSerializerSettings()
			{
				CheckAdditionalContent = true
			};
			if (YasenData != null)
			{
				string YasenData_str = YasenData.ToString();
				Battle_replay.Battles[0].Yasen = JsonConvert.DeserializeObject(YasenData_str, KCjsonSerializer);

			}
			if (DayData != null)
			{
				string DayData_str = DayData.ToString();
				Battle_replay.Battles[0].Data = JsonConvert.DeserializeObject(DayData_str, KCjsonSerializer);
			}

			try
			{
				string info;
				string parent = ReplayLogPath;
				if (!Directory.Exists(parent))
					Directory.CreateDirectory(parent);

				KCDatabase db = KCDatabase.Instance;
				info = $"{db.Battle.Compass.MapAreaID}-{db.Battle.Compass.MapInfoID}-{db.Battle.Compass.Destination}";
				string path = $"{parent}\\{DateTimeHelper.GetTimeStamp()}@{info}-Replay.txt";
				using (StreamWriter rep_file = File.CreateText(path))
				{
					JsonSerializer serializer = new JsonSerializer();
					serializer.Serialize(rep_file, Battle_replay);
					await rep_file.FlushAsync().ConfigureAwait(false);
					Utility.Logger.Add(2, "Replay written to:" + path);
				}

			}

			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, "戦闘ログの出力に失敗しました。");
			}
		}

		private void GetFleet1()
		{
			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet[1]; //fleet 1
			Battle_replay.Fleet1.Clear();

			for (int i = 0; i < fleet.Members.Count; i++)
			{
				if (fleet[i] == -1)
					continue;

				ShipData ship = fleet.MembersInstance[i];
				//Utility.Logger.Add(2, ship.Name);
				List<int> kyouka_list = ship.Kyouka.ToList();
				Battle_replay.Fleet1.Add(new Fleet1()
				{
					MstId = ship.ShipID,
					Level = ship.Level,
					Morale = ship.Condition,
					Kyouka = kyouka_list,
					Equip = ship.AllSlotMaster.ToList(),

				});

			}
		}

		private void GetFleet2()
		{
			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet[2]; //fleet 1
			Battle_replay.Fleet2.Clear();

			for (int i = 0; i < fleet.Members.Count; i++)
			{
				if (fleet[i] == -1)
					continue;

				ShipData ship = fleet.MembersInstance[i];
				//Utility.Logger.Add(2, ship.Name);
				List<int> kyouka_list = ship.Kyouka.ToList();
				Battle_replay.Fleet2.Add(new Fleet2()
				{
					MstId = ship.ShipID,
					Level = ship.Level,
					Morale = ship.Condition,
					Kyouka = kyouka_list,
					Equip = ship.AllSlotMaster.ToList(),

				});

			}
		}
		private void GetFleet3()
		{
			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet[3]; //fleet 1
			Battle_replay.Fleet3.Clear();

			for (int i = 0; i < fleet.Members.Count; i++)
			{
				if (fleet[i] == -1)
					continue;

				ShipData ship = fleet.MembersInstance[i];
				//Utility.Logger.Add(2, ship.Name);
				List<int> kyouka_list = ship.Kyouka.ToList();
				Battle_replay.Fleet3.Add(new Fleet3()
				{
					MstId = ship.ShipID,
					Level = ship.Level,
					Morale = ship.Condition,
					Kyouka = kyouka_list,
					Equip = ship.AllSlotMaster.ToList(),

				});

			}
		}
		private void GetFleet4()
		{
			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet[4]; //fleet 1
			Battle_replay.Fleet3.Clear();

			for (int i = 0; i < fleet.Members.Count; i++)
			{
				if (fleet[i] == -1)
					continue;

				ShipData ship = fleet.MembersInstance[i];
				//Utility.Logger.Add(2, ship.Name);
				List<int> kyouka_list = ship.Kyouka.ToList();
				Battle_replay.Fleet4.Add(new Fleet4()
				{
					MstId = ship.ShipID,
					Level = ship.Level,
					Morale = ship.Condition,
					Kyouka = kyouka_list,
					Equip = ship.AllSlotMaster.ToList(),

				});

			}
		}


	}
}
