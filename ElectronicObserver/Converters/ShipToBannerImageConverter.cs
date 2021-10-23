using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;

namespace ElectronicObserver.Converters;

public class ShipToBannerImageConverter : IValueConverter
{
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is not IShipDataMaster ship) return null;

		try
		{
			string? imageUri = KCResourceHelper
				.GetShipImagePath(ship.ID, false, KCResourceHelper.ResourceTypeShipBanner);

			return new BitmapImage(new Uri(imageUri, UriKind.RelativeOrAbsolute).ToAbsolute());
		}
		catch
		{
			return null;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
