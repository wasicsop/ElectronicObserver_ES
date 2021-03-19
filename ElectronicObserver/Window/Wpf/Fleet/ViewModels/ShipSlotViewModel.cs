using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ElectronicObserver.Resource;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using PropertyChanged;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels
{
    public class ShipSlotViewModel : ObservableObject
    {
	    private IEquipmentData? _equipment;
	    public Visibility SlotVisibility { get; set; }

		[DoNotNotify]
	    public IEquipmentData? Equipment
	    {
		    get => _equipment;
		    set
		    {
			    _equipment = value;

			    Level = Equipment?.Level ?? 0;
			    AircraftLevel = Equipment?.AircraftLevel ?? 0;
			    NameWithLevel = Equipment?.NameWithLevel ?? "";
				EquipmentIconType = Equipment?.MasterEquipment.IconTypeTyped ?? EquipmentIconType.Nothing;
		    }
	    }

	    public int CurrentAircraft { get; set; }
		public int Size { get; set; }

		private int Level { get; set; }
		public int AircraftLevel { get; set; }
		public string NameWithLevel { get; set; } = "";

		public string LevelString => Level switch
		{
			10 => "★",
			> 0 => $"+{Level}",
			_ => ""
		};

		public string CurrentAircraftString => CurrentAircraft switch
		{
			0 => "",
			{ } s => $"{s}"
		};

		private List<SolidColorBrush> CurrentAircraftBrushes { get; } = new()
		{
			new(Colors.Red),
			new(Colors.Gray)
		};

		public SolidColorBrush CurrentAircraftBrush => (CurrentAircraft < Size) switch
		{
			true => CurrentAircraftBrushes[0],
			_ => CurrentAircraftBrushes[1],
		};

		public EquipmentIconType EquipmentIconType { get; set; } = EquipmentIconType.Nothing;
		
		public ImageSource? EquipmentIcon => ImageSourceIcons.GetEquipmentIcon(EquipmentIconType);

		public ImageSource? AircraftLevelIcon => AircraftLevel switch
		{
			>= 0 and < 8 => AircraftLevelIcons[AircraftLevel],
			_ => AircraftLevelIcons[0]
		};

		private static ImageSource?[] AircraftLevelIcons { get; } =
		{
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.AircraftLevelTop0),
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.AircraftLevelTop1),
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.AircraftLevelTop2),
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.AircraftLevelTop3),
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.AircraftLevelTop4),
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.AircraftLevelTop5),
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.AircraftLevelTop6),
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.AircraftLevelTop7),
		};
    }
}
