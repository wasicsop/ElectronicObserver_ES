using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Tools.DevelopmentRecordViewer;

public enum DevelopmentRecordOption
{
	[Display(ResourceType = typeof(Properties.Window.Dialog.DialogDevelopmentRecordViewer), Name = "NameAny")]
	All,
	[Display(ResourceType = typeof(Properties.Window.Dialog.DialogDevelopmentRecordViewer), Name = "NameExist")]
	Success,
	[Display(ResourceType = typeof(Properties.Window.Dialog.DialogDevelopmentRecordViewer), Name = "NameNotExist")]
	Failure
}