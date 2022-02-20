using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public partial class AllowedShipTypesConditionViewModel : ObservableObject, IConditionViewModel
{
	public AllowedShipTypesConditionModel Model { get; set; }

	public ShipTypes SelectedType { get; set; } = ShipTypes.Destroyer;
	public IEnumerable<ShipTypes> AllTypes { get; }

	public string Display => $"({Properties.Window.Dialog.QuestTrackerManager.ConditionType_AllowedShipTypes}：{CountConditionDisplay})";

	private string CountConditionDisplay => string.Join("・", Model.Types.Select(s => s.Display()));

	public AllowedShipTypesConditionViewModel(AllowedShipTypesConditionModel model)
	{
		Model = model;

		AllTypes = Enum.GetValues<ShipTypes>();

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
		return !fleet.MembersInstance
			.Where(s => s is not null)
			.Any(s => !Model.Types.Contains(s.MasterShip.ShipType));
	}
}
