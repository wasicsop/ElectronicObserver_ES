using ElectronicObserver.Window;

namespace ElectronicObserver.ViewModels.Translations
{
	public class FormShipGroupTranslationViewModel : TranslationBaseViewModel
	{
		public string Title => Properties.Window.FormShipGroup.Title;

		public string ShipView_ShipType => Properties.Window.FormShipGroup.ShipView_ShipType;
		public string ShipView_Name => Properties.Window.FormShipGroup.ShipView_Name;
		public string ShipView_NextRemodel => Properties.Window.FormShipGroup.ShipView_NextRemodel;

		public string ShipView_Fuel => GeneralRes.Fuel;
		public string ShipView_Ammo => GeneralRes.Ammo;
		public string ShipView_Slot1 => Properties.Window.FormShipGroup.ShipView_Slot1;
		public string ShipView_Slot2 => Properties.Window.FormShipGroup.ShipView_Slot2;
		public string ShipView_Slot3 => Properties.Window.FormShipGroup.ShipView_Slot3;
		public string ShipView_Slot4 => Properties.Window.FormShipGroup.ShipView_Slot4;
		public string ShipView_Slot5 => Properties.Window.FormShipGroup.ShipView_Slot5;
		public string ShipView_ExpansionSlot => GeneralRes.Expansion;

		public string ShipView_Aircraft1 => GeneralRes.Planes + " 1";
		public string ShipView_Aircraft2 => GeneralRes.Planes + " 2";
		public string ShipView_Aircraft3 => GeneralRes.Planes + " 3";
		public string ShipView_Aircraft4 => GeneralRes.Planes + " 4";
		public string ShipView_Aircraft5 => GeneralRes.Planes + " 5";
		public string ShipView_AircraftTotal => GeneralRes.Planes + GeneralRes.Total;

		public string ShipView_Fleet => Properties.Window.FormShipGroup.ShipView_Fleet;
		public string ShipView_RepairTime => Properties.Window.FormShipGroup.ShipView_RepairTime;
		public string ShipView_RepairSteel => Properties.Window.FormShipGroup.ShipView_RepairSteel;
		public string ShipView_RepairFuel => Properties.Window.FormShipGroup.ShipView_RepairFuel;

		public string ShipView_Firepower => GeneralRes.Firepower;
		public string ShipView_FirepowerRemain => GeneralRes.Firepower + GeneralRes.ModRemaining;
		public string ShipView_FirepowerTotal => GeneralRes.Firepower + GeneralRes.Total;

		public string ShipView_Torpedo => GeneralRes.Torpedo;
		public string ShipView_TorpedoRemain => GeneralRes.Torpedo + GeneralRes.ModRemaining;
		public string ShipView_TorpedoTotal => GeneralRes.Torpedo + GeneralRes.Total;

		public string ShipView_AA => GeneralRes.AntiAir;
		public string ShipView_AARemain => GeneralRes.AntiAir + GeneralRes.ModRemaining;
		public string ShipView_AATotal => GeneralRes.AntiAir + GeneralRes.Total;

		public string ShipView_Armor => GeneralRes.Armor;
		public string ShipView_ArmorRemain => GeneralRes.Armor + GeneralRes.ModRemaining;
		public string ShipView_ArmorTotal => GeneralRes.Armor + GeneralRes.Total;

		public string ShipView_ASW => GeneralRes.ASW;
		public string ShipView_ASWTotal => GeneralRes.ASW + GeneralRes.Total;

		public string ShipView_Evasion => GeneralRes.Evasion;
		public string ShipView_EvasionTotal => GeneralRes.Evasion + GeneralRes.Total;

		public string ShipView_LOS => GeneralRes.LoS;
		public string ShipView_LOSTotal => GeneralRes.LoS + GeneralRes.Total;

		public string ShipView_Luck => GeneralRes.Luck;
		public string ShipView_LuckRemain => GeneralRes.Luck + GeneralRes.ModRemaining;
		public string ShipView_LuckTotal => GeneralRes.Luck + GeneralRes.Total;

		public string ShipView_BomberTotal => GeneralRes.Bombers + GeneralRes.Total;
		public string ShipView_Speed => GeneralRes.Speed;
		public string ShipView_Range => GeneralRes.Range;

		public string ShipView_AirBattlePower => GeneralRes.Air + GeneralRes.Power;
		public string ShipView_ShellingPower => GeneralRes.Shelling + GeneralRes.Power;
		public string ShipView_AircraftPower => GeneralRes.Bombing + GeneralRes.Power;
		public string ShipView_AntiSubmarinePower => GeneralRes.ASW + GeneralRes.Power;
		public string ShipView_TorpedoPower => GeneralRes.Torpedo + GeneralRes.Power;
		public string ShipView_NightBattlePower => Properties.Window.FormShipGroup.ShipView_NightBattlePower;

		public string ShipView_Locked => GeneralRes.Lock;
		public string ShipView_SallyArea => Properties.Window.FormShipGroup.ShipView_SallyArea;

		public string MenuMember_AddToGroup => Properties.Window.FormShipGroup.MenuMember_AddToGroup;
		public string MenuMember_CreateGroup => Properties.Window.FormShipGroup.MenuMember_CreateGroup;
		public string MenuMember_Exclude => Properties.Window.FormShipGroup.MenuMember_Exclude;
		public string MenuMember_Filter => Properties.Window.FormShipGroup.MenuMember_Filter;
		public string MenuMember_ColumnFilter => Properties.Window.FormShipGroup.MenuMember_ColumnFilter;
		public string MenuMember_SortOrder => Properties.Window.FormShipGroup.MenuMember_SortOrder;
		public string MenuMember_CSVOutput => Properties.Window.FormShipGroup.MenuMember_CSVOutput;

		public string MenuGroup_Add => Properties.Window.FormShipGroup.MenuGroup_Add;
		public string MenuGroup_Copy => Properties.Window.FormShipGroup.MenuGroup_Copy;
		public string MenuGroup_Rename => Properties.Window.FormShipGroup.MenuGroup_Rename;
		public string MenuGroup_Delete => Properties.Window.FormShipGroup.MenuGroup_Delete;
		public string MenuGroup_AutoUpdate => Properties.Window.FormShipGroup.MenuGroup_AutoUpdate;
		public string MenuGroup_ShowStatusBar => Properties.Window.FormShipGroup.MenuGroup_ShowStatusBar;
	}
}