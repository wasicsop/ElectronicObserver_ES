using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.ShipPicker;
/// <summary>
/// Interaction logic for ShipPickerView.xaml
/// </summary>
public partial class ShipPickerView : System.Windows.Window
{
	public ShipPickerViewModel ViewModel { get; }

	public ShipPickerView(ShipPickerViewModel viewModel)
	{
		InitializeComponent();

		ViewModel = viewModel;
		ViewModel.PropertyChanged += ViewModel_PropertyChanged;

		DataContext = ViewModel;

		Closing += ShipPicker_Closing;
	}

	private void ShipPicker_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
	{
		ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
	}

	private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ViewModel.PickedShip)) return;

		PickedShip = ViewModel.PickedShip;
		DialogResult = true;
	}

	public IShipDataMaster? PickedShip { get; private set; }
}
