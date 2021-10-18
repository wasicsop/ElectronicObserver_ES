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

namespace ElectronicObserver.Window.Wpf.Arsenal;

/// <summary>
/// Interaction logic for ArsenalView.xaml
/// </summary>
public partial class ArsenalView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(ArsenalViewModel), typeof(ArsenalView), new PropertyMetadata(default(ArsenalViewModel)));

	public ArsenalViewModel ViewModel
	{
		get => (ArsenalViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public ArsenalView()
	{
		InitializeComponent();
	}
}
