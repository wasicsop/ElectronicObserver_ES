using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using Moq;
using Xunit;

namespace ElectronicObserverCoreTests;

public class NightAttackTests
{
	private int Precision => 3;

	[Fact]
	public void NightAttackTest1()
	{
		ShipStats stats = new ShipStats
		{
			Level = 175,
			Luck = 84,
		};

		List<IEquipmentData?> equip = new List<IEquipmentData?>
		{
			Equipment.MainGun46Kai(10),
			Equipment.Quint(10),
			Equipment.Quint(10),
		};

		IShipData bismarck = Ship.BismarckDrei(stats, equip);

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			bismarck
		}));

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			NightAttackKind.CutinTorpedoTorpedo,
			NightAttackKind.Shelling
		};

		List<Enum> actual = bismarck.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(293, bismarck.GetNightAttackPower(actual[0]));
		Assert.Equal(195, bismarck.GetNightAttackPower(actual[1]));

		List<double> attackRates = actual.Select(a => bismarck.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.Equal(0.787, totalRates[0], Precision);
		Assert.Equal(0.213, totalRates[1], Precision);
	}

	[Fact]
	public void NightAttackTest2()
	{
		ShipStats stats = new ShipStats
		{
			Level = 122,
			Luck = 25
		};

		List<IEquipmentData?> equip = new List<IEquipmentData?>
		{
			Equipment.T97NightAttacker(),
			Equipment.ReppuuKaiNiESkilled(),
			Equipment.T97NightAttacker(),
			Equipment.ReppuuKaiNiE(),
			Equipment.NightScamp()
		};

		IShipData akagi = Ship.AkagiKaiNi(stats, equip);

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			akagi
		}));

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			CvnciKind.FighterFighterAttacker,
			CvnciKind.FighterAttacker,
			CvnciKind.FighterOtherOther,
			NightAttackKind.AirAttack
		};

		List<Enum> actual = akagi.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(314, akagi.GetNightAttackPower(actual[0]));
		Assert.Equal(313, akagi.GetNightAttackPower(actual[1]));
		Assert.Equal(313, akagi.GetNightAttackPower(actual[2]));
		Assert.Equal(310, akagi.GetNightAttackPower(actual[3]));

		List<double> attackRates = actual.Select(a => akagi.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.Equal(0.6, totalRates[0], Precision);
		Assert.Equal(0.219, totalRates[1], Precision);
		Assert.Equal(0.091, totalRates[2], Precision);
		Assert.Equal(0.09, totalRates[3], Precision);
	}

	[Fact]
	public void NightAttackTest3()
	{
		ShipStats stats = new ShipStats
		{
			Level = 125,
			Luck = 17
		};

		List<IEquipmentData?> equip = new List<IEquipmentData?>
		{
			Equipment.T97NightAttacker(),
			Equipment.T97NightAttacker(),
			Equipment.ReppuuKaiNiESkilled(),
			Equipment.NightScamp()
		};

		IShipData taiyou = Ship.TaiyouKaiNi(stats, equip);

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			taiyou
		}));

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			CvnciKind.FighterAttacker,
			CvnciKind.FighterOtherOther,
			NightAttackKind.AirAttack
		};

		List<Enum> actual = taiyou.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(251, taiyou.GetNightAttackPower(actual[0]));
		Assert.Equal(247, taiyou.GetNightAttackPower(actual[1]));
		Assert.Equal(209, taiyou.GetNightAttackPower(actual[2]));

		List<double> attackRates = actual.Select(a => taiyou.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.Equal(0.478, totalRates[0], Precision);
		Assert.Equal(0.23, totalRates[1], Precision);
		Assert.Equal(0.292, totalRates[2], Precision);
	}

	[Fact]
	public void ArkRoyal()
	{
		ShipStats stats = new ShipStats
		{
			Level = 130,
			Luck = 16
		};

		List<IEquipmentData?> equip = new List<IEquipmentData?>
		{
			Equipment.SwordfishMk3Skilled(),
			Equipment.ReppuuKaiNiESkilled(),
			Equipment.OTO(10),
			Equipment.OTO(10),
		};

		IShipData ark = Ship.ArkRoyalKai(stats, equip);

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			ark
		}));

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			NightAttackKind.DoubleShelling,
			NightAttackKind.Shelling
		};

		List<Enum> actual = ark.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(74, ark.GetNightAttackPower(actual[0]));
		Assert.Equal(62, ark.GetNightAttackPower(actual[1]));

		List<double> attackRates = actual.Select(a => ark.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.Equal(0.99, totalRates[0], Precision);
		Assert.Equal(0.01, totalRates[1], Precision);
	}
}
