using System;
using System.Globalization;
using System.Windows.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Tools.DevelopmentRecordViewer;

public class DevelopmentFilterToDisplayConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			IShipDataMaster ship => ship.NameEN,
			IEquipmentDataMaster equip => equip.NameEN,
			EquipmentTypes e => KCDatabase.Instance.EquipmentTypes[(int)e].NameEN,
			Enum e => e.Display(),
			string s => s,
			_ => "???"
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
