using System.Windows.Forms;
using System.Windows.Interop;

namespace ElectronicObserver.ViewModels;

public static class DialogExtensions
{
	public static void Show(this System.Windows.Window window, System.Windows.Window mainWindow)
	{
		window.Owner = mainWindow;
		window.Show();
	}

	public static bool? ShowDialog(this System.Windows.Window window, System.Windows.Window mainWindow)
	{
		window.Owner = mainWindow;
		return window.ShowDialog();
	}

	public static void Show(this Form form, System.Windows.Window mainWindow)
	{
		WindowInteropHelper helper = new(mainWindow);
		NativeWindow win32Parent = new();
		win32Parent.AssignHandle(helper.Handle);
		
		form.Show(win32Parent);
	}

	public static DialogResult ShowDialog(this Form form, System.Windows.Window mainWindow)
	{
		WindowInteropHelper helper = new(mainWindow);
		NativeWindow win32Parent = new();
		win32Parent.AssignHandle(helper.Handle);

		form.TopMost = true;

		return form.ShowDialog(win32Parent);
	}

	public static DialogResult ShowDialog(this CommonDialog dialog, System.Windows.Window mainWindow)
	{
		WindowInteropHelper helper = new(mainWindow);
		NativeWindow win32Parent = new();
		win32Parent.AssignHandle(helper.Handle);

		return dialog.ShowDialog(win32Parent);
	}
}
