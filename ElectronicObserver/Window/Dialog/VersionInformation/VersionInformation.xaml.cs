using System.Diagnostics;
using System.Globalization;
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
	}

	private void Translate()
	{
		TextVersion.Text = Translation.TextVersion;
		TextAuthor.Text = Translation.Developer;
		TextAuthorLink.Text = Translation.TextAuthor;
		ButtonClose.Content = Translation.ButtonClose;
		TextProjectSite.Content = Translation.ProjectSite;
		TextModifiedBy.Content = Translation.ModifiedBy;
		TextMaintainers.Content = Translation.Maintainers;
		Title = Translation.Title;
	}

	private void TextAuthorLink_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		ProcessStartInfo psi = new ProcessStartInfo
		{
			FileName = "https://twitter.com/andanteyk",
			UseShellExecute = true
		};
		Process.Start(psi);
	}

	private void ProjectSiteLink_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		ProcessStartInfo psi = new ProcessStartInfo
		{
			FileName = "https://github.com/gre4bee/ElectronicObserver",
			UseShellExecute = true
		};
		Process.Start(psi);
	}

	private void ButtonClose_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		this.Close();
	}
}
