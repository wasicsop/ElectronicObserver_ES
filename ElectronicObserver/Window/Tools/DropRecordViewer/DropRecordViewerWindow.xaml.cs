using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;
/// <summary>
/// Interaction logic for DropRecordViewerWindow.xaml
/// </summary>
public partial class DropRecordViewerWindow : System.Windows.Window
{
	private DropRecordViewerViewModel ViewModel = new();

	public DropRecordViewerWindow()
	{
		InitializeComponent();

		DataContext = ViewModel;
	}

	private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (sender is not DataGrid dataGrid) return;

		ViewModel.SelectedRows = dataGrid.SelectedItems.Cast<DropRecordRow>().ToList();
	}

	private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
	{
		ViewModel.RecordView_CellDoubleClick();
	}
}
