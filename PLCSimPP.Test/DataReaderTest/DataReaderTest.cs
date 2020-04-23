using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCSimPP.Communication.Support;

namespace PLCSimPP.Test.DataReaderTest
{
    [TestClass]
    public class DataReaderTest
    {
        [TestMethod]
        public void ReadCommandData()
        {
            byte[] rawData = { 0x60, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x04, 0x00, 0x20 };

            var msg = EncoderHelper.ConvertToMsg(rawData);

            Assert.IsTrue(msg.Command == "0004");
            Assert.IsTrue(msg.Param == " ");
            Assert.IsTrue(msg.UnitAddr == "000000007F");
        }

        [TestMethod]
        public void TestHandleData()
        {
            byte[] rawData = { 0xe0, 0x00, };

            var result =DataHelper.CheckData(rawData);
            //var msg = EncoderHelper.ConvertToMsg(rawData);

            //Assert.IsTrue(msg.Command == "0004");
            //Assert.IsTrue(msg.Param == " ");
            //Assert.IsTrue(msg.UnitAddr == "000000007F");
        }
    }
}
