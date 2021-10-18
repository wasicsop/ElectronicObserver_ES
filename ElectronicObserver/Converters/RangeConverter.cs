using System;
using System.Globalization;
using System.Windows.Data;
using ElectronicObserver.Data;

namespace ElectronicObserver.Converters;

public class RangeConverter : IValueConverter
{
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			int i => Constants.GetRange(i),
			_ => null
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
