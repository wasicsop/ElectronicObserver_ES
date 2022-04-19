using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Dialog.ShipPicker;

public class Filter : ObservableObject
{
	public ShipTypeGroup Value { get; set; }
	public string Display => Value.Display();
	public bool IsChecked { get; set; }

	public Filter(ShipTypeGroup value)
	{
		Value = value;
	}
}
