using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf.Compass.ViewModels;

public class MasterShipSlotViewModel : ObservableObject
{
	public IEquipmentDataMaster? Equipment { get; set; }
	public int Size { get; set; }

	public string SizeString => Size switch
	{
		> 0 => $"{Size}",
		_ => ""
	};
	public ImageSource? EquipmentIcon =>
		ImageSourceIcons.GetEquipmentIcon(Equipment?.IconTypeTyped ?? EquipmentIconType.Nothing);
}
