using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Wpf.Dock;

/// <summary>
/// Interaction logic for DockView.xaml
/// </summary>
public partial class DockView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(DockViewModel), typeof(DockView), new PropertyMetadata(default(DockViewModel)));

	public DockViewModel ViewModel
	{
		get => (DockViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public DockView()
	{
		InitializeComponent();
	}
}
