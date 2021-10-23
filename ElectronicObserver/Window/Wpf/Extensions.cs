using System;
using System.Diagnostics;
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

	public static float ToSize(this System.Drawing.Font font) => font.Size * font.Unit switch
	{
		System.Drawing.GraphicsUnit.Point => 4 / 3f,
		_ => 1
	};

	public static Uri ToAbsolute(this Uri uri) => uri switch
	{
		{ IsAbsoluteUri: true } => uri,
		_ => new(new Uri(Process.GetCurrentProcess().MainModule.FileName), uri)
	};
}
