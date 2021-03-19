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
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Wpf.Fleet;
using ElectronicObserverTypes;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window.Wpf
{
	public partial class FormFleetWpf : DockContent
	{
		private ElementHost WpfHost { get; } = new();
		private FleetView FleetView { get; }

		public FormFleetWpf(int fleetId)
		{
			FleetView = new(fleetId, SetIcon);

			InitializeComponent();

			WpfHost.Dock = DockStyle.Fill;
			WpfHost.Child = FleetView;

			Controls.Add(WpfHost);

			Text = $"#{fleetId}";
			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet]);
		}

		private void SetIcon(ResourceManager.IconContent icon)
		{
			if (Icon != null) ResourceManager.DestroyIcon(Icon);
			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)icon]);
			// Refresh();
		}
	}
}
