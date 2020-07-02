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

		private IShipData Bismarck
		{
			get
			{
				ShipStats stats = new ShipStats
				{
					Luck = 84,
					LoS = 87
				};

				List<IEquipmentData?> equip = new List<IEquipmentData?>
				{
					Equipment.MainGun46Kai(10),
					Equipment.MainGun46Kai(10),
					Equipment.YuraReconSkilled(),
					Equipment.T1ApShell(8),
				};

				OtherData other = new OtherData
				{
					Fleet = 1
				};

				IShipData bismarck = Ship.BismarckDrei(stats, equip, other: other);

				return bismarck;
			}
		}

		private IShipData Isek2
		{
			get
			{
				ShipStats stats = new ShipStats
				{
					Level = 130,
					LoS = 85,
					Luck = 46
				};

				List<IEquipmentData?> equip = new List<IEquipmentData?>
				{
					Equipment.MainGun41K2Triple(6),
					Equipment.MainGun41K2Triple(6),
					Equipment.Zuiunk2(10),
					Equipment.Zuiun634(10),
					Equipment.T1ApShell(8)
				};

				VisibleFits fits = new VisibleFits
				{
					Firepower = 3 + 3 + 5 + 3
				};

				IShipData ise = Ship.IseKaiNi(stats, equip, fits);

				return ise;
			}
		}

		private IShipData Taihou
		{
			get
			{
				ShipStats stats = new ShipStats
				{
					Level = 149,
					LoS = 90,
					Luck = 7
				};

				List<IEquipmentData?> equip = new List<IEquipmentData?>
				{
					Equipment.T97Tomonaga(),
					Equipment.T99Egusa(),
					Equipment.T99Egusa(),
					Equipment.T52CIwai()
				};

				IShipData taihou = Ship.TaihouKai(stats, equip);

				return taihou;
			}
		}

		[Fact]
		public void DayAttackTest1()
		{
			var fleetMock = new Mock<IFleetData>();

			fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
				new List<IShipData?>
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

			Assert.Equal(271, Bismarck.GetDayAttackPower(actual[0], fleet));
			Assert.Equal(217, Bismarck.GetDayAttackPower(actual[1], fleet));
			Assert.Equal(181, Bismarck.GetDayAttackPower(actual[2], fleet));

			List<double> asAttackRates = actual.Select(a => Bismarck.GetDayAttackRate(a, fleet)).ToList();
			List<double> totalRates = asAttackRates.ToList().TotalRates();

			Assert.Equal(0.587, totalRates[0], Precision);
			Assert.Equal(0.28, totalRates[1], Precision);
			Assert.Equal(0.133, totalRates[2], Precision);
		}

		[Fact]
		public void DayAttackTest2()
		{
			var fleetMock = new Mock<IFleetData>();

			fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
				new List<IShipData?>
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

			Assert.Equal(234, Isek2.GetDayAttackPower(actual[0], fleet));
			Assert.Equal(261, Isek2.GetDayAttackPower(actual[1], fleet));
			Assert.Equal(208, Isek2.GetDayAttackPower(actual[2], fleet));
			Assert.Equal(174, Isek2.GetDayAttackPower(actual[3], fleet));

			List<double> asAttackRates = actual.Select(a => Isek2.GetDayAttackRate(a, fleet)).ToList();
			List<double> totalRates = asAttackRates.ToList().TotalRates();

			Assert.Equal(0.625, totalRates[0], Precision);
			Assert.Equal(0.188, totalRates[1], Precision);
			Assert.Equal(0.108, totalRates[2], Precision);
			Assert.Equal(0.079, totalRates[3], Precision);
		}

		[Fact]
		public void DayAttackTest3()
		{
			var fleetMock = new Mock<IFleetData>();

			fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
				new List<IShipData?>
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
			Assert.Equal(230, Taihou.GetDayAttackPower(actual[0], fleet));
			Assert.Equal(220, Taihou.GetDayAttackPower(actual[1], fleet));
			Assert.Equal(211, Taihou.GetDayAttackPower(actual[2], fleet));
			Assert.Equal(184, Taihou.GetDayAttackPower(actual[3], fleet));

			List<double> asAttackRates = actual.Select(a => Taihou.GetDayAttackRate(a, fleet)).ToList();
			List<double> totalRates = asAttackRates.ToList().TotalRates();

			Assert.Equal(0.552, totalRates[0], Precision);
			Assert.Equal(0.229, totalRates[1], Precision);
			Assert.Equal(0.101, totalRates[2], Precision);
			Assert.Equal(0.118, totalRates[3], Precision);
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
			mock.Setup(s => s.AllSlotInstance)
				.Returns(new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>
				{
					mg356,
					mg356,
					seaplane
				}));
			mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> {4, 4, 4, 4}));
			mock.Setup(s => s.MasterShip.ShipId).Returns(ShipId.IseKaiNi);
			mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.AviationBattleship);

			IShipData Nagato = mock.Object;

			var fleetMock = new Mock<IFleetData>();

			fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
				new List<IShipData?>
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
			Assert.Equal(171, Nagato.GetDayAttackPower(actual[0], fleet));
			Assert.Equal(143, Nagato.GetDayAttackPower(actual[1], fleet));

			List<double> asAttackRates = actual.Select(a => Nagato.GetDayAttackRate(a, fleet)).ToList();
			List<double> totalRates = asAttackRates.ToList().TotalRates();

			Assert.Equal(0.415, totalRates[0], Precision);
			Assert.Equal(0.585, totalRates[1], Precision);
		}

		[Fact]
		public void DayAttackTest5()
		{
			var fleetMock = new Mock<IFleetData>();

			fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
				new List<IShipData?>
				{
					Bismarck,
					Isek2,
					Taihou
				}));
			fleetMock.Setup(f => f.FleetType).Returns(FleetType.Surface);

			IFleetData fleet = fleetMock.Object;

			List<Enum> expected = new List<Enum>
			{
				DayAttackKind.CutinMainMain,
				DayAttackKind.DoubleShelling,
				DayAttackKind.Shelling
			};

			List<Enum> actual = Bismarck.GetDayAttacks().ToList();

			Assert.Equal(expected, actual);

			Assert.Equal(274, Bismarck.GetDayAttackPower(actual[0], fleet));
			Assert.Equal(219, Bismarck.GetDayAttackPower(actual[1], fleet));
			Assert.Equal(183, Bismarck.GetDayAttackPower(actual[2], fleet));

			List<double> asAttackRates = actual.Select(a => Bismarck.GetDayAttackRate(a, fleet)).ToList();
			List<double> totalRates = asAttackRates.ToList().TotalRates();

			Assert.Equal(0.587, totalRates[0], Precision);
			Assert.Equal(0.28, totalRates[1], Precision);
			Assert.Equal(0.133, totalRates[2], Precision);
		}
	}
}
