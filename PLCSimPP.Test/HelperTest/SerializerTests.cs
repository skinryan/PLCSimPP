using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Config;
using BCI.PLCSimPP.Service.Devicies;
using System.Linq;

namespace BCI.PLCSimPP.Test.HelperTest
{
    [TestClass]
    public class SerializerTests
    {
        [TestMethod]
        public void SerializerLayoutTest()
        {
            var UnitCollection = new ObservableCollection<IUnit>(TestDataSource.GetLayout());
            var xmlstring = XmlConverter.SerializeIUnit(UnitCollection);
            Assert.IsInstanceOfType(xmlstring, typeof(string));

            var list = XmlConverter.DeserializeIUnit(xmlstring);

            foreach (var item in list)
            {
                Assert.IsInstanceOfType(item, typeof(IUnit));

                if (item.HasChild)
                {
                    foreach (var child in item.Children)
                    {
                        Assert.IsInstanceOfType(child, typeof(IUnit));
                    }
                }
            }
        }

        [TestMethod]
        public void SerializerSampleTest()
        {
            var xmlStr = XmlConverter.SerializeISample(TestDataSource.GetSample());
            Assert.IsInstanceOfType(xmlStr, typeof(string));

            var list = XmlConverter.DeserializeISample(xmlStr).ToList();

            Assert.AreEqual(list.Count, 20);
        }
    }
}
