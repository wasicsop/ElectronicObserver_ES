using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class AirPowerTests
{
	private DatabaseFixture Db { get; }

	public AirPowerTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "AS patrol")]
	public void AirPowerTest1()
	{
		ShipDataMock hyuuga = new(Db.MasterShips[ShipId.HyuugaKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.ASPatrol_Type1FighterHayabusaModelII_20thSquadron])
				{
					Level = 10,
					AircraftLevel = 7,
				},
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				hyuuga,
			}),
		};

		Assert.Equal(33, Calculator.GetAirSuperiority(fleet));

		hyuuga.SlotInstance = new List<IEquipmentData?>
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.ASPatrol_Type3CommandLiaisonAircraftKai])
			{
				Level = 10,
				AircraftLevel = 7,
			},
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.ASPatrol_Type3CommandLiaisonAircraft_ASW])
			{
				Level = 10,
				AircraftLevel = 7,
			},
		};

		Assert.Equal(0, Calculator.GetAirSuperiority(fleet));
	}

	[Fact]
	public void AirPowerTest2()
	{
		ShipDataMock taihou = new(Db.MasterShips[ShipId.TaihouKai])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Suisei_EgusaSquadron])
				{
					AircraftLevel = 6,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TenzanModel12_MurataSquadron])
				{
					Level = 2,
					AircraftLevel = 7,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TenzanModel12_MurataSquadron])
				{
					AircraftLevel = 7,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedFighter_PrototypeJinpuu])
				{
					AircraftLevel = 7,
				},
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				taihou,
			}),
		};

		Assert.Equal(85, Calculator.GetAirSuperiority(fleet));
	}
}
