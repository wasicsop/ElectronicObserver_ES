using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Integrate;
using ElectronicObserver.Window.Wpf.WinformsHost;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers;

public class FormIntegrateViewModel : WinformsHostViewModel
{
	private FormIntegrate Integrate { get; }
	public override string ContentId => Integrate.PersistString;
	public override ICommand CloseCommand { get; }

	public FormIntegrateViewModel(FormIntegrate integrate, FormMainViewModel parent)
		: base("Integrate", "Integrate", ImageSourceIcons.GetIcon(IconContent.FormJson))
	{
		Integrate = integrate;

		integrate.TopLevel = false;
		WinformsControl = integrate;

		WindowsFormsHost.Child = WinformsControl;

		CloseCommand = new RelayCommand(() => parent.CloseIntegrate(this));

		integrate.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(FormIntegrate.Icon)) return;

			IconSource = Imaging.CreateBitmapSourceFromHIcon(
				Integrate.Icon.Handle,
				Int32Rect.Empty,
				BitmapSizeOptions.FromWidthAndHeight(16, 16));
		};

		integrate.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(FormIntegrate.Text)) return;

			Title = Integrate.Text;
		};
	}
}