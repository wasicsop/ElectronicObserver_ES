using System.Windows.Media;

namespace ElectronicObserver.Services;

public class ColorService
{
	/// <summary>
	/// Returns black or white depending on which one should be more visible on the given background. <br />
	/// todo: it's currently hardcoded to use black and white, which ignores custom themes
	/// </summary>
	/// <param name="backgroundColor"></param>
	/// <returns></returns>
	public Color GetForegroundColor(Color backgroundColor)
	{
		double luminance = (0.299 * backgroundColor.R + 0.587 * backgroundColor.G + 0.114 * backgroundColor.B) / 255;

		return luminance switch
		{
			> 0.5 => Colors.Black,
			_ => Colors.White,
		};
	}
}
