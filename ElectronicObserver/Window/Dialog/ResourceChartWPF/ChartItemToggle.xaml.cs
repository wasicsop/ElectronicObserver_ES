using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicObserver.Window.Dialog.ResourceChartWPF;
/// <summary>
/// Interaction logic for ChartItemToggle.xaml
/// </summary>
public partial class ChartItemToggle : UserControl
{
	public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
		nameof(IsChecked), typeof(bool), typeof(ChartItemToggle), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

	public bool IsChecked
	{
		get => (bool)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}

	public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
		nameof(Text), typeof(string), typeof(ChartItemToggle), new PropertyMetadata(default(string)));

	public string? Text
	{
		get => (string?)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
		nameof(Color), typeof(Brush), typeof(ChartItemToggle), new PropertyMetadata(default(Brush)));

	public Brush Color
	{
		get => (Brush)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}

	public ChartItemToggle()
	{
		InitializeComponent();
	}

	private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
	{
		IsChecked = !IsChecked;
	}
}
