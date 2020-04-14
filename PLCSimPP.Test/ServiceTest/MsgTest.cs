using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Service.Devicies;
using PLCSimPP.Service.Devicies.StandardResponds;

namespace PLCSimPP.Test.ServiceTest
{
    [TestClass]
    public class MsgTest
    {

        [TestMethod]
        public void TestMsg1017()
        {
            string recev = "13678860130     11031K31 ";
            IUnit stocker = new Stocker()
            {
                Port = 2,
                Address = "0000001000",
                DisplayName = "stocker#1"
            };
            var ret = SendMsg.GetMsg_1015(stocker, recev);

            Assert.IsTrue(ret.Param == "3678860130     1103100");

        }
    }
}
