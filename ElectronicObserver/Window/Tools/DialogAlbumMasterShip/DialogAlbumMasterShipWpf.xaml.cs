using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterShip
{
	/// <summary>
    /// Interaction logic for DialogAlbumMasterShipWpf.xaml
    /// </summary>
    public partial class DialogAlbumMasterShipWpf : System.Windows.Window
	{
		private DialogAlbumMasterShipViewModel ViewModel { get; } = new();

		public DialogAlbumMasterShipWpf()
        {
            InitializeComponent();

            // ViewModel.PropertyChanged += SelectedChipChanged;
            DataContext = ViewModel;

		}

		public DialogAlbumMasterShipWpf(int shipId) : this()
		{
			IShipDataMaster ship = KCDatabase.Instance.MasterShips[shipId];
			ViewModel = new(ship);

			// ViewModel.PropertyChanged += SelectedChipChanged;
			DataContext = ViewModel;
		}

		public DialogAlbumMasterShipWpf(IShipDataMaster ship) : this()
		{
			ViewModel = new(ship);

			// ViewModel.PropertyChanged += SelectedChipChanged;
			DataContext = ViewModel;
		}

		// todo: doesn't work for some reason
		private void SelectedChipChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName is not nameof(ViewModel.SelectedShip)) return;
			if (ViewModel.SelectedShip is null) return;

			DataGrid.UpdateLayout();
			DataGrid.ScrollIntoView(ViewModel.SelectedShip);
		}

		private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
		{
			if (sender is not DataGridRow { DataContext: ShipDataRecord { Ship: { } ship } }) return;

			ViewModel.OpenShipEncyclopediaCommand.Execute(ship);
		}
	}
}
