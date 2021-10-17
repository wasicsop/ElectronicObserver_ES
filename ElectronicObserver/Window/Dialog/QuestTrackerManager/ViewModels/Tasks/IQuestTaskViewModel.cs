namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

public interface IQuestTaskViewModel
{
	public string Display { get; }
	public string? Progress { get; }
	public string ClearCondition { get; }
}