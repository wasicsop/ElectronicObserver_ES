using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using Translation = ElectronicObserver.Properties.Window.Dialog.DialogVersion;

namespace ElectronicObserver.Window.Dialog;

public partial class DialogVersion : Form
{
	public DialogVersion()
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

	public void Translate()
	{
		TextVersion.Text = Translation.TextVersion;
		label1.Text = Translation.Developer;
		TextAuthor.Text = Translation.TextAuthor;
		ButtonClose.Text = Translation.ButtonClose;
		TextInformation.Text = Translation.TextInformation;
		label2.Text = Translation.ProjectSite;
		label3.Text = Translation.ModifiedBy;
		label4.Text = Translation.Maintainers;


		Text = Translation.Title;
	}

	private void TextAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		ProcessStartInfo psi = new ProcessStartInfo
		{
			FileName = "https://twitter.com/andanteyk",
			UseShellExecute = true
		};
		Process.Start(psi);
	}

	private void ButtonClose_Click(object sender, EventArgs e)
	{

		this.Close();

	}

	private void TextInformation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		ProcessStartInfo psi = new ProcessStartInfo
		{
			FileName = "https://github.com/gre4bee/ElectronicObserver",
			UseShellExecute = true
		};
		Process.Start(psi);
	}

	private void DialogVersion_Load(object sender, EventArgs e)
	{

		this.Icon = ResourceManager.Instance.AppIcon;
	}


}
