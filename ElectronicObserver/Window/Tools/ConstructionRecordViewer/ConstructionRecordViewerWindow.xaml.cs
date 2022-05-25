using System.Linq;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Tools.ConstructionRecordViewer;
/// <summary>
/// Interaction logic for ConstructionRecordViewerWindow.xaml
/// </summary>
public partial class ConstructionRecordViewerWindow
{
	public ConstructionRecordViewerWindow() : base(new ConstructionRecordViewerViewModel())
	{
		InitializeComponent();
	}

	private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (sender is not DataGrid dataGrid) return;

		ViewModel.SelectedRows = dataGrid.SelectedItems.Cast<ConstructionRecordRow>().ToList();
	}
}
