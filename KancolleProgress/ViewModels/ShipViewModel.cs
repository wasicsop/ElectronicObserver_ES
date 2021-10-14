using System.Windows.Media;
using ElectronicObserverTypes;

namespace KancolleProgress.ViewModels;

public class ShipViewModel
{
	public IShipData Ship { get; }
	public string Name => Ship.Name;
	public int Level => Ship.Level;
	public SolidColorBrush Color => KancolleProgressView.Instance.ShipColorBrush(Ship.Level);

	public ShipViewModel(IShipData ship)
	{
		Ship = ship;
	}
}