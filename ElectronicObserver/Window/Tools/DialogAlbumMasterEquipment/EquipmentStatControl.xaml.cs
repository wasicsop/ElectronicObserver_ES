using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment;

/// <summary>
/// Interaction logic for EquipmentStatControl.xaml
/// </summary>
public partial class EquipmentStatControl : UserControl
{
	public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
		nameof(Image), typeof(ImageSource), typeof(EquipmentStatControl), new PropertyMetadata(default(ImageSource)));

	public ImageSource Image
	{
		get => (ImageSource)GetValue(ImageProperty);
		set => SetValue(ImageProperty, value);
	}

	public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
		nameof(Text), typeof(string), typeof(EquipmentStatControl), new PropertyMetadata(default(string)));

	public string Text
	{
		get => (string)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}

	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
		nameof(Value), typeof(int), typeof(EquipmentStatControl), new PropertyMetadata(default(int)));

	public int Value
	{
		get => (int)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	public EquipmentStatControl()
	{
		InitializeComponent();
	}
}
