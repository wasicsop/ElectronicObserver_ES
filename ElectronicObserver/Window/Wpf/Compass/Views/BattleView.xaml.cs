using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Wpf.Compass.Views;

/// <summary>
/// Interaction logic for BattleView.xaml
/// </summary>
public partial class BattleView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(CompassViewModel), typeof(BattleView), new PropertyMetadata(default(CompassViewModel)));

	public CompassViewModel ViewModel
	{
		get => (CompassViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public BattleView()
	{
		InitializeComponent();
	}
}
