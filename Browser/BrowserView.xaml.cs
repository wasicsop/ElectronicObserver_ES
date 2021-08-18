using System.Windows;
using System.Windows.Controls;

namespace Browser
{
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

			ViewModel.ActualHeight = control.ActualHeight;
			ViewModel.ActualWidth = control.ActualWidth;
		}

		private void FrameworkElement_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			
		}
	}
	/*
	public static class CustomCommands
	{
		public static readonly RoutedUICommand Screenshot = new RoutedUICommand
		(
			"",
			"Screenshot",
			typeof(CustomCommands),
			new InputGestureCollection
			{
				new KeyGesture(Key.F2)
			}
		);

		public static readonly RoutedUICommand Refresh = new RoutedUICommand
		(
			"",
			"Refresh",
			typeof(CustomCommands),
			new InputGestureCollection
			{
				new KeyGesture(Key.F5)
			}
		);

		public static readonly RoutedUICommand HardRefresh = new RoutedUICommand
		(
			"",
			"HardRefresh",
			typeof(CustomCommands),
			new InputGestureCollection
			{
				new KeyGesture(Key.F5, ModifierKeys.Control)
			}
		);

		public static readonly RoutedUICommand Mute = new RoutedUICommand
		(
			"",
			"Mute",
			typeof(CustomCommands),
			new InputGestureCollection
			{
				new KeyGesture(Key.F7)
			}
		);

		public static readonly RoutedUICommand OpenDeveloperTools = new RoutedUICommand
		(
			"",
			"OpenDeveloperTools",
			typeof(CustomCommands),
			new InputGestureCollection
			{
				new KeyGesture(Key.F12)
			}
		);
	}
	*/
}
