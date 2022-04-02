using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public enum DropRecordOption
{
	[Display(ResourceType = typeof(Properties.Window.Dialog.DialogDropRecordViewer), Name = "NameAny")]
	All,
	[Display(ResourceType = typeof(Properties.Window.Dialog.DialogDropRecordViewer), Name = "NameExist")]
	Drop,
	[Display(ResourceType = typeof(Properties.Window.Dialog.DialogDropRecordViewer), Name = "NameNotExist")]
	NoDrop,
	[Display(ResourceType = typeof(Properties.Window.Dialog.DialogDropRecordViewer), Name = "NameFullPort")]
	FullPort,
}
