using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronicObserver.Data
{
	public class TsunDbShipData : TsunDbEntity
	{
		protected override string Url => throw new NotImplementedException();

		#region Json properties
		[JsonProperty("id")]
		public int Id;

		[JsonProperty("name")]
		public string Name;

		[JsonProperty("shiplock")]
		public int Shiplock;

		[JsonProperty("level")]
		public int Level;

		[JsonProperty("type")]
		public int Type;

		[JsonProperty("speed")]
		public int Speed;

		[JsonProperty("flee")]
		public int Flee;

		[JsonProperty("equip")]
		public int[] Equip;

		[JsonProperty("exslot")]
		public int Exslot;

		public TsunDbShipData(IShipData ship)
		{
			this.Id = ship.ShipID;
			this.Name = ship.MasterShip.Name;
			this.Shiplock = ship.SallyArea;
			this.Level = ship.Level;
			this.Type = ship.MasterShip.ShipTypeInstance.TypeID;
			this.Speed = ship.Speed;
			this.Equip = ship.SlotInstanceMaster.Select(eq => eq != null ? eq.EquipmentID : -1).ToArray();
			this.Exslot = ship.IsExpansionSlotAvailable ? ship.ExpansionSlotInstanceMaster.EquipmentID : -1;
		}
		#endregion
	}
}
