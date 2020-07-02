using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using Xunit;
using Moq;

namespace ElectronicObserverCoreTests
{
	public static class NumberExtensions
	{
		public static double RoundDown(this double value, int precision = 0)
		{
			double power = Math.Pow(10, precision);
			return Math.Floor(value * power) / power;
		}
	}

	// LoS tests won't pass since the formula was changed to use LOSTotal instead of LOSBase
	// to handle fit bonus
    public class LoSTests
    {
	    private int AdmiralLevel => 120;

	    private IShipData Kamikaze
	    {
		    get
		    {
				var mock = new Mock<IShipData>();

				mock.Setup(s => s.LOSBase).Returns(27);
				mock.Setup(s => s.AllSlotInstance).Returns(NoEquip);

				return mock.Object;
		    }
	    }

	    private IShipData Asakaze
	    {
		    get
		    {
			    var mock = new Mock<IShipData>();

			    mock.Setup(s => s.LOSBase).Returns(28);
			    mock.Setup(s => s.AllSlotInstance).Returns(NoEquip);

			    return mock.Object;
		    }
	    }

	    private IShipData Harukaze
	    {
		    get
		    {
			    var mock = new Mock<IShipData>();

			    mock.Setup(s => s.LOSBase).Returns(26);
			    mock.Setup(s => s.AllSlotInstance).Returns(NoEquip);

			    return mock.Object;
		    }
	    }

	    private IShipData Matsukaze
	    {
		    get
		    {
			    var mock = new Mock<IShipData>();

			    mock.Setup(s => s.LOSBase).Returns(28);
			    mock.Setup(s => s.AllSlotInstance).Returns(NoEquip);

			    return mock.Object;
		    }
	    }

	    private IShipData Hatakaze
	    {
		    get
		    {
			    var mock = new Mock<IShipData>();

			    mock.Setup(s => s.LOSBase).Returns(26);
			    mock.Setup(s => s.AllSlotInstance).Returns(NoEquip);

			    return mock.Object;
		    }
	    }

		private ReadOnlyCollection<IEquipmentData> NoEquip => new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>());

		[Fact]
        public void LoSTest1()
        {
			// no equip, non-kai Kamikaze class
	        var mockFleet = new Mock<IFleetData>();

			ReadOnlyCollection<IShipData> ships = new ReadOnlyCollection<IShipData>(new []{ Kamikaze, Asakaze, Harukaze, Matsukaze, Hatakaze });
	        mockFleet.Setup(f => f.MembersWithoutEscaped).Returns(ships);

	        IFleetData fleet = mockFleet.Object;

	        double expected = -20.03;
	        double actual = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);

			Assert.Equal(expected, actual.RoundDown(2));
        }

        [Fact]
        public void LoSTest2()
        {
			// Perth with maxed night recon
	        var mockFleet = new Mock<IFleetData>();

	        var nightReconMock = new Mock<IEquipmentData>();
	        nightReconMock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneRecon);
	        nightReconMock.Setup(e => e.MasterEquipment.LOS).Returns(3);
	        nightReconMock.Setup(e => e.Level).Returns(10);
	        IEquipmentData nightRecon = nightReconMock.Object;

			ReadOnlyCollection<IEquipmentData> perthGear = new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>
			{
				nightRecon
			});

	        var perthMock = new Mock<IShipData>();
	        perthMock.Setup(s => s.LOSBase).Returns(77);
	        perthMock.Setup(s => s.AllSlotInstance).Returns(perthGear);
	        IShipData perth = perthMock.Object;

	        ReadOnlyCollection<IShipData> ships = new ReadOnlyCollection<IShipData>(new[] { perth });
	        mockFleet.Setup(f => f.MembersWithoutEscaped).Returns(ships);

	        IFleetData fleet = mockFleet.Object;

	        double expected1 = -21.08;
	        double expected2 = -12.92;
	        double expected3 = -4.77;
	        double expected4 = 3.38;

			double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
			double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
			double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
			double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

			Assert.Equal(expected1, actual1.RoundDown(2));
			Assert.Equal(expected2, actual2.RoundDown(2));
			Assert.Equal(expected3, actual3.RoundDown(2));
			Assert.Equal(expected4, actual4.RoundDown(2));
        }

        [Fact]
        public void LoSTest3()
        {
	        var mockFleet = new Mock<IFleetData>();

	        var nightReconMock = new Mock<IEquipmentData>();
	        nightReconMock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneRecon);
	        nightReconMock.Setup(e => e.MasterEquipment.LOS).Returns(3);
	        nightReconMock.Setup(e => e.Level).Returns(0);
	        IEquipmentData nightRecon = nightReconMock.Object;

	        ReadOnlyCollection<IEquipmentData> nisshinGear = new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>
	        {
		        nightRecon,
		        nightRecon,
	        });

	        var nisshinMock = new Mock<IShipData>();
	        nisshinMock.Setup(s => s.LOSBase).Returns(121);
	        nisshinMock.Setup(s => s.AllSlotInstance).Returns(nisshinGear);
	        IShipData nisshin = nisshinMock.Object;

	        var yuubariMock = new Mock<IShipData>();
			yuubariMock.Setup(s => s.LOSBase).Returns(74);
			yuubariMock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
			IShipData yuubari = yuubariMock.Object;

			var kuroshioMock = new Mock<IShipData>();
			kuroshioMock.Setup(s => s.LOSBase).Returns(60);
			kuroshioMock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
			IShipData kuroshio = kuroshioMock.Object;

			var kagerouMock = new Mock<IShipData>();
			kagerouMock.Setup(s => s.LOSBase).Returns(62);
			kagerouMock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
			IShipData kagerou = kagerouMock.Object;

			var shiranuiMock = new Mock<IShipData>();
			shiranuiMock.Setup(s => s.LOSBase).Returns(63);
			shiranuiMock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
			IShipData shiranui = shiranuiMock.Object;

			var naganamiMock = new Mock<IShipData>();
			naganamiMock.Setup(s => s.LOSBase).Returns(66);
			naganamiMock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
			IShipData naganami = naganamiMock.Object;

			ReadOnlyCollection<IShipData> ships = new ReadOnlyCollection<IShipData>(new[] { nisshin, yuubari, kuroshio, kagerou, shiranui, naganami });
	        mockFleet.Setup(f => f.MembersWithoutEscaped).Returns(ships);

	        IFleetData fleet = mockFleet.Object;

			double expected1 = 10.48;
			double expected2 = 17.68;
			double expected3 = 24.88;
			double expected4 = 32.08;

			double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
			double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
			double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
			double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

			Assert.Equal(expected1, actual1.RoundDown(2));
			Assert.Equal(expected2, actual2.RoundDown(2));
			Assert.Equal(expected3, actual3.RoundDown(2));
			Assert.Equal(expected4, actual4.RoundDown(2));
		}

		[Fact]
		public void LoSTest4()
		{
			var mockFleet = new Mock<IFleetData>();

			var nightReconMock = new Mock<IEquipmentData>();
			nightReconMock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneRecon);
			nightReconMock.Setup(e => e.MasterEquipment.LOS).Returns(3);
			nightReconMock.Setup(e => e.Level).Returns(0);
			IEquipmentData nightRecon = nightReconMock.Object;

			var skilledLookoutsMock = new Mock<IEquipmentData>();
			skilledLookoutsMock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.SurfaceShipPersonnel);
			skilledLookoutsMock.Setup(e => e.MasterEquipment.LOS).Returns(2);
			skilledLookoutsMock.Setup(e => e.Level).Returns(0);
			IEquipmentData skilledLookouts = skilledLookoutsMock.Object;

			ReadOnlyCollection<IEquipmentData> nisshinGear = new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>
			{
				nightRecon,
				nightRecon,
				skilledLookouts
			});

			var nisshinMock = new Mock<IShipData>();
			nisshinMock.Setup(s => s.LOSBase).Returns(121);
			nisshinMock.Setup(s => s.AllSlotInstance).Returns(nisshinGear);
			IShipData nisshin = nisshinMock.Object;

			ReadOnlyCollection<IEquipmentData> skilledLookout = new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>
			{
				skilledLookouts
			});

			var yuubariMock = new Mock<IShipData>();
			yuubariMock.Setup(s => s.LOSBase).Returns(74);
			yuubariMock.Setup(s => s.AllSlotInstance).Returns(skilledLookout);
			IShipData yuubari = yuubariMock.Object;

			var kuroshioMock = new Mock<IShipData>();
			kuroshioMock.Setup(s => s.LOSBase).Returns(60);
			kuroshioMock.Setup(s => s.AllSlotInstance).Returns(skilledLookout);
			IShipData kuroshio = kuroshioMock.Object;

			var kagerouMock = new Mock<IShipData>();
			kagerouMock.Setup(s => s.LOSBase).Returns(62);
			kagerouMock.Setup(s => s.AllSlotInstance).Returns(skilledLookout);
			IShipData kagerou = kagerouMock.Object;

			var shiranuiMock = new Mock<IShipData>();
			shiranuiMock.Setup(s => s.LOSBase).Returns(63);
			shiranuiMock.Setup(s => s.AllSlotInstance).Returns(skilledLookout);
			IShipData shiranui = shiranuiMock.Object;

			var naganamiMock = new Mock<IShipData>();
			naganamiMock.Setup(s => s.LOSBase).Returns(66);
			naganamiMock.Setup(s => s.AllSlotInstance).Returns(skilledLookout);
			IShipData naganami = naganamiMock.Object;

			ReadOnlyCollection<IShipData> ships = new ReadOnlyCollection<IShipData>(new[] { nisshin, yuubari, kuroshio, kagerou, shiranui, naganami });
			mockFleet.Setup(f => f.MembersWithoutEscaped).Returns(ships);

			IFleetData fleet = mockFleet.Object;

			double expected1 = 17.68;
			double expected2 = 32.08;
			double expected3 = 46.48;
			double expected4 = 60.88;

			double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
			double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
			double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
			double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

			Assert.Equal(expected1, actual1.RoundDown(2));
			Assert.Equal(expected2, actual2.RoundDown(2));
			Assert.Equal(expected3, actual3.RoundDown(2));
			Assert.Equal(expected4, actual4.RoundDown(2));
		}
	}
}
