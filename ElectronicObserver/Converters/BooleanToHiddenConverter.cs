using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ElectronicObserver.Converters;

public class BooleanToHiddenConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			true => Visibility.Visible,
			_ => Visibility.Hidden
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
