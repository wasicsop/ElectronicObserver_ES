using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Wpf.Compass.ViewModels;

namespace ElectronicObserver.Window.Wpf.Compass;

public class CompassTemplateSelector : DataTemplateSelector
{
	public DataTemplate? EmptyTemplate { get; set; }
	public DataTemplate? TextTemplate { get; set; }
	public DataTemplate? EnemyListTemplate { get; set; }
	public DataTemplate? BattleTemplate { get; set; }

	public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
	{
		EmptyViewModel => EmptyTemplate,
		TextViewModel => TextTemplate,
		EnemyListViewModel => EnemyListTemplate,
		BattleViewModel => BattleTemplate,
			
		_ => base.SelectTemplate(item, container)
	};
}