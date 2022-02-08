using System;
using System.ComponentModel;
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

	public static int ToSerializableValue(this ListSortDirection? sortDirection) => sortDirection switch
	{
		null => -1,
		{ } => (int)sortDirection
	};

	public static ListSortDirection? ToSortDirection(this int sortDirection) => sortDirection switch
	{
		0 or 1 => (ListSortDirection)sortDirection,
		_ => null
	};
}
