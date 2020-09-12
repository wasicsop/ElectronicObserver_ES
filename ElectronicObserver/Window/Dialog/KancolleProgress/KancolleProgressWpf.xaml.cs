using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		private List<IEquipmentData> UserEquipment { get; }

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
					SortID = s.SortID,
					AllSlotInstance = new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>())
				})
				.Cast<IShipData>()
				.ToList();
			UserEquipment = KCDatabase.Instance.Equipments.Values.Cast<IEquipmentData>().ToList();

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

		private enum DaihatsuGroup
		{
			None,
			Daihatsu,
			Tank,
			DaihatsuAndTank
		}

		private enum FcfGroup
		{
			None,
			Fcf
		}

		private enum AswGroup
		{
			None,
			NoSonar,
			SingleSonar
		}

		private DaihatsuGroup GetDaihatsuGroup(IShipData ship) => ship switch
		{
			_ when ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft) &&
			       ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank)
				=> DaihatsuGroup.DaihatsuAndTank,

			_ when ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft)
				=> DaihatsuGroup.Daihatsu,

			_ when ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank)
				=> DaihatsuGroup.Tank,

			_ => DaihatsuGroup.None
		};

		private FcfGroup GetFcfGroup(IShipData ship) => ship switch
		{
			_ when ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.CommandFacility)
				=> FcfGroup.Fcf,

			_ => FcfGroup.None
		};

		private AswGroup GetAswGroup(IShipData ship) => ship switch
		{
			_ when ship.CanNoSonarOpeningAsw
				=> AswGroup.NoSonar,

			_ when ship.ASWBase >= 100 - UserEquipment
					.Where(e => e.MasterEquipment.IsSonar)
					.Max(e => e.MasterEquipment.ASW)
				=> AswGroup.SingleSonar,

			_ => AswGroup.None
		};

		private void MakeEventCheckList(object? sender, RoutedEventArgs? e)
		{
			ShipTypeGroupContainer.Children.Clear();

			var destroyers = UserShips
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
				.ToList();

			IEnumerable<IGrouping<T, IShipData>> GroupShips<T>(IEnumerable<IShipData> ships,
				Func<IShipData, T> groupSelector, Func<IGrouping<T, IShipData>, bool> groupFilter)
				where T : Enum
			{
				return ships
					.GroupBy(groupSelector)
					.Where(groupFilter)
					.OrderBy(s => s.Key);
			}

			var daihatsuGroup = GroupShips(destroyers,
				s => GetDaihatsuGroup(s),
				g => g.Key != DaihatsuGroup.None);

			var fcfGroup = GroupShips(destroyers,
				s => GetFcfGroup(s),
				g => g.Key != FcfGroup.None);

			var openingAswGroup = GroupShips(destroyers,
				s => GetAswGroup(s),
				g => g.Key != AswGroup.None);


			void DisplayGroups<T>(IEnumerable<IGrouping<T, IShipData>> groups) where T : Enum
			{
				foreach (IGrouping<T, IShipData> group in groups)
				{
					ShipTypeGroupContainer.Children.Add(new ShipTypeGroupControl
					{
						GroupLabel = group.Key.Display(),
						Group = group
					});
				}
			}

			DisplayGroups(daihatsuGroup);
			DisplayGroups(fcfGroup);
			DisplayGroups(openingAswGroup);
		}
	}
}