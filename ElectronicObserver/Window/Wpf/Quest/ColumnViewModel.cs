using System.ComponentModel;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Wpf.Quest;

public class ColumnViewModel : ObservableObject
{
	public DataGridLength Width { get; set; } = new(0, DataGridLengthUnitType.Auto);
	public bool Visible { get; set; } = true;
	public ListSortDirection? SortDirection { get; set; }
}
