using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
