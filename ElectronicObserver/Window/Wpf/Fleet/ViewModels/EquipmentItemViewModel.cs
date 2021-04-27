using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ElectronicObserver.Window.Control;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels
{
	public class EquipmentItemViewModel : ObservableObject
	{
		public bool ShowAircraft { get; set; }
		public ShipStatusEquipment.LevelVisibilityFlag LevelVisibility { get; set; }
		public bool ShowAircraftLevelByNumber { get; set; }
		public string? ToolTip { get; set; }

		public List<ShipSlotViewModel> Slots { get; set; } = new()
		{
			new(),
			new(),
			new(),
			new(),
			new(),
			new(),
		};

		public void SetSlotList(IShipData ship)
		{
			var slots = ship.AllSlotInstance
				.Zip(ship.Aircraft, (eq, s) => (Equipment: eq, CurrentAircraft: s))
				.Zip(ship.MasterShip.Aircraft, (slot, t) => (slot.Equipment, slot.CurrentAircraft, Size: t))
				.ToList();

			for (int i = 0; i < Slots.Count; i++)
			{
				if (i < ship.SlotSize)
				{
					Slots[i].Equipment = slots[i].Equipment;
					Slots[i].CurrentAircraft = slots[i].CurrentAircraft;
					Slots[i].Size = slots[i].Size;
					Slots[i].SlotVisibility = Visibility.Visible;
				}
				else
				{
					Slots[i].SlotVisibility = Visibility.Collapsed;
				}
			}

			if (ship.IsExpansionSlotAvailable)
			{
				Slots[^1].Equipment = ship.ExpansionSlotInstance;
				Slots[^1].SlotVisibility = Visibility.Visible;
			}
		}
	}
}