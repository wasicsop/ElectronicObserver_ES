using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest
{
	// todo: library with full enum
	public enum ShipId
	{
		Mutsuki = 1,
		Kisaragi = 2,
		Kagerou = 17,
		Shiranui = 18,
		Kikuzuki = 30,
		Mochizuki = 31,
		Arare = 48,
		Kasumi = 49,
		Yayoi = 164,
		Uzuki = 165,
		YuraKaiNi = 488,
		IseKaiNi = 553,
		HyuugaKaiNi = 554,
		YuubariKaiNi = 622,
		YuubariKaiNiToku = 623,
		YuubariKaiNiD = 624
	}

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

			switch (questId)
			{
				case 329:
					return fleet.MembersInstance.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer ||
					                                        s.MasterShip.ShipType == ShipTypes.Escort) >= 2;

				case 330:
					return fleet.MembersInstance[0].MasterShip.IsAircraftCarrier &&
					       fleet.MembersInstance.Count(s => s.MasterShip.IsAircraftCarrier) >= 2 &&
					       fleet.MembersInstance.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer) >= 2;

				case 337:
					return fleet.MembersInstance.Any(s => s.MasterShip.BaseShip().ShipID == (int)ShipId.Kagerou) &&
					       fleet.MembersInstance.Any(s => s.MasterShip.BaseShip().ShipID == (int)ShipId.Shiranui) &&
					       fleet.MembersInstance.Any(s => s.MasterShip.BaseShip().ShipID == (int)ShipId.Arare) &&
					       fleet.MembersInstance.Any(s => s.MasterShip.BaseShip().ShipID == (int)ShipId.Kasumi);
			}

			return true;
		}

		public override string GetClearCondition() {
			return "Exercise " + ( WinOnly ? "victories ×" : "×") + ProgressMax;
		}
	}
}
