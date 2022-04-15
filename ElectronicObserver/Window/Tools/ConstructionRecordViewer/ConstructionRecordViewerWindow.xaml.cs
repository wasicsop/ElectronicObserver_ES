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
