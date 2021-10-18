using System.Windows;

namespace ElectronicObserver.ViewModels;

public class WindowPosition
{
	public double Top { get; set; }
	public double Left { get; set; }
	public double Height { get; set; } = 840;
	public double Width { get; set; } = 1220;
	public WindowState WindowState { get; set; } = WindowState.Maximized;
}
