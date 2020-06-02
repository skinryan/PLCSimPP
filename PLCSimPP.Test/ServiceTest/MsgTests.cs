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
using Prism.Unity;
using BCI.PLCSimPP.Service.Analyzer;
using BCI.PLCSimPP.Service.Config;
using System.Threading;
using GC = BCI.PLCSimPP.Service.Devicies.GC;

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
            //var provider = new UnityServiceLocator(mTestContainer.Registrations);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocatorAdapter(mTestContainer));

            mTestContainer.RegisterSingleton<IEventAggregator, EventAggregatorForUT>();
            mTestContainer.RegisterSingleton<IRouterService, RouterServiceForUT>();
            mTestContainer.RegisterSingleton<ISendMsgBehavior, MsgSenderForUT>();
            mTestContainer.RegisterSingleton<ILogService, LogService>();
            mTestContainer.RegisterSingleton<IConfigService, ConifgService>();
            mTestContainer.RegisterSingleton<IAnalyzerSimService, AnalyzerSimServiceForUT>("DCSimService");
            mTestContainer.RegisterSingleton<IAnalyzerSimService, AnalyzerSimServiceForUT>("DxCSimService");

            mUnitCollection = new ObservableCollection<IUnit>(TestDataSource.GetLayout());

            mRouterService = mTestContainer.Resolve<IRouterService>();// ServiceLocator.Current.GetInstance<IRouterService>();
            mRouterService.SetSiteMap(mUnitCollection);
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
        public void DynamicInletTest()
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
            var unit = (UnitBase)mRouterService.FindNextDestination(dynamic);
            int pendingCount = 0;

            for (int i = 0; i < 10; i++)
            {
                pendingCount = unit.PendingCount;
                Thread.Sleep(1000);
            }

            Assert.IsTrue(pendingCount == 1);
        }

        [TestMethod]
        public void HMOutletTest()
        {
            Sample testSample1 = new Sample()
            {
                SampleID = "0001_0011",
                Rack = Comm.RackType.Bypass
            };
            Sample testSample2 = new Sample()
            {
                SampleID = "0001_0017",
                Rack = Comm.RackType.Bypass
            };

            HMOutlet mhOut = (HMOutlet)mRouterService.FindTargetUnit("0000000001").First();

            mhOut.ResetQueue();
            mhOut.InitUnit();
            mhOut.EnqueueSample(testSample1);

            mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)mRouterService.FindNextDestination(mhOut);
            int pendingCount = 0;

            for (int i = 0; i < 10; i++)
            {
                pendingCount = unit.PendingCount;
                Thread.Sleep(1000);
            }

            Assert.IsTrue(pendingCount == 1);

            //----------------------------------------

            mhOut.EnqueueSample(testSample2);

            var stored = 0;
            for (int i = 0; i < 10; i++)
            {
                stored = mhOut.StoredCount;
                Thread.Sleep(1000);
            }

            Assert.IsTrue(stored == 1);
        }

        [TestMethod]
        public void CentrifugeTest()
        {
            Sample testSample1 = new Sample()
            {
                SampleID = "0001_0011",
                Rack = Comm.RackType.Bypass
            };

            Centrifuge cent = (Centrifuge)mRouterService.FindTargetUnit("0000000004").First();
            mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)mRouterService.FindNextDestination(cent);

            cent.InitUnit();
            cent.EnqueueSample(testSample1);

            Thread.Sleep(40000);

            Assert.IsTrue(unit.PendingCount == 1);

            MsgSenderForUT msut = (MsgSenderForUT)mTestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = msut.MessageList.Where(t => t.UnitAddr == "0000000004");

            Assert.IsTrue(centMsg.Count(t => t.Command == "1022") == 3);
            Assert.IsTrue(centMsg.Count(t => t.Command == "101A") == 1);

        }

        [TestMethod]
        public void LevelDetectorTest()
        {
            Sample testSample1 = new Sample() { SampleID = "0001", Rack = Comm.RackType.Bypass };
            Sample testSample2 = new Sample() { SampleID = "0002", Rack = Comm.RackType.Bypass };
            Sample testSample3 = new Sample() { SampleID = "0003", Rack = Comm.RackType.Bypass };
            Sample testSample4 = new Sample() { SampleID = "0004", Rack = Comm.RackType.Bypass };
            Sample testSample5 = new Sample() { SampleID = "0005", Rack = Comm.RackType.Bypass };

            LevelDetector ld = (LevelDetector)mRouterService.FindTargetUnit("0000000010").First();
            mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)mRouterService.FindNextDestination(ld);

            ld.InitUnit();
            ld.EnqueueSample(testSample1);
            ld.EnqueueSample(testSample2);
            ld.EnqueueSample(testSample3);
            ld.EnqueueSample(testSample4);
            ld.EnqueueSample(testSample5);

            Thread.Sleep(10000);

            Assert.IsTrue(unit.PendingCount == 5);

            MsgSenderForUT msut = (MsgSenderForUT)mTestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = msut.MessageList.Where(t => t.UnitAddr == "0000000010");

            Assert.IsTrue(centMsg.Count(t => t.Command == "1012") == 1);

        }

        [TestMethod]
        public void LabelerAndAliquoterTest()
        {
            Sample testSample1 = new Sample() { SampleID = "0001", Rack = Comm.RackType.Bypass };

            Labeler lbr = (Labeler)mRouterService.FindTargetUnit("0000000020").First();
            Aliquoter aqr = (Aliquoter)mRouterService.FindTargetUnit("0000000040").First();
            mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)mRouterService.FindNextDestination(aqr);

            lbr.InitUnit();
            aqr.InitUnit();
            lbr.EnqueueSample(testSample1);

            Thread.Sleep(10000);

            MsgSenderForUT msut = (MsgSenderForUT)mTestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = msut.MessageList.Where(t => t.UnitAddr == "0000000020" || t.UnitAddr == "0000000040");

            Assert.IsTrue(centMsg.Count(t => t.Command == "1011") == 4);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1013") == 1);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1014") == 1);
            Assert.IsTrue(unit.PendingCount == 2);
        }

        [TestMethod]
        public void GCTest()
        {
            Sample testSample1 = new Sample() { SampleID = "00011", Rack = Comm.RackType.Bypass, DcToken = "AAA", IsSubTube = true };

            GC gc = (GC)mRouterService.FindTargetUnit("0000000100").First();
            mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)mRouterService.FindNextDestination(gc);

            gc.InitUnit();
            gc.EnqueueSample(testSample1);

            Thread.Sleep(5000);

            MsgSenderForUT msut = (MsgSenderForUT)mTestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = msut.MessageList.Where(t => t.UnitAddr == "0000000100");

            Assert.IsTrue(centMsg.Count(t => t.Command == "1011") == 3);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1015") == 1);
            Assert.IsTrue(unit.PendingCount == 1);
        }

        [TestMethod]
        public void DxCTest()
        {
            Sample testSample1 = new Sample() { SampleID = "00011", Rack = Comm.RackType.Bypass, DcToken = "AAA", IsSubTube = true };

            DxC dxc = (DxC)mRouterService.FindTargetUnit("0000200000").First();
            mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)mRouterService.FindNextDestination(dxc);

            dxc.InitUnit();
            dxc.EnqueueSample(testSample1);

            Thread.Sleep(5000);

            MsgSenderForUT msut = (MsgSenderForUT)mTestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = msut.MessageList.Where(t => t.UnitAddr == "0000200000");

            Assert.IsTrue(centMsg.Count(t => t.Command == "1011") == 2);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1015") == 1);
            Assert.IsTrue(unit.PendingCount == 1);
        }

        [TestMethod]
        public void StockerTest()
        {
            Sample testSample1 = new Sample() { SampleID = "0001", Rack = Comm.RackType.Bypass };
            Sample testSample2 = new Sample() { SampleID = "0002", Rack = Comm.RackType.Bypass };

            Stocker stocker = (Stocker)mRouterService.FindTargetUnit("0002000000").First();
            mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)mRouterService.FindNextDestination(stocker);

            stocker.InitUnit();
            stocker.EnqueueSample(testSample1);
            stocker.EnqueueSample(testSample2);

            Thread.Sleep(10000);

            MsgSenderForUT msut = (MsgSenderForUT)mTestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = msut.MessageList.Where(t => t.UnitAddr == "0002000000");
            Assert.IsTrue(centMsg.Count(t => t.Command == "1011") == 4);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1026") == 1);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1015") == 1);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1019") == 1);
            Assert.IsTrue(unit.PendingCount == 2);
            
        }
    }
}
