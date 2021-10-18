using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using Moq;
using Xunit;
using FleetDataCustom = ElectronicObserver.Data.FleetDataCustom;

namespace ElectronicObserverCoreTests;

public class AswAttackTest
{
	[Fact]
	public void DayAttackTest1()
	{
		IFleetData fleet = new FleetDataCustom();

		var mock = new Mock<IShipData>();

		mock.Setup(s => s.ASWBase).Returns(58);
		mock.Setup(s => s.HPRate).Returns(1);
		mock.Setup(s => s.AllSlotInstance).Returns(new ReadOnlyCollection<IEquipmentData?>(new List<IEquipmentData?>
		{
			Equipment.HFDF(),
			Equipment.AswTorpedo(),
			Equipment.Type2DepthCharge()
		}));

		IShipData akebono = mock.Object;

		Assert.Equal(104, akebono.GetAswAttackPower(DayAttackKind.DepthCharge, fleet));
	}
}
