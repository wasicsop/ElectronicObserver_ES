using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Browser.AirControlSimulator;

/// <summary>
/// Interaction logic for AirControlSimulatorWindow.xaml
/// </summary>
public partial class AirControlSimulatorWindow : Window
{
	private AirControlSimulatorViewModel ViewModel { get; } = new();

	public AirControlSimulatorWindow(string url)
	{
		ViewModel.Uri = new(url);
		DataContext = ViewModel;

		InitializeComponent();

		ViewModel.ExecuteScriptAsync = s => Browser.ExecuteScriptAsync(s);
	}
}
