namespace ElectronicObserver.ViewModels.Translations
{
	public class FormBaseAirCorpsTranslationViewModel : TranslationBaseViewModel
	{
		public string Area => Properties.Window.FormBaseAirCorps.Area.Replace("_", "__").Replace("&", "_");
		public string UnknownArea => Properties.Window.FormBaseAirCorps.UnknownArea.Replace("_", "__").Replace("&", "_");
		public string Unsupplied => Properties.Window.FormBaseAirCorps.Unsupplied.Replace("_", "__").Replace("&", "_");
		public string AirControlSummary => Properties.Window.FormBaseAirCorps.AirControlSummary.Replace("_", "__").Replace("&", "_");
		public string TotalDistance => Properties.Window.FormBaseAirCorps.TotalDistance.Replace("_", "__").Replace("&", "_");
		public string Empty => Properties.Window.FormBaseAirCorps.Empty.Replace("_", "__").Replace("&", "_");
		public string Range => Properties.Window.FormBaseAirCorps.Range.Replace("_", "__").Replace("&", "_");
		public string CopyOrganizationFormat => Properties.Window.FormBaseAirCorps.CopyOrganizationFormat.Replace("_", "__").Replace("&", "_");

		public string ContextMenuBaseAirCorps_CopyOrganization => Properties.Window.FormBaseAirCorps.CopyOrganization.Replace("_", "__").Replace("&", "_");
		public string ContextMenuBaseAirCorps_DisplayRelocatedEquipments => Properties.Window.FormBaseAirCorps.DisplayRelocatedEquipments.Replace("_", "__").Replace("&", "_");

		public string Title => Properties.Window.FormBaseAirCorps.Title.Replace("_", "__").Replace("&", "_");
	}
}