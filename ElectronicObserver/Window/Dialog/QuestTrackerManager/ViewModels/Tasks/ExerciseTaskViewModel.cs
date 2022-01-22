using System;
using System.Collections.Generic;
using System.Text;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Quest;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

public class ExerciseTaskViewModel : ObservableObject, IQuestTaskViewModel
{
	public ExerciseTaskModel Model { get; }

	public IEnumerable<BattleRank> Ranks { get; }

	public string Display => $"{Model.Progress}/{Model.Count} {GetClearCondition()}";
	public string Progress => (Model.Progress >= Model.Count) switch
	{
		true => "",
		_ => Display
	};
	public string ClearCondition => GetClearCondition();

	private string GetClearCondition()
	{
		StringBuilder sb = new();

		sb.Append(QuestTracking.Exercise);

		sb.Append(Model.Rank switch
		{
			BattleRank.Any => QuestTracking.ClearConditionClear,
			BattleRank.D or BattleRank.C => Constants.GetWinRank((int)Model.Rank) + QuestTracking.ClearConditionOnly,
			BattleRank.B => "",
			BattleRank.A or BattleRank.S or BattleRank.SS =>
				Constants.GetWinRank((int)Model.Rank) + QuestTracking.ClearConditionRankVictories,

			_ or BattleRank.E => QuestTracking.ClearConditionBattle,
		});
		sb.Append(Model.Count);

		return sb.ToString();
	}

	public ExerciseTaskViewModel(ExerciseTaskModel model)
	{
		Model = model;

		Ranks = Ranks = Enum.GetValues<BattleRank>();

		Model.PropertyChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		PropertyChanged += (_, e) =>
		{
			if (e.PropertyName is not nameof(Display)) return;

			KCDatabase.Instance.Quest.OnQuestUpdated();
		};
	}

	public void Increment(BattleRank rank)
	{ 
		if (rank >= Model.Rank)
		{
			Model.Progress++;
		}
	}
}
