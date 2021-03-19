using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf.Fleet
{
    /// <summary>
    /// Interaction logic for FleetView.xaml
    /// </summary>
    public partial class FleetView : UserControl
    {
		public FleetViewModel ViewModel { get; }

		public FleetView(int fleetId, Action<ResourceManager.IconContent> setIcon)
        {
	        ViewModel = new(fleetId, setIcon);
	        DataContext = ViewModel;

            InitializeComponent();
        }
    }
}