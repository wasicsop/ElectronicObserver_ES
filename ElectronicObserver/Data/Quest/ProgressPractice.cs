using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.Quest
{
	/// <summary>
	/// 演習任務の進捗を管理します。
	/// </summary>
	[DataContract(Name = "ProgressPractice")]
	public class ProgressPractice : ProgressData
	{

		/// <summary>
		/// 勝利のみカウントする
		/// </summary>
		[DataMember]
		private bool WinOnly { get; set; }

		/// <summary>
		/// 条件を満たす最低ランク
		/// </summary>
		[DataMember]
		private int LowestRank { get; set; }

		public ProgressPractice(QuestData quest, int maxCount, bool winOnly)
			: base(quest, maxCount)
		{
			LowestRank = winOnly ? Constants.GetWinRank("B") : Constants.GetWinRank("");
			WinOnly = winOnly;
		}

		public ProgressPractice(QuestData quest, int maxCount, string lowestRank)
			: base(quest, maxCount)
		{
			LowestRank = Constants.GetWinRank(lowestRank);
		}

		public void Increment(string rank)
		{
			if (Constants.GetWinRank(rank) < LowestRank) return;

			if (!MeetsSpecialRequirements(QuestID)) return;

			Increment();
		}

		private bool MeetsSpecialRequirements(int questId)
		{
			FleetData fleet = KCDatabase.Instance.Fleet.Fleets.Values
				.FirstOrDefault(f => f.IsInPractice);

			if (fleet == null) return false;

			return questId switch
			{
				329 => fleet.MembersInstance.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer ||
				                                        s.MasterShip.ShipType == ShipTypes.Escort) >= 2,
				330 => fleet.MembersInstance[0].MasterShip.IsAircraftCarrier &&
				       fleet.MembersInstance.Count(s => s.MasterShip.IsAircraftCarrier) >= 2 &&
				       fleet.MembersInstance.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer) >= 2,
				337 => fleet.MembersInstance.Any(s => s.MasterShip.BaseShip().ShipID == (int) ShipId.Kagerou) &&
				       fleet.MembersInstance.Any(s => s.MasterShip.BaseShip().ShipID == (int) ShipId.Shiranui) &&
				       fleet.MembersInstance.Any(s => s.MasterShip.BaseShip().ShipID == (int) ShipId.Arare) &&
				       fleet.MembersInstance.Any(s => s.MasterShip.BaseShip().ShipID == (int) ShipId.Kasumi),
				341 => fleet.MembersInstance.Count(s => s.MasterShip.BaseShip().ShipId switch
				{
					ShipId.Oboro => true,
					ShipId.Akebono => true,
					ShipId.Sazanami => true,
					ShipId.Ushio => true,

					_ => false
				}) >= 2,

				_ => true,
			};
		}

		public override string GetClearCondition() {
			return "Exercise " + ( WinOnly ? "victories ×" : "×") + ProgressMax;
		}
	}
}
