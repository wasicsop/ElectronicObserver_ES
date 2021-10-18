namespace ElectronicObserverTypes.Mocks;

public class MockEquipmentData : IEquipmentData
{
	public int MasterID { get; set; }
	public int EquipmentID { get; set; }
	public EquipmentId EquipmentId { get; set; }
	public bool IsLocked { get; set; }
	public int Level { get; set; }
	public int AircraftLevel { get; set; }
	public IEquipmentDataMaster MasterEquipment { get; set; }
	public string Name { get; set; }
	public string NameWithLevel { get; set; }
	public bool IsRelocated { get; set; }
	public int ID { get; set; }
	public dynamic RawData { get; set; }
	public bool IsAvailable { get; set; }
	public void LoadFromResponse(string apiname, dynamic data)
	{
		throw new System.NotImplementedException();
	}
}
