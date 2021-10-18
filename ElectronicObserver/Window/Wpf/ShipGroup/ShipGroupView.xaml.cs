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
using ElectronicObserver.Window.Wpf.ShipGroup.ViewModels;

namespace ElectronicObserver.Window.Wpf.ShipGroup;

/// <summary>
/// Interaction logic for ShipGroupView.xaml
/// </summary>
public partial class ShipGroupView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(ShipGroupViewModel), typeof(ShipGroupView), new PropertyMetadata(default(ShipGroupViewModel), PropertyChangedCallback));

	private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is not ShipGroupView view) return;

		view.ViewModel.DataGrid = view.DataGrid;
	}

	public ShipGroupViewModel ViewModel
	{
		get => (ShipGroupViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public ShipGroupView()
	{
		InitializeComponent();
	}
}
