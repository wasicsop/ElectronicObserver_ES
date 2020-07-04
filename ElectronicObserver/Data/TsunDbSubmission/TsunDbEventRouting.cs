using ElectronicObserver.Utility.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
			this.CurrentMapHP = (int)api_data.api_eventmap.api_now_maphp;
			this.MaxMapHP = (int)api_data.api_eventmap.api_max_maphp;
			this.Difficulty = (int)api_data.api_eventmap.api_selected_rank;
			this.GaugeNum = (int)api_data.api_gauge_num;
			this.GaugeType = (int)api_data.api_gauge_type;
			this.DebuffSound = (int)api_data.debuffSound;
		}
		#endregion
	}
}
