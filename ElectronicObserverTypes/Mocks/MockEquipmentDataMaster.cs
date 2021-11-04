using System.Collections.Generic;

namespace ElectronicObserverTypes.Mocks;

public class MockEquipmentDataMaster : IEquipmentDataMaster
{
	public bool IsSurfaceRadar { get; set; }
	public bool IsSonar { get; set; }
	public bool IsDepthCharge { get; set; }
	public bool IsDepthChargeProjector { get; set; }
	public bool IsNightAviationPersonnel { get; set; }
	public bool IsHightAltitudeFighter { get; set; }
	public bool IsAARocketLauncher { get; set; }
	public int ID { get; set; }
	public EquipmentId EquipmentId { get; set; }
	public dynamic RawData { get; set; }
	public bool IsAvailable { get; set; }
	public void LoadFromResponse(string apiname, dynamic data)
	{
		throw new System.NotImplementedException();
	}

	public int EquipmentID { get; set; }
	public int AlbumNo { get; set; }
	public string Name { get; set; }
	public string NameEN { get; set; }
	public bool IsTranslated { get; set; }
	public IList<int> EquipmentType { get; set; }
	public int Armor { get; set; }
	public int Firepower { get; set; }
	public int Torpedo { get; set; }
	public int Bomber { get; set; }
	public int AA { get; set; }
	public int ASW { get; set; }
	public int Accuracy { get; set; }
	public int Evasion { get; set; }
	public int LOS { get; set; }
	public int Luck { get; set; }
	public int Range { get; set; }
	public int Rarity { get; set; }
	public IList<int> Material { get; set; }
	public string Message { get; set; }
	public int AircraftCost { get; set; }
	public int AircraftDistance { get; set; }
	public bool IsAbyssalEquipment { get; set; }
	public bool IsListedInAlbum { get; set; }
	public int CardType { get; set; }
	public EquipmentTypes CategoryType { get; set; }
	public IEquipmentType CategoryTypeInstance { get; set; }
	public int IconType { get; set; }
	public EquipmentIconType IconTypeTyped { get; set; }
	public IEnumerable<int> EquippableShipsAtExpansion { get; set; }
	public bool IsGun { get; set; }
	public bool IsMainGun { get; set; }
	public bool IsSecondaryGun { get; set; }
	public bool IsTorpedo { get; set; }
	public bool IsLateModelTorpedo { get; set; }
	public bool IsHighAngleGun { get; set; }
	public bool IsHighAngleGunWithAADirector { get; set; }
	public bool IsConcentratedAAGun { get; set; }
	public bool IsAircraft { get; set; }
	public bool IsCombatAircraft { get; set; }
	public bool IsReconAircraft { get; set; }
	public bool IsAntiSubmarineAircraft { get; set; }
	public bool IsNightAircraft { get; set; }
	public bool IsNightFighter { get; set; }
	public bool IsNightAttacker { get; set; }
	public bool IsSwordfish { get; set; }
	public bool IsRadar { get; set; }
	public bool IsAirRadar { get; set; }
}
