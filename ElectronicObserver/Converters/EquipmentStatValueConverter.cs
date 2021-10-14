using System;
using System.Globalization;
using System.Windows.Data;

namespace ElectronicObserver.Converters;

public class EquipmentStatValueConverter : IValueConverter
{
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			int i => i.ToString("+#;-#;0"),
			_ => null
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}