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

			List<IShipData> ships = fleet.MembersInstance.Where(s => s != null).ToList();

			return questId switch
			{
				329 => ships.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer ||
				                                        s.MasterShip.ShipType == ShipTypes.Escort) >= 2,
				330 => ships[0].MasterShip.IsAircraftCarrier &&
				       ships.Count(s => s.MasterShip.IsAircraftCarrier) >= 2 &&
				       ships.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer) >= 2,
				337 => ships.Any(s => s.MasterShip.BaseShip().ShipID == (int) ShipId.Kagerou) &&
				       ships.Any(s => s.MasterShip.BaseShip().ShipID == (int) ShipId.Shiranui) &&
				       ships.Any(s => s.MasterShip.BaseShip().ShipID == (int) ShipId.Arare) &&
				       ships.Any(s => s.MasterShip.BaseShip().ShipID == (int) ShipId.Kasumi),
				339 => ships.Count(s => s.MasterShip.BaseShip().ShipId switch
				{
					ShipId.Isonami => true,
					ShipId.Uranami => true,
					ShipId.Ayanami => true,
					ShipId.Shikinami => true,

					_ => false
				}) >= 4,
				341 => ships.Count(s => s.MasterShip.BaseShip().ShipId switch
				{
					ShipId.Oboro => true,
					ShipId.Akebono => true,
					ShipId.Sazanami => true,
					ShipId.Ushio => true,

					_ => false
				}) >= 2,
				// DD+DE >= 3 and another DD/DE or CL
				342 => ships.Count(s => s.MasterShip.ShipType switch
				{
					ShipTypes.Destroyer => true,
					ShipTypes.Escort => true,

					_ => false
				}) >= 3
				&& ships.Count(s => s.MasterShip.ShipType switch
				{
					ShipTypes.Destroyer => true,
					ShipTypes.Escort => true,
					ShipTypes.LightCruiser => true,

					_ => false
				}) >= 4,

				_ => true,
			};
		}

		public override string GetClearCondition() {
			return "Exercise " + ( WinOnly ? "victories ×" : "×") + ProgressMax;
		}
	}
}
