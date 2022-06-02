using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jot;

namespace Browser.AirControlSimulator;

/// <summary>
/// Interaction logic for AirControlSimulatorWindow.xaml
/// </summary>
public partial class AirControlSimulatorWindow : Window
{
	private Tracker Tracker { get; }
	private AirControlSimulatorViewModel ViewModel { get; } = new();

	public AirControlSimulatorWindow(string url)
	{
		Tracker = Ioc.Default.GetService<Tracker>()!;

		ViewModel.Uri = url;
		DataContext = ViewModel;

		InitializeComponent();

		ViewModel.ExecuteScriptAsync = s => Browser.ExecuteScriptAsync(s);

		InitializeAsync();

		Loaded += (_, _) =>
		{
			StartJotTracking();
		};
	}

	private void StartJotTracking()
	{
		Tracker.Track(this);
	}

	private async void InitializeAsync()
	{
		await Browser.EnsureCoreWebView2Async(BrowserViewModel.Environment);

		Browser.CoreWebView2.Navigate(ViewModel.Uri);
	}
}
