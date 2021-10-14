using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Browser;

public static class Interop
{
	public static BitmapSource ToBitmapSource(this Bitmap bitmap)
	{
		using MemoryStream memory = new();

		bitmap.Save(memory, ImageFormat.Bmp);
		memory.Position = 0;
		BitmapImage bitmapimage = new();
		bitmapimage.BeginInit();
		bitmapimage.StreamSource = memory;
		bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
		bitmapimage.EndInit();

		return bitmapimage;
	}
}