using System;
using System.Collections.Generic;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public class TaskComparer : IComparer<IQuestTask?>
{
	public int Compare(IQuestTask? x, IQuestTask? y)
	{
		if (x is null) return 1;
		if (y is null) return -1;

		if (x.TaskOrder() > y.TaskOrder()) return 1;
		if (x.TaskOrder() < y.TaskOrder()) return -1;

		if (x is BattleNodeIdTaskModel a1 && y is BattleNodeIdTaskModel b1) return Compare(a1, b1);
		if (x is IBattleQuestTask a0 && y is IBattleQuestTask b0) return Compare(a0, b0);
		if (x is NodeReachTaskModel a2 && y is NodeReachTaskModel b2) return Compare(a2, b2);
		if (x is MapFirstClearTaskModel a3 && y is MapFirstClearTaskModel b3) return Compare(a3, b3);
		if (x is ExpeditionTaskModel a5 && y is ExpeditionTaskModel b5) return Compare(a5, b5);
		if (x is EquipmentScrapTaskModel a6 && y is EquipmentScrapTaskModel b6) return Compare(a6, b6);
		if (x is EquipmentCategoryScrapTaskModel a7 && y is EquipmentCategoryScrapTaskModel b7) return Compare(a7, b7);
		if (x is EquipmentCardTypeScrapTaskModel a8 && y is EquipmentCardTypeScrapTaskModel b8) return Compare(a8, b8);
		if (x is EquipmentIconTypeScrapTaskModel a9 && y is EquipmentIconTypeScrapTaskModel b9) return Compare(a9, b9);

		return 0;
	}

	private int Compare(IBattleQuestTask a, IBattleQuestTask b)
	{
		if (a.Map.AreaId < b.Map.AreaId) return -1;
		if (a.Map.AreaId > b.Map.AreaId) return 1;

		if (a.Map.InfoId < b.Map.InfoId) return -1;
		if (a.Map.InfoId > b.Map.InfoId) return 1;

		return 0;
	}

	private int Compare(BattleNodeIdTaskModel a, BattleNodeIdTaskModel b)
	{
		if (a.Map.AreaId < b.Map.AreaId) return -1;
		if (a.Map.AreaId > b.Map.AreaId) return 1;

		if (a.Map.InfoId < b.Map.InfoId) return -1;
		if (a.Map.InfoId > b.Map.InfoId) return 1;

		if (a.Name is null) return 1;
		if (b.Name is null) return -1;

		return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
	}

	private int Compare(NodeReachTaskModel a, NodeReachTaskModel b)
	{
		if (a.Map.AreaId < b.Map.AreaId) return -1;
		if (a.Map.AreaId > b.Map.AreaId) return 1;

		if (a.Map.InfoId < b.Map.InfoId) return -1;
		if (a.Map.InfoId > b.Map.InfoId) return 1;

		if (a.Name is null) return 1;
		if (b.Name is null) return -1;

		return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
	}

	private int Compare(MapFirstClearTaskModel a, MapFirstClearTaskModel b)
	{
		if (a.Map.AreaId < b.Map.AreaId) return -1;
		if (a.Map.AreaId > b.Map.AreaId) return 1;

		if (a.Map.InfoId < b.Map.InfoId) return -1;
		if (a.Map.InfoId > b.Map.InfoId) return 1;

		return 0;
	}

	private int Compare(ExpeditionTaskModel a, ExpeditionTaskModel b)
	{
		List<int> expeditionOrder = new()
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			100,
			101,
			102,
			103,
			104,
			105,
			9,
			10,
			11,
			12,
			13,
			14,
			15,
			16,
			110,
			111,
			112,
			113,
			114,
			115,
			17,
			18,
			19,
			20,
			21,
			22,
			23,
			24,
			41,
			42,
			43,
			44,
			45,
			46,
			25,
			26,
			27,
			28,
			29,
			30,
			31,
			32,
			131,
			132,
			133,
			33,
			34,
			35,
			36,
			37,
			38,
			39,
			40,
			141,
			142,
		};

		if (!expeditionOrder.Contains(a.Expedition.MissionId)) return 1;
		if (!expeditionOrder.Contains(b.Expedition.MissionId)) return -1;

		return expeditionOrder.IndexOf(a.Expedition.MissionId) - expeditionOrder.IndexOf(b.Expedition.MissionId);
	}

	private int Compare(EquipmentScrapTaskModel a, EquipmentScrapTaskModel b)
	{
		if (a.Id < b.Id) return -1;
		if (a.Id > b.Id) return 1;

		return 0;
	}

	private int Compare(EquipmentCategoryScrapTaskModel a, EquipmentCategoryScrapTaskModel b)
	{
		if (a.Category < b.Category) return -1;
		if (a.Category > b.Category) return 1;

		return 0;
	}

	private int Compare(EquipmentCardTypeScrapTaskModel a, EquipmentCardTypeScrapTaskModel b)
	{
		if (a.CardType < b.CardType) return -1;
		if (a.CardType > b.CardType) return 1;

		return 0;
	}

	private int Compare(EquipmentIconTypeScrapTaskModel a, EquipmentIconTypeScrapTaskModel b)
	{
		if (a.IconType < b.IconType) return -1;
		if (a.IconType > b.IconType) return 1;

		return 0;
	}
}
