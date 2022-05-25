using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Wpf.Compass.Views;

/// <summary>
/// Interaction logic for TextView.xaml
/// </summary>
public partial class TextView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(CompassViewModel), typeof(TextView), new PropertyMetadata(default(CompassViewModel)));

	public CompassViewModel ViewModel
	{
		get => (CompassViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public TextView()
	{
		InitializeComponent();
	}
}
