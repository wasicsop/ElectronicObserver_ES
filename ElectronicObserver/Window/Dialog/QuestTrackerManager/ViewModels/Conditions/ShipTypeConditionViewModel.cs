using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public partial class ShipTypeConditionViewModel : ObservableObject, IConditionViewModel
{
	public ShipTypeConditionModel Model { get; set; }

	public ShipTypes SelectedType { get; set; } = ShipTypes.Destroyer;
	public IEnumerable<ShipTypes> AllTypes { get; }

	public bool MustBeFlagship { get; set; }

	public string Display => $"({CountConditionDisplay}) >= {Model.Count}{FlagshipConditionDisplay}";

	private string CountConditionDisplay => string.Join($" {Properties.Window.Dialog.QuestTrackerManager.Operator_Or} ",
		Model.Types.Select(s => s.Display()));

	private string FlagshipConditionDisplay => MustBeFlagship switch
	{
		true => $"({Properties.Window.Dialog.QuestTrackerManager.Flagship})",
		_ => ""
	};

	public ShipTypeConditionViewModel(ShipTypeConditionModel model)
	{
		Model = model;

		MustBeFlagship = Model.MustBeFlagship;

		AllTypes = Enum.GetValues(typeof(ShipTypes)).Cast<ShipTypes>();

		Model.PropertyChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		Model.Types.CollectionChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(MustBeFlagship)) return;

			Model.MustBeFlagship = MustBeFlagship;
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
		List<IShipData> ships = fleet.MembersInstance.Where(s => s is not null).ToList();

		bool flagshipCondition = !Model.MustBeFlagship ||
			Model.Types.Contains(ships[0].MasterShip.ShipType);

		bool countCondition = ships
			.Count(s => Model.Types.Contains(s.MasterShip.ShipType)) >= Model.Count;

		return flagshipCondition && countCondition;
	}
}
