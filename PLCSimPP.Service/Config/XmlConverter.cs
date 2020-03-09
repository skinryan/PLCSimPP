using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Service.Devicies;

namespace PLCSimPP.Service.Config
{
    public class XmlConverter
    {
        public static string Serialize<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                serializer.Serialize(writer, obj);
                string xml = writer.ToString();
                return xml;
            }
        }

        public static T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xml))
            {
                T result = (T)(serializer.Deserialize(reader));
                return result;
            }
        }

        public static string SerializeIUnit(IEnumerable<IUnit> UnitList)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = new UTF8Encoding(false);
                settings.NewLineChars = Environment.NewLine;

                using (XmlWriter xmlWriter = XmlWriter.Create(ms, settings))
                {

                    //write xml header <?xml version="1.0" encoding="utf-8" ?>
                    xmlWriter.WriteStartDocument(false);
                    //root node
                    xmlWriter.WriteStartElement("root");
                    WiriteObjest(UnitList, xmlWriter);

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                }

                string xml = Encoding.UTF8.GetString(ms.ToArray());
                return xml;
            }
        }

        private static void WiriteObjest(IEnumerable<IUnit> UnitList, XmlWriter xmlWriter)
        {
            foreach (var unit in UnitList)
            {
                var type = unit.GetType();

                xmlWriter.WriteStartElement(type.Name);

                xmlWriter.WriteElementString("Port", unit.Port.ToString());
                xmlWriter.WriteElementString("Address", unit.Address);
                xmlWriter.WriteElementString("DisplayName", unit.DisplayName);

                if (unit.HasChild)
                {
                    xmlWriter.WriteStartElement("Children");
                    WiriteObjest(unit.Children, xmlWriter);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
            }
        }

        public static IEnumerable<IUnit> DeserializeIUnit(string xml)
        {
            List<IUnit> result = new List<IUnit>();

            using (StringReader strReader = new StringReader(xml))
            {
                using (XmlReader xrdr = XmlReader.Create(strReader))
                {
                    while (xrdr.Read())
                    {
                        //如果是开始节点
                        if (xrdr.NodeType == XmlNodeType.Element)
                        {
                            //通过rdr.Name得到节点名
                            string elementName = xrdr.Name;

                            if (elementName == "root")
                            {
                                continue;
                            }
                            //TODO :bulid a factory to create instance
                            else if (elementName == "Aliquoter")
                            {
                                IUnit aliquoter = new Aliquoter();

                                ReadIUnit(aliquoter, xrdr);

                                result.Add(aliquoter);
                            }
                        }
                    }
                }
            }
            
            return result;
        }

        private static void ReadIUnit(IUnit unit, XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == unit.GetType().Name)
                {
                    break;
                }

                if (reader.NodeType == XmlNodeType.Element)
                {
                    string name = reader.Name;
                    if (name == "Port")
                    {
                        reader.Read();
                        unit.Port = int.Parse(reader.Value);
                    }
                    if (name == "Address")
                    {
                        reader.Read();
                        unit.Address = reader.Value;
                    }
                    if (name == "DisplayName")
                    {
                        reader.Read();
                        unit.DisplayName = reader.Value;
                    }
                }
            }
        }
    }
}
