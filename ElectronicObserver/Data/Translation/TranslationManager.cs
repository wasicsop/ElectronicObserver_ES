using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicObserver.Data.Translation
{
	public class TranslationManager
	{
		public static string WorkingFolder = SoftwareUpdater.AppDataFolder + "\\Translations";
		public DestinationData Destination { get; private set; }
		public QuestTranslationData Quest { get; private set; }
		public EquipmentTranslationData Equipment { get; private set; }
		public MissionTranslationData Mission { get; private set; }
		public ShipTranslationData Ship { get; private set; }
		public OperationData Operation { get; private set; }

		public TranslationManager()
		{
			Initialize();
		}

		public void Initialize()
		{
			Destination = new DestinationData();
			Equipment = new EquipmentTranslationData();
			Mission = new MissionTranslationData();
			Operation = new OperationData();
			Quest = new QuestTranslationData();
			Ship = new ShipTranslationData();
		}


		public enum TranslationFile
		{
			destination,
			equipment,
			expedition,
			operation,
			quest,
			ship
		}
	}
}
