using System.Linq;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Tools.DevelopmentRecordViewer
{
	/// <summary>
	/// Interaction logic for DevelopmentRecordViewerWindow.xaml
	/// </summary>
	public partial class DevelopmentRecordViewerWindow
	{
		public DevelopmentRecordViewerWindow() : base(new DevelopmentRecordViewerViewModel())
		{
			InitializeComponent();
		}

		private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is not DataGrid dataGrid) return;

			ViewModel.SelectedRows = dataGrid.SelectedItems.Cast<DevelopmentRecordRow>().ToList();
		}
	}
}
