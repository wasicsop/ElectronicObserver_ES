using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.ViewModels;

namespace ElectronicObserver;

public class ViewStyleSelector : StyleSelector
{
	public Style? AnchorableStyle { get; set; }

	public override Style? SelectStyle(object item, DependencyObject container) => item switch
	{
		AnchorableViewModel => AnchorableStyle,

		_ => base.SelectStyle(item, container)
	};
}
