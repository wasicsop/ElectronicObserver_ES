using System.Windows.Media;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.Window.Dialog;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public class FleetLevelViewModel : ObservableObject
{
	public string? TextNext { get; internal set; }
	public int Value { get; set; }
	public int MaximumValue { get; set; }
	public int ValueNext { get; set; }
	public int Tag { get; set; }
	public string? ToolTip { get; set; }
	public SerializableFont SubFont { get; set; }
	public System.Drawing.Color SubFontColor { get; set; }
	public bool NextVisible { get; set; }

	public FontFamily SubFontFamily => new(SubFont.FontData.FontFamily.Name);
	public double SubFontSize => SubFont.FontData.ToSize();
	public SolidColorBrush SubForeground => SubFontColor.ToBrush();

	public IRelayCommand ShipLevelRightClick { get; }

	public FleetLevelViewModel()
	{
		ShipLevelRightClick = new RelayCommand(() => new DialogExpChecker(Tag).Show());
	}
}
