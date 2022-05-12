using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Database;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public partial class EventLockPlannerViewModel : WindowViewModelBase
{
	public EventLockPlannerTranslationViewModel EventLockPlanner { get; }

	private List<ShipLockViewModel> AllShipLocks { get; }

	public LockGroupViewModel NoLockGroup { get; } = new(0) { Name = "0", Color = Colors.Transparent };
	public ObservableCollection<LockGroupViewModel> LockGroups { get; } = new();
	public ObservableCollection<EventPhaseViewModel> EventPhases { get; } = new();

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
			NoLockGroup.Add(ship);
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

			LockGroups[shipLock.ActualLock - 1].Move(shipLock, NoLockGroup);
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
			NoLockGroup.Move(ship, group);
		}

		LockGroups.Remove(group);
	}

	[ICommand]
	private void AddPhase()
	{
		EventPhases.Add(new(LockGroups));
	}

	[ICommand]
	private void RemovePhase()
	{
		if (EventPhases.Count is 0) return;

		EventPhases.Remove(EventPhases[^1]);
	}

	private void Save()
	{
		using ElectronicObserverContext db = new();
		EventLockPlannerModel? model = db.EventLockPlans.FirstOrDefault();

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

		model.Phases = EventPhases.Select(p => new EventPhaseModel
		{
			Name = p.Name,
			PhaseLockGroups = p.PhaseLockGroups.Select(g => g.Id).ToList(),
			PhaseShips = p.Ships.Select(s => s.Ship.MasterID).ToList(),
		}).ToList();

		db.SaveChanges();
	}

	private void Load()
	{
		using ElectronicObserverContext db = new();
		EventLockPlannerModel model = db.EventLockPlans.FirstOrDefault() ?? new();

		LoadModel(model);
	}

	private void LoadModel(EventLockPlannerModel model)
	{
		NoLockGroup.Clear();
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

			lockGroup.Move(shipLock, NoLockGroup);
		}

		foreach (EventPhaseModel eventPhaseModel in model.Phases)
		{
			EventPhaseViewModel phase = new(LockGroups)
			{
				Name = eventPhaseModel.Name,
			};

			EventPhases.Add(phase);

			foreach (int lockGroupId in eventPhaseModel.PhaseLockGroups)
			{
				LockGroupViewModel? group = LockGroups.Skip(lockGroupId - 1).FirstOrDefault();

				if(group is null) continue;

				phase.PhaseLockGroups.Add(group);
			}

			foreach (int phaseShipId in eventPhaseModel.PhaseShips)
			{
				ShipLockViewModel? shipLock = NoLockGroup.Ships
					.Concat(LockGroups.SelectMany(g => g.Ships))
					.FirstOrDefault(l => l.Ship.ID == phaseShipId);

				if (shipLock is null) continue;
				
				phase.Add(shipLock);
			}
		}
	}
}
