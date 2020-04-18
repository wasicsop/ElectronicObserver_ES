using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Xaml;
using ElectronicObserver.Window.Dialog.KancolleProgress;

namespace ElectronicObserver.Window.Dialog
{
    public partial class DialogKancolleProgress : Form
    {
		// dotnet core designer doesn't let you add it for some reason
	    private ElementHost elementHost;

		public DialogKancolleProgress()
        {
            InitializeComponent();
            this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormEquipmentList]);

            elementHost = new ElementHost {Dock = DockStyle.Fill};

            ClientSize = new Size(1444, 962);
            Controls.Add(elementHost);
		}

		private void DialogKancolleProgress_Load(object sender, EventArgs e)
		{
			elementHost.Child = new KancolleProgressWpf();
		}
	}
}