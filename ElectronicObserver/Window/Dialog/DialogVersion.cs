using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogVersion : Form
	{
		public DialogVersion()
		{
			InitializeComponent();

			TextVersion.Text = string.Format("{0} {1} ({2} release)", SoftwareInformation.SoftwareNameEnglish, SoftwareInformation.VersionEnglish, SoftwareInformation.UpdateTime.ToString("d"));
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
}
