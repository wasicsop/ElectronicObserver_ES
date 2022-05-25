using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;

namespace ElectronicObserver.Window.Wpf.Fleet.Views;

/// <summary>
/// Interaction logic for FleetStateView.xaml
/// </summary>
public partial class FleetStateView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(FleetStateViewModel), typeof(FleetStateView), new PropertyMetadata(default(FleetStateViewModel)));

	public FleetStateViewModel ViewModel
	{
		get { return (FleetStateViewModel)GetValue(ViewModelProperty); }
		set { SetValue(ViewModelProperty, value); }
	}

	public FleetStateView()
	{
		InitializeComponent();
	}
}
