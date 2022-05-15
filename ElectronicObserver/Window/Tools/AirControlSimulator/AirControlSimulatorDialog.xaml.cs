using System.ComponentModel;
using System.Windows.Threading;

namespace ElectronicObserver.Window.Tools.AirControlSimulator;
/// <summary>
/// Interaction logic for BaseAirCorpsSimulationContentDialog.xaml
/// </summary>
public partial class BaseAirCorpsSimulationContentDialog
{
	public BaseAirCorpsSimulationContentDialog(AirControlSimulatorViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();

		ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

		// https://github.com/Kinnara/ModernWpf/issues/378
		SourceInitialized += (s, a) =>
		{
			Dispatcher.Invoke(InvalidateVisual, DispatcherPriority.Input);
		};
	}

	private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ViewModel.DialogResult)) return;

		ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
		Result = ViewModel;
		DialogResult = ViewModel.DialogResult;
	}

	public AirControlSimulatorViewModel? Result { get; set; }
}
