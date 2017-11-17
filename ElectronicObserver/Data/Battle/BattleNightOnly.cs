﻿using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 通常艦隊開幕夜戦
	/// </summary>
	public class BattleNightOnly : BattleNight
	{

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			NightBattle = new PhaseNightBattle(this, "夜戦", false);

			NightBattle.EmulateBattle(_resultHPs, _attackDamages);

		}


		public override string APIName => "api_req_battle_midnight/sp_midnight";

		public override string BattleName => ConstantsRes.Title_NightOnly;

		public override BattleTypeFlag BattleType => BattleTypeFlag.Night;


		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return Searching;
			yield return NightBattle;
		}
	}

}
