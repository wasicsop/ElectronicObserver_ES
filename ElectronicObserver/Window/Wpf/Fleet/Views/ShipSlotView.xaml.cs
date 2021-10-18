using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;

namespace ElectronicObserver.Window.Wpf.Fleet.Views;

/// <summary>
/// Interaction logic for ShipSlotView.xaml
/// </summary>
public partial class ShipSlotView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(ShipSlotViewModel), typeof(ShipSlotView), new PropertyMetadata(default(ShipSlotViewModel)));

	public ShipSlotViewModel ViewModel
	{
		get { return (ShipSlotViewModel)GetValue(ViewModelProperty); }
		set { SetValue(ViewModelProperty, value); }
	}

	public ShipSlotView()
	{
		InitializeComponent();
	}
}
