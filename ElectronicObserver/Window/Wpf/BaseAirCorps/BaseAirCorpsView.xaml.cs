using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Wpf.BaseAirCorps;

/// <summary>
/// Interaction logic for BaseAirCorpsView.xaml
/// </summary>
public partial class BaseAirCorpsView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(BaseAirCorpsViewModel), typeof(BaseAirCorpsView), new PropertyMetadata(default(BaseAirCorpsViewModel)));

	public BaseAirCorpsViewModel ViewModel
	{
		get => (BaseAirCorpsViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public BaseAirCorpsView()
	{
		InitializeComponent();
	}

	private void FrameworkElement_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
	{
		if (KCDatabase.Instance.BaseAirCorps.Count == 0)
		{
			e.Handled = true;
		}
	}
}
