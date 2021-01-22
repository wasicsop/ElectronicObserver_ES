using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data
{
	public class ShipDataCustom : IShipData
	{
		public int MasterID { get; set; }
		public int SortID { get; set; }
		public int ShipID { get; set; }
		public int Level { get; set; }
		public int ExpTotal { get; set; }
		public int ExpNext { get; set; }
		public int HPCurrent { get; set; }
		public int HPMax { get; set; }
		public int Speed { get; set; }
		public int Range { get; set; }
		public IList<int> Slot { get; set; }
		public IList<int> SlotMaster { get; set; }
		public IList<IEquipmentData> SlotInstance { get; set; }
		public IList<IEquipmentDataMaster> SlotInstanceMaster { get; set; }
		public int ExpansionSlot { get; set; }
		public int ExpansionSlotMaster { get; set; }
		public IEquipmentData ExpansionSlotInstance { get; set; }
		public IEquipmentDataMaster ExpansionSlotInstanceMaster { get; set; }
		public IList<int> AllSlot { get; set; }
		public IList<int> AllSlotMaster { get; set; }
		public IList<int> AllSlotMasterReplay { get; set; }
		public IList<IEquipmentData> AllSlotInstance { get; set; }
		public IList<IEquipmentDataMaster> AllSlotInstanceMaster { get; set; }
		public IList<int> Aircraft { get; set; }
		public int AircraftTotal { get; set; }
		public int Fuel { get; set; }
		public int Ammo { get; set; }
		public int SlotSize { get; set; }
		public int RepairTime { get; set; }
		public int RepairSteel { get; set; }
		public int RepairFuel { get; set; }
		public int Condition { get; set; }
		public int[] Kyouka { get; set; }
		public int FirepowerModernized { get; set; }
		public int TorpedoModernized { get; set; }
		public int AAModernized { get; set; }
		public int ArmorModernized { get; set; }
		public int LuckModernized { get; set; }
		public int HPMaxModernized { get; set; }
		public int ASWModernized { get; set; }
		public int FirepowerRemain { get; set; }
		public int TorpedoRemain { get; set; }
		public int AARemain { get; set; }
		public int ArmorRemain { get; set; }
		public int LuckRemain { get; set; }
		public int HPMaxRemain { get; set; }
		public int ASWRemain { get; set; }
		public int FirepowerTotal { get; set; }
		public int TorpedoTotal { get; set; }
		public int AATotal { get; set; }
		public int ArmorTotal { get; set; }
		public int EvasionTotal { get; set; }
		public int ASWTotal { get; set; }
		public int LOSTotal { get; set; }
		public int LuckTotal { get; set; }
		public int BomberTotal { get; set; }
		public int AccuracyTotal { get; set; }
		public int FirepowerBase { get; set; }
		public int TorpedoBase { get; set; }
		public int AABase { get; set; }
		public int ArmorBase { get; set; }
		public int EvasionBase { get; set; }
		public int ASWBase { get; set; }
		public int LOSBase { get; set; }
		public int LuckBase { get; set; }
		public int EvasionMax { get; set; }
		public int ASWMax { get; set; }
		public int LOSMax { get; set; }
		public bool IsLocked { get; set; }
		public bool IsLockedByEquipment { get; set; }
		public int SallyArea { get; set; }
		public IShipDataMaster MasterShip { get; set; }
		public int RepairingDockID { get; set; }
		public int Fleet { get; set; }
		public string FleetWithIndex { get; set; }
		public bool IsMarried { get; set; }
		public int ExpNextRemodel { get; set; }
		public string Name { get; set; }
		public string NameWithLevel { get; set; }
		public double HPRate { get; set; }
		public int FuelMax { get; set; }
		public int AmmoMax { get; set; }
		public double FuelRate { get; set; }
		public double AmmoRate { get; set; }
		public int SupplyFuel { get; set; }
		public int SupplyAmmo { get; set; }
		public IList<double> AircraftRate { get; set; }
		public double AircraftTotalRate { get; set; }
		public bool IsExpansionSlotAvailable { get; set; }
		public int AirBattlePower { get; set; }
		public IList<int> AirBattlePowers { get; set; }
		public int ShellingPower { get; set; }
		public int AircraftPower { get; set; }
		public int AntiSubmarinePower { get; set; }
		public int TorpedoPower { get; set; }
		public int NightBattlePower { get; set; }
		public bool CanAttackSubmarine { get; set; }
		public bool CanOpeningASW { get; set; }
		public bool CanNoSonarOpeningAsw { get; set; }
		public bool CanAttackAtNight { get; set; }
		public int DamageControlID { get; set; }
		public int ID { get; set; }
		public Dictionary<string, string> RequestData { get; set; }
		public dynamic RawData { get; set; }
		public bool IsAvailable { get; set; }
		public void LoadFromResponse(string apiname, dynamic data)
		{
			throw new System.NotImplementedException();
		}

		public void LoadFromRequest(string apiname, Dictionary<string, string> data)
		{
			throw new System.NotImplementedException();
		}
	}
}