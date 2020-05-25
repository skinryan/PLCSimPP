using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Service.Devicies;
using BCI.PLCSimPP.Service.Devicies.StandardResponds;

namespace BCI.PLCSimPP.Test.ServiceTest
{
    [TestClass]
    public class MsgTest
    {

        [TestMethod]
        public void Msg1017Test()
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
