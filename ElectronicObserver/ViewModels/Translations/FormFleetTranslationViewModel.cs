using ElectronicObserver.Data;

namespace ElectronicObserver.ViewModels.Translations
{
	public class FormFleetTranslationViewModel : TranslationBaseViewModel
	{
		// .Replace("_", "__").Replace("&", "_");

		public string CopyFleetText => Properties.Window.FormFleet.CopyFleetText;
		public string ShipNameToolTip => Properties.Window.FormFleet.ShipNameToolTip;
		public string ExpCalcHint => Properties.Window.FormFleet.ExpCalcHint;

		public string CvciFba => Properties.Window.FormFleet.CvciFba;
		public string CvciBba => Properties.Window.FormFleet.CvciBba;
		public string CvciBa => Properties.Window.FormFleet.CvciBa;

		public string CvnciFfa => Properties.Window.FormFleet.CvnciFfa;
		public string CvnciFa => Properties.Window.FormFleet.CvnciFa;
		public string CvnciPhoto => Properties.Window.FormFleet.CvnciPhoto;
		public string CvnciFoo => Properties.Window.FormFleet.CvnciFoo;

		public string LateModelTorpedoSubmarineEquipment => Properties.Window.FormFleet.LateModelTorpedoSubmarineEquipment;
		public string LateModelTorpedo2 => Properties.Window.FormFleet.LateModelTorpedo2;

		public string Power => Properties.Window.FormFleet.Power;
		public string Accuracy => Properties.Window.FormFleet.Accuracy;

		public string Asw => Properties.Window.FormFleet.Asw;
		public string OpeningAsw => Properties.Window.FormFleet.OpeningAsw;
		public string Aarb => Properties.Window.FormFleet.Aarb;
		public string AirstrikePower => Properties.Window.FormFleet.AirstrikePower;

		public string NoShips => FleetRes.NoShips;
		public string CriticalDamageAdvance => FleetRes.CriticalDamageAdvance;
		public string OnSortie => FleetRes.OnSortie;
		public string ExpeditionToolTip => FleetRes.ExpeditionToolTip;
		public string CriticallyDamagedShip => FleetRes.CriticallyDamagedShip;
		public string Repairing => FleetRes.Repairing;
		public string RepairTimeHeader => FleetRes.RepairTimeHeader;
		public string RepairTimeDetail => FleetRes.RepairTimeDetail;
		public string OnDock => FleetRes.OnDock;
		public string DockCompletionTime => FleetRes.DockCompletionTime;
		public string SupplyNeeded => FleetRes.SupplyNeeded;
		public string ResupplyTooltip => FleetRes.ResupplyTooltip;
		public string RecoveryTimeToolTip => FleetRes.RecoveryTimeToolTip;
		public string FightingSpiritHigh => FleetRes.FightingSpiritHigh;
		public string SparkledTooltip => FleetRes.SparkledTooltip;
		public string ReadyToSortie => FleetRes.ReadyToSortie;
		public string Fatigued => FleetRes.Fatigued;

		public string SupportTypeNone => Properties.Window.FormFleet.SupportTypeNone;
		public string SupportTypeAerial => Properties.Window.FormFleet.SupportTypeAerial;
		public string SupportTypeShelling => Properties.Window.FormFleet.SupportTypeShelling;
		public string SupportTypeTorpedo => Properties.Window.FormFleet.SupportTypeTorpedo;

		public string FleetNameToolTip => Properties.Window.FormFleet.FleetNameToolTip;
		public string WithoutProficiency => Properties.Window.FormFleet.WithoutProficiency;
		public string WithProficiency => Properties.Window.FormFleet.WithProficiency;
		public string FleetLosToolTip => Properties.Window.FormFleet.FleetLosToolTip;
		public string ContactSelection => Properties.Window.FormFleet.ContactSelection;
		public string ContactProbability => Properties.Window.FormFleet.ContactProbability;
		public string ResourceToolTip => FleetRes.ResourceToolTip;

		public string ContextMenuFleet_CopyFleet => Properties.Window.FormFleet.ContextMenuFleet_CopyFleet;
		public string ContextMenuFleet_CopyFleetDeckBuilder => Properties.Window.FormFleet.ContextMenuFleet_CopyFleetDeckBuilder;
		public string ContextMenuFleet_CopyKanmusuList => Properties.Window.FormFleet.ContextMenuFleet_CopyKanmusuList;
		public string ContextMenuFleet_CopyFleetAnalysis => Properties.Window.FormFleet.ContextMenuFleet_CopyFleetAnalysis;
		public string ContextMenuFleet_CopyFleetAnalysisLockedEquip => Properties.Window.FormFleet.ContextMenuFleet_CopyFleetAnalysisLockedEquip;
		public string ContextMenuFleet_CopyFleetAnalysisAllEquip => Properties.Window.FormFleet.ContextMenuFleet_CopyFleetAnalysisAllEquip;

		public string ContextMenuFleet_AntiAirDetails => Properties.Window.FormFleet.ContextMenuFleet_AntiAirDetails;
		public string ContextMenuFleet_Capture => Properties.Window.FormFleet.ContextMenuFleet_Capture;
		public string ContextMenuFleet_OutputFleetImage => Properties.Window.FormFleet.ContextMenuFleet_OutputFleetImage;
	}
}