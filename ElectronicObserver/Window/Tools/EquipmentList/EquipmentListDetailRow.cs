using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Tools.EquipmentList;

public class EquipmentListDetailRow : ObservableObject
{
	public int Level { get; }
	public int AircraftLevel { get; }
	public int CountAll { get; }
	public int CountRemain { get; }
	public string EquippedShip { get; }

	public EquipmentListDetailRow(int level, int aircraftLevel, int countAll, int countRemain, string equippedShip)
	{
		Level = level;
		AircraftLevel = aircraftLevel;
		CountAll = countAll;
		CountRemain = countRemain;
		EquippedShip = equippedShip;
	}
}