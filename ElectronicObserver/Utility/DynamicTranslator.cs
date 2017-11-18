using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
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
        private XDocument _versionManifest;
        private string _shipsVersion;
        private string _shipTypesVersion;
        private string _equipmentVersion;
        private string _equipTypesVersion;
        private string _operationsVersion;
        private string _questsVersion;
        private string _expeditionsVersion;
	    private readonly string _locale = Thread.CurrentThread.CurrentCulture.Name;

		internal DynamicTranslator()
        {
            try
            {
                if (Thread.CurrentThread.CurrentCulture.Name != "ja-JP")
                {
                    if (File.Exists("Translations\\Ships.xml")) _shipsXml = XDocument.Load("Translations\\Ships.xml");
                    if (File.Exists("Translations\\ShipTypes.xml")) _shipTypesXml = XDocument.Load("Translations\\ShipTypes.xml");
                    if (File.Exists("Translations\\Equipment.xml")) _equipmentXml = XDocument.Load("Translations\\Equipment.xml");
                    if (File.Exists("Translations\\EquipmentTypes.xml")) _equipTypesXml = XDocument.Load("Translations\\EquipmentTypes.xml");
                    if (File.Exists("Translations\\Operations.xml")) _operationsXml = XDocument.Load("Translations\\Operations.xml");
                    if (File.Exists("Translations\\Quests.xml")) _questsXml = XDocument.Load("Translations\\Quests.xml");
                    if (File.Exists("Translations\\Expeditions.xml")) _expeditionsXml = XDocument.Load("Translations\\Expeditions.xml");
				}
            }
            catch (Exception ex)
            {
                Logger.Add(3, "Could not load translation file: " + ex.Message);
            }
            GetVersions();
            CheckForUpdates();
        }

        private void GetVersions()
        {
			_shipsVersion = LoadXml(_shipsXml);
			_shipTypesVersion = LoadXml(_shipTypesXml);
			_equipmentVersion = LoadXml(_equipmentXml);
			_equipTypesVersion = LoadXml(_equipTypesXml);
			_operationsVersion = LoadXml(_operationsXml);
			_questsVersion = LoadXml(_questsXml);
			_expeditionsVersion = LoadXml(_expeditionsXml);
		}

		private string LoadXml(XDocument xml)
		{
			if (xml == null)
				return "0.0.0";
			else
			{
				return xml.Root.Attribute("Version").Value;
			}
		}

        private void CheckForUpdates()
        {
            Directory.CreateDirectory("Translations");
			string newShipVer = _shipsVersion;
			string newShipTypeVer = _shipTypesVersion;
			string newEquipVer = _equipmentVersion;
			string newEquipTypeVer = _equipTypesVersion;
			string newOperationVer = _operationsVersion;
			string newQuestVer = _questsVersion;
			string newExpedVer = _expeditionsVersion;
            if (_locale != "ja-JP")
            {
				try
				{
					WebRequest rq = WebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + _locale + "/VersionManifest.xml");
					using (WebResponse resp = rq.GetResponse())
					{
						Stream responseStream = resp.GetResponseStream();
						_versionManifest = XDocument.Load(responseStream);
					}
					newShipVer = _versionManifest.Root.Element("Ships").Attribute("version").Value;
					newShipTypeVer = _versionManifest.Root.Element("ShipTypes").Attribute("version").Value;
					newEquipVer = _versionManifest.Root.Element("Equipment").Attribute("version").Value;
					newEquipTypeVer = _versionManifest.Root.Element("EquipmentTypes").Attribute("version").Value;
					newOperationVer = _versionManifest.Root.Element("Operations").Attribute("version").Value;
					newQuestVer = _versionManifest.Root.Element("Quests").Attribute("version").Value;
					newExpedVer = _versionManifest.Root.Element("Expeditions").Attribute("version").Value;
				}
				catch (Exception e)
				{
					Logger.Add(3, "Failed to check translation updates: " + e.Message);
				}
				if (newShipVer != _shipsVersion)
                {
					_shipsXml = null;
                    WebRequest r2 = WebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + _locale + "/Ships.xml");
                    using (WebResponse resp = r2.GetResponse())
					{
						Stream responseStream = resp.GetResponseStream();
						_shipsXml = XDocument.Load(responseStream);
						_shipsXml.Save("Translations\\Ships.xml");
                    }
                    Logger.Add(2, "Updated ship translations to v" + newShipVer + ".");
                }
                if (newShipTypeVer != _shipTypesVersion)
                {
                    _shipTypesXml = null;
                    WebRequest r2 = WebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + _locale + "/ShipTypes.xml");
                    using (WebResponse resp = r2.GetResponse())
                    {
                        Stream responseStream = resp.GetResponseStream();
						_shipTypesXml = XDocument.Load(responseStream);
                        _shipTypesXml.Save("Translations\\ShipTypes.xml");
                    }
                    Logger.Add(2, "Updated ship type translations to v" + newShipTypeVer + ".");
                }
                if (newEquipVer != _equipmentVersion)
                {
                    _equipmentXml = null;
                    WebRequest r2 = WebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + _locale + "/Equipment.xml");
                    using (WebResponse resp = r2.GetResponse())
                    {
                        Stream responseStream = resp.GetResponseStream();
						_equipmentXml = XDocument.Load(responseStream);
                        _equipmentXml.Save("Translations\\Equipment.xml");
                    }
                    Logger.Add(2, "Updated equipment translations to v" + newEquipVer + ".");
                }
                if (newEquipTypeVer != _equipTypesVersion)
                {
                    _equipTypesXml = null;
                    WebRequest r2 = WebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + _locale + "/EquipmentTypes.xml");
                    using (WebResponse resp = r2.GetResponse())
                    {
                        Stream responseStream = resp.GetResponseStream();
						_equipTypesXml = XDocument.Load(responseStream);
                        _equipTypesXml.Save("Translations\\EquipmentTypes.xml");
                    }
                    Logger.Add(2, "Updated equipment type translations to v" + newEquipTypeVer + ".");
                }
                if (newOperationVer != _operationsVersion)
                {
                    _operationsXml = null;
                    WebRequest r2 = WebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + _locale + "/Operations.xml");
                    using (WebResponse resp = r2.GetResponse())
                    {
                        Stream responseStream = resp.GetResponseStream();
						_operationsXml = XDocument.Load(responseStream);
                        _operationsXml.Save("Translations\\Operations.xml");
                    }
                    Logger.Add(2, "Updated operation translations to v" + newOperationVer + ".");
                }
                if (newQuestVer != _questsVersion)
                {
                    _questsXml = null;
                    WebRequest r2 = WebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + _locale + "/Quests.xml");
                    using (WebResponse resp = r2.GetResponse())
                    {
                        Stream responseStream = resp.GetResponseStream();
						_questsXml = XDocument.Load(responseStream);
                        _questsXml.Save("Translations\\Quests.xml");
                    }
                    Logger.Add(2, "Updated quest translations to v" + newQuestVer + ".");
                }
                if (newExpedVer != _expeditionsVersion)
                {
                    _expeditionsXml = null;
                    WebRequest r2 = WebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + _locale + "/Expeditions.xml");
                    using (WebResponse resp = r2.GetResponse())
                    {
                        Stream responseStream = resp.GetResponseStream();
						_expeditionsXml = XDocument.Load(responseStream);
                        _expeditionsXml.Save("Translations\\Expeditions.xml");
                    }
                    Logger.Add(2, "Updated expedition translations to v" + newExpedVer + ".");
                }
            }
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
