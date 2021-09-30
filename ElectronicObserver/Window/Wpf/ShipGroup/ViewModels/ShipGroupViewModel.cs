using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Wpf.ShipGroup.ViewModels
{
	public class ShipGroupViewModel : AnchorableViewModel
	{
		private KCDatabase Db { get; }

		public ObservableCollection<IShipData> Ships { get; set; } = new();
		public ObservableCollection<ShipGroupData> Groups { get; }

		public DataGrid? DataGrid { get; set; }

		public ICommand SelectGroupCommand { get; }

		public ShipGroupViewModel() : base("Group", "Group",
			ImageSourceIcons.GetIcon(IconContent.FormShipGroup))
		{
			Db = KCDatabase.Instance;

			SelectGroupCommand = new RelayCommand<string>(ShipGroupSelected);

			APIObserver o = APIObserver.Instance;

			o.APIList["api_port/port"].ResponseReceived += APIUpdated;
			o.APIList["api_get_member/ship2"].ResponseReceived += APIUpdated;
			o.APIList["api_get_member/ship_deck"].ResponseReceived += APIUpdated;

			Groups = new(Db.ShipGroup.ShipGroups.Values);
		}

		private void APIUpdated(string apiname, dynamic data)
		{
			Ships = new(Db.Ships.Values);
		}

		private static string GetColumnBinding(string winformsName) => winformsName switch
		{
			"ShipView_ID" => nameof(IShipData.MasterID),
			"ShipView_ShipType" => "MasterShip.ShipTypeName",
			"ShipView_Name" => nameof(IShipData.Name),
			"ShipView_Level" => nameof(IShipData.Level),
			"ShipView_Exp" => nameof(IShipData.ExpTotal),
			"ShipView_Next" => nameof(IShipData.ExpNext),
			"ShipView_NextRemodel" => nameof(IShipData.ExpNextRemodel),
			"ShipView_HP" => nameof(IShipData.HPCurrent),
			"ShipView_Condition" => nameof(IShipData.Condition),
			"ShipView_Fuel" => nameof(IShipData.FuelRate),
			"ShipView_Ammo" => nameof(IShipData.AmmoRate),
			"ShipView_Slot1" => "AllSlotInstance[0]",
			"ShipView_Slot2" => "AllSlotInstance[1]",
			"ShipView_Slot3" => "AllSlotInstance[2]",
			"ShipView_Slot4" => "AllSlotInstance[3]",
			"ShipView_Slot5" => "AllSlotInstance[4]",
			"ShipView_ExpansionSlot" => "ExpansionSlotInstance.NameWithLevel",
			"ShipView_Aircraft1" => "Aircraft[0]",
			"ShipView_Aircraft2" => "Aircraft[1]",
			"ShipView_Aircraft3" => "Aircraft[2]",
			"ShipView_Aircraft4" => "Aircraft[3]",
			"ShipView_Aircraft5" => "Aircraft[4]",
			"ShipView_AircraftTotal" => nameof(IShipData.AircraftTotal),
			"ShipView_Fleet" => nameof(IShipData.Fleet),
			"ShipView_RepairTime" => nameof(IShipData.RepairTime),
			"ShipView_RepairSteel" => nameof(IShipData.RepairSteel),
			"ShipView_RepairFuel" => nameof(IShipData.RepairFuel),
			"ShipView_Firepower" => nameof(IShipData.FirepowerBase),
			"ShipView_FirepowerRemain" => nameof(IShipData.FirepowerRemain),
			"ShipView_FirepowerTotal" => nameof(IShipData.FirepowerTotal),
			"ShipView_Torpedo" => nameof(IShipData.TorpedoBase),
			"ShipView_TorpedoRemain" => nameof(IShipData.TorpedoRemain),
			"ShipView_TorpedoTotal" => nameof(IShipData.TorpedoTotal),
			"ShipView_AA" => nameof(IShipData.AABase),
			"ShipView_AARemain" => nameof(IShipData.AARemain),
			"ShipView_AATotal" => nameof(IShipData.AATotal),
			"ShipView_Armor" => nameof(IShipData.ArmorBase),
			"ShipView_ArmorRemain" => nameof(IShipData.ArmorRemain),
			"ShipView_ArmorTotal" => nameof(IShipData.ArmorTotal),
			"ShipView_ASW" => nameof(IShipData.ASWBase),
			"ShipView_ASWTotal" => nameof(IShipData.ASWTotal),
			"ShipView_Evasion" => nameof(IShipData.EvasionBase),
			"ShipView_EvasionTotal" => nameof(IShipData.EvasionTotal),
			"ShipView_LOS" => nameof(IShipData.LOSBase),
			"ShipView_LOSTotal" => nameof(IShipData.LOSTotal),
			"ShipView_Luck" => nameof(IShipData.LuckBase),
			"ShipView_LuckRemain" => nameof(IShipData.LuckRemain),
			"ShipView_LuckTotal" => nameof(IShipData.LuckTotal),
			"ShipView_BomberTotal" => nameof(IShipData.BomberTotal),
			"ShipView_Speed" => nameof(IShipData.Speed),
			"ShipView_Range" => nameof(IShipData.Range),
			"ShipView_AirBattlePower" => nameof(IShipData.AirBattlePower),
			"ShipView_ShellingPower" => nameof(IShipData.ShellingPower),
			"ShipView_AircraftPower" => nameof(IShipData.AircraftPower),
			"ShipView_AntiSubmarinePower" => nameof(IShipData.AntiSubmarinePower),
			"ShipView_TorpedoPower" => nameof(IShipData.TorpedoPower),
			"ShipView_NightBattlePower" => nameof(IShipData.NightBattlePower),
			"ShipView_Locked" => nameof(IShipData.IsLocked),
			"ShipView_SallyArea" => nameof(IShipData.SallyArea),
		};

		private static string GetColumnName(string winformsName) => winformsName switch
		{
			"ShipView_ID" => "ID",
			"ShipView_ShipType" => "Type",
			"ShipView_Name" => nameof(IShipData.Name),
			"ShipView_Level" => "Lv",
			"ShipView_Exp" => "Exp",
			"ShipView_Next" => "next",
			"ShipView_NextRemodel" => "Remodel",
			"ShipView_HP" => "HP",
			"ShipView_Condition" => "Cond",
			"ShipView_Fuel" => "Fuel",
			"ShipView_Ammo" => "Ammo",
			"ShipView_Slot1" => "Eq 1",
			"ShipView_Slot2" => "Eq 2",
			"ShipView_Slot3" => "Eq 3",
			"ShipView_Slot4" => "Eq 4",
			"ShipView_Slot5" => "Eq 5",
			"ShipView_ExpansionSlot" => "Expansion",
			"ShipView_Aircraft1" => "Aircraft 1",
			"ShipView_Aircraft2" => "Aircraft 2",
			"ShipView_Aircraft3" => "Aircraft 3",
			"ShipView_Aircraft4" => "Aircraft 4",
			"ShipView_Aircraft5" => "Aircraft 5",
			"ShipView_AircraftTotal" => "Aircraft",
			"ShipView_Fleet" => "Position",
			"ShipView_RepairTime" => "Repair",
			"ShipView_RepairSteel" => "Steel",
			"ShipView_RepairFuel" => "Fuel",
			"ShipView_Firepower" => "FP",
			"ShipView_FirepowerRemain" => "FP",
			"ShipView_FirepowerTotal" => "FP",
			"ShipView_Torpedo" => "Torpedo",
			"ShipView_TorpedoRemain" => "Torpedo",
			"ShipView_TorpedoTotal" => "Torpedo",
			"ShipView_AA" => "AA",
			"ShipView_AARemain" => "AA",
			"ShipView_AATotal" => "AA",
			"ShipView_Armor" => "Armor",
			"ShipView_ArmorRemain" => "Armor",
			"ShipView_ArmorTotal" => "Armor",
			"ShipView_ASW" => "ASW",
			"ShipView_ASWTotal" => "ASW",
			"ShipView_Evasion" => "Evasion",
			"ShipView_EvasionTotal" => "Evasion",
			"ShipView_LOS" => "LoS",
			"ShipView_LOSTotal" => "LoS",
			"ShipView_Luck" => "Luck",
			"ShipView_LuckRemain" => "Luck",
			"ShipView_LuckTotal" => "Luck",
			"ShipView_BomberTotal" => "Bombers Total",
			"ShipView_Speed" => "Speed",
			"ShipView_Range" => "Range",
			"ShipView_AirBattlePower" => "Air",
			"ShipView_ShellingPower" => "Shelling Power",
			"ShipView_AircraftPower" => "Bombing Power",
			"ShipView_AntiSubmarinePower" => "ASW Power",
			"ShipView_TorpedoPower" => "Torpedo Power",
			"ShipView_NightBattlePower" => "NB",
			"ShipView_Locked" => "Lock",
			"ShipView_SallyArea" => "Fleet",
		};


		private void ShipGroupSelected(string name)
		{
			if (DataGrid is null) return;

			DataGrid.Columns.Clear();
			foreach ((string key, ShipGroupData.ViewColumnData column) in
				Db.ShipGroup.ShipGroups.Values.First(g => g.Name == name).ViewColumns)
			{
				if(!column.Visible) continue;

				DataGrid.Columns.Add(MakeColumn(GetColumnName(key), GetColumnBinding(key), column.Width));
			}

			static DataGridTextColumn MakeColumn(string header, string binding, int width) => new()
			{
				Header = header,
				Binding = new Binding(binding)
				{
					Mode = BindingMode.OneTime
				},
				Width = new DataGridLength(width)
			};
		}
	}
}