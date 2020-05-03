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
		private int Precision => 3;

		private IEquipmentData MainGun46kai
		{
			get
			{
				var mock = new Mock<IEquipmentData>();

				mock.Setup(s => s.Level).Returns(10);
				mock.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.MainGunLarge);
				mock.Setup(s => s.MasterEquipment.IsMainGun).Returns(true);

				return mock.Object;
			}
		}

		private IEquipmentData MainGun41k2triple
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
				mock.Setup(s => s.EquipmentId).Returns(EquipmentId.SeaplaneBomber_ZuiunKaiNi_634AirGroup);
				mock.Setup(s => s.MasterEquipment.LOS).Returns(7);
				mock.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneBomber);

				return mock.Object;
			}
		}

		private IEquipmentData Zuiun634
		{
			get
			{
				var mock = new Mock<IEquipmentData>();

				mock.Setup(s => s.Level).Returns(10);
				mock.Setup(s => s.EquipmentId).Returns(EquipmentId.SeaplaneBomber_Zuiun_634AirGroup);
				mock.Setup(s => s.MasterEquipment.LOS).Returns(6);
				mock.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneBomber);

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

		private IEquipmentData YuraReconSkilled
		{
			get
			{
				var mock = new Mock<IEquipmentData>();

				mock.Setup(s => s.MasterEquipment.LOS).Returns(8);
				mock.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneRecon);

				return mock.Object;
			}
		}

		private IEquipmentData Fighter
		{
			get
			{
				var mock = new Mock<IEquipmentData>();

				mock.Setup(s => s.MasterEquipment.LOS).Returns(1);
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
				mock.Setup(s => s.MasterEquipment.LOS).Returns(3);
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
				mock.Setup(s => s.MasterEquipment.LOS).Returns(4);
				mock.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.CarrierBasedTorpedo);

				return mock.Object;
			}
		}

		private IShipData Bismarck
		{
			get
			{
				var mock = new Mock<IShipData>();

				mock.Setup(s => s.HPRate).Returns(1);
				mock.Setup(s => s.FirepowerTotal).Returns(164);
				mock.Setup(s => s.LOSBase).Returns(87);
				mock.Setup(s => s.LuckTotal).Returns(84);
				mock.Setup(s => s.MasterID).Returns(1);
				mock.Setup(s => s.AllSlotInstance)
					.Returns(new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>
					{
						MainGun46kai,
						MainGun46kai,
						YuraReconSkilled,
						ApShell,
					}));
				mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> { 4, 4, 4, 4 }));
				mock.Setup(s => s.MasterShip.ShipId).Returns(ShipId.IseKaiNi);
				mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.AviationBattleship);

				return mock.Object;
			}
		}

		private IShipData Isek2
		{
			get
			{
				var mock = new Mock<IShipData>();

				mock.Setup(s => s.HPRate).Returns(1);
				mock.Setup(s => s.FirepowerTotal).Returns(159);
				mock.Setup(s => s.LOSBase).Returns(85);
				mock.Setup(s => s.LuckTotal).Returns(46);
				mock.Setup(s => s.MasterID).Returns(2);
				mock.Setup(s => s.AllSlotInstance)
					.Returns(new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>
					{
						MainGun41k2triple,
						MainGun41k2triple,
						Zuiunk2,
						Zuiun634,
						ApShell
					}));
				mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> { 2, 2, 22, 22, 9 }));
				mock.Setup(s => s.MasterShip.ShipId).Returns(ShipId.IseKaiNi);
				mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.AviationBattleship);

				return mock.Object;
			}
		}

		private IShipData Taihou
		{
			get
			{
				var mock = new Mock<IShipData>();

				mock.Setup(s => s.HPRate).Returns(1);
				mock.Setup(s => s.FirepowerTotal).Returns(59);
				mock.Setup(s => s.LOSBase).Returns(90);
				mock.Setup(s => s.LuckTotal).Returns(7);
				mock.Setup(s => s.MasterID).Returns(3);
				mock.Setup(s => s.AllSlotInstance)
					.Returns(new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>
					{
						Tomonaga,
						Egusa,
						Egusa,
						Fighter
					}));
				mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> { 30, 24, 24, 8 }));
				mock.Setup(s => s.MasterShip.IsAircraftCarrier).Returns(true);

				return mock.Object;
			}
		}

		[Fact]
		public void DayAttackTest1()
		{
			var fleetMock = new Mock<IFleetData>();

			fleetMock.Setup(f => f.MembersInstance).Returns(new ReadOnlyCollection<IShipData>(new List<IShipData>
			{
				Bismarck,
				Isek2,
				Taihou
			}));

			IFleetData fleet = fleetMock.Object;

			List<Enum> expected = new List<Enum>
			{
				DayAttackKind.CutinMainMain,
				DayAttackKind.DoubleShelling,
				DayAttackKind.Shelling
			};

			List<Enum> actual = Bismarck.GetDayAttacks().ToList();

			Assert.Equal(expected, actual);

			Assert.Equal(271, Bismarck.GetDayShellingPower(actual[0], fleet));
			Assert.Equal(217, Bismarck.GetDayShellingPower(actual[1], fleet));
			Assert.Equal(181, Bismarck.GetDayShellingPower(actual[2], fleet));

			List<double> asAttackRates = actual.Select(a => Bismarck.GetDayAttackRate(a, fleet)).ToList();
			List<double> allAsRates = asAttackRates.ToList().TotalRates();

			Assert.Equal(0.587, allAsRates[0], Precision);
			Assert.Equal(0.28, allAsRates[1], Precision);
			Assert.Equal(0.133, allAsRates[2], Precision);
		}

		[Fact]
		public void DayAttackTest2()
		{
			var fleetMock = new Mock<IFleetData>();

			fleetMock.Setup(f => f.MembersInstance).Returns(new ReadOnlyCollection<IShipData>(new List<IShipData>
			{
				Bismarck,
				Isek2,
				Taihou
			}));

			IFleetData fleet = fleetMock.Object;

			List<Enum> expected = new List<Enum>
			{
				DayAttackKind.ZuiunMultiAngle,
				DayAttackKind.CutinMainMain,
				DayAttackKind.DoubleShelling,
				DayAttackKind.Shelling
			};

			List<Enum> actual = Isek2.GetDayAttacks().ToList();

			Assert.Equal(expected, actual);

			Assert.Equal(234, Isek2.GetDayShellingPower(actual[0], fleet));
			Assert.Equal(261, Isek2.GetDayShellingPower(actual[1], fleet));
			Assert.Equal(208, Isek2.GetDayShellingPower(actual[2], fleet));
			Assert.Equal(174, Isek2.GetDayShellingPower(actual[3], fleet));

			List<double> asAttackRates = actual.Skip(1).Select(a => Isek2.GetDayAttackRate(a, fleet)).ToList();
			List<double> allAsRates = asAttackRates.ToList().TotalRates();

			// Assert.Equal(0.587, allAsRates[0], Precision);
			Assert.Equal(0.507, allAsRates[0], Precision);
			Assert.Equal(0.288, allAsRates[1], Precision);
			Assert.Equal(0.205, allAsRates[2], Precision);
		}

		[Fact]
		public void DayAttackTest3()
		{
			var fleetMock = new Mock<IFleetData>();

			fleetMock.Setup(f => f.MembersInstance).Returns(new ReadOnlyCollection<IShipData>(new List<IShipData>
			{
				Bismarck,
				Isek2,
				Taihou
			}));

			IFleetData fleet = fleetMock.Object;

			

			List<Enum> expected = new List<Enum>
			{
				DayAirAttackCutinKind.FighterBomberAttacker,
				DayAirAttackCutinKind.BomberBomberAttacker,
				DayAirAttackCutinKind.BomberAttacker,
				DayAttackKind.AirAttack
			};

			List<Enum> actual = Taihou.GetDayAttacks().ToList();

			Assert.Equal(expected, actual);
			Assert.Equal(230, Taihou.GetDayShellingPower(actual[0], fleet));
			Assert.Equal(220, Taihou.GetDayShellingPower(actual[1], fleet));
			Assert.Equal(211, Taihou.GetDayShellingPower(actual[2], fleet));
			Assert.Equal(184, Taihou.GetDayShellingPower(actual[3], fleet));

			List<double> asAttackRates = actual.Select(a => Taihou.GetDayAttackRate(a, fleet)).ToList();
			List<double> allAsRates = asAttackRates.ToList().TotalRates();

			Assert.Equal(0.56, allAsRates[0], Precision);
			Assert.Equal(0.22, allAsRates[1], Precision);
			Assert.Equal(0.099, allAsRates[2], Precision);
			Assert.Equal(0.121, allAsRates[3], Precision);
		}

		[Fact]
		public void DayAttackTest4()
		{
			var mockEq1 = new Mock<IEquipmentData>();

			mockEq1.Setup(s => s.Level).Returns(10);
			mockEq1.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.MainGunLarge);
			mockEq1.Setup(s => s.MasterEquipment.IsMainGun).Returns(true);

			IEquipmentData mg356 = mockEq1.Object;

			var mockEq2 = new Mock<IEquipmentData>();

			mockEq2.Setup(s => s.MasterEquipment.LOS).Returns(6);
			mockEq2.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneRecon);

			IEquipmentData seaplane = mockEq2.Object;

			var mock = new Mock<IShipData>();

			mock.Setup(s => s.HPRate).Returns(1);
			mock.Setup(s => s.FirepowerTotal).Returns(129);
			mock.Setup(s => s.LOSBase).Returns(39);
			mock.Setup(s => s.LuckTotal).Returns(32);
			mock.Setup(s => s.MasterID).Returns(1);
			mock.Setup(s => s.AllSlotInstance)
				.Returns(new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>
				{
					mg356,
					mg356,
					seaplane
				}));
			mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> { 4, 4, 4, 4 }));
			mock.Setup(s => s.MasterShip.ShipId).Returns(ShipId.IseKaiNi);
			mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.AviationBattleship);

			IShipData Nagato = mock.Object;

			var fleetMock = new Mock<IFleetData>();

			fleetMock.Setup(f => f.MembersInstance).Returns(new ReadOnlyCollection<IShipData>(new List<IShipData>
			{
				Nagato
			}));

			IFleetData fleet = fleetMock.Object;

			List<Enum> expected = new List<Enum>
			{
				DayAttackKind.DoubleShelling,
				DayAttackKind.Shelling
			};

			List<Enum> actual = Nagato.GetDayAttacks().ToList();

			Assert.Equal(expected, actual);
			Assert.Equal(171, Nagato.GetDayShellingPower(actual[0], fleet));
			Assert.Equal(143, Nagato.GetDayShellingPower(actual[1], fleet));

			List<double> asAttackRates = actual.Select(a => Nagato.GetDayAttackRate(a, fleet)).ToList();
			List<double> allAsRates = asAttackRates.ToList().TotalRates();

			Assert.Equal(0.415, allAsRates[0], Precision);
			Assert.Equal(0.585, allAsRates[1], Precision);
		}
	}
}
