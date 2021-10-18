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

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterShip;

/// <summary>
/// Interaction logic for ShipStatControl.xaml
/// </summary>
public partial class ShipStatControl : UserControl
{
	public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
		nameof(Icon), typeof(ImageSource), typeof(ShipStatControl), new PropertyMetadata(default(ImageSource)));

	public ImageSource Icon
	{
		get => (ImageSource)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public static readonly DependencyProperty StatNameProperty = DependencyProperty.Register(
		nameof(StatName), typeof(string), typeof(ShipStatControl), new PropertyMetadata(default(string)));

	public string StatName
	{
		get => (string)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}

	public static readonly DependencyProperty BaseProperty = DependencyProperty.Register(
		nameof(Base), typeof(string), typeof(ShipStatControl), new PropertyMetadata(default(string)));

	public string Base
	{
		get => (string)GetValue(BaseProperty);
		set => SetValue(BaseProperty, value);
	}

	public static readonly DependencyProperty BaseToolTipProperty = DependencyProperty.Register(
		nameof(BaseToolTip), typeof(string), typeof(ShipStatControl), new PropertyMetadata(default(string?)));

	public string? BaseToolTip
	{
		get => (string?)GetValue(BaseToolTipProperty);
		set => SetValue(BaseToolTipProperty, value);
	}

	public static readonly DependencyProperty MaxProperty = DependencyProperty.Register(
		nameof(Max), typeof(string), typeof(ShipStatControl), new PropertyMetadata(default(string)));

	public string Max
	{
		get => (string)GetValue(MaxProperty);
		set => SetValue(MaxProperty, value);
	}

	public static readonly DependencyProperty MaxToolTipProperty = DependencyProperty.Register(
		nameof(MaxToolTip), typeof(string), typeof(ShipStatControl), new PropertyMetadata(default(string?)));

	public string? MaxToolTip
	{
		get => (string?)GetValue(MaxToolTipProperty);
		set => SetValue(MaxToolTipProperty, value);
	}

	public static readonly DependencyProperty ScaledProperty = DependencyProperty.Register(
		nameof(Scaled), typeof(string), typeof(ShipStatControl), new PropertyMetadata(default(string)));

	public string Scaled
	{
		get => (string)GetValue(ScaledProperty);
		set => SetValue(ScaledProperty, value);
	}

	public ShipStatControl()
	{
		InitializeComponent();
	}
}
