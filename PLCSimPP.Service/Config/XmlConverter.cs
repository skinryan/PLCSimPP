using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Enums;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Devices;

namespace BCI.PLCSimPP.Service.Config
{
    public class XmlConverter
    {

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
                    WiriteIUnit(UnitList, xmlWriter);

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                }

                string xml = Encoding.UTF8.GetString(ms.ToArray());
                return xml;
            }
        }

        private static void WiriteIUnit(IEnumerable<IUnit> UnitList, XmlWriter xmlWriter)
        {
            foreach (var unit in UnitList)
            {
                var type = unit.GetType();

                xmlWriter.WriteStartElement(type.Name);

                xmlWriter.WriteElementString("Port", unit.Port.ToString());
                xmlWriter.WriteElementString("Address", unit.Address);
                xmlWriter.WriteElementString("DisplayName", unit.DisplayName);
                xmlWriter.WriteElementString("IsMaster", unit.IsMaster.ToString());

                if (unit.HasChild)
                {
                    xmlWriter.WriteStartElement("Children");
                    WiriteIUnit(unit.Children, xmlWriter);
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
                        //node of start mark
                        if (xrdr.NodeType == XmlNodeType.Element)
                        {
                            //get node name by xrdr.Name
                            string elementName = xrdr.Name;

                            if (elementName == "root")
                            {
                                continue;
                            }
                            //create instance
                            else
                            {
                                IUnit device = BuildDevice(elementName);
                                ReadIUnit(device, xrdr);
                                result.Add(device);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static IUnit BuildDevice(string elementName)
        {
            IUnit device;
            switch (elementName)
            {
                case "Aliquoter":
                    device = new Aliquoter();
                    break;
                case "Centrifuge":
                    device = new Centrifuge();
                    break;
                case "DxC":
                    device = new DxC();
                    break;
                case "DynamicInlet":
                    device = new DynamicInlet();
                    break;
                case "HLane":
                    device = new HLane();
                    break;
                case "HMOutlet":
                    device = new HMOutlet();
                    break;
                case "ILane":
                    device = new ILane();
                    break;
                case "Labeler":
                    device = new Labeler();
                    break;
                case "LevelDetector":
                    device = new LevelDetector();
                    break;
                case "Outlet":
                    device = new Outlet();
                    break;
                case "Stocker":
                    device = new Stocker();
                    break;
                default:
                    device = new Devices.GC();
                    break;
            }

            return device;
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
                    if (name == "IsMaster")
                    {
                        reader.Read();
                        if (bool.TryParse(reader.Value, out bool result))
                            unit.IsMaster = result;
                    }
                    if (name == "Children")
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Children")
                            {
                                break;
                            }
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                IUnit device = BuildDevice(reader.Name);
                                ReadIUnit(device, reader);
                                device.Parent = unit;
                                unit.Children.Add(device);
                            }
                        }
                    }
                }
            }
        }

        public static string SerializeISample(IEnumerable<ISample> sampleList)
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
                    foreach (var sample in sampleList)
                    {
                        var type = sample.GetType();
                        xmlWriter.WriteStartElement(type.Name);

                        xmlWriter.WriteElementString("SampleID", sample.SampleID);
                        xmlWriter.WriteElementString("Rack", "" + (int)sample.Rack);
                        xmlWriter.WriteElementString("DcToken", sample.DcToken);
                        xmlWriter.WriteElementString("DxCToken", sample.DxCToken);

                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                }

                string xml = Encoding.UTF8.GetString(ms.ToArray());
                return xml;
            }
        }

        public static IEnumerable<ISample> DeserializeISample(string xml)
        {
            List<ISample> result = new List<ISample>();

            using (StringReader strReader = new StringReader(xml))
            {
                using (XmlReader xrdr = XmlReader.Create(strReader))
                {
                    while (xrdr.Read())
                    {
                        //node of start mark
                        if (xrdr.NodeType == XmlNodeType.Element)
                        {
                            //get node name by xrdr.Name
                            string elementName = xrdr.Name;

                            if (elementName == "root")
                            {
                                continue;
                            }
                            //create instance
                            else
                            {
                                ISample sample = new Sample();
                                ReadISample(sample, xrdr);
                                result.Add(sample);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static void ReadISample(ISample sample, XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == sample.GetType().Name)
                {
                    break;
                }

                if (reader.NodeType == XmlNodeType.Element)
                {
                    string name = reader.Name;
                    if (name == "SampleID")
                    {
                        reader.Read();
                        sample.SampleID = reader.Value;
                    }
                    if (name == "Rack")
                    {
                        reader.Read();
                        sample.Rack = (RackType)int.Parse(reader.Value);
                    }
                    if (name == "DcToken")
                    {
                        reader.Read();
                        sample.DcToken = reader.Value;
                    }
                    if (name == "DxCToken")
                    {
                        reader.Read();
                        sample.DxCToken = reader.Value;
                    }
                }
            }
        }
    }
}
