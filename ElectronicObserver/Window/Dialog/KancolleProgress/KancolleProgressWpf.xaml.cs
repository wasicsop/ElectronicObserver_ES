using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.KancolleProgress
{
	/// <summary>
	/// Interaction logic for KancolleProgressWpf.xaml
	/// </summary>
	public partial class KancolleProgressWpf : UserControl
    {
	    public KancolleProgressWpf()
        {
            InitializeComponent();

			int BaseShipId(int shipId)
			{
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipId];
				while (ship.RemodelBeforeShipID != 0)
					ship = ship.RemodelBeforeShip;
				return ship.ID;
			}

			List<IShipData> userShips = KCDatabase.Instance.Ships.Values.Cast<IShipData>().ToList();

			Dictionary<int, ShipDataCustom> baseShips = KCDatabase.Instance.MasterShips.Values
				.Where(s => !s.IsAbyssalShip && s.RemodelBeforeShipID == 0)
				.Select(s => new ShipDataCustom
				{
					Name = s.Name,
					Level = 0,
					ShipID = s.ShipID,
					MasterShip = s,
					SortID = s.SortID
				})
				.ToDictionary(s => s.ShipID, s => s);

			foreach (IShipData ship in userShips)
			{
				int baseShipId = BaseShipId(ship.ShipID);

				if (baseShips[baseShipId].Level < ship.Level)
				{
					baseShips[baseShipId].Level = ship.Level;
				}
			}

			IEnumerable<IGrouping<ShipTypeGroup, IShipData>> groups = baseShips.Values
				.OrderBy(s => s.SortID)
				.GroupBy(s => s.MasterShip.ShipType.ToGroup())
				.OrderBy(s => s.Key);

			foreach (IGrouping<ShipTypeGroup, IShipData> group in groups)
			{
				ShipTypeGroupContainer.Children.Add(new ShipTypeGroupControl { Group = group });
			}

			ShipTypeGroupContainer.Children.Add(new ColorFilterContainerControl
			{
				Ships = baseShips.Values.Cast<IShipData>().ToList()
			});
        }
	}
}
