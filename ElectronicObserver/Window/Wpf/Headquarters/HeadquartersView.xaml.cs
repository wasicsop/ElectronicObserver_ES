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

namespace ElectronicObserver.Window.Wpf.Headquarters;

/// <summary>
/// Interaction logic for HeadquartersView.xaml
/// </summary>
public partial class HeadquartersView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(HeadquartersViewModel), typeof(HeadquartersView), new PropertyMetadata(default(HeadquartersViewModel)));

	public HeadquartersViewModel ViewModel
	{
		get => (HeadquartersViewModel) GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public HeadquartersView()
	{
		InitializeComponent();
	}
}