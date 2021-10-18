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
