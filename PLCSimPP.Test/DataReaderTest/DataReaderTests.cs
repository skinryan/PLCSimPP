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
    public class DataReaderTests
    {
        [TestMethod]
        public void ReadCommandDataTest()
        {
            byte[] rawData = { 0x60, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x04, 0x00, 0x20 };

            var msg = EncoderHelper.ConvertToMsg(rawData);

            Assert.IsTrue(msg.Command == "0004");
            Assert.IsTrue(msg.Param == " ");
            Assert.IsTrue(msg.UnitAddr == "000000007F");
        }

        [TestMethod]
        public void HandleDataTest()
        {
            byte[] rawData = { 0xe0, 0x00, };
            var result = DataHelper.CheckData(rawData);
            Assert.AreEqual(result.Result, Communication.Models.ResultType.Confirm);

            //TODO : ADD ROWDATA from log

            //var data = DataHelper.BuildSendData(new MsgCmd()
            //{
            //    Command = UnitCmds._1024,
            //    Param = "1EEE0001        14",
            //    Port = 1,
            //    UnitAddr = "0000000002"
            //});

            //var data1string = data.ToString();
            //var data2 = DataHelper.BuildSendData(new MsgCmd()
            //{
            //    Command = UnitCmds._1024,
            //    Param = "1EEE0001001     14",
            //    Port = 1,
            //    UnitAddr = "0000000002",

            //});
            //var msg = EncoderHelper.ConvertToMsg(rawData);

            //Assert.IsTrue(msg.Command == "0004");
            //Assert.IsTrue(msg.Param == " ");
            //Assert.IsTrue(msg.UnitAddr == "000000007F");
        }


    }
}
