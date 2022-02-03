using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using ElectronicObserver.Utility;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Org.BouncyCastle.Asn1.X509;
using System.Runtime.InteropServices;

namespace ElectronicObserver.ViewModels;

public static class DialogExtensions
{
	public static void ShowDialogExt(this System.Windows.Window window, FormMainViewModel formMainViewModel)
	{
		window.Owner = formMainViewModel.Window;
		window.ShowDialog();
	}
	public static void ShowExt(this System.Windows.Forms.Form form, FormMainViewModel formMainViewModel)
	{
		WindowInteropHelper helper = new WindowInteropHelper(formMainViewModel.Window);
		SetParent(form.Handle, helper.Handle);
		form.ShowInTaskbar = true;
		form.Show();
	}
	[DllImport("User32.dll")]
	static extern IntPtr SetParent(IntPtr hWnd, IntPtr hParent);
}
