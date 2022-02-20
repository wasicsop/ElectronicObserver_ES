using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.TestData;
using ElectronicObserverTypes;
using Xunit;

namespace ElectronicObserverCoreTests;

// https://xunit.net/docs/shared-context
[CollectionDefinition(Name)]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
	public const string Name = "Database collection";

	// This class has no code, and is never created. Its purpose is simply
	// to be the place to apply [CollectionDefinition] and all the
	// ICollectionFixture<> interfaces.
}

public class DatabaseFixture : IDisposable
{
	public Dictionary<ShipId, IShipDataMaster> MasterShips { get; }
	public Dictionary<EquipmentId, IEquipmentDataMaster> MasterEquipment { get; }

	public DatabaseFixture()
	{
		using TestDataContext db = new();

		MasterShips = db.MasterShips
			.Select(s => s.ToMasterShip())
			.ToDictionary(s => (ShipId)s.ShipID);

		MasterEquipment = db.MasterEquipment
			.Select(e => e.ToMasterEquipment())
			.ToDictionary(e => (EquipmentId)e.EquipmentID);
	}

	public void Dispose()
	{
		// ... clean up test data from the database ...
	}
}
