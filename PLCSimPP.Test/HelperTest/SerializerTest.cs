using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.Config;
using PLCSimPP.Service.Devicies;

namespace PLCSimPP.Test.HelperTest
{
    [TestClass]
    public class SerializerTest
    {
        [TestMethod]
        public void SerializerInterfaceTest()
        {
            //1, "0000222222", "TestDevice"
            IUnit tmp = new Aliquoter()
            {
                Address = "0000222222",
                Port = 1,
                DisplayName = "TestDevice"
            };
            var UnitCollection = new ObservableCollection<IUnit>();
            UnitCollection.Add(tmp);


            var xmlstring = XmlConverter.SerializeIUnit(UnitCollection);
            var list = XmlConverter.DeserializeIUnit(xmlstring);

            //Aliquoter ali = list.ToList()[0];
        }

        [TestMethod]
        public void SerializerJsonTest()
        {


        }
    }
}
