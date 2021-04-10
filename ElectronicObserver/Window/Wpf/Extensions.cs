using System.Windows.Media;

namespace ElectronicObserver.Window.Wpf
{
	public static class Extensions
	{
		public static SolidColorBrush ToBrush(this System.Drawing.Color color) =>
			new(Color.FromRgb(color.R, color.G, color.B));
	}
}