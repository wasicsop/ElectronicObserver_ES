using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentList;

public class EquipmentListRow
{
	public int Id { get; }
	public EquipmentIconType IconType { get; }
	public string Name { get; }
	public int AllCount { get; }
	public int RemainCount { get; }
	public string ToolTipText { get; set; }
	public int Tag { get; set; }

	public EquipmentListRow(int id, EquipmentIconType iconType, string name, int allCount, int remainCount)
	{
		Id = id;
		IconType = iconType;
		Name = name;
		AllCount = allCount;
		RemainCount = remainCount;
	}
}