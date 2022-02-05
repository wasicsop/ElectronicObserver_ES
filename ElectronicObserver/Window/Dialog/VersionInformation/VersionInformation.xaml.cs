using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using ElectronicObserver.Utility;
using Translation = ElectronicObserver.Properties.Window.Dialog.DialogVersion;

namespace ElectronicObserver.Window.Dialog.VersionInformation;
/// <summary>
/// Interaction logic for VersionInformation.xaml
/// </summary>
public partial class VersionInformationWindow
{
	public VersionInformationWindow()
	{
		InitializeComponent();

		Translate();
		string versionText = CultureInfo.CurrentCulture.Name switch
		{
			"ja-JP" => SoftwareInformation.VersionJapanese,
			_ => SoftwareInformation.SoftwareNameEnglish
		};

		TextVersion.Text = string.Format(Translation.TextVersionFormat, versionText, SoftwareInformation.VersionEnglish, SoftwareInformation.UpdateTime.ToString("d"));

		// https://github.com/Kinnara/ModernWpf/issues/378
		SourceInitialized += (s, a) =>
		{
			Dispatcher.Invoke(InvalidateVisual, DispatcherPriority.Input);
		};
	}

	private void Translate()
	{
		TextVersion.Text = Translation.TextVersion;
		TextAuthor.Text = Translation.Developer;
		TextAuthorLink.Inlines.Add(Translation.TextAuthor);
		ButtonClose.Content = Translation.ButtonClose;
		TextProjectSite.Text = Translation.ProjectSite;
		TextModifiedBy.Text = Translation.ModifiedBy;
		TextMaintainers.Text = Translation.Maintainers;
		Title = Translation.Title;
	}

	private void OpenHyperlink(object sender, RequestNavigateEventArgs e)
	{
		ProcessStartInfo psi = new()
		{
			FileName = e.Uri.AbsoluteUri,
			UseShellExecute = true
		};
		Process.Start(psi);
	}

	private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
	{
		Close();
	}
}
