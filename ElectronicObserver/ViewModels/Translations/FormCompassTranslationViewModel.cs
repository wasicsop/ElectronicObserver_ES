namespace ElectronicObserver.ViewModels.Translations;

public class FormCompassTranslationViewModel : TranslationBaseViewModel
{
	public string MapClearCount => Properties.Window.FormCompass.MapClearCount.Replace("_", "__").Replace("&", "_");
	public string SpecialAttackActivated => Properties.Window.FormCompass.SpecialAttackActivated.Replace("_", "__").Replace("&", "_");

	public string EnemySighted => Properties.Window.FormCompass.EnemySighted.Replace("_", "__").Replace("&", "_");
	public string TargetSighted => Properties.Window.FormCompass.TargetSighted.Replace("_", "__").Replace("&", "_");
	public string CoursePatrol => Properties.Window.FormCompass.CoursePatrol.Replace("_", "__").Replace("&", "_");
	public string EnemyPlaneSighted => Properties.Window.FormCompass.EnemyPlaneSighted.Replace("_", "__").Replace("&", "_");

	public string NoEnemySighted => Properties.Window.FormCompass.NoEnemySighted.Replace("_", "__").Replace("&", "_");
	public string BranchChoice => Properties.Window.FormCompass.BranchChoice.Replace("_", "__").Replace("&", "_");
	public string CalmSea => Properties.Window.FormCompass.CalmSea.Replace("_", "__").Replace("&", "_");
	public string CalmStrait => Properties.Window.FormCompass.CalmStrait.Replace("_", "__").Replace("&", "_");
	public string NeedToBeCareful => Properties.Window.FormCompass.NeedToBeCareful.Replace("_", "__").Replace("&", "_");
	public string CalmSea2 => Properties.Window.FormCompass.CalmSea2.Replace("_", "__").Replace("&", "_");

	public string BranchChoiceSeparator => Properties.Window.FormCompass.BranchChoiceSeparator.Replace("_", "__").Replace("&", "_");
	public string RepairPossibility => Properties.Window.FormCompass.RepairPossibility.Replace("_", "__").Replace("&", "_");
	public string AirRaid => Properties.Window.FormCompass.AirRaid.Replace("_", "__").Replace("&", "_");
	public string FleetCount => Properties.Window.FormCompass.FleetCount.Replace("_", "__").Replace("&", "_");
	public string UnknownItem => Properties.Window.FormCompass.UnknownItem.Replace("_", "__").Replace("&", "_");
	public string None => Properties.Window.FormCompass.None.Replace("_", "__").Replace("&", "_");
	public string AirValues => Properties.Window.FormCompass.AirValues.Replace("_", "__").Replace("&", "_");

	public string TextMapArea => Properties.Window.FormCompass.TextMapArea.Replace("_", "__").Replace("&", "_");
	public string TextDestination => Properties.Window.FormCompass.TextDestination.Replace("_", "__").Replace("&", "_");
	public string TextEventKind => Properties.Window.FormCompass.TextEventKind.Replace("_", "__").Replace("&", "_");
	public string TextEventDetail => Properties.Window.FormCompass.TextEventDetail.Replace("_", "__").Replace("&", "_");

	public string TextEnemyFleetName => Properties.Window.FormCompass.TextEnemyFleetName.Replace("_", "__").Replace("&", "_");
	public string TextFormation => Properties.Window.FormCompass.TextFormation.Replace("_", "__").Replace("&", "_");
	public string TextAirSuperiority => Properties.Window.FormCompass.TextAirSuperiority.Replace("_", "__").Replace("&", "_");

	public string Title = Properties.Window.FormCompass.Title.Replace("_", "__").Replace("&", "_");
}
