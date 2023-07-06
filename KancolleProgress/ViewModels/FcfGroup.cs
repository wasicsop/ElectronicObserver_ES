using System.ComponentModel.DataAnnotations;
namespace KancolleProgress.ViewModels;

public enum FcfGroup
{
	None,

	[Display(ResourceType = typeof(KancolleProgressResources), Name = "Fcf")]
	Fcf,
}
