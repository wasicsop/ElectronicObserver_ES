using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{

	/// <summary>
	/// 戦闘フェーズの基底クラスです。
	/// </summary>
	public abstract class PhaseBase
	{
		protected BattleData Battle;
		public List<BattleDetail> BattleDetails { get; protected set; }
		public readonly string Title;


		private string TranslatedPhaseTitle(string title) {
			switch (title)	{
				default: return title;
				case "噴式基地航空隊攻撃": return "Land-based Jet Air Attack";
				case "噴式航空戦": return "Jet Air Attack";
				case "基地航空隊攻撃": return "Land-based Air Attack";
				case "防空戦": return "Land Base Air Raid";
				case "航空戦": return "Air Battle";
				case "空襲戦": return "Air Raid";
				case "第一次航空戦": return "Air Attack, 1st"; 
				case "第二次航空戦": return "Air Attack, 2nd";
				case "支援攻撃": return "Support Expedition";
				case "先制対潜": return "Opening ASW";
				case "先制雷撃": return "Opening Torpedo Salvo";
				case "第一次砲撃戦": return "Shelling, 1st Round";
				case "第二次砲撃戦": return "Shelling, 2nd Round";
				case "第三次砲撃戦": return "Shelling, 3rd Round";
				case "雷撃戦": return "Closing Torpedo Salvo";
				case "夜戦": return "Night Battle";
				case "夜間支援攻撃": return "Night Support Expedition";


			}
		}

		protected PhaseBase(BattleData battle, string title)
		{
			Battle = battle;
			BattleDetails = new List<BattleDetail>();
			Title = TranslatedPhaseTitle(title);
		}


		protected dynamic RawData => Battle.RawData;


		protected static bool IsIndexFriend(int index) => 0 <= index && index < 12;
		protected static bool IsIndexEnemy(int index) => 12 <= index && index < 24;


		/// <summary>
		/// 被ダメージ処理を行います。
		/// </summary>
		/// <param name="hps">各艦のHPリスト。</param>
		/// <param name="index">ダメージを受ける艦のインデックス。</param>
		/// <param name="damage">ダメージ。</param>
		protected void AddDamage(int[] hps, int index, int damage)
		{

			hps[index] -= Math.Max(damage, 0);

			// 自軍艦の撃沈が発生した場合(ダメコン処理)
			if (hps[index] <= 0 && IsIndexFriend(index) && !Battle.IsPractice)
			{
				var ship = Battle.Initial.GetFriendShip(index);
				if (ship == null)
					return;

				int id = ship.DamageControlID;

				if (id == 42)
					hps[index] = (int)(ship.HPMax * 0.2);

				else if (id == 43)
					hps[index] = ship.HPMax;

			}
		}


		protected virtual IEnumerable<BattleDetail> SearchBattleDetails(int index)
		{
			return BattleDetails.Where(d => d.AttackerIndex == index || d.DefenderIndex == index);
		}
		public virtual string GetBattleDetail(int index)
		{
			IEnumerable<BattleDetail> list;
			if (index == -1)
				list = BattleDetails;
			else
				list = SearchBattleDetails(index);

			if (list.Any())
			{
				return string.Join("\r\n", list) + "\r\n";
			}
			else return null;
		}
		public virtual string GetBattleDetail() { return GetBattleDetail(-1); }


		public override string ToString() => string.Join(" / \r\n", BattleDetails);



		/// <summary>
		/// データが有効かどうかを示します。
		/// </summary>
		public abstract bool IsAvailable { get; }

		/// <summary>
		/// 戦闘をエミュレートします。
		/// </summary>
		/// <param name="hps">各艦のHPリスト。</param>
		/// <param name="damages">各艦の与ダメージリスト。</param>
		public abstract void EmulateBattle(int[] hps, int[] damages);


	}
}
