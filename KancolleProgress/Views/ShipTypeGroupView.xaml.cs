using System.Windows;
using System.Windows.Controls;
using KancolleProgress.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace KancolleProgress.Views;

public sealed partial class ShipTypeGroupView : UserControl
{
	public static readonly DependencyProperty ShipTypeGroupProperty = DependencyProperty.Register(
		"ShipTypeGroup", typeof(ShipTypeGroup), typeof(ShipTypeGroupView), new PropertyMetadata(default(ShipTypeGroup)));

	public ShipTypeGroup ShipTypeGroup
	{
		get => (ShipTypeGroup)GetValue(ShipTypeGroupProperty);
		set => SetValue(ShipTypeGroupProperty, value);
	}

	public ShipTypeGroupView()
	{
		this.InitializeComponent();
	}
}
