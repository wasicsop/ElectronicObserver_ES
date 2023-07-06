using System.ComponentModel.DataAnnotations;
namespace KancolleProgress.ViewModels;

public enum DaihatsuGroup
{
	None,

	[Display(ResourceType = typeof(KancolleProgressResources), Name = "Daihatsu")]
	Daihatsu,

	[Display(ResourceType = typeof(KancolleProgressResources), Name = "Tank")]
	Tank,

	[Display(ResourceType = typeof(KancolleProgressResources), Name = "DaihatsuAndTank")]
	DaihatsuAndTank,
}
