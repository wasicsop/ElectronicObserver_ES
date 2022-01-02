using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public partial class GroupConditionViewModel : ObservableObject, IConditionViewModel
{
	public bool CanBeRemoved { get; set; } = true;
	public Visibility RemoveButtonVisibility => CanBeRemoved.ToVisibility();

	public GroupConditionModel Model { get; }

	public IEnumerable<IConditionViewModel> Conditions { get; set; }

	public IEnumerable<Operator> Operators { get; }
	public Operator GroupOperator { get; set; }

	public IEnumerable<ConditionType> ConditionTypes { get; }
	public ConditionType SelectedConditionType { get; set; } = ConditionType.ShipType;

	public string Display => string.Join($"{Separator}{GroupOperator.Display()}{Separator}",
		Conditions.Select(c => Grouping(c.Display)));

	private string Separator => GroupOperator switch
	{
		Operator.Or => "\n",
		_ => " "
	};

	private string Grouping(string s) => GroupOperator switch
	{
		Operator.Or => $"({s})",
		_ => s
	};

	public GroupConditionViewModel(GroupConditionModel model)
	{
		Operators = Enum.GetValues(typeof(Operator)).Cast<Operator>();
		ConditionTypes = Enum.GetValues(typeof(ConditionType)).Cast<ConditionType>();

		Model = model;

		GroupOperator = Model.GroupOperator;
		Conditions = CreateViewModels(Model);

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(GroupOperator)) return;

			Model.GroupOperator = GroupOperator;
		};

		Model.Conditions.CollectionChanged += (_, e) =>
		{
			Conditions = CreateViewModels(Model);
		};
	}

	private IEnumerable<IConditionViewModel> CreateViewModels(GroupConditionModel model)
	{
		List<IConditionViewModel> conditions = model.Conditions.Select(c => c switch
		{
			GroupConditionModel g => (IConditionViewModel)new GroupConditionViewModel(g),
			ShipConditionModel s => new ShipConditionViewModel(s),
			ShipTypeConditionModel g => new ShipTypeConditionViewModel(g),
			PartialShipConditionModel p => new PartialShipConditionViewModel(p),
			AllowedShipTypesConditionModel a => new AllowedShipTypesConditionViewModel(a),
		}).ToList();

		foreach (IConditionViewModel condition in conditions)
		{
			condition.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName is not nameof(IConditionViewModel.Display)) return;

				OnPropertyChanged(nameof(Display));
			};
		}

		return conditions;
	}

	[ICommand]
	private void AddCondition()
	{
		Model.Conditions.Add(SelectedConditionType switch
		{
			ConditionType.Group => new GroupConditionModel(),
			ConditionType.Ship => new ShipConditionModel(),
			ConditionType.ShipType => new ShipTypeConditionModel(),
			ConditionType.PartialShip => new PartialShipConditionModel(),
			ConditionType.AllowedShipTypes => new AllowedShipTypesConditionModel(),

			_ => throw new NotImplementedException(),
		});
	}

	[ICommand]
	private void RemoveCondition(ICondition? condition)
	{
		if (condition is null) return;

		Model.Conditions.Remove(condition);
	}

	public bool ConditionMet(IFleetData fleet)
	{
		if (!Conditions.Any()) return true;

		return GroupOperator switch
		{
			Operator.And => Conditions.All(c => c.ConditionMet(fleet)),
			Operator.Or => Conditions.Any(c => c.ConditionMet(fleet)),

			_ => throw new NotImplementedException()
		};
	}
}
