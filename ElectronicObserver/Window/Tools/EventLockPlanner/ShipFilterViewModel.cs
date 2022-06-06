using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Window.Dialog.ShipPicker;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using WanaKanaNet;
namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public class ShipFilterViewModel : ObservableObject
{
	public ShipFilterTranslationViewModel ShipFilter { get; }

	public List<Filter> TypeFilters { get; }

	public int LevelMin { get; set; } = 0;
	public int LevelMax { get; set; } = 175;

	public int AswMin { get; set; } = 0;
	public int AswMax { get; set; } = 200;

	public int LuckMin { get; set; } = 0;
	public int LuckMax { get; set; } = 200;

	public bool CanEquipDaihatsu { get; set; }
	public bool CanEquipTank { get; set; }
	public bool HasExpansionSlot { get; set; }
	public string? NameFilter { get; set; } = "";
	private static Dictionary<ShipId, string> RomajiCache { get; } = new();
	public ShipFilterViewModel()
	{
		ShipFilter = Ioc.Default.GetService<ShipFilterTranslationViewModel>()!;

		TypeFilters = Enum.GetValues<ShipTypeGroup>()
			.Select(t => new Filter(t)
			{
				IsChecked = true,
			})
			.ToList();

		foreach (Filter filter in TypeFilters)
		{
			filter.PropertyChanged += (_, _) => OnPropertyChanged(string.Empty);
		}
	}

	public bool MeetsFilterCondition(IShipData ship)
	{
		List<ShipTypes> enabledFilters = TypeFilters
			.Where(f => f.IsChecked)
			.SelectMany(f => f.Value.ToTypes())
			.ToList();

		if (!enabledFilters.Contains(ship.MasterShip.ShipType)) return false;
		if (ship.Level < LevelMin) return false;
		if (ship.Level > LevelMax) return false;
		if (ship.ASWBase < AswMin) return false;
		if (ship.ASWBase > AswMax) return false;
		if (ship.LuckBase < LuckMin) return false;
		if (ship.LuckBase > LuckMax) return false;
		if (CanEquipDaihatsu && !ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft)) return false;
		if (CanEquipTank && !ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank)) return false;
		if (HasExpansionSlot && !ship.IsExpansionSlotAvailable) return false;
		NameFilter ??= "";
		if (!Matches(ship.MasterShip, NameFilter.ToLower(),WanaKana.ToRomaji(NameFilter)) && NameFilter != string.Empty) return false;
		// other filters

		return true;
	}
	private static string GetRomajiName(IShipDataMaster ship)
	{
		if (!RomajiCache.ContainsKey(ship.ShipId))
		{
			string name = ship.IsAbyssalShip switch
			{
				true => ship.NameWithClass.ToLower(),
				_ => ship.NameReading,
			};

			RomajiCache.Add(ship.ShipId, WanaKana.ToRomaji(name));
		}

		return RomajiCache[ship.ShipId];
	}
	private static bool Matches(IShipDataMaster ship, string filter, string romajiFilter)
	{
		bool literalSearch = ship.NameWithClass.ToLower().Contains(filter.ToLower());
		if (!ship.IsAbyssalShip)
		{
			literalSearch |= ship.NameReading.Contains(filter.ToLower());
		}

		if (literalSearch)
		{
			return true;
		}

		// when using kanji to filter, only do a literal search
		// 神 matches 北上 if you use romaji compare
		if (filter.ToCharArray().Any(WanaKana.IsKanji)) return false;
		// abyssals should get matched with literal search only
		if (ship.IsAbyssalShip) return false;
		// when using English ship names, do literal searches only
		if (!Utility.Configuration.Config.UI.JapaneseShipName) return false;

		string romajiName = GetRomajiName(ship);

		bool result = romajiName.Contains(romajiFilter.ToLower());

		return result;
	}
}
