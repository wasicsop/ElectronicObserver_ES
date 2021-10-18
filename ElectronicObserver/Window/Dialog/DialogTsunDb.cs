using System;
using System.Diagnostics;
using System.Windows.Forms;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Dialog;

public partial class DialogTsunDb : Form
{
	public DialogTsunDb()
	{
		InitializeComponent();
	}

	#region Events
	private void linkLabelTsunDbText_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start(new ProcessStartInfo("https://github.com/Jebzou/ElectronicObserver/wiki/Privacy-policy") { UseShellExecute = true });
	}
	#endregion

	private void buttonEnable_Click(object sender, EventArgs e)
	{
		Configuration.Config.Control.SubmitDataToTsunDb = true;
		this.DialogResult = DialogResult.Yes;
		this.Close();
	}

	private void buttonDisable_Click(object sender, EventArgs e)
	{
		Configuration.Config.Control.SubmitDataToTsunDb = false;
		this.DialogResult = DialogResult.No;
		this.Close();
	}
}
