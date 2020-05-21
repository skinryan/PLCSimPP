using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.DB;
using PLCSimPP.Service.Log;

namespace PLCSimPP.Test.ServiceTest
{
    [TestClass]
    public class LogServiceTest
    {
        [TestMethod]
        public void TestInitDB()
        {
            var directory = "./DB/";
            var dbName = "PLCSimPP.db3";

            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(directory + dbName))
            {
                SQLiteHelper.NewDbFile(directory + dbName);

                FileInfo file = new FileInfo(directory + dbName);
                SQLiteHelper.NewTable(file.FullName);
            }

            Assert.IsTrue(File.Exists(directory + dbName));
        }


        [TestMethod]
        public void TestMsgLogSave()
        {
            //var path = AppDomain.CurrentDomain.BaseDirectory;

            TestInitDB();

            LogService logServ = new LogService();

            MsgLog testMsg = new MsgLog
            {
                Address = "00000001",
                Command = "0201",
                Details = "F10001",
                Token = "test1"
            };
            logServ.LogRecvMsg(testMsg);

            DBService db = new DBService("data source=.\\DB\\PLCSimPP.db3;Version=3;");
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
