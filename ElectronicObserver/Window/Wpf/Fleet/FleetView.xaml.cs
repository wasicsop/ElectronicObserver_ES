using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using ElectronicObserverTypes;

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
