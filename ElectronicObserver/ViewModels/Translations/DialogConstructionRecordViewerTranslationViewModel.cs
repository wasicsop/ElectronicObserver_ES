using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.ViewModels.Translations;

public class DialogConstructionRecordViewerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => Properties.Window.Dialog.DialogConstructionRecordViewer.Title;

	public string Hull => Properties.Window.Dialog.DialogConstructionRecordViewer.Hull;
	public string ShipName => EncycloRes.ShipName;
	public string Flagship => EncycloRes.Flagship;

	public string From => Properties.Window.Dialog.DialogConstructionRecordViewer.From;
	public string Until => Properties.Window.Dialog.DialogConstructionRecordViewer.Until;

	public string Recipe => Properties.Window.Dialog.DialogConstructionRecordViewer.Recipe;
	public string DevMats => Properties.Window.Dialog.DialogConstructionRecordViewer.DevMats;
	public string EmptyDocks => Properties.Window.Dialog.DialogConstructionRecordViewer.EmptyDocks;
	public string IsLargeConstruction => Properties.Window.Dialog.DialogConstructionRecordViewer.IsLargeConstruction;
	public string MergeRows => Properties.Window.Dialog.DialogConstructionRecordViewer.MergeRows;
	public string ButtonRun => Properties.Window.Dialog.DialogConstructionRecordViewer.ButtonRun;

	public string RecordView_Name => Properties.Window.Dialog.DialogConstructionRecordViewer.RecordView_Name;
	public string RecordView_Date => Properties.Window.Dialog.DialogConstructionRecordViewer.RecordView_Date;
	public string RecordView_Recipe => Properties.Window.Dialog.DialogConstructionRecordViewer.RecordView_Recipe;
	public string RecordView_SecretaryShip => Properties.Window.Dialog.DialogConstructionRecordViewer.RecordView_SecretaryShip;

	public string Tries => Properties.Window.Dialog.DialogConstructionRecordViewer.Tries;
	public string DevMat1 => Properties.Window.Dialog.DialogConstructionRecordViewer.DevMat + "×1";
	public string DevMat20 => Properties.Window.Dialog.DialogConstructionRecordViewer.DevMat + "×20";
	public string DevMat100 => Properties.Window.Dialog.DialogConstructionRecordViewer.DevMat + "×100";

	public string Today => Properties.Window.Dialog.DialogDropRecordViewer.Today;
}
