using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Storage;
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

	    public SerializableFont Font { get; set; }
	    public bool ShowAircraft { get; set; } = true;

		public int AircraftCurrent { get; set; }
		public int AircraftMax { get; set; }

		public int Level { get; set; }
		public int AircraftLevel { get; set; }
		public string NameWithLevel { get; set; } = "";

		public string LevelString => Level switch
		{
			10 => "★",
			> 0 => $"+{Level}",
			_ => ""
		};

		public string CurrentAircraftString { get; set; } = "";

		public FontFamily FontFamily => new(Font.FontData.FontFamily.Name);
		public float FontSize => Font.FontData.Size;

		public System.Drawing.Color EquipmentLevelColor { get; set; }
		public SolidColorBrush EquipmentLevelBrush => EquipmentLevelColor.ToBrush();

		public System.Drawing.Color CurrentAircraftColor { get; set; }
		public SolidColorBrush CurrentAircraftBrush => CurrentAircraftColor.ToBrush();

		public EquipmentIconType EquipmentIconType { get; set; } = EquipmentIconType.Nothing;
		
		public ImageSource? EquipmentIcon => ImageSourceIcons.GetEquipmentIcon(EquipmentIconType);

		public ImageSource? AircraftLevelIcon => AircraftLevel switch
		{
			>= 0 and < 8 => AircraftLevelIcons[AircraftLevel],
			_ => AircraftLevelIcons[0]
		};

		private static ImageSource?[] AircraftLevelIcons { get; } =
		{
			ImageSourceIcons.GetIcon(IconContent.AircraftLevelTop0),
			ImageSourceIcons.GetIcon(IconContent.AircraftLevelTop1),
			ImageSourceIcons.GetIcon(IconContent.AircraftLevelTop2),
			ImageSourceIcons.GetIcon(IconContent.AircraftLevelTop3),
			ImageSourceIcons.GetIcon(IconContent.AircraftLevelTop4),
			ImageSourceIcons.GetIcon(IconContent.AircraftLevelTop5),
			ImageSourceIcons.GetIcon(IconContent.AircraftLevelTop6),
			ImageSourceIcons.GetIcon(IconContent.AircraftLevelTop7),
		};

		public ShipSlotViewModel()
		{
			Font = Utility.Configuration.Config.UI.SubFont;

			AircraftColorDisabled = System.Drawing.Color.FromArgb(0xAA, 0xAA, 0xAA);
			AircraftColorLost = Utility.Configuration.Config.UI.Color_Magenta;
			AircraftColorDamaged = Utility.Configuration.Config.UI.Color_Red;
			AircraftColorFull = Utility.Configuration.Config.UI.ForeColor;

			EquipmentLevelColor = Utility.Configuration.Config.UI.Fleet_EquipmentLevelColor;
			/*
			AircraftLevelColorLow = System.Drawing.Color.FromArgb(0x66, 0x99, 0xEE);
			AircraftLevelColorHigh = System.Drawing.Color.FromArgb(0xFF, 0xAA, 0x00);

			InvalidSlotColor = System.Drawing.Color.FromArgb(0x40, 0xFF, 0x00, 0x00);
			*/

			PropertyChanged += AircraftChange;
		}

		/// <summary>
		/// 艦載機非搭載スロットの文字色
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "170, 170, 170")]
		[Description("艦載機非搭載スロットの文字色を指定します。")]
		public System.Drawing.Color AircraftColorDisabled { get; set; }

		/// <summary>
		/// 艦載機全滅スロットの文字色
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "255, 0, 255")]
		[Description("艦載機全滅スロットの文字色を指定します。")]
		public System.Drawing.Color AircraftColorLost { get; set; }

		/// <summary>
		/// 艦載機被撃墜スロットの文字色
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "255, 0, 0")]
		[Description("艦載機被撃墜スロットの文字色を指定します。")]
		public System.Drawing.Color AircraftColorDamaged { get; set; }

		/// <summary>
		/// 艦載機満載スロットの文字色
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "0, 0, 0")]
		[Description("艦載機満載スロットの文字色を指定します。")]
		public System.Drawing.Color AircraftColorFull { get; set; }

		private void AircraftChange(object sender, PropertyChangedEventArgs e)
		{
			// when EquipmentIconType changes the plane count needs to be redrawn
			// if you equip a plane on a 0 slot, the 0 must be displayed
			// if you equip a non-plane on a 0 slot, the 0 must not be displayed
			// if you don't redraw when switching plane <-> non-plane, the 0 will be displayed incorrectly
			if (e.PropertyName is not (nameof(AircraftCurrent) or nameof(AircraftMax) or nameof(ShowAircraft)
				or nameof(EquipmentIconType)))
			{
				return;
			}

			System.Drawing.Color aircraftColor = AircraftColorDisabled;
			bool drawAircraftSlot = ShowAircraft;

			if (AircraftMax == 0)
			{
				if (Equipment?.MasterEquipment.IsAircraft ?? false)
				{
					aircraftColor = AircraftColorDisabled;
				}
				else
				{
					drawAircraftSlot = false;
				}

			}
			else if (AircraftCurrent == 0)
			{
				aircraftColor = AircraftColorLost;

			}
			else if (AircraftCurrent < AircraftMax)
			{
				aircraftColor = AircraftColorDamaged;

			}
			else if (!(Equipment?.MasterEquipment.IsAircraft ?? false))
			{
				aircraftColor = AircraftColorDisabled;

			}
			else
			{
				aircraftColor = AircraftColorFull;
			}

			CurrentAircraftColor = aircraftColor;

			CurrentAircraftString = drawAircraftSlot switch
			{
				true => $"{AircraftCurrent}",
				_ => ""
			};
		}
	}
}
