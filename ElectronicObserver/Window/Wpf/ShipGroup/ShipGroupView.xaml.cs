using System.Windows;

namespace ElectronicObserver.Window.Wpf.ShipGroup;

/// <summary>
/// Interaction logic for ShipGroupView.xaml
/// </summary>
public partial class ShipGroupView
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		nameof(ViewModel), typeof(ShipGroupViewModel), typeof(ShipGroupView), new PropertyMetadata(default(ShipGroupViewModel)));

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
