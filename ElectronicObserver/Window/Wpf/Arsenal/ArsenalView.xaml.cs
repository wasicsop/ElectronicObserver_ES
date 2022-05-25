using System.Windows;
using System.Windows.Controls;

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
