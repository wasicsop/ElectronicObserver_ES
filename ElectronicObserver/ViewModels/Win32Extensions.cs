using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
namespace ElectronicObserver.ViewModels;

class Wpf32Window : IWin32Window
{
	public IntPtr Handle { get; private set; }

	public Wpf32Window(System.Windows.Window wpfWindow)
	{
		Handle = new WindowInteropHelper(wpfWindow).Handle;
	}
}

public static class WindowExtensions
{
	public static IWin32Window GetWin32Window(this System.Windows.Window parent)
	{
		return new Wpf32Window(parent);
	}
}
