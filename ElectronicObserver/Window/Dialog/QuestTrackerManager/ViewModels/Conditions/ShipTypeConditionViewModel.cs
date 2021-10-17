using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public partial class ShipTypeConditionViewModel : ObservableObject, IConditionViewModel
{
	public ShipTypeConditionModel Model { get; set; }

	public ShipTypes SelectedType { get; set; } = ShipTypes.Destroyer;
	public IEnumerable<ShipTypes> AllTypes { get; }

	public string Display =>
		$"({string.Join(Properties.Window.Dialog.QuestTrackerManager.Operator_Or, Model.Types.Select(s => s.Display()))}) >= {Model.Count}";

	public ShipTypeConditionViewModel(ShipTypeConditionModel model)
	{
		Model = model;

		AllTypes = Enum.GetValues(typeof(ShipTypes)).Cast<ShipTypes>();

		Model.PropertyChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		Model.Types.CollectionChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};
	}

	[ICommand]
	private void AddType()
	{
		Model.Types.Add(SelectedType);
	}

	[ICommand]
	private void RemoveType(ShipTypes s)
	{
		Model.Types.Remove(s);
	}

	public bool ConditionMet(IFleetData fleet)
	{
		return fleet.MembersInstance
			.Count(s => Model.Types.Contains(s.MasterShip.ShipType)) >= Model.Count;
	}
}