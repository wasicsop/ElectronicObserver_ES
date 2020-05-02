using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using Moq;
using Xunit;

namespace ElectronicObserverCoreTests
{	
	public class DayShellingPowerTests
	{
		[Fact]
		public void DayShellingPowerTest1()
		{
			var fleetMock = new Mock<IFleetData>();

			IFleetData fleet = fleetMock.Object;

			var mock = new Mock<IShipData>();

			mock.Setup(s => s.FirepowerTotal).Returns(28);
			mock.Setup(s => s.HPRate).Returns(1);
			mock.Setup(s => s.AllSlotInstance)
				.Returns(new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>()));
			mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int>()));

			mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.Destroyer);

			IShipData kamikaze = mock.Object;

			Assert.Equal(33, kamikaze.GetDayShellingPower(DayAttackKind.Shelling, fleet));
		}
	}
}