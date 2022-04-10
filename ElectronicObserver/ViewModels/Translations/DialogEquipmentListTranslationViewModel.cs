using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.ViewModels.Translations;

public class DialogEquipmentListTranslationViewModel : TranslationBaseViewModel
{
	public string Title => EncycloRes.EquipmentList;

	public string TopMenu_File => Properties.Window.Dialog.DialogEquipmentList.TopMenu_File.Replace("_", "__").Replace("&", "_");
	public string TopMenu_File_CSVOutput => Properties.Window.Dialog.DialogEquipmentList.TopMenu_File_CSVOutput.Replace("_", "__").Replace("&", "_");
	public string TopMenu_File_Update => Properties.Window.Dialog.DialogEquipmentList.TopMenu_File_Update.Replace("_", "__").Replace("&", "_");
	public string TopMenu_File_CopyToFleetAnalysis => Properties.Window.Dialog.DialogEquipmentList.TopMenu_File_CopyToFleetAnalysis.Replace("_", "__").Replace("&", "_");

	public string View => Properties.Window.Dialog.DialogEquipmentList.View;
	public string ShowLockedEquipmentOnly => Properties.Window.Dialog.DialogEquipmentList.ShowLockedEquipmentOnly;

	public string EquipmentView_Name => EncycloRes.EquipName;
	public string EquipmentView_CountAll => EncycloRes.CountAll;
	public string EquipmentView_CountRemain => EncycloRes.CountExtra;

	public string DetailView_Level => EncycloRes.StarLevel;
	public string DetailView_AircraftLevel => EncycloRes.SkillLevel;
	public string DetailView_CountAll => EncycloRes.CountAll;
	public string DetailView_CountRemain => EncycloRes.CountExtra;

	public string SaveCSVDialog => Properties.Window.Dialog.DialogEquipmentList.SaveCSVDialog;
	public string Error => Properties.Window.Dialog.DialogEquipmentList.Error;
}
