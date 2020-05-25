using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Communication.Support;

namespace BCI.PLCSimPP.Test.DataReaderTest
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

            var result = DataHelper.CheckData(rawData);

            var data = DataHelper.BuildSendData(new MsgCmd()
            {
                Command = UnitCmds._1024,
                Param= "1EEE0001        14",
                Port=1,
                UnitAddr= "0000000002",
                
            });

            var data1string = data.ToString();
            var data2 = DataHelper.BuildSendData(new MsgCmd()
            {
                Command = UnitCmds._1024,
                Param = "1EEE0001001     14",
                Port = 1,
                UnitAddr = "0000000002",

            });
            var msg = EncoderHelper.ConvertToMsg(rawData);

            //Assert.IsTrue(msg.Command == "0004");
            //Assert.IsTrue(msg.Param == " ");
            //Assert.IsTrue(msg.UnitAddr == "000000007F");
        }

        //public void TestConverter()
        //{
        //    BuildSendData
        //}
    }
}
