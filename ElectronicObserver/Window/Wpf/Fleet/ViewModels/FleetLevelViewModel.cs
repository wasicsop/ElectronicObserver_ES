using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public partial class FleetLevelViewModel : ObservableObject
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

	[ICommand]
	private void OpenExpChecker()
	{
		new DialogExpChecker(Tag).Show(App.Current.MainWindow);
	}
}
