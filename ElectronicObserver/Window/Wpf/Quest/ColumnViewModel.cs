using System.Windows.Controls;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Wpf.Quest;

public class ColumnViewModel : ObservableObject
{
	public DataGridLength Width { get; set; } = new(0, DataGridLengthUnitType.Auto);
	public bool Visible { get; set; } = true;
}