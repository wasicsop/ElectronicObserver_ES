using DynaJson;
using ElectronicObserver.Resource.Record;
using Newtonsoft.Json;
using static ElectronicObserver.Resource.Record.MapRecord;

namespace ElectronicObserver.Data
{
	public class TsunDbEventRouting : TsunDbRouting
	{
		protected override string Url => "eventrouting";

		#region Json Properties
		[JsonProperty("currentMapHP")]
		private int CurrentMapHP;

		[JsonProperty("maxMapHP")]
		private int MaxMapHP;

		[JsonProperty("difficulty")]
		private int Difficulty;

		[JsonProperty("gaugeNum")]
		private int GaugeNum;

		[JsonProperty("gaugeType")]
		private int GaugeType;

		[JsonProperty("debuffSound")]
		private int DebuffSound;
		#endregion

		#region public methods
		/// <summary>
		/// Process next node request
		/// </summary>
		/// <param name="api_data"></param>
		public void ProcessEvent(dynamic api_data)
		{
			KCDatabase db = KCDatabase.Instance; 
			JsonObject jData = (JsonObject)api_data;

			this.CurrentMapHP = (int)api_data.api_eventmap.api_now_maphp;
			this.MaxMapHP = (int)api_data.api_eventmap.api_max_maphp;
			this.Difficulty = db.Battle.Compass.MapInfo.EventDifficulty;
			this.GaugeNum = db.Battle.Compass.MapInfo.CurrentGaugeIndex;
			this.GaugeType = db.Battle.Compass.MapInfo.GaugeType;

			// --- Debuff sound 
			MapRecordElement? record = RecordManager.Instance.Map.Record
					.Find(_record => _record.MapAreaId == db.Battle.Compass.MapInfo.MapAreaID && _record.MapId == db.Battle.Compass.MapInfo.MapID);

			this.DebuffSound = 0;
		}
		#endregion
	}
}
