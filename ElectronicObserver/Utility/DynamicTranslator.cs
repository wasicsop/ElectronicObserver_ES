using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DynaJson;

namespace ElectronicObserver.Utility
{
	public class DynamicTranslator
	{
		private readonly string workingDirectory = SoftwareUpdater.TranslationFolder;

		private XDocument shipsXml;
        private XDocument shipTypesXml;
        private XDocument equipmentXml;
        private XDocument equipTypesXml;
        private XDocument operationsXml;
        private XDocument questsXml;
        private XDocument expeditionsXml;

		internal DynamicTranslator()
		{
			CheckUpdate();
		    LoadFile();

		}

        private void LoadFile()
        {
            try
            {
                if (File.Exists(workingDirectory + "\\Ships.xml")) shipsXml = XDocument.Load(workingDirectory + "\\Ships.xml");
                if (File.Exists(workingDirectory + "\\ShipTypes.xml")) shipTypesXml = XDocument.Load(workingDirectory + "\\ShipTypes.xml");
                if (File.Exists(workingDirectory + "\\Equipment.xml")) equipmentXml = XDocument.Load(workingDirectory + "\\Equipment.xml");
                if (File.Exists(workingDirectory + "\\EquipmentTypes.xml")) equipTypesXml = XDocument.Load(workingDirectory + "\\EquipmentTypes.xml");
                if (File.Exists(workingDirectory + "\\Operations.xml")) operationsXml = XDocument.Load(workingDirectory + "\\Operations.xml");
                if (File.Exists(workingDirectory + "\\Quests.xml")) questsXml = XDocument.Load(workingDirectory + "\\Quests.xml");
                if (File.Exists(workingDirectory + "\\Expeditions.xml")) expeditionsXml = XDocument.Load(workingDirectory + "\\Expeditions.xml");
            }
            catch (Exception ex)
            {
                Logger.Add(3, "Could not load translation file: " + ex.Message);
            }
        }

        private void CheckUpdate()
	    {
			if (!Directory.Exists(workingDirectory))
				Directory.CreateDirectory(workingDirectory);

			SoftwareUpdater.CheckVersion();
        }

        private IEnumerable<XElement> GetTranslationList(TranslationType type)
        {
            switch (type)
            {
                case TranslationType.Ships:
                    if (shipsXml != null)
                        return shipsXml.Descendants("Ship");
                    break;
                case TranslationType.ShipTypes:
                    if (shipTypesXml != null)
                        return shipTypesXml.Descendants("Type");
                    break;
                case TranslationType.Equipment:
                    if (equipmentXml != null)
                        return equipmentXml.Descendants("Item");
                    break;
				case TranslationType.EquipmentDesc:
					if (equipmentXml != null)
						return equipmentXml.Descendants("ItemDesc");
					break;
				case TranslationType.EquipmentType:
                    if (equipTypesXml != null)
                        return equipTypesXml.Descendants("Type");
                    break;
                case TranslationType.OperationMaps:
                    if (operationsXml != null)
                        return operationsXml.Descendants("Map");
                    break;
                case TranslationType.OperationSortie:
                    if (operationsXml != null)
                        return operationsXml.Descendants("Sortie");
                    break;
				case TranslationType.OperationMapNodes:
					if (operationsXml != null)
						return operationsXml.Descendants("MapNode");
					break;
				case TranslationType.Quests:
                case TranslationType.QuestTitle:
                case TranslationType.QuestDetail:
                    if (questsXml != null)
                        return questsXml.Descendants("Quest");
                    break;
                case TranslationType.Expeditions:
                case TranslationType.ExpeditionTitle:
                case TranslationType.ExpeditionDetail:
                    if (expeditionsXml != null)
                        return expeditionsXml.Descendants("Expedition");
                    break;
                default:
                    return null;
            }
            return null;
        }

        public string GetTranslation(string jpString, TranslationType type, int id = -1)
        {
	        bool translate = type switch
	        {
		        TranslationType.Ships => !Configuration.Config.UI.JapaneseShipName,
		        TranslationType.ShipTypes => !Configuration.Config.UI.JapaneseShipType,
		        TranslationType.Equipment => !Configuration.Config.UI.JapaneseEquipmentName,
		        TranslationType.EquipmentDesc => !Configuration.Config.UI.JapaneseEquipmentName,
		        TranslationType.EquipmentType => !Configuration.Config.UI.JapaneseEquipmentType,

				TranslationType.Expeditions => !Configuration.Config.UI.DisableOtherTranslations,
				TranslationType.ExpeditionTitle => !Configuration.Config.UI.DisableOtherTranslations,
				TranslationType.ExpeditionDetail => !Configuration.Config.UI.DisableOtherTranslations,
		        TranslationType.Quests => !Configuration.Config.UI.DisableOtherTranslations,
		        TranslationType.QuestTitle => !Configuration.Config.UI.DisableOtherTranslations,
		        TranslationType.QuestDetail => !Configuration.Config.UI.DisableOtherTranslations,
		        TranslationType.OperationMaps => !Configuration.Config.UI.DisableOtherTranslations,
				TranslationType.OperationSortie => !Configuration.Config.UI.DisableOtherTranslations,

		        _ => true
	        };
	        if (!translate) return jpString;
	        try
	        {
		        var translationList = GetTranslationList(type);
		        if (translationList == null) return jpString;
		        var jpChildElement = "JP-Name";
		        var trChildElement = "TR-Name";
		        if (type == TranslationType.QuestDetail || type == TranslationType.ExpeditionDetail)
		        {
			        jpChildElement = "JP-Detail";
			        trChildElement = "TR-Detail";
		        }
		        var translated = jpString;
		        if (GetTranslation(jpString, translationList, jpChildElement, trChildElement, id, ref translated))
			        return translated;
	        }
	        catch (Exception e)
	        {
		        Logger.Add(3, "Can't output translation: " + e.Message);
	        }
	        return jpString;
        }

		private bool GetTranslation(string jpString, IEnumerable<XElement> translationList, string jpChildElement, string trChildElement, int id, ref string translate)
        {
            var foundTranslation = translationList.Where(el =>
            {
                try
                {
                    if (el.Element(jpChildElement).Value.Equals(jpString)) return true;
                    if (el.Attribute("mode") != null)
                    {
                        if (el.Attribute("mode").Value.Equals("suffix"))
                        {
                            var sl = el.Element(jpChildElement).Value.Length;
                            if (jpString.Length > sl)
                            {
                                if (el.Element(jpChildElement).Value.Equals(jpString.Substring(jpString.Length - sl))) return true;
                            }
                        }
                    }
                }
                catch
                {
                    return false;
                }
                return false;
            }
            );

            var foundWrongId = false;
	        foreach (var el in foundTranslation)
            {
                if (el.Attribute("mode") != null && !el.Attribute("mode").Value.Equals("normal"))
                {
                    if (el.Attribute("mode").Value.Equals("suffix")) {
                        try
                        {
                            var t = jpString.Substring(0, jpString.Length - el.Element(jpChildElement).Value.Length);
                            if (GetTranslation(t, translationList, jpChildElement, trChildElement, -1, ref t))
                            {
                                if (el.Attribute("suffixType") != null && el.Attribute("suffixType").Value.Equals("pre")) translate = el.Element(trChildElement).Value + t;
                                else translate = t + el.Element(trChildElement).Value;
                                        return true;
                            }
                        }
                        catch (NullReferenceException)
                        {

                        }
                    }
                    continue;
                }

                try
                {
                    if(id >= 0)
                    {
                        if(!int.TryParse(el.Element("ID").Value, out _))
                        {
                            foundWrongId = true;
                            translate = el.Element(trChildElement).Value;
                        }
                        else
                        {
	                        if (id < 0 || el.Element("ID") == null || Convert.ToInt32(el.Element("ID").Value) != id)
		                        continue;
	                        translate = el.Element(trChildElement).Value;
	                        return true;
                        }
                    }
                    else
                    {
                        translate = el.Element(trChildElement).Value;
                        return true;
                    }
                }
                catch (NullReferenceException)
                {

                }
            }

            return foundWrongId;
        }

	}

	public enum TranslationType
    {
        App,
        Equipment,
		EquipmentDesc,
		EquipmentType,
        Operations,
        Quests,
        Ships,
        ShipTypes,
        OperationMaps,
        OperationSortie,
		OperationMapNodes,
		QuestDetail,
        QuestTitle,
        Expeditions,
        ExpeditionDetail,
        ExpeditionTitle
    }

	internal enum TranslationFile
	{
		Ships,
		ShipTypes,
		Equipment,
		EquipmentTypes,
		Operations,
		Quests,
		Expeditions
	}
}
