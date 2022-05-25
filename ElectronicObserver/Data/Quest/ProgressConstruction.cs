using System.Runtime.Serialization;

namespace ElectronicObserver.Data.Quest;

/// <summary>
/// 艦船建造任務の進捗を管理します。
/// </summary>
[DataContract(Name = "ProgressConstruction")]
public class ProgressConstruction : ProgressData
{

	public ProgressConstruction(QuestData quest, int maxCount)
		: base(quest, maxCount)
	{
	}


	public override string GetClearCondition()
	{
		return QuestTracking.Construction + ProgressMax;
	}
}
