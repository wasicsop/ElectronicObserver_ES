using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Converters;

public class EquipmentStatForegroundConverter : IValueConverter
{
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> new SolidColorBrush(value switch
		{
			0 => Colors.Silver,
			< 0 => Colors.Red,
			_ => Color.FromArgb(
				Configuration.Config.UI.ForeColor.A,
				Configuration.Config.UI.ForeColor.R,
				Configuration.Config.UI.ForeColor.G,
				Configuration.Config.UI.ForeColor.B)
		});

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
