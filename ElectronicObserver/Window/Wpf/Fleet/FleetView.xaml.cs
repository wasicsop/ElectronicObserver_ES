using System;
using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Resource;

namespace ElectronicObserver.Window.Wpf.Fleet;

/// <summary>
/// Interaction logic for FleetView.xaml
/// </summary>
public partial class FleetView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(FleetViewModel), typeof(FleetView), new PropertyMetadata(default(FleetViewModel)));

	public FleetViewModel ViewModel
	{
		get => (FleetViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	// public FleetViewModel ViewModel { get; }

	public FleetView()
	{
		InitializeComponent();
	}

	public FleetView(int fleetId, Action<IconContent> setIcon)
	{
		/*
        ViewModel = new(fleetId, setIcon);
        DataContext = ViewModel;
		*/

		InitializeComponent();
	}
}
