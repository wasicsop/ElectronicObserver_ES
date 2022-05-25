using System.Windows;
using System.Windows.Controls;

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
		get => (HeadquartersViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public HeadquartersView()
	{
		InitializeComponent();
	}
}
