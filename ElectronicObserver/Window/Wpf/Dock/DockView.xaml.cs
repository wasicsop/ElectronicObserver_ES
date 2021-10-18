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

namespace ElectronicObserver.Window.Wpf.Dock;

/// <summary>
/// Interaction logic for DockView.xaml
/// </summary>
public partial class DockView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(DockViewModel), typeof(DockView), new PropertyMetadata(default(DockViewModel)));

	public DockViewModel ViewModel
	{
		get => (DockViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public DockView()
	{
		InitializeComponent();
	}
}
