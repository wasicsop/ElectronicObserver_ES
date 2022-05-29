using System;
using System.Globalization;
using System.Windows.Data;
using ElectronicObserver.Data;

namespace ElectronicObserver.Converters;

public class AaciDisplayConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			int id => $"{id}: {Constants.GetAACutinKind(id)}",
			_ => "???"
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
