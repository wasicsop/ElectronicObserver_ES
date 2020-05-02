using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using Moq;
using Xunit;

namespace ElectronicObserverCoreTests
{
	public class DayAttackTests
	{
		private IEquipmentData MainGun
		{
			get
			{
				var mock = new Mock<IEquipmentData>();

				mock.Setup(s => s.Level).Returns(6);
				mock.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.MainGunLarge);
				mock.Setup(s => s.MasterEquipment.IsMainGun).Returns(true);

				return mock.Object;
			}
		}

		private IEquipmentData Zuiunk2
		{
			get
			{
				var mock = new Mock<IEquipmentData>();

				mock.Setup(s => s.Level).Returns(10);
				mock.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneBomber);
				mock.Setup(s => s.EquipmentId).Returns(EquipmentId.SeaplaneBomber_ZuiunKaiNi_634AirGroup);

				return mock.Object;
			}
		}

		private IEquipmentData Zuiun634
		{
			get
			{
				var mock = new Mock<IEquipmentData>();

				mock.Setup(s => s.Level).Returns(10);
				mock.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneBomber);
				mock.Setup(s => s.EquipmentId).Returns(EquipmentId.SeaplaneBomber_Zuiun_634AirGroup);

				return mock.Object;
			}
		}

		private IEquipmentData ApShell
		{
			get
			{
				var mock = new Mock<IEquipmentData>();

				mock.Setup(s => s.Level).Returns(8);
				mock.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.APShell);

				return mock.Object;
			}
		}

		private IEquipmentData Fighter
		{
			get
			{
				var mock = new Mock<IEquipmentData>();

				mock.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.CarrierBasedFighter);

				return mock.Object;
			}
		}

		private IEquipmentData Egusa
		{
			get
			{
				var mock = new Mock<IEquipmentData>();

				mock.Setup(s => s.MasterEquipment.Bomber).Returns(11);
				mock.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.CarrierBasedBomber);

				return mock.Object;
			}
		}

		private IEquipmentData Tomonaga
		{
			get
			{
				var mock = new Mock<IEquipmentData>();

				mock.Setup(s => s.MasterEquipment.Torpedo).Returns(11);
				mock.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.CarrierBasedTorpedo);

				return mock.Object;
			}
		}

		[Fact]
		public void DayAttackTest1()
		{
			var fleetMock = new Mock<IFleetData>();

			IFleetData fleet = fleetMock.Object;

			var mock = new Mock<IShipData>();

			mock.Setup(s => s.HPRate).Returns(1);
			mock.Setup(s => s.FirepowerTotal).Returns(159);
			mock.Setup(s => s.AllSlotInstance)
				.Returns(new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>
				{
					MainGun,
					MainGun,
					Zuiunk2,
					Zuiun634,
					ApShell
				}));
			mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> {2, 2, 22, 22, 9}));
			mock.Setup(s => s.MasterShip.ShipId).Returns(ShipId.IseKaiNi);
			mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.AviationBattleship);

			IShipData isek2 = mock.Object;

			List<Enum> expected = new List<Enum>
			{
				DayAttackKind.ZuiunMultiAngle,
				DayAttackKind.CutinMainMain,
				DayAttackKind.DoubleShelling,
				DayAttackKind.Shelling
			};

			List<Enum> actual = isek2.GetDayAttacks().ToList();

			Assert.Equal(expected, actual);

			Assert.Equal(234, isek2.GetDayShellingPower(actual[0], fleet));
			Assert.Equal(261, isek2.GetDayShellingPower(actual[1], fleet));
			Assert.Equal(208, isek2.GetDayShellingPower(actual[2], fleet));
			Assert.Equal(174, isek2.GetDayShellingPower(actual[3], fleet));
		}

		[Fact]
		public void DayAttackTest2()
		{
			var fleetMock = new Mock<IFleetData>();

			IFleetData fleet = fleetMock.Object;

			var mock = new Mock<IShipData>();

			mock.Setup(s => s.HPRate).Returns(1);
			mock.Setup(s => s.FirepowerTotal).Returns(59);
			mock.Setup(s => s.BomberTotal).Returns(13);
			mock.Setup(s => s.AllSlotInstance)
				.Returns(new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>
				{
					Tomonaga,
					Tomonaga,
					Egusa,
					Fighter
				}));
			mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> { 30, 24, 24, 8 }));
			mock.Setup(s => s.MasterShip.IsAircraftCarrier).Returns(true);

			IShipData taihou = mock.Object;

			List<Enum> expected = new List<Enum>
			{
				DayAirAttackCutinKind.FighterBomberAttacker,
				DayAirAttackCutinKind.BomberAttacker,
				DayAttackKind.AirAttack
			};

			List<Enum> actual = taihou.GetDayAttacks().ToList();

			Assert.Equal(expected, actual);
			Assert.Equal(230, taihou.GetDayShellingPower(expected.First(), fleet));
		}
	}
}
