using System;
using System.Globalization;
using System.Windows.Data;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Converters;

public class EnumDisplayConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			EquipmentTypes e => KCDatabase.Instance.EquipmentTypes[(int)e].NameEN,
			Enum e => e.Display(),
			_ => throw new NotSupportedException()
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
