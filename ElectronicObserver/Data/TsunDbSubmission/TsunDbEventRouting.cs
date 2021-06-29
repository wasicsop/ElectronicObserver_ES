using DynaJson;
using ElectronicObserver.Resource.Record;
using Newtonsoft.Json;

namespace ElectronicObserver.Data
{
	public class TsunDbEventRouting : TsunDbRouting
	{
		protected override string Url => "eventrouting";

		#region Json Properties
		[JsonProperty("currentMapHP")]
		public int CurrentMapHP { get; private set; }

		[JsonProperty("maxMapHP")]
		public int MaxMapHP { get; private set; }

		[JsonProperty("difficulty")]
		public int Difficulty { get; private set; }

		[JsonProperty("gaugeNum")]
		public int GaugeNum { get; private set; }

		[JsonProperty("gaugeType")]
		public int GaugeType { get; private set; }

		[JsonProperty("debuffSound")]
		public int DebuffSound { get; private set; }
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

			this.DebuffSound = 0;
		}
		#endregion
	}
}
