using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using AppSettings = ElectronicObserver.Properties.Settings;

namespace ElectronicObserver.Utility
{
	public class DynamicTranslator
    {
        private XDocument _shipsXml;
        private XDocument _shipTypesXml;
        private XDocument _equipmentXml;
        private XDocument _equipTypesXml;
        private XDocument _operationsXml;
        private XDocument _questsXml;
        private XDocument _expeditionsXml;
	    private readonly string _url = AppSettings.Default.EOTranslations.AbsoluteUri + "en-US";

		internal DynamicTranslator()
		{
			CheckForUpdates();

			try
            {
	            if (File.Exists("Translations\\Ships.xml")) _shipsXml = XDocument.Load("Translations\\Ships.xml");
	            if (File.Exists("Translations\\ShipTypes.xml")) _shipTypesXml = XDocument.Load("Translations\\ShipTypes.xml");
	            if (File.Exists("Translations\\Equipment.xml")) _equipmentXml = XDocument.Load("Translations\\Equipment.xml");
	            if (File.Exists("Translations\\EquipmentTypes.xml")) _equipTypesXml = XDocument.Load("Translations\\EquipmentTypes.xml");
	            if (File.Exists("Translations\\Operations.xml")) _operationsXml = XDocument.Load("Translations\\Operations.xml");
	            if (File.Exists("Translations\\Quests.xml")) _questsXml = XDocument.Load("Translations\\Quests.xml");
	            if (File.Exists("Translations\\Expeditions.xml")) _expeditionsXml = XDocument.Load("Translations\\Expeditions.xml");
			}
            catch (Exception ex)
            {
                Logger.Add(3, "Could not load translation file: " + ex.Message);
            }
        }

        private void CheckForUpdates()
        {
	        if (!Directory.Exists("Translations"))
		        Directory.CreateDirectory("Translations");
			XDocument versionManifest = null;

			try
			{
		        var rq = WebRequest.Create(_url + "/VersionManifest.xml");
				using (var resp = rq.GetResponse())
			        versionManifest = XDocument.Load(resp.GetResponseStream());
			}
	        catch (Exception e)
	        {
		        Logger.Add(3, "Failed to check translation updates: " + e.Message);
	        }

	        foreach (TranslationFile filename in Enum.GetValues(typeof(TranslationFile)))
	        {
		        var current = "0.0.0";
				var path = $"Translations\\{filename}.xml";
				if (File.Exists(path))
		        {
					var translationfile = XDocument.Load(path);
			        current = translationfile?.Root.Attribute("Version").Value ?? "0.0.0";
				}
				
				var latest = versionManifest.Root.Element($"{filename}").Attribute("version").Value;
				if (current != latest)
					UpdateTranslation(filename, latest);
			}
		}

	    private void UpdateTranslation(TranslationFile filename, string latestVersion)
	    {
		    var r2 = WebRequest.Create(_url + $"/{filename}.xml");
		    using (var resp = r2.GetResponse())
		    {
			    var doc = XDocument.Load(resp.GetResponseStream());
			    doc.Save($"Translations\\{filename}.xml");
		    }
		    Logger.Add(2, $"Updated {filename} translations to v{latestVersion}.");
		}

        private IEnumerable<XElement> GetTranslationList(TranslationType type)
        {
            switch (type)
            {
                case TranslationType.Ships:
                    if (_shipsXml != null)
                        return _shipsXml.Descendants("Ship");
                    break;
                case TranslationType.ShipTypes:
                    if (_shipTypesXml != null)
                        return _shipTypesXml.Descendants("Type");
                    break;
                case TranslationType.Equipment:
                    if (_equipmentXml != null)
                        return _equipmentXml.Descendants("Item");
                    break;
				case TranslationType.EquipmentDesc:
					if (_equipmentXml != null)
						return _equipmentXml.Descendants("ItemDesc");
					break;
				case TranslationType.EquipmentType:
                    if (_equipTypesXml != null)
                        return _equipTypesXml.Descendants("Type");
                    break;
                case TranslationType.OperationMaps:
                    if (_operationsXml != null)
                        return _operationsXml.Descendants("Map");
                    break;
                case TranslationType.OperationSortie:
                    if (_operationsXml != null)
                        return _operationsXml.Descendants("Sortie");
                    break;
				case TranslationType.OperationMapNodes:
					if (_operationsXml != null)
						return _operationsXml.Descendants("MapNode");
					break;
				case TranslationType.Quests:
                case TranslationType.QuestTitle:
                case TranslationType.QuestDetail:
                    if (_questsXml != null)
                        return _questsXml.Descendants("Quest");
                    break;
                case TranslationType.Expeditions:
                case TranslationType.ExpeditionTitle:
                case TranslationType.ExpeditionDetail:
                    if (_expeditionsXml != null)
                        return _expeditionsXml.Descendants("Expedition");
                    break;
                default:
                    return null;
            }
            return null;
        }

        public string GetTranslation(string jpString, TranslationType type, int id = -1)
        {
			bool translate = true;
			switch (type)
			{
				case TranslationType.Ships: translate = !Configuration.Config.UI.JapaneseShipName; break;
				case TranslationType.ShipTypes: translate = !Configuration.Config.UI.JapaneseShipType; break;
				case TranslationType.Equipment: translate = !Configuration.Config.UI.JapaneseEquipmentName; break;
				case TranslationType.EquipmentDesc: translate = !Configuration.Config.UI.JapaneseEquipmentName; break;
				case TranslationType.EquipmentType: translate = !Configuration.Config.UI.JapaneseEquipmentType; break;
			}
			if (translate)
			{
				try
				{
					IEnumerable<XElement> translationList = GetTranslationList(type);
					if (translationList == null) return jpString;
					string jpChildElement = "JP-Name";
					string trChildElement = "TR-Name";
					if (type == TranslationType.QuestDetail || type == TranslationType.ExpeditionDetail)
					{
						jpChildElement = "JP-Detail";
						trChildElement = "TR-Detail";
					}
					string translated = jpString;
					if (GetTranslation(jpString, translationList, jpChildElement, trChildElement, id, ref translated))
						return translated;
				}
				catch (Exception e)
				{
					Logger.Add(3, "Can't output translation: " + e.Message);
				}
				return jpString;
			}
			else { return jpString; }
        }
		
		public string GetMapNodes(int mapAreaId, int mapInfoId, int mapNodeId, TranslationType type, int id = -1)
		{
			try
			{
				IEnumerable<XElement> translationList = GetTranslationList(type);

				if (translationList == null)
					return mapNodeId.ToString();

				string idChildElement = "Node";
				string labelChildElement = "Label";
				
				string nodeinfo = mapAreaId.ToString("D3") + mapInfoId.ToString("D3") + mapNodeId.ToString("D3");
				string converted = nodeinfo;
				if (GetTranslation(nodeinfo, translationList, idChildElement, labelChildElement, id, ref converted))
					return converted;
			}
			catch (Exception e)
			{
				Logger.Add(3, "Can't output translation: " + e.Message);
			}

			return mapNodeId.ToString();
		}

		private bool GetTranslation(string jpString, IEnumerable<XElement> translationList, string jpChildElement, string trChildElement, int id, ref string translate)
        {
            IEnumerable<XElement> foundTranslation = translationList.Where(el =>
            {
                try
                {
                    if (el.Element(jpChildElement).Value.Equals(jpString)) return true;
                    if (el.Attribute("mode") != null)
                    {
                        if (el.Attribute("mode").Value.Equals("suffix"))
                        {
                            int sl = el.Element(jpChildElement).Value.Length;
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

            bool foundWrongId = false;
	        foreach (XElement el in foundTranslation)
            {
                if (el.Attribute("mode") != null && !el.Attribute("mode").Value.Equals("normal"))
                {
                    if (el.Attribute("mode").Value.Equals("suffix")) {
                        try
                        {
                            string t = jpString.Substring(0, jpString.Length - el.Element(jpChildElement).Value.Length);
                            if (GetTranslation(t, translationList, jpChildElement, trChildElement, -1, ref t))
                            {
                                if ((el.Attribute("suffixType") != null) && el.Attribute("suffixType").Value.Equals("pre")) translate = el.Element(trChildElement).Value + t;
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
                        if(!Int32.TryParse(el.Element("ID").Value, out _))
                        {
                            foundWrongId = true;
                            translate = el.Element(trChildElement).Value;
                        }
                        else
                        {
                            if (id >= 0 && el.Element("ID") != null && Convert.ToInt32(el.Element("ID").Value) == id)
                            {
                                translate = el.Element(trChildElement).Value;
                                return true;
                            }
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

            if(foundWrongId)
            {
                return true;
            }
            return false;
        }

	    private enum TranslationFile
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
}
