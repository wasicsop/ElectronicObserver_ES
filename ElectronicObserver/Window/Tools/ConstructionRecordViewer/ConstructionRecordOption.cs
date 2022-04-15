using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Tools.ConstructionRecordViewer;

public enum ConstructionRecordOption
{
	[Display(ResourceType = typeof(Properties.Window.Dialog.DialogConstructionRecordViewer), Name = "NameAny")]
	All,
}