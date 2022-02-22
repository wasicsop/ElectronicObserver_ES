using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class ShellingSupportTests
{
	private DatabaseFixture Db { get; }
	private int Precision => 3;

	public ShellingSupportTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void ShellingSupportTest1()
	{
		ShipDataMock ise = new ShipDataMock(Db.MasterShips[ShipId.IseKaiNi])
		{
			Level = 99,
			Condition = 85,
			FirepowerFit = 6,
			AllSlotInstance = new List<EquipmentDataMock>
			{
				new(Db.MasterEquipment[EquipmentId.MainGunLarge_41cmTripleGunKai2]) { Level = 6 },
				new(Db.MasterEquipment[EquipmentId.MainGunLarge_41cmTripleGunKai2]) { Level = 6 },
				new(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
				new(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
				new(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
			}.Cast<IEquipmentData>().ToList(),
		};

		Assert.Equal(153, ise.GetShellingSupportDamage());
		Assert.Equal(159.6, ise.GetShellingSupportAccuracy(), Precision);
	}

	[Fact]
	public void ShellingSupportTest2()
	{
		ShipDataMock taihou = new ShipDataMock(Db.MasterShips[ShipId.TaihouKai])
		{
			Level = 99,
			Condition = 85,
			AllSlotInstance = new List<EquipmentDataMock>
			{
				new(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Suisei_EgusaSquadron]),
				new(Db.MasterEquipment[EquipmentId.RadarLarge_Type32SurfaceRADAR]),
				new(Db.MasterEquipment[EquipmentId.RadarLarge_Type32SurfaceRADAR]),
				new(Db.MasterEquipment[EquipmentId.RadarLarge_Type32SurfaceRADAR]),
			}.Cast<IEquipmentData>().ToList(),
		};

		Assert.Equal(166, taihou.GetShellingSupportDamage());
		Assert.Equal(136.8, taihou.GetShellingSupportAccuracy(), Precision);
	}
}
