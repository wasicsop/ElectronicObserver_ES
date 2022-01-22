using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Dialog.ShipPicker;

public partial class ShipPickerViewModel : ObservableObject
{
	public List<Filter> TypeFilters { get; }

	private List<IShipDataMaster> AllShips => KCDatabase.Instance.MasterShips.Values
		.Where(s => !s.IsAbyssalShip)
		.OrderBy(s => s.SortID)
		.Cast<IShipDataMaster>()
		.ToList();

	public ObservableCollection<ClassGroup> ShipClassGroups { get; set; } = new();

	public IShipDataMaster? PickedShip { get; private set; }

	public ShipPickerViewModel()
	{
		TypeFilters = Enum.GetValues<ShipTypeGroup>().Select(t => new Filter(t)).ToList();

		foreach (Filter filter in TypeFilters)
		{
			filter.PropertyChanged += Filter_PropertyChanged;
		}
	}

	[ICommand]
	private void SelectShip(IShipDataMaster ship)
	{
		PickedShip = ship;
	}

	private void Filter_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		List<ShipTypes> enabledFilters = TypeFilters
			.Where(f => f.IsChecked)
			.SelectMany(f => f.Value.ToTypes())
			.ToList();

		ShipClassGroups = new(AllShips
			.Where(s => enabledFilters.Contains(s.ShipType))
			.GroupBy(s => s.ShipClass)
			.Select(g => new ClassGroup
			{
				Id = g.Key,
				Name = Constants.GetShipClass(g.Key),
				Ships = g.ToList(),
			}).ToList());
	}
}
