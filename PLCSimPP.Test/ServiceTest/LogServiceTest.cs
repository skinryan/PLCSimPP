using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.Log;

namespace PLCSimPP.Test.ServiceTest
{
    [TestClass]
    public class LogServiceTest
    {
        [TestMethod]
        public void TestMsgLogSave()
        {
            LogService logServ = new LogService();

            MsgLog testMsg = new MsgLog
            {
                Address = "00000001",
                Command = "0201",
                Details = "F10001",
                Token = "test1"
            };
            logServ.LogRecvMsg(testMsg);

            //Assert.IsTrue( );
            //Assert.IsTrue(logServ.LogRecvMsg("00000001", "0201", "F10001", "test1") == 1);
            //Assert.IsTrue(logServ.LogSendMsg(testMsg) == 1);
            //Assert.IsTrue(logServ.LogRecvMsg("00000001", "0201", "F10001", "test1") == 1);
        }
    }
}
