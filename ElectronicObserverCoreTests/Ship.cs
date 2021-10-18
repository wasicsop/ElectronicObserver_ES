using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using Moq;

namespace ElectronicObserverCoreTests;

public class ShipStats
{
	public int? Level { get; set; }
	public int? Luck { get; set; }
	public int? LoS { get; set; }
}

public class VisibleFits
{
	public int Firepower { get; set; }
	public int Torpedo { get; set; }
}

public class OtherData
{
	public int Fleet { get; set; }
}

public static class EquipExtensions
{
	public static int Firepower(this IEnumerable<IEquipmentData?> equip) =>
		equip.Sum(e => e?.MasterEquipment.Firepower ?? 0);

	public static int Torpedo(this IEnumerable<IEquipmentData?> equip) =>
		equip.Sum(e => e?.MasterEquipment.Torpedo ?? 0);
}

public static class Ship
{
	public static IShipData BismarckDrei(ShipStats? stats = null, List<IEquipmentData?>? equip = null,
		VisibleFits? fits = null, OtherData? other = null)
	{
		stats ??= new ShipStats();
		equip ??= new List<IEquipmentData?>();
		fits ??= new VisibleFits();
		other ??= new OtherData();

		var mock = new Mock<IShipData>();

		mock.Setup(s => s.Level).Returns(stats.Level ?? 1);
		mock.Setup(s => s.HPRate).Returns(1);
		mock.Setup(s => s.FirepowerTotal).Returns(99 + equip.Firepower() + fits.Firepower);
		mock.Setup(s => s.TorpedoTotal).Returns(36 + equip.Torpedo() + fits.Torpedo);
		mock.Setup(s => s.LOSBase).Returns(stats.LoS ?? 22);
		mock.Setup(s => s.LuckTotal).Returns(stats.Luck ?? 26);
		mock.Setup(s => s.AllSlotInstance).Returns(new ReadOnlyCollection<IEquipmentData?>(equip));
		mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> { 4, 4, 4, 4 }));
		mock.Setup(s => s.MasterShip.ShipId).Returns(ShipId.BismarckDrei);
		mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.Battlecruiser);
		mock.Setup(s => s.Fleet).Returns(other.Fleet);

		return mock.Object;
	}

	public static IShipData IseKaiNi(ShipStats? stats = null, List<IEquipmentData?>? equip = null,
		VisibleFits? fits = null)
	{
		stats ??= new ShipStats();
		equip ??= new List<IEquipmentData?>();
		fits ??= new VisibleFits();

		var mock = new Mock<IShipData>();

		mock.Setup(s => s.Level).Returns(stats.Level ?? 1);
		mock.Setup(s => s.HPRate).Returns(1);
		mock.Setup(s => s.FirepowerTotal).Returns(88 + equip.Firepower() + fits.Firepower);
		mock.Setup(s => s.LOSBase).Returns(stats.LoS ?? 30);
		mock.Setup(s => s.LuckTotal).Returns(stats.Luck ?? 40);
		mock.Setup(s => s.AllSlotInstance).Returns(new ReadOnlyCollection<IEquipmentData?>(equip));
		mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> { 2, 2, 22, 22, 9 }));
		mock.Setup(s => s.MasterShip.ShipId).Returns(ShipId.IseKaiNi);
		mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.AviationBattleship);

		return mock.Object;
	}

	public static IShipData AkagiKaiNi(ShipStats? stats = null, List<IEquipmentData?>? equip = null,
		VisibleFits? fits = null)
	{
		stats ??= new ShipStats();
		equip ??= new List<IEquipmentData?>();
		fits ??= new VisibleFits();

		var mock = new Mock<IShipData>();

		mock.Setup(s => s.Level).Returns(stats.Level ?? 1);
		mock.Setup(s => s.HPRate).Returns(1);
		mock.Setup(s => s.FirepowerBase).Returns(60);
		mock.Setup(s => s.FirepowerTotal).Returns(60 + equip.Firepower() + fits.Firepower);
		mock.Setup(s => s.LuckTotal).Returns(stats.Luck ?? 20);
		mock.Setup(s => s.MasterShip.IsAircraftCarrier).Returns(true);
		mock.Setup(s => s.AllSlotInstance).Returns(new ReadOnlyCollection<IEquipmentData?>(equip));
		mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> { 21, 21, 32, 12, 4 }));
		mock.Setup(s => s.MasterShip.ShipId).Returns(ShipId.AkagiKaiNi);
		mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.AircraftCarrier);

		return mock.Object;
	}

	public static IShipData TaihouKai(ShipStats? stats = null, List<IEquipmentData?>? equip = null,
		VisibleFits? fits = null)
	{
		stats ??= new ShipStats();
		equip ??= new List<IEquipmentData?>();
		fits ??= new VisibleFits();

		var mock = new Mock<IShipData>();

		mock.Setup(s => s.Level).Returns(stats.Level ?? 1);
		mock.Setup(s => s.HPRate).Returns(1);
		mock.Setup(s => s.FirepowerTotal).Returns(59 + equip.Firepower() + fits.Firepower);
		mock.Setup(s => s.LOSBase).Returns(stats.LoS ?? 50);
		mock.Setup(s => s.LuckTotal).Returns(stats.Luck ?? 4);
		mock.Setup(s => s.AllSlotInstance).Returns(new ReadOnlyCollection<IEquipmentData?>(equip));
		mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> { 30, 24, 24, 8 }));
		mock.Setup(s => s.MasterShip.IsAircraftCarrier).Returns(true);
		mock.Setup(s => s.MasterShip.ShipId).Returns(ShipId.TaihouKai);
		mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.ArmoredAircraftCarrier);

		return mock.Object;
	}

	public static IShipData TaiyouKaiNi(ShipStats? stats = null, List<IEquipmentData?>? equip = null,
		VisibleFits? fits = null)
	{
		stats ??= new ShipStats();
		equip ??= new List<IEquipmentData?>();
		fits ??= new VisibleFits();

		var mock = new Mock<IShipData>();

		mock.Setup(s => s.Level).Returns(stats.Level ?? 1);
		mock.Setup(s => s.HPRate).Returns(1);
		mock.Setup(s => s.FirepowerBase).Returns(39);
		mock.Setup(s => s.FirepowerTotal).Returns(39 + equip.Firepower() + fits.Firepower);
		mock.Setup(s => s.LuckTotal).Returns(stats.Luck ?? 14);
		mock.Setup(s => s.MasterShip.IsAircraftCarrier).Returns(true);
		mock.Setup(s => s.AllSlotInstance).Returns(new ReadOnlyCollection<IEquipmentData?>(equip));
		mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> { 14, 14, 8, 3 }));
		mock.Setup(s => s.MasterShip.ShipId).Returns(ShipId.TaiyouKaiNi);
		mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.LightAircraftCarrier);

		return mock.Object;
	}

	public static IShipData ArkRoyalKai(ShipStats? stats = null, List<IEquipmentData?>? equip = null,
		VisibleFits? fits = null)
	{
		stats ??= new ShipStats();
		equip ??= new List<IEquipmentData?>();
		fits ??= new VisibleFits();

		var mock = new Mock<IShipData>();

		mock.Setup(s => s.Level).Returns(stats.Level ?? 1);
		mock.Setup(s => s.HPRate).Returns(1);
		mock.Setup(s => s.FirepowerBase).Returns(50);
		mock.Setup(s => s.FirepowerTotal).Returns(50 + equip.Firepower() + fits.Firepower);
		mock.Setup(s => s.LuckTotal).Returns(stats.Luck ?? 13);
		mock.Setup(s => s.MasterShip.IsAircraftCarrier).Returns(true);
		mock.Setup(s => s.AllSlotInstance).Returns(new ReadOnlyCollection<IEquipmentData?>(equip));
		mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> { 24, 30, 12, 12 }));
		mock.Setup(s => s.MasterShip.ShipId).Returns(ShipId.ArkRoyalKai);
		mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.AircraftCarrier);

		return mock.Object;
	}
}
