using System.Drawing;
using System.Windows.Media;
using ElectronicObserver.Resource;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf;

public static class ImageSourceIcons
{
	public static ImageSource? GetIcon(IconContent type) => type switch
	{
		IconContent.Nothing => null,
		_ => BytesToImageSource(GetBytes(ResourceManager.Instance.Icons.Images[(int)type]))
	};

	public static ImageSource? GetEquipmentIcon(EquipmentIconType type) =>
		BytesToImageSource(GetBytes(ResourceManager.GetEquipmentImage((int)type)));

	private static ImageSource? BytesToImageSource(byte[]? bytes) => bytes switch
	{
		{ } => (ImageSource?)new ImageSourceConverter().ConvertFrom(bytes),
		_ => null
	};

	private static byte[]? GetBytes(Image? image) => image switch
	{
		{ } => (byte[])new ImageConverter().ConvertTo(image, typeof(byte[])),
		_ => null
	};
}
