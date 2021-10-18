using System.Windows;
using System.Windows.Media;

namespace ElectronicObserver.Window.Wpf;

public static class Extensions
{
	public static Visibility ToVisibility(this bool visible) => visible switch
	{
		true => Visibility.Visible,
		_ => Visibility.Collapsed
	};

	public static SolidColorBrush ToBrush(this System.Drawing.Color color) =>
		new(Color.FromArgb(color.A, color.R, color.G, color.B));
}
