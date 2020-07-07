using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicObserver.Data.Translation
{
	public class TranslationManager : DataStorage
	{
		public DestinationData Destination { get; private set; }

		public TranslationManager()
		{
			Initialize();
		}

		public override void Initialize()
		{
			Destination = new DestinationData();
		}
	}
}
