using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Wpf.ShipGroup.ViewModels;

namespace ElectronicObserver.Window.Wpf.ShipGroup;

/// <summary>
/// Interaction logic for ShipGroupView.xaml
/// </summary>
public partial class ShipGroupView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		nameof(ViewModel), typeof(ShipGroupViewModel), typeof(ShipGroupView), new PropertyMetadata(default(ShipGroupViewModel), PropertyChangedCallback));

	private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is not ShipGroupView view) return;

		view.ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ShipGroupViewModel.SelectedGroup)) return;
			if (view.ViewModel.SelectedGroup is null) return;

			foreach ((string key, ShipGroupData.ViewColumnData column) in view.ViewModel.SelectedGroup.ViewColumns)
			{
				DataGridColumn dataGridColumn = view.GetColumnByName(key);
				dataGridColumn.Width = new(column.Width);
				dataGridColumn.Visibility = (System.Windows.Visibility)new BooleanToVisibilityConverter().Convert(column.Visible, null, null, null);
				dataGridColumn.DisplayIndex = column.DisplayIndex;
			}
		};
	}

	public ShipGroupViewModel ViewModel
	{
		get => (ShipGroupViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	private DataGridColumn GetColumnByName(string name) => name switch
	{
		"ShipView_ID" => ShipView_ID,
		"ShipView_ShipType" => ShipView_ShipType,
		"ShipView_Name" => ShipView_Name,
		"ShipView_Level" => ShipView_Level,
		"ShipView_Exp" => ShipView_Exp,
		"ShipView_Next" => ShipView_Next,
		"ShipView_NextRemodel" => ShipView_NextRemodel,
		"ShipView_HP" => ShipView_HP,
		"ShipView_Condition" => ShipView_Condition,
		"ShipView_Fuel" => ShipView_Fuel,
		"ShipView_Ammo" => ShipView_Ammo,
		"ShipView_Slot1" => ShipView_Slot1,
		"ShipView_Slot2" => ShipView_Slot2,
		"ShipView_Slot3" => ShipView_Slot3,
		"ShipView_Slot4" => ShipView_Slot4,
		"ShipView_Slot5" => ShipView_Slot5,
		"ShipView_ExpansionSlot" => ShipView_ExpansionSlot,
		"ShipView_Aircraft1" => ShipView_Aircraft1,
		"ShipView_Aircraft2" => ShipView_Aircraft2,
		"ShipView_Aircraft3" => ShipView_Aircraft3,
		"ShipView_Aircraft4" => ShipView_Aircraft4,
		"ShipView_Aircraft5" => ShipView_Aircraft5,
		"ShipView_AircraftTotal" => ShipView_AircraftTotal,
		"ShipView_Fleet" => ShipView_Fleet,
		"ShipView_RepairTime" => ShipView_RepairTime,
		"ShipView_RepairSteel" => ShipView_RepairSteel,
		"ShipView_RepairFuel" => ShipView_RepairFuel,
		"ShipView_Firepower" => ShipView_Firepower,
		"ShipView_FirepowerRemain" => ShipView_FirepowerRemain,
		"ShipView_FirepowerTotal" => ShipView_FirepowerTotal,
		"ShipView_Torpedo" => ShipView_Torpedo,
		"ShipView_TorpedoRemain" => ShipView_TorpedoRemain,
		"ShipView_TorpedoTotal" => ShipView_TorpedoTotal,
		"ShipView_AA" => ShipView_AA,
		"ShipView_AARemain" => ShipView_AARemain,
		"ShipView_AATotal" => ShipView_AATotal,
		"ShipView_Armor" => ShipView_Armor,
		"ShipView_ArmorRemain" => ShipView_ArmorRemain,
		"ShipView_ArmorTotal" => ShipView_ArmorTotal,
		"ShipView_ASW" => ShipView_ASW,
		"ShipView_ASWTotal" => ShipView_ASWTotal,
		"ShipView_Evasion" => ShipView_Evasion,
		"ShipView_EvasionTotal" => ShipView_EvasionTotal,
		"ShipView_LOS" => ShipView_LOS,
		"ShipView_LOSTotal" => ShipView_LOSTotal,
		"ShipView_Luck" => ShipView_Luck,
		"ShipView_LuckRemain" => ShipView_LuckRemain,
		"ShipView_LuckTotal" => ShipView_LuckTotal,
		"ShipView_BomberTotal" => ShipView_BomberTotal,
		"ShipView_Speed" => ShipView_Speed,
		"ShipView_Range" => ShipView_Range,
		"ShipView_AirBattlePower" => ShipView_AirBattlePower,
		"ShipView_ShellingPower" => ShipView_ShellingPower,
		"ShipView_AircraftPower" => ShipView_AircraftPower,
		"ShipView_AntiSubmarinePower" => ShipView_AntiSubmarinePower,
		"ShipView_TorpedoPower" => ShipView_TorpedoPower,
		"ShipView_NightBattlePower" => ShipView_NightBattlePower,
		"ShipView_Locked" => ShipView_Locked,
		"ShipView_SallyArea" => ShipView_SallyArea,
	};

	public ShipGroupView()
	{
		InitializeComponent();
	}

	private void DataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		ViewModel.SelectedShips = DataGrid.SelectedItems.Cast<ShipGroupItemViewModel>().ToList();
	}
}
