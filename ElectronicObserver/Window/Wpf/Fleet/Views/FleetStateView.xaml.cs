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
		get { return (FleetStateViewModel) GetValue(ViewModelProperty); }
		set { SetValue(ViewModelProperty, value); }
	}

	public FleetStateView()
	{
		InitializeComponent();
	}
}