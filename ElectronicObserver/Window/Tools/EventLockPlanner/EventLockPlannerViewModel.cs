using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using GongSolutions.Wpf.DragDrop;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public class ShipLockViewModel : ObservableObject
{
	public IShipData Ship { get; }

	public int PlannedLock { get; set; }
	public int ActualLock => Ship.SallyArea;

	public string Display => ActualLock switch
	{
		< 1 => Ship.Name,
		_ => $"[{ActualLock}] {Ship.Name}"
	};

	public int NightPowerBase => Ship.FirepowerBase + Ship.TorpedoBase;
	public bool CanUseDaihatsu => Ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft);
	public bool CanUseTank => Ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank);

	public ShipLockViewModel(IShipData ship)
	{
		Ship = ship;
	}
}

public class LockGroupViewModel : ObservableObject, IDropTarget
{
	public int Id { get; }
	public Color Color { get; set; }
	public SolidColorBrush Background => new(Color);
	public string Name { get; set; } = "";
	public ObservableCollection<ShipLockViewModel> Ships { get; } = new();

	public LockGroupViewModel(int id)
	{
		Id = id;
		Color = Color.FromRgb((byte)Random.Shared.Next(256), (byte)Random.Shared.Next(256), (byte)Random.Shared.Next(256));
	}

	public bool CanAdd(ShipLockViewModel shipLock) => Id switch
	{
		0 => shipLock.ActualLock <= 0,
		{ } => shipLock.ActualLock <= 0 || shipLock.ActualLock == Id
	};

	private void Insert(int index, ShipLockViewModel shipLock)
	{
		shipLock.PlannedLock = Id;
		Ships.Insert(index, shipLock);
	}

	public void DragOver(IDropInfo dropInfo)
	{
		if (dropInfo.Data is not ShipLockViewModel shipLock) return;

		if (CanAdd(shipLock))
		{
			dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
			dropInfo.Effects = DragDropEffects.Move;
		}
		else
		{
			dropInfo.Effects = DragDropEffects.None;
		}
	}

	public void Drop(IDropInfo dropInfo)
	{
		if (dropInfo.Data is not ShipLockViewModel shipLock) return;
		if (dropInfo.DragInfo.SourceCollection is not ObservableCollection<ShipLockViewModel> source) return;

		Insert(dropInfo.InsertIndex, shipLock, source);
	}

	public void Move(ShipLockViewModel shipLock, ObservableCollection<ShipLockViewModel> source) =>
		Insert(Ships.Count, shipLock, source);

	private void Insert(int index, ShipLockViewModel shipLock, ObservableCollection<ShipLockViewModel> source)
	{
		if (!CanAdd(shipLock)) return;

		// index needs to be adjusted for moves within a group
		if (source == Ships)
		{
			int oldIndex = Ships.IndexOf(shipLock);
			if (oldIndex < index)
			{
				index--;
			}
		}

		source.Remove(shipLock);
		Insert(index, shipLock);
	}
}

public class ShipLockModel
{
	public int Id { get; set; }
	public int PlannedLock { get; set; }
}

public class EventLockModel
{
	public int Id { get; set; }
	public byte A { get; set; }
	public byte R { get; set; }
	public byte G { get; set; }
	public byte B { get; set; }
	public string Name { get; set; } = "";
}

public class EventLockPlanModel
{
	public int Id { get; set; }
	public List<EventLockModel> Locks { get; set; } = new();
	public List<ShipLockModel> ShipLocks { get; set; } = new();
}

public partial class EventLockPlannerViewModel : WindowViewModelBase
{
	public EventLockPlannerTranslationViewModel EventLockPlanner { get; }

	private List<ShipLockViewModel> AllShipLocks { get; }

	public LockGroupViewModel NoLockGroup { get; } = new(0);
	public ObservableCollection<LockGroupViewModel> LockGroups { get; } = new();
	
	public EventLockPlannerViewModel(IEnumerable<IShipData> allShips)
	{
		EventLockPlanner = Ioc.Default.GetService<EventLockPlannerTranslationViewModel>()!;

		AllShipLocks = allShips.Select(s => new ShipLockViewModel(s)).ToList();

		GenerateUnlockedShips();
		PopulateLocks();
	}

	public override void Loaded()
	{
		base.Loaded();

		Load();
	}

	public override void Closed()
	{
		base.Closed();

		Save();
	}

	private void GenerateUnlockedShips()
	{
		List<ShipLockViewModel> unlockedShips = AllShipLocks
			.Where(l => l.Ship.SallyArea <= 0)
			.ToList();

		foreach (ShipLockViewModel ship in unlockedShips)
		{
			NoLockGroup.Ships.Add(ship);
		}
	}

	private void PopulateLocks()
	{
		foreach (ShipLockViewModel shipLock in AllShipLocks.Where(l => l.Ship.SallyArea > 0))
		{
			while (shipLock.ActualLock > LockGroups.Count)
			{
				AddLock();
			}

			LockGroups[shipLock.ActualLock - 1].Move(shipLock, NoLockGroup.Ships);
		}
	}

	[ICommand]
	private void AddLock()
	{
		int id = LockGroups.Count + 1;
		LockGroups.Add(new(id) { Name = $"{id}" });
	}

	[ICommand]
	private void RemoveLock()
	{
		if (LockGroups.Count is 0) return;

		LockGroupViewModel group = LockGroups[^1];

		foreach (ShipLockViewModel ship in group.Ships.ToList())
		{
			NoLockGroup.Move(ship, group.Ships);
		}

		LockGroups.Remove(group);
	}

	private void Save()
	{
		using ElectronicObserverContext db = new();
		EventLockPlanModel? model = db.EventLockPlans.FirstOrDefault();

		if (model is null)
		{
			model = new();
			db.Add(model);
		}

		model.Locks = LockGroups.Select(g => new EventLockModel
		{
			Id = g.Id,
			A = g.Color.A,
			R = g.Color.R,
			G = g.Color.G,
			B = g.Color.B,
			Name = g.Name,
		}).ToList();

		model.ShipLocks = LockGroups.SelectMany(g => g.Ships.Select(s => new ShipLockModel
		{
			Id = s.Ship.ID,
			PlannedLock = s.PlannedLock,
		})).ToList();

		db.SaveChanges();
	}

	private void Load()
	{
		using ElectronicObserverContext db = new();
		EventLockPlanModel model = db.EventLockPlans.FirstOrDefault() ?? new();

		LoadModel(model);
	}

	private void LoadModel(EventLockPlanModel model)
	{
		NoLockGroup.Ships.Clear();
		LockGroups.Clear();

		GenerateUnlockedShips();
		PopulateLocks();

		foreach (EventLockModel eventLock in model.Locks)
		{
			LockGroups.Add(new(eventLock.Id)
			{
				Color = Color.FromArgb(eventLock.A, eventLock.R, eventLock.G, eventLock.B),
				Name = eventLock.Name,
			});
		}

		foreach (ShipLockModel shipLockModel in model.ShipLocks)
		{
			LockGroupViewModel lockGroup = LockGroups[shipLockModel.PlannedLock - 1];
			ShipLockViewModel? shipLock = NoLockGroup.Ships.FirstOrDefault(l => l.Ship.ID == shipLockModel.Id);

			if (shipLock is null) continue;
			if (!lockGroup.CanAdd(shipLock)) continue;

			lockGroup.Move(shipLock, NoLockGroup.Ships);
		}
	}
}
