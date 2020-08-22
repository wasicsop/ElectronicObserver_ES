using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DiscordRPC.Logging;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.KancolleProgress
{
	/// <summary>
	/// Interaction logic for KancolleProgressWpf.xaml
	/// </summary>
	public partial class KancolleProgressWpf : UserControl
    {
	    private List<IShipData> UserShips { get; }
	    private List<IShipData> AllShips { get; }

	    public KancolleProgressWpf()
        {
            InitializeComponent();

			UserShips = KCDatabase.Instance.Ships.Values.Cast<IShipData>().ToList();
			AllShips = KCDatabase.Instance.MasterShips.Values
				.Where(s => !s.IsAbyssalShip)
				.Select(s => new ShipDataCustom
				{
					Name = s.NameEN,
					Level = 0,
					ShipID = s.ShipID,
					MasterShip = s,
					SortID = s.SortID
				})
				.Cast<IShipData>()
				.ToList();

			MakeKancolleProgress(null, null);
        }

	    private void MakeKancolleProgress(object? sender, RoutedEventArgs? e)
	    {
		    ShipTypeGroupContainer.Children.Clear();

			Dictionary<int, ShipDataCustom> baseShips = AllShips
			    .Where(s => !s.MasterShip.IsAbyssalShip && s.MasterShip.RemodelBeforeShipID == 0)
			    .Select(s => new ShipDataCustom
			    {
				    Name = s.MasterShip.NameEN,
				    Level = 0,
				    ShipID = s.ShipID,
				    MasterShip = s.MasterShip,
				    SortID = s.SortID
			    })
			    .ToDictionary(s => s.ShipID, s => s);

		    foreach (IShipData ship in UserShips)
		    {
			    int baseShipId = ship.MasterShip.BaseShip().ShipID;

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
			    ShipTypeGroupContainer.Children.Add(new ShipTypeGroupControl
			    {
					GroupLabel = group.Key.Display(),
					Group = group
			    });
		    }

		    ShipTypeGroupContainer.Children.Add(new ColorFilterContainerControl
		    {
			    Ships = baseShips.Values.Cast<IShipData>().ToList()
		    });
		}

		private enum EventListGroup
		{
			None,
			Daihatsu,
			Tank,
			DaihatsuAndTank
		}

		private EventListGroup GetEventListGroup(IShipData ship) => ship switch
		{
			_ when ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft) &&
			       ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank)
			=> EventListGroup.DaihatsuAndTank,

			_ when ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft)
			=> EventListGroup.Daihatsu,

			_ when ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank)
			=> EventListGroup.Tank,

			_ => EventListGroup.None
		};

	    private void MakeEventCheckList(object sender, RoutedEventArgs e)
	    {
		    ShipTypeGroupContainer.Children.Clear();

		    var groups = UserShips
			    .Concat(AllShips)
			    .Where(s => s.MasterShip.ShipType == ShipTypes.Destroyer)
			    .OrderBy(s => s.MasterShip.SortID)
			    .GroupBy(s => s.MasterShip.BaseShip().ShipId)
				.Select(g => g
				    .OrderByDescending(s => s.Level)
				    .ThenByDescending(s => s.SortID))
				.SelectMany(g => g.First().Level switch
			    {
					0 => g.Take(1),
					_ => g.TakeWhile(s => s.Level > 0)
			    })
				.GroupBy(s => GetEventListGroup(s))
				.Where(g => g.Key != EventListGroup.None)
			    .OrderBy(s => s.Key);

		    foreach (IGrouping<EventListGroup, IShipData> group in groups)
		    {
			    ShipTypeGroupContainer.Children.Add(new ShipTypeGroupControl
			    {
				    GroupLabel = group.Key.Display(),
				    Group = group
			    });
		    }
		}
    }
}
