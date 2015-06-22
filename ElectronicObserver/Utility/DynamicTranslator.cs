using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility
{
    public class DynamicTranslator
    {
        private XDocument shipsXml;
        private XDocument shipTypesXml;
        private XDocument equipmentXml;
        private XDocument operationsXml;
        private XDocument questsXml;
        private XDocument expeditionsXml;

        internal DynamicTranslator()
        {
            try
            {
                if (File.Exists("Translations\\Ships.xml")) this.shipsXml = XDocument.Load("Translations\\Ships.xml");
                if (File.Exists("Translations\\ShipTypes.xml")) this.shipTypesXml = XDocument.Load("Translations\\ShipTypes.xml");
                if (File.Exists("Translations\\Equipment.xml")) this.equipmentXml = XDocument.Load("Translations\\Equipment.xml");
                if (File.Exists("Translations\\Operations.xml")) this.operationsXml = XDocument.Load("Translations\\Operations.xml");
                if (File.Exists("Translations\\Quests.xml")) this.questsXml = XDocument.Load("Translations\\Quests.xml");
                if (File.Exists("Translations\\Expeditions.xml")) this.expeditionsXml = XDocument.Load("Translations\\Expeditions.xml");
            }
            catch (Exception ex)
            {
                Logger.Add(3, "Could not load translation file: " + ex.Message);
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
                case TranslationType.OperationMaps:
                    if (this.operationsXml != null)
                        return this.operationsXml.Descendants("Map");
                    break;
                case TranslationType.OperationSortie:
                    if (this.operationsXml != null)
                        return this.operationsXml.Descendants("Sortie");
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
                {
                    return translated;
                }
            }
            catch (Exception e)
            {
                Logger.Add(3, "Can't output translation: " + e.Message);
            }

            return jpString;
        }

        public bool GetTranslation(string jpString, IEnumerable<XElement> translationList, string jpChildElement, string trChildElement, int id, ref string translate)
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
                catch (Exception e)
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
        Operations,
        Quests,
        Ships,
        ShipTypes,
        OperationMaps,
        OperationSortie,
        QuestDetail,
        QuestTitle,
        Expeditions,
        ExpeditionDetail,
        ExpeditionTitle
    }
}
