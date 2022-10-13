using ElectronicObserver.Utility;
using Microsoft.Win32;

namespace ElectronicObserver.Services;

public class FileService
{
	private string LayoutFilter => "Layout File|*.xml";

	/// <summary>
	/// Opens a file selection dialog to select a layout file.
	/// </summary>
	/// <param name="path">Current layout path.</param>
	/// <returns>Selected file path or null if no path was selected.</returns>
	public string? OpenLayoutPath(string path)
	{
		OpenFileDialog dialog = new()
		{
			Filter = LayoutFilter,
			Title = Properties.Window.FormMain.OpenLayoutCaption,
		};

		PathHelper.InitOpenFileDialog(path, dialog);

		return dialog.ShowDialog(App.Current!.MainWindow) switch
		{
			true => PathHelper.GetPathFromOpenFileDialog(dialog),
			_ => null,
		};
	}

	/// <summary>
	/// Opens a file save dialog to save a layout file.
	/// </summary>
	/// <param name="path">Current layout path.</param>
	/// <returns>Selected file path or null if no path was selected.</returns>
	public string? SaveLayoutPath(string path)
	{
		SaveFileDialog dialog = new()
		{
			Filter = LayoutFilter,
			Title = Properties.Window.FormMain.OpenLayoutCaption,
		};

		PathHelper.InitSaveFileDialog(path, dialog);

		return dialog.ShowDialog(App.Current!.MainWindow) switch
		{
			true => PathHelper.GetPathFromSaveFileDialog(dialog),
			_ => null,
		};
	}
}
