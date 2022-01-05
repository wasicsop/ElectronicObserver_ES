using System;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BrowserLibCore;
using CefSharp.Internals;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
namespace Browser;

/// <summary>
/// Interaction logic for BrowserView.xaml
/// </summary>
public partial class BrowserView : Window
{
	public BrowserViewModel ViewModel { get; }
	public BrowserView(string host, int port, string culture)
	{
		InitializeComponent();

		ViewModel = new(host, port, culture);

		Loaded += ViewModel.OnLoaded;
		DataContext = ViewModel;
	}


	private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (sender is not FrameworkElement control) return;

		ViewModel.DpiScale = VisualTreeHelper.GetDpi(this);

		ViewModel.ActualHeight = control.ActualHeight;
		ViewModel.ActualWidth = control.ActualWidth;
	}

	private void FrameworkElement_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
	{

	}



}

