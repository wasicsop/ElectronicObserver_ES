using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Window;
using ElectronicObserver.Properties.Window.Dialog;

namespace ElectronicObserver.ViewModels.Translations;
public class DialogResourceChartTranslationViewModel : TranslationBaseViewModel
{
	public string Title => GeneralRes.ResourceChart.Replace("_", "__").Replace("&", "_");
	
	public string Start => DialogDropRecordViewer.Start;
	public string End => DialogDropRecordViewer.End;

	public string Menu_File => Properties.Window.Dialog.DialogResourceChart.Menu_File.Replace("_", "__").Replace("&", "_");
	public string Menu_File_SaveImage => Properties.Window.Dialog.DialogResourceChart.Menu_File_SaveImage.Replace("_", "__").Replace("&", "_");
	
	public string Menu_Graph => Properties.Window.Dialog.DialogResourceChart.Menu_Graph.Replace("_", "__").Replace("&", "_");
	public string Menu_Graph_Resource => Properties.Window.Dialog.DialogResourceChart.Menu_Graph_Resource.Replace("_", "__").Replace("&", "_");
	public string Menu_Graph_ResourceDiff => Properties.Window.Dialog.DialogResourceChart.Menu_Graph_ResourceDiff.Replace("_", "__").Replace("&", "_");
	public string Menu_Graph_Material => Properties.Window.Dialog.DialogResourceChart.Menu_Graph_Material.Replace("_", "__").Replace("&", "_");
	public string Menu_Graph_MaterialDiff => Properties.Window.Dialog.DialogResourceChart.Menu_Graph_MaterialDiff.Replace("_", "__").Replace("&", "_");
	public string Menu_Graph_Experience => Properties.Window.Dialog.DialogResourceChart.Menu_Graph_Experience.Replace("_", "__").Replace("&", "_");
	public string Menu_Graph_ExperienceDiff => Properties.Window.Dialog.DialogResourceChart.Menu_Graph_ExperienceDiff.Replace("_", "__").Replace("&", "_");
	
	public string Menu_Option_DivideByDay => Properties.Window.Dialog.DialogResourceChart.Menu_Option_DivideByDay.Replace("_", "__").Replace("&", "_");
	public string Menu_Option_ShowAllData => Properties.Window.Dialog.DialogResourceChart.Menu_Option_ShowAllData.Replace("_", "__").Replace("&", "_");
	
	public string Menu_Span => Properties.Window.Dialog.DialogResourceChart.Menu_Span.Replace("_", "__").Replace("&", "_");
	public string Menu_Span_Day => GeneralRes.Day.Replace("_", "__").Replace("&", "_");
	public string Menu_Span_Month => GeneralRes.Month.Replace("_", "__").Replace("&", "_");
	public string Menu_Span_Year => GeneralRes.Year.Replace("_", "__").Replace("&", "_");
	public string Menu_Span_Week => GeneralRes.Week.Replace("_", "__").Replace("&", "_");
	public string Menu_Span_Season => GeneralRes.ThreeMonths.Replace("_", "__").Replace("&", "_");
	public string Menu_Span_All => GeneralRes.AllData.Replace("_", "__").Replace("&", "_");

	public string Menu_Span_WeekFirst => DialogResourceChart.WeekFirst;
	public string Menu_Span_MonthFirst => DialogResourceChart.MonthFirst;
	public string Menu_Span_SeasonFirst => DialogResourceChart.SeasonFirst;
	public string Menu_Span_YearFirst => DialogResourceChart.YearFirst;

	public string OptionsMenu => GeneralRes.Option.Replace("_", "__").Replace("&", "_");
	
	public string Ammo => GeneralRes.Ammo.Replace("_", "__").Replace("&", "_");
	public string Fuel => GeneralRes.Fuel.Replace("_", "__").Replace("&", "_");
	public string Steel => GeneralRes.Steel.Replace("_", "__").Replace("&", "_");
	public string Baux => GeneralRes.Baux.Replace("_", "__").Replace("&", "_");
	
	public string InstantRepair => GeneralRes.Bucket.Replace("_", "__").Replace("&", "_");
	public string ModdingMaterial => GeneralRes.ImpMat.Replace("_", "__").Replace("&", "_");
	public string DevelopmentMaterial => GeneralRes.DevMat.Replace("_", "__").Replace("&", "_");
	public string InstantConstruction => GeneralRes.Flamethrower.Replace("_", "__").Replace("&", "_");
	
	public string Experience => GeneralRes.Experience.Replace("_", "__").Replace("&", "_");
	
	public string Today => DialogDropRecordViewer.Today;
}
