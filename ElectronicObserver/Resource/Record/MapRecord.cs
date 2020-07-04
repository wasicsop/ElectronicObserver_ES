using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElectronicObserver.Resource.Record
{

	[DebuggerDisplay("{Record.Count} Records")]
	public class MapRecord : RecordBase
	{

		[DebuggerDisplay("[{Date}] : {MapAreaId} - {MapId}")]
		public sealed class MapRecordElement : RecordElementBase
		{
			public int MapAreaId { get; set; }

			public int MapId { get; set; }

			public int DebuffCount { get; set; }

			public MapRecordElement()
			{

			}

			public MapRecordElement(string line)
				: this()
			{
				LoadLine(line);
			}

			public MapRecordElement(int mapAreaId, int mapId, int debuffCount)
			{
				MapAreaId = mapAreaId;
				MapId = mapId;
				DebuffCount = debuffCount;
			}


			public override void LoadLine(string line)
			{
				string[] elem = CsvHelper.ParseCsvLine(line).ToArray();
				if (elem.Length < 15) throw new ArgumentException("要素数が少なすぎます。");

				MapAreaId = int.Parse(elem[0]);
				MapId = int.Parse(elem[1]);
				DebuffCount = int.Parse(elem[2]);

			}

			public override string SaveLine()
			{
				return string.Join(",",
					MapAreaId,
					MapId,
					DebuffCount);
			}
		}



		public List<MapRecordElement> Record { get; private set; }
		private int LastSavedCount;


		public MapRecord()
			: base()
		{
			Record = new List<MapRecordElement>();
		}

		public override void RegisterEvents()
		{
			// nop
		}


		public MapRecordElement this[int i]
		{
			get { return Record[i]; }
			set { Record[i] = value; }
		}

		public void Add(int shipID, int itemID, int equipmentID)
		{

			Record.Add(new MapRecordElement(shipID, itemID, equipmentID));
		}


		protected override void LoadLine(string line)
		{
			Record.Add(new MapRecordElement(line));
		}

		protected override string SaveLinesAll()
		{
			var sb = new StringBuilder();
			foreach (var elem in Record)
			{
				sb.AppendLine(elem.SaveLine());
			}
			return sb.ToString();
		}

		protected override string SaveLinesPartial()
		{
			var sb = new StringBuilder();
			foreach (var elem in Record.Skip(LastSavedCount))
			{
				sb.AppendLine(elem.SaveLine());
			}
			return sb.ToString();
		}

		protected override void UpdateLastSavedIndex()
		{
			LastSavedCount = Record.Count;
		}

		public override bool NeedToSave => LastSavedCount < Record.Count;

		public override bool SupportsPartialSave => true;


		protected override void ClearRecord()
		{
			Record.Clear();
			LastSavedCount = 0;
		}


		public override string RecordHeader => "MapAreaId,MapId,DebuffCount";

		public override string FileName => "MapRecord.csv";
	}

}
