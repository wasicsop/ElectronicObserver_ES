using System;
using System.Globalization;
using System.Windows.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;

namespace ElectronicObserver.Converters;

public class ImageSourceConverter : IValueConverter
{
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			IconContent i => ImageSourceIcons.GetIcon(i),
			EquipmentIconType e => ImageSourceIcons.GetEquipmentIcon(e),
			_ => null
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
