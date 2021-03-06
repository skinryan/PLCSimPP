﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.DB;
using BCI.PLCSimPP.Service.Log;

namespace BCI.PLCSimPP.Test.ServiceTest
{
    [TestClass]
    public class LogServiceTests
    {
        [TestMethod]
        public void TestMsgLogSave()
        {
            LogService logServ = new LogService();  
            DBService db = new DBService("data source=.;initial catalog=PLCSimPP;integrated security=SSPI;");
            db.TruncateTable();

            MsgLog testMsg = new MsgLog
            {
                Address = "00000001",
                Command = "0201",
                Details = "F10001",
                Token = "test1"
            };
            logServ.LogRecvMsg(testMsg);

            var results = db.QueryLogContents();
            var count = 0;
            foreach (var item in results)
            {
                if (item.Token == "test1")
                    count += 1;
            }

            Assert.IsTrue(count == 1);
        }
    }
}
