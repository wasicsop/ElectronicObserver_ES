using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public class TaskTemplateSelector : DataTemplateSelector
{
	public DataTemplate? BossKill { get; set; }
	public DataTemplate? Expedition { get; set; }

	public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
	{
		BossKillTaskViewModel => BossKill,
		ExpeditionTask => Expedition,

		_ => throw new NotImplementedException(),
	};
}