using System.Windows;

namespace Browser.AirControlSimulator;

/// <summary>
/// Interaction logic for AirControlSimulatorWindow.xaml
/// </summary>
public partial class AirControlSimulatorWindow : Window
{
	private AirControlSimulatorViewModel ViewModel { get; } = new();

	public AirControlSimulatorWindow(string url)
	{
		ViewModel.Uri = url;
		DataContext = ViewModel;

		InitializeComponent();

		ViewModel.ExecuteScriptAsync = s => Browser.ExecuteScriptAsync(s);

		InitializeAsync();
	}

	private async void InitializeAsync()
	{
		await Browser.EnsureCoreWebView2Async(BrowserViewModel.Environment);

		Browser.CoreWebView2.Navigate(ViewModel.Uri);
	}
}
