using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Wpf.FleetOverview;

/// <summary>
/// Interaction logic for FleetOverviewView.xaml
/// </summary>
public partial class FleetOverviewView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(FleetOverviewViewModel), typeof(FleetOverviewView), new PropertyMetadata(default(FleetOverviewViewModel)));

	public FleetOverviewViewModel ViewModel
	{
		get => (FleetOverviewViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public FleetOverviewView()
	{
		InitializeComponent();
	}
}
