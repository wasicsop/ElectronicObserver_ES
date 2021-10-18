using System.Text;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterShip;

public record EquipmentSlot(int Size, IEquipmentDataMaster? Equipment, EquipmentStatus Status)
{
	public string Name => Equipment?.NameEN ?? Status switch
	{
		EquipmentStatus.Known => Properties.Window.Dialog.DialogAlbumMasterShip.Empty,
		EquipmentStatus.Unknown => "??"
	};

	public EquipmentIconType IconType => Equipment?.IconTypeTyped ?? Status switch
	{
		EquipmentStatus.Known => EquipmentIconType.Nothing,
		EquipmentStatus.Unknown => EquipmentIconType.Unknown
	};

	public string? ToolTip => MakeToolTip(Equipment);

	private string? MakeToolTip(IEquipmentDataMaster? eq)
	{
		if (eq is null) return null;

		StringBuilder sb = new();

		sb.AppendFormat("{0} {1} (ID: {2})\r\n", eq.CategoryTypeInstance.NameEN, eq.NameEN, eq.EquipmentID);
		if (eq.Firepower != 0) sb.AppendFormat(Properties.Window.Dialog.DialogAlbumMasterShip.Firepower + " {0:+0;-0}\r\n", eq.Firepower);
		if (eq.Torpedo != 0) sb.AppendFormat(Properties.Window.Dialog.DialogAlbumMasterShip.Torpedo + " {0:+0;-0}\r\n", eq.Torpedo);
		if (eq.AA != 0) sb.AppendFormat(Properties.Window.Dialog.DialogAlbumMasterShip.AA + " {0:+0;-0}\r\n", eq.AA);
		if (eq.Armor != 0) sb.AppendFormat(Properties.Window.Dialog.DialogAlbumMasterShip.Armor + " {0:+0;-0}\r\n", eq.Armor);
		if (eq.ASW != 0) sb.AppendFormat(Properties.Window.Dialog.DialogAlbumMasterShip.ASW + " {0:+0;-0}\r\n", eq.ASW);
		if (eq.Evasion != 0) sb.AppendFormat("{0} {1:+0;-0}\r\n", eq.CategoryType == EquipmentTypes.Interceptor ? Properties.Window.Dialog.DialogAlbumMasterShip.Interception : Properties.Window.Dialog.DialogAlbumMasterShip.Evasion, eq.Evasion);
		if (eq.LOS != 0) sb.AppendFormat(Properties.Window.Dialog.DialogAlbumMasterShip.LOS + " {0:+0;-0}\r\n", eq.LOS);
		if (eq.Accuracy != 0) sb.AppendFormat("{0} {1:+0;-0}\r\n", eq.CategoryType == EquipmentTypes.Interceptor ? Properties.Window.Dialog.DialogAlbumMasterShip.AntiBomb : Properties.Window.Dialog.DialogAlbumMasterShip.Accuracy, eq.Accuracy);
		if (eq.Bomber != 0) sb.AppendFormat(Properties.Window.Dialog.DialogAlbumMasterShip.Bombing + " {0:+0;-0}\r\n", eq.Bomber);
		sb.AppendLine(Properties.Window.Dialog.DialogAlbumMasterShip.RightClickToOpenInNewWindow);

		return sb.ToString();
	}
}
