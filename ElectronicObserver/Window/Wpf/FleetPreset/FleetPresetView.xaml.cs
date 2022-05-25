using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Wpf.FleetPreset;

/// <summary>
/// Interaction logic for FleetPresetView.xaml
/// </summary>
public partial class FleetPresetView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(FleetPresetViewModel), typeof(FleetPresetView), new PropertyMetadata(default(FleetPresetViewModel)));

	public FleetPresetViewModel ViewModel
	{
		get => (FleetPresetViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public FleetPresetView()
	{
		InitializeComponent();
	}
}
