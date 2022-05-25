using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Wpf.Compass;

/// <summary>
/// Interaction logic for CompassView.xaml
/// </summary>
public partial class CompassView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(CompassViewModel), typeof(CompassView), new PropertyMetadata(default(CompassViewModel)));

	public CompassViewModel ViewModel
	{
		get => (CompassViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public CompassView()
	{
		InitializeComponent();
	}
}
