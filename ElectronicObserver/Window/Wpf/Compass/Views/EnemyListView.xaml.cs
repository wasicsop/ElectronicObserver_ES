using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Wpf.Compass.Views;

/// <summary>
/// Interaction logic for EnemyListView.xaml
/// </summary>
public partial class EnemyListView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(CompassViewModel), typeof(EnemyListView), new PropertyMetadata(default(CompassViewModel)));

	public CompassViewModel ViewModel
	{
		get => (CompassViewModel) GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public EnemyListView()
	{
		InitializeComponent();
	}
}