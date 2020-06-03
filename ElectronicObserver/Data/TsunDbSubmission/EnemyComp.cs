using ElectronicObserver.Data.Battle.Phase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Data
{
	public class EnemyComp : TsunDbEntity
	{
		protected override string Url => "";

		[JsonProperty("ship")]
		public int[]? Ship;

		[JsonProperty("lvl")]
		public int[]? Lvl;

		[JsonProperty("hp")]
		public int[]? HP;

		[JsonProperty("stats")]
		public int[][]? Stats;

		[JsonProperty("equip")]
		public int[][]? Equips;

		[JsonProperty("formation")]
		public int Formation;

		[JsonProperty("isAirRaid")]
		public bool IsAirRaid;


		[JsonProperty("gaugeNum")]
		public int GaugeNum;

		[JsonProperty("currentHP")]
		public int CurrentHP;

		[JsonProperty("maxHP")]
		public int MaxHP;


		[JsonProperty("shipEscort")]
		public int[]? ShipEscort;

		[JsonProperty("lvlEscort")]
		public int[]? LvlEscort;

		[JsonProperty("hpEscort")]
		public int[]? HPEscort;

		[JsonProperty("statsEscort")]
		public int[][]? StatsEscort;

		[JsonProperty("equipEscort")]
		public int[][]? EquipsEscort;

		public EnemyComp()
		{

		}

		public void PrepareEnemyCompFromCurrentState()
		{
			KCDatabase db = KCDatabase.Instance;
			PhaseInitial initial = db.Battle.FirstBattle.Initial;

			int shipCount = initial.EnemyMembers.Count((id) => { return id != -1; });

			Ship = initial.EnemyMembers;
			Lvl = initial.EnemyLevels;
			HP = initial.EnemyMaxHPs;
			Stats = initial.EnemyParameters;
			Equips = initial.EnemySlots;
			Formation = db.Battle.FirstBattle.Searching.FormationEnemy;

			Array.Resize(ref Ship, shipCount);
			Array.Resize(ref Lvl, shipCount);
			Array.Resize(ref HP, shipCount);
			Array.Resize(ref Stats, shipCount);
			Array.Resize(ref Equips, shipCount);

			// --- If this is an event map
			if (db.Battle.Compass.MapAreaID > 30)
			{
				MapInfoData mapInfoData = db.MapInfo[db.Battle.Compass.MapAreaID * 10 + db.Battle.Compass.MapInfoID];

				GaugeNum = mapInfoData.CurrentGaugeIndex;
				CurrentHP = mapInfoData.MapHPCurrent;
				MaxHP = mapInfoData.MapHPMax;
			}

			// --- If enemy fleet is combined
			if (initial.IsEnemyCombined)
			{
				ShipEscort = initial.EnemyMembersEscort;
				LvlEscort = initial.EnemyLevelsEscort;
				HPEscort = initial.EnemyMaxHPsEscort;
				StatsEscort = initial.EnemyParametersEscort;
				EquipsEscort = initial.EnemySlotsEscort;

				Array.Resize(ref ShipEscort, shipCount);
				Array.Resize(ref LvlEscort, shipCount);
				Array.Resize(ref HPEscort, shipCount);
				Array.Resize(ref StatsEscort, shipCount);
				Array.Resize(ref EquipsEscort, shipCount);
			}

			IsAirRaid = db.Battle.BattleMode == Battle.BattleManager.BattleModes.AirRaid;
		}
	}
}
