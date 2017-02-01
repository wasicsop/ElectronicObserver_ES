using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using AppSettings = ElectronicObserver.Properties.Settings;

namespace ElectronicObserver.Utility
{
    public class DynamicTranslator
    {
        private XDocument shipsXml;
        private XDocument shipTypesXml;
        private XDocument equipmentXml;
        private XDocument equipTypesXML;
        private XDocument operationsXml;
        private XDocument questsXml;
        private XDocument expeditionsXml;
        private XDocument versionManifest;
        private string shipsVersion;
        private string shipTypesVersion;
        private string equipmentVersion;
        private string equipTypesVersion;
        private string operationsVersion;
        private string questsVersion;
        private string expeditionsVersion;
		string locale = Thread.CurrentThread.CurrentCulture.Name;

		internal DynamicTranslator()
        {
            try
            {
                if (Thread.CurrentThread.CurrentCulture.Name != "ja-JP")
                {
                    if (File.Exists("Translations\\Ships.xml")) this.shipsXml = XDocument.Load("Translations\\Ships.xml");
                    if (File.Exists("Translations\\ShipTypes.xml")) this.shipTypesXml = XDocument.Load("Translations\\ShipTypes.xml");
                    if (File.Exists("Translations\\Equipment.xml")) this.equipmentXml = XDocument.Load("Translations\\Equipment.xml");
                    if (File.Exists("Translations\\EquipmentTypes.xml")) this.equipTypesXML = XDocument.Load("Translations\\EquipmentTypes.xml");
                    if (File.Exists("Translations\\Operations.xml")) this.operationsXml = XDocument.Load("Translations\\Operations.xml");
                    if (File.Exists("Translations\\Quests.xml")) this.questsXml = XDocument.Load("Translations\\Quests.xml");
                    if (File.Exists("Translations\\Expeditions.xml")) this.expeditionsXml = XDocument.Load("Translations\\Expeditions.xml");
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
			this.shipsVersion = LoadXml(this.shipsXml);
			this.shipTypesVersion = LoadXml(this.shipTypesXml);
			this.equipmentVersion = LoadXml(this.equipmentXml);
			this.equipTypesVersion = LoadXml(this.equipTypesXML);
			this.operationsVersion = LoadXml(this.operationsXml);
			this.questsVersion = LoadXml(this.questsXml);
			this.expeditionsVersion = LoadXml(this.expeditionsXml);
		}

		private string LoadXml(XDocument xml)
		{
			if (xml == null)
				return "0.0.0";
			else
			{
				var element = xml.Root.Attribute("Version");
				return element.Value;
			}
		}

        private void CheckForUpdates()
        {
            Directory.CreateDirectory("Translations");
			string newShipVer = shipsVersion;
			string newShipTypeVer = shipTypesVersion;
			string newEquipVer = equipmentVersion;
			string newEquipTypeVer = equipTypesVersion;
			string newOperationVer = operationsVersion;
			string newQuestVer = questsVersion;
			string newExpedVer = expeditionsVersion;
            if (locale != "ja-JP")
            {
				try
				{
					WebRequest rq = HttpWebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + locale + "/VersionManifest.xml");
					using (WebResponse resp = rq.GetResponse())
					{
						Stream responseStream = resp.GetResponseStream();
						this.versionManifest = XDocument.Load(responseStream);
					}
					newShipVer = versionManifest.Root.Element("Ships").Attribute("version").Value;
					newShipTypeVer = versionManifest.Root.Element("ShipTypes").Attribute("version").Value;
					newEquipVer = versionManifest.Root.Element("Equipment").Attribute("version").Value;
					newEquipTypeVer = versionManifest.Root.Element("EquipmentTypes").Attribute("version").Value;
					newOperationVer = versionManifest.Root.Element("Operations").Attribute("version").Value;
					newQuestVer = versionManifest.Root.Element("Quests").Attribute("version").Value;
					newExpedVer = versionManifest.Root.Element("Expeditions").Attribute("version").Value;
				}
				catch (Exception e)
				{
					Logger.Add(3, "Failed to check translation updates: " + e.Message);
				}
				if (newShipVer != shipsVersion)
                {
					shipsXml = null;
                    WebRequest r2 = HttpWebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + locale + "/Ships.xml");
                    using (WebResponse resp = r2.GetResponse())
					{
						Stream responseStream = resp.GetResponseStream();
                        this.shipsXml = XDocument.Load(responseStream);
						shipsXml.Save("Translations\\Ships.xml");
                    }
                    Logger.Add(2, "Updated ship translations to version " + newShipVer + ".");
                }
                if (newShipTypeVer != shipTypesVersion)
                {
                    shipTypesXml = null;
                    WebRequest r2 = HttpWebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + locale + "/ShipTypes.xml");
                    using (WebResponse resp = r2.GetResponse())
                    {
                        Stream responseStream = resp.GetResponseStream();
                        this.shipTypesXml = XDocument.Load(responseStream);
                        shipTypesXml.Save("Translations\\ShipTypes.xml");
                    }
                    Logger.Add(2, "Updated ship type translations to version " + newShipTypeVer + ".");
                }
                if (newEquipVer != equipmentVersion)
                {
                    equipmentXml = null;
                    WebRequest r2 = HttpWebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + locale + "/Equipment.xml");
                    using (WebResponse resp = r2.GetResponse())
                    {
                        Stream responseStream = resp.GetResponseStream();
                        this.equipmentXml = XDocument.Load(responseStream);
                        equipmentXml.Save("Translations\\Equipment.xml");
                    }
                    Logger.Add(2, "Updated equipment translations to version " + newEquipVer + ".");
                }
                if (newEquipTypeVer != equipTypesVersion)
                {
                    equipTypesXML = null;
                    WebRequest r2 = HttpWebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + locale + "/EquipmentTypes.xml");
                    using (WebResponse resp = r2.GetResponse())
                    {
                        Stream responseStream = resp.GetResponseStream();
                        this.equipTypesXML = XDocument.Load(responseStream);
                        equipTypesXML.Save("Translations\\EquipmentTypes.xml");
                    }
                    Logger.Add(2, "Updated equipment type translations to version " + newEquipTypeVer + ".");
                }
                if (newOperationVer != operationsVersion)
                {
                    operationsXml = null;
                    WebRequest r2 = HttpWebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + locale + "/Operations.xml");
                    using (WebResponse resp = r2.GetResponse())
                    {
                        Stream responseStream = resp.GetResponseStream();
                        this.operationsXml = XDocument.Load(responseStream);
                        operationsXml.Save("Translations\\Operations.xml");
                    }
                    Logger.Add(2, "Updated operation translations to version " + newOperationVer + ".");
                }
                if (newQuestVer != questsVersion)
                {
                    questsXml = null;
                    WebRequest r2 = HttpWebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + locale + "/Quests.xml");
                    using (WebResponse resp = r2.GetResponse())
                    {
                        Stream responseStream = resp.GetResponseStream();
                        this.questsXml = XDocument.Load(responseStream);
                        questsXml.Save("Translations\\Quests.xml");
                    }
                    Logger.Add(2, "Updated quest translations to version " + newQuestVer + ".");
                }
                if (newExpedVer != expeditionsVersion)
                {
                    expeditionsXml = null;
                    WebRequest r2 = HttpWebRequest.Create(AppSettings.Default.EOTranslations.AbsoluteUri + locale + "/Expeditions.xml");
                    using (WebResponse resp = r2.GetResponse())
                    {
                        Stream responseStream = resp.GetResponseStream();
                        this.expeditionsXml = XDocument.Load(responseStream);
                        expeditionsXml.Save("Translations\\Expeditions.xml");
                    }
                    Logger.Add(2, "Updated expedition translations to version " + newExpedVer + ".");
                }
            }
        }

        private IEnumerable<XElement> GetTranslationList(TranslationType type)
        {
            switch (type)
            {
                case TranslationType.Ships:
                    if (this.shipsXml != null)
                        return this.shipsXml.Descendants("Ship");
                    break;
                case TranslationType.ShipTypes:
                    if (this.shipTypesXml != null)
                        return this.shipTypesXml.Descendants("Type");
                    break;
                case TranslationType.Equipment:
                    if (this.equipmentXml != null)
                        return this.equipmentXml.Descendants("Item");
                    break;
				case TranslationType.EquipmentDesc:
					if (this.equipmentXml != null)
						return this.equipmentXml.Descendants("ItemDesc");
					break;
				case TranslationType.EquipmentType:
                    if (this.equipTypesXML != null)
                        return this.equipTypesXML.Descendants("Type");
                    break;
                case TranslationType.OperationMaps:
                    if (this.operationsXml != null)
                        return this.operationsXml.Descendants("Map");
                    break;
                case TranslationType.OperationSortie:
                    if (this.operationsXml != null)
                        return this.operationsXml.Descendants("Sortie");
                    break;
				case TranslationType.OperationMapNodes:
					if (this.operationsXml != null)
						return this.operationsXml.Descendants("MapNode");
					break;
				case TranslationType.Quests:
                case TranslationType.QuestTitle:
                case TranslationType.QuestDetail:
                    if (this.questsXml != null)
                        return this.questsXml.Descendants("Quest");
                    break;
                case TranslationType.Expeditions:
                case TranslationType.ExpeditionTitle:
                case TranslationType.ExpeditionDetail:
                    if (this.expeditionsXml != null)
                        return this.expeditionsXml.Descendants("Expedition");
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
				default: break;
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
					IEnumerable<XElement> translationList = this.GetTranslationList(type);
					if (translationList == null) return jpString;
					string jpChildElement = "JP-Name";
					string trChildElement = "TR-Name";
					if (type == TranslationType.QuestDetail || type == TranslationType.ExpeditionDetail)
					{
						jpChildElement = "JP-Detail";
						trChildElement = "TR-Detail";
					}
					string translated = jpString;
					if (this.GetTranslation(jpString, translationList, jpChildElement, trChildElement, id, ref translated))
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
		
		public string GetMapNodes(int mapAreaID, int mapInfoID, int mapNodeID, TranslationType type, int id = -1)
		{
			try
			{
				IEnumerable<XElement> translationList = this.GetTranslationList(type);

				if (translationList == null)
					return mapNodeID.ToString();

				string idChildElement = "Node";
				string labelChildElement = "Label";
				
				string nodeinfo = mapAreaID.ToString("D3") + mapInfoID.ToString("D3") + mapNodeID.ToString("D3");
				string converted = nodeinfo;
				if (this.GetTranslation(nodeinfo, translationList, idChildElement, labelChildElement, id, ref converted))
					return converted;
			}
			catch (Exception e)
			{
				Logger.Add(3, "Can't output translation: " + e.Message);
			}

			return mapNodeID.ToString();
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

            bool foundWrongID = false;
            int n;
            foreach (XElement el in foundTranslation)
            {
                if (el.Attribute("mode") != null && !el.Attribute("mode").Value.Equals("normal"))
                {
                    if (el.Attribute("mode").Value.Equals("suffix")) {
                        try
                        {
                            string t = jpString.Substring(0, jpString.Length - el.Element(jpChildElement).Value.Length);
                            if (this.GetTranslation(t, translationList, jpChildElement, trChildElement, -1, ref t))
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
                        if(!Int32.TryParse(el.Element("ID").Value, out n))
                        {
                            foundWrongID = true;
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

            if(foundWrongID)
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
