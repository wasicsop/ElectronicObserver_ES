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
using ElectronicObserver.Window.Tools.DropRecordViewer;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.ShipPicker;
/// <summary>
/// Interaction logic for ShipPickerView.xaml
/// </summary>
public partial class ShipPickerView
{
	public ShipPickerView(ShipPickerViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
		
		ViewModel.PropertyChanged += ViewModel_PropertyChanged;
		ViewModel.PropertyChanged += ViewModel_PropertyChanged2;

		Closing += ShipPicker_Closing;
	}

	private void ShipPicker_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
	{
		ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
		ViewModel.PropertyChanged -= ViewModel_PropertyChanged2;
	}

	private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ViewModel.PickedShip)) return;

		PickedShip = ViewModel.PickedShip;
		PickedOption = null;
		DialogResult = true;
	}

	private void ViewModel_PropertyChanged2(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ViewModel.PickedOption)) return;

		PickedShip = null;
		PickedOption = ViewModel.PickedOption;
		DialogResult = true;
	}

	public IShipDataMaster? PickedShip { get; private set; }
	public DropRecordOption? PickedOption { get; private set; }
}
