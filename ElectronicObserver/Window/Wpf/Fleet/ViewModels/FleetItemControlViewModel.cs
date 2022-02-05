using System;
using System.Windows.Media;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Tools.DialogAlbumMasterShip;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Color = System.Drawing.Color;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public class FleetItemControlViewModel : ObservableObject
{
	public int MaxWidth { get; set; }
	public string? Text { get; set; }
	public int Tag { get; set; }
	public string? ToolTip { get; set; }
	public Color ForeColor { get; set; }
	public Color BackColor { get; set; }
	public bool Visible { get; set; }
	public Enum? ImageIndex { get; set; }

	public SolidColorBrush Foreground => ForeColor.ToBrush();
	public SolidColorBrush Background => BackColor.ToBrush();
	public ImageSource? Icon => ImageIndex switch
	{
		IconContent i => ImageSourceIcons.GetIcon(i),
		ResourceManager.EquipmentContent e => ImageSourceIcons.GetEquipmentIcon((EquipmentIconType)e),
		_ => null
	};
	public IRelayCommand ShipNameRightClick { get; }

	public FleetItemControlViewModel()
	{
		ShipNameRightClick = new RelayCommand(() => new DialogAlbumMasterShipWpf(Tag).Show(App.Current.MainWindow));

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		ConfigurationChanged();
	}

	private void ConfigurationChanged()
	{
		ForeColor = Utility.Configuration.Config.UI.ForeColor;
		BackColor = Color.Transparent;
	}
}
