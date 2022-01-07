using ElectronicObserver.Window;

namespace ElectronicObserver.ViewModels.Translations;

public class FormShipGroupTranslationViewModel : TranslationBaseViewModel
{
	public string Title => Properties.Window.FormShipGroup.Title.Replace("_", "__").Replace("&", "_");

	public string ShipView_ShipType => Properties.Window.FormShipGroup.ShipView_ShipType.Replace("_", "__").Replace("&", "_");
	public string ShipView_Name => Properties.Window.FormShipGroup.ShipView_Name.Replace("_", "__").Replace("&", "_");
	public string ShipView_NextRemodel => Properties.Window.FormShipGroup.ShipView_NextRemodel.Replace("_", "__").Replace("&", "_");

	public string ShipView_Fuel => GeneralRes.Fuel.Replace("_", "__").Replace("&", "_");
	public string ShipView_Ammo => GeneralRes.Ammo.Replace("_", "__").Replace("&", "_");
	public string ShipView_Slot1 => Properties.Window.FormShipGroup.ShipView_Slot1.Replace("_", "__").Replace("&", "_");
	public string ShipView_Slot2 => Properties.Window.FormShipGroup.ShipView_Slot2.Replace("_", "__").Replace("&", "_");
	public string ShipView_Slot3 => Properties.Window.FormShipGroup.ShipView_Slot3.Replace("_", "__").Replace("&", "_");
	public string ShipView_Slot4 => Properties.Window.FormShipGroup.ShipView_Slot4.Replace("_", "__").Replace("&", "_");
	public string ShipView_Slot5 => Properties.Window.FormShipGroup.ShipView_Slot5.Replace("_", "__").Replace("&", "_");
	public string ShipView_ExpansionSlot => GeneralRes.Expansion.Replace("_", "__").Replace("&", "_");

	public string ShipView_Aircraft1 => GeneralRes.Planes + " 1".Replace("_", "__").Replace("&", "_");
	public string ShipView_Aircraft2 => GeneralRes.Planes + " 2".Replace("_", "__").Replace("&", "_");
	public string ShipView_Aircraft3 => GeneralRes.Planes + " 3".Replace("_", "__").Replace("&", "_");
	public string ShipView_Aircraft4 => GeneralRes.Planes + " 4".Replace("_", "__").Replace("&", "_");
	public string ShipView_Aircraft5 => GeneralRes.Planes + " 5".Replace("_", "__").Replace("&", "_");
	public string ShipView_AircraftTotal => GeneralRes.Planes + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_Fleet => Properties.Window.FormShipGroup.ShipView_Fleet.Replace("_", "__").Replace("&", "_");
	public string ShipView_RepairTime => Properties.Window.FormShipGroup.ShipView_RepairTime.Replace("_", "__").Replace("&", "_");
	public string ShipView_RepairSteel => Properties.Window.FormShipGroup.ShipView_RepairSteel.Replace("_", "__").Replace("&", "_");
	public string ShipView_RepairFuel => Properties.Window.FormShipGroup.ShipView_RepairFuel.Replace("_", "__").Replace("&", "_");

	public string ShipView_Firepower => GeneralRes.Firepower.Replace("_", "__").Replace("&", "_");
	public string ShipView_FirepowerRemain => GeneralRes.Firepower + GeneralRes.ModRemaining.Replace("_", "__").Replace("&", "_");
	public string ShipView_FirepowerTotal => GeneralRes.Firepower + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_Torpedo => GeneralRes.Torpedo.Replace("_", "__").Replace("&", "_");
	public string ShipView_TorpedoRemain => GeneralRes.Torpedo + GeneralRes.ModRemaining.Replace("_", "__").Replace("&", "_");
	public string ShipView_TorpedoTotal => GeneralRes.Torpedo + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_AA => GeneralRes.AntiAir.Replace("_", "__").Replace("&", "_");
	public string ShipView_AARemain => GeneralRes.AntiAir + GeneralRes.ModRemaining.Replace("_", "__").Replace("&", "_");
	public string ShipView_AATotal => GeneralRes.AntiAir + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_Armor => GeneralRes.Armor.Replace("_", "__").Replace("&", "_");
	public string ShipView_ArmorRemain => GeneralRes.Armor + GeneralRes.ModRemaining.Replace("_", "__").Replace("&", "_");
	public string ShipView_ArmorTotal => GeneralRes.Armor + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_ASW => GeneralRes.ASW.Replace("_", "__").Replace("&", "_");
	public string ShipView_ASWTotal => GeneralRes.ASW + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_Evasion => GeneralRes.Evasion.Replace("_", "__").Replace("&", "_");
	public string ShipView_EvasionTotal => GeneralRes.Evasion + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_LOS => GeneralRes.LoS.Replace("_", "__").Replace("&", "_");
	public string ShipView_LOSTotal => GeneralRes.LoS + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_Luck => GeneralRes.Luck.Replace("_", "__").Replace("&", "_");
	public string ShipView_LuckRemain => GeneralRes.Luck + GeneralRes.ModRemaining.Replace("_", "__").Replace("&", "_");
	public string ShipView_LuckTotal => GeneralRes.Luck + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_BomberTotal => GeneralRes.Bombers + GeneralRes.Total.Replace("_", "__").Replace("&", "_");
	public string ShipView_Speed => GeneralRes.Speed.Replace("_", "__").Replace("&", "_");
	public string ShipView_Range => GeneralRes.Range.Replace("_", "__").Replace("&", "_");

	public string ShipView_AirBattlePower => GeneralRes.Air + GeneralRes.Power.Replace("_", "__").Replace("&", "_");
	public string ShipView_ShellingPower => GeneralRes.Shelling + GeneralRes.Power.Replace("_", "__").Replace("&", "_");
	public string ShipView_AircraftPower => GeneralRes.Bombing + GeneralRes.Power.Replace("_", "__").Replace("&", "_");
	public string ShipView_AntiSubmarinePower => GeneralRes.ASW + GeneralRes.Power.Replace("_", "__").Replace("&", "_");
	public string ShipView_TorpedoPower => GeneralRes.Torpedo + GeneralRes.Power.Replace("_", "__").Replace("&", "_");
	public string ShipView_NightBattlePower => Properties.Window.FormShipGroup.ShipView_NightBattlePower.Replace("_", "__").Replace("&", "_");

	public string ShipView_Locked => GeneralRes.Lock.Replace("_", "__").Replace("&", "_");
	public string ShipView_SallyArea => Properties.Window.FormShipGroup.ShipView_SallyArea.Replace("_", "__").Replace("&", "_");

	public string MenuMember_AddToGroup => Properties.Window.FormShipGroup.MenuMember_AddToGroup.Replace("_", "__").Replace("&", "_");
	public string MenuMember_CreateGroup => Properties.Window.FormShipGroup.MenuMember_CreateGroup.Replace("_", "__").Replace("&", "_");
	public string MenuMember_Exclude => Properties.Window.FormShipGroup.MenuMember_Exclude.Replace("_", "__").Replace("&", "_");
	public string MenuMember_Filter => Properties.Window.FormShipGroup.MenuMember_Filter.Replace("_", "__").Replace("&", "_");
	public string MenuMember_ColumnFilter => Properties.Window.FormShipGroup.MenuMember_ColumnFilter.Replace("_", "__").Replace("&", "_");
	public string MenuMember_SortOrder => Properties.Window.FormShipGroup.MenuMember_SortOrder.Replace("_", "__").Replace("&", "_");
	public string MenuMember_CSVOutput => Properties.Window.FormShipGroup.MenuMember_CSVOutput.Replace("_", "__").Replace("&", "_");

	public string MenuGroup_Add => Properties.Window.FormShipGroup.MenuGroup_Add.Replace("_", "__").Replace("&", "_");
	public string MenuGroup_Copy => Properties.Window.FormShipGroup.MenuGroup_Copy.Replace("_", "__").Replace("&", "_");
	public string MenuGroup_Rename => Properties.Window.FormShipGroup.MenuGroup_Rename.Replace("_", "__").Replace("&", "_");
	public string MenuGroup_Delete => Properties.Window.FormShipGroup.MenuGroup_Delete.Replace("_", "__").Replace("&", "_");
	public string MenuGroup_AutoUpdate => Properties.Window.FormShipGroup.MenuGroup_AutoUpdate.Replace("_", "__").Replace("&", "_");
	public string MenuGroup_ShowStatusBar => Properties.Window.FormShipGroup.MenuGroup_ShowStatusBar.Replace("_", "__").Replace("&", "_");

	public string DialogGroupAddTitle => Properties.Window.FormShipGroup.DialogGroupAddTitle.Replace("_", "__").Replace("&", "_");
	public string DialogGroupAddDescription => Properties.Window.FormShipGroup.DialogGroupAddDescription.Replace("_", "__").Replace("&", "_");
	public string DialogGroupCopyTitle => Properties.Window.FormShipGroup.DialogGroupCopyTitle.Replace("_", "__").Replace("&", "_");
	public string DialogGroupCopyDescription => Properties.Window.FormShipGroup.DialogGroupCopyDescription.Replace("_", "__").Replace("&", "_");
	public string DialogGroupDeleteTitle => Properties.Window.FormShipGroup.DialogGroupDeleteTitle.Replace("_", "__").Replace("&", "_");
	public string DialogGroupDeleteDescription => Properties.Window.FormShipGroup.DialogGroupDeleteDescription.Replace("_", "__").Replace("&", "_");
	public string DialogGroupRenameTitle => Properties.Window.FormShipGroup.DialogGroupRenameTitle.Replace("_", "__").Replace("&", "_");
	public string DialogGroupRenameDescription => Properties.Window.FormShipGroup.DialogGroupRenameDescription.Replace("_", "__").Replace("&", "_");
}
