using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Service.Devicies;
using BCI.PLCSimPP.Service.Devicies.StandardResponds;
using System.Linq;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Models;
using Unity;
using BCI.PLCSimPP.Test.TestTool;
using CommonServiceLocator;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Service.Log;
using BCI.PLCSimPP.Service.Router;
using Prism.Events;
using System.Collections.ObjectModel;

namespace BCI.PLCSimPP.Test.ServiceTest
{
    [TestClass]
    public class MsgTests
    {
        private IUnityContainer mTestContainer = new UnityContainer();
        private IRouterService mRouterService;
        private ObservableCollection<IUnit> mUnitCollection;

        public MsgTests()
        {
            mUnitCollection = new ObservableCollection<IUnit>(TestDataSource.GetLayout());
            var provider = new UnityServiceLocator(mTestContainer.Registrations);
            ServiceLocator.SetLocatorProvider(() => provider);

            mTestContainer.RegisterSingleton<ISendMsgBehavior, MsgSenderForUT>();
            mTestContainer.RegisterSingleton<ILogService, LogService>();
            mTestContainer.RegisterSingleton<IRouterService, RouterService>();
            mTestContainer.RegisterSingleton<ILogService, LogService>();
            mTestContainer.RegisterSingleton<IEventAggregator, EventAggregatorForUT>();

            mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            mRouterService.SetSiteMap(mUnitCollection);
        }

        [TestMethod]
        public void Msg0017Test()
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

        [TestMethod]
        public void Msg0004Test()
        {
            IUnit stocker = new Stocker()
            {
                Port = 2,
                Address = "0000001000",
                DisplayName = "stocker#1"
            };

            IResponds handler = RespondsFactory.GetRespondsHandler(LcCmds._0004);
            var respMsgList = handler.GetRespondsMsg(stocker, " ");

            Assert.IsTrue(respMsgList.Count == 1);

            var msg = respMsgList.First();
            Assert.IsTrue(msg.Param == "A");
            Assert.IsTrue(msg.Command == UnitCmds._1001);
            Assert.IsTrue(msg.Port == stocker.Port);
            Assert.IsTrue(msg.UnitAddr == stocker.Address);
        }

        [TestMethod]
        public void Msg0005Test()
        {
            IUnit stocker = new Stocker()
            {
                Port = 2,
                Address = "0000001000",
                DisplayName = "stocker#1"
            };

            IResponds handler = RespondsFactory.GetRespondsHandler(LcCmds._0005);
            var respMsgList = handler.GetRespondsMsg(stocker, " ");
            Assert.IsTrue(respMsgList.Count == 3);

            var msg = respMsgList[0];
            Assert.IsTrue(msg.Param == "0BUZZ CMD");
            Assert.IsTrue(msg.Command == UnitCmds._10E0);
            Assert.IsTrue(msg.Port == stocker.Port);
            Assert.IsTrue(msg.UnitAddr == stocker.Address);

            var msg2 = respMsgList[1];
            Assert.IsTrue(msg2.Param == "104".PadRight(15));
            Assert.IsTrue(msg2.Command == UnitCmds._1002);
            Assert.IsTrue(msg2.Port == stocker.Port);
            Assert.IsTrue(msg2.UnitAddr == stocker.Address);

            var msg3 = respMsgList[2];
            Assert.IsTrue(msg3.Param == "103".PadRight(15));
            Assert.IsTrue(msg3.Command == UnitCmds._1002);
            Assert.IsTrue(msg3.Port == stocker.Port);
            Assert.IsTrue(msg3.UnitAddr == stocker.Address);
        }

        [TestMethod]
        public void Msg0006Test()
        {
            IUnit stocker = new Stocker()
            {
                Port = 2,
                Address = "0000001000",
                DisplayName = "stocker#1",
                CurrentSample = new Sample() { SampleID = "0001", Rack = Comm.RackType.Bypass }
            };

            IResponds handler = RespondsFactory.GetRespondsHandler(LcCmds._0006);
            var respMsgList = handler.GetRespondsMsg(stocker, "1");
            Assert.IsTrue(respMsgList.Count == 1);

            var msg = respMsgList.First();
            Assert.IsTrue(msg.Param == "1" + stocker.CurrentSample.SampleID.PadRight(15));
            Assert.IsTrue(msg.Command == UnitCmds._1011);
            Assert.IsTrue(msg.Port == stocker.Port);
            Assert.IsTrue(msg.UnitAddr == stocker.Address);
        }

        [TestMethod]
        public void Msg1024Test()
        {
            Sample testSample = new Sample()
            {
                SampleID = "0001",
                Rack = Comm.RackType.Bypass
            };

            DynamicInlet dynamic = (DynamicInlet)mRouterService.FindTargetUnit("0000000002").First();

            dynamic.InitUnit();
            dynamic.EnqueueSample(testSample);



            mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();

        }
    }
}
