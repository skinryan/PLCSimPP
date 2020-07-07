using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Enums;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Config;
using BCI.PLCSimPP.Service.Devices;
using BCI.PLCSimPP.Service.Devices.StandardResponds;
using BCI.PLCSimPP.Service.Log;
using BCI.PLCSimPP.Test.TestTool;
using BCI.PLCSimPP.TestTool;
using CommonServiceLocator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using Prism.Unity;
using Unity;

namespace BCI.PLCSimPP.Test.ServiceTest
{
    [TestClass]
    public class MsgTests
    {
        public static IUnityContainer TestContainer;
        public static IRouterService RouterService;
        public static ObservableCollection<IUnit> UnitCollection;

        [ClassInitialize]
        public static void ClassInit (TestContext context)
        {
            TestContainer = new UnityContainer().AddExtension(new Diagnostic());
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocatorAdapter(TestContainer));

            TestContainer.RegisterSingleton<IEventAggregator, EventAggregatorForUT>();
            TestContainer.RegisterSingleton<IRouterService, RouterServiceForUT>();
            TestContainer.RegisterSingleton<ISendMsgBehavior, MsgSenderForUT>();
            TestContainer.RegisterSingleton<ILogService, LogService>();
            TestContainer.RegisterSingleton<IConfigService, ConfigService>();
            TestContainer.RegisterSingleton<IAnalyzerSimService, AnalyzerSimServiceForUT>("DCSimService");
            TestContainer.RegisterSingleton<IAnalyzerSimService, AnalyzerSimServiceForUT>("DxCSimService");

            UnitCollection = new ObservableCollection<IUnit>(TestDataSource.GetLayout());

            RouterService = TestContainer.Resolve<IRouterService>();
            RouterService.SetSiteMap(UnitCollection);
        }

        [TestCleanup]
        public void TestCleanUp ()
        {
            UnitCollection = new ObservableCollection<IUnit>(TestDataSource.GetLayout());
        }

        [TestMethod]
        public void Msg0004Test ()
        {
            IUnit stocker = new Stocker
            {
                Port = 2,
                Address = "0000001000",
                DisplayName = "stocker#1"
            };

            var handler = RespondsFactory.GetRespondsHandler(LcCmds._0004);
            var respMsgList = handler.GetRespondsMsg(stocker, " ");

            Assert.IsTrue(respMsgList.Count == 1);

            var msg = respMsgList.First();
            Assert.IsTrue(msg.Param == "A");
            Assert.IsTrue(msg.Command == UnitCmds._1001);
            Assert.IsTrue(msg.Port == stocker.Port);
            Assert.IsTrue(msg.UnitAddr == stocker.Address);
        }

        [TestMethod]
        public void Msg0005Test ()
        {
            IUnit stocker = new Stocker
            {
                Port = 2,
                Address = "0000001000",
                DisplayName = "stocker#1"
            };

            var handler = RespondsFactory.GetRespondsHandler(LcCmds._0005);
            var respMsgList = handler.GetRespondsMsg(stocker, " ");
            Assert.IsTrue(respMsgList.Count == 3);

            var msg = respMsgList[0];
            Assert.IsTrue(msg.Param == "0BUZZ CMD");
            Assert.IsTrue(msg.Command == UnitCmds._10E0);
            Assert.IsTrue(msg.Port == stocker.Port);
            Assert.IsTrue(msg.UnitAddr == stocker.Address);

            var msg2 = respMsgList[1];
            Assert.IsTrue(msg2.Param == "104".PadRight(18));
            Assert.IsTrue(msg2.Command == UnitCmds._1002);
            Assert.IsTrue(msg2.Port == stocker.Port);
            Assert.IsTrue(msg2.UnitAddr == stocker.Address);

            var msg3 = respMsgList[2];
            Assert.IsTrue(msg3.Param == "103".PadRight(18));
            Assert.IsTrue(msg3.Command == UnitCmds._1002);
            Assert.IsTrue(msg3.Port == stocker.Port);
            Assert.IsTrue(msg3.UnitAddr == stocker.Address);
        }

        [TestMethod]
        public void Msg0006Test ()
        {
            IUnit stocker = new Stocker
            {
                Port = 2,
                Address = "0000001000",
                DisplayName = "stocker#1",
                CurrentSample = new Sample {SampleID = "0001", Rack = RackType.Bypass}
            };

            var handler = RespondsFactory.GetRespondsHandler(LcCmds._0006);
            var respMsgList = handler.GetRespondsMsg(stocker, "1");
            Assert.IsTrue(respMsgList.Count == 1);

            var msg = respMsgList.First();
            Assert.IsTrue(msg.Param == "1" + stocker.CurrentSample.SampleID.PadRight(15));
            Assert.IsTrue(msg.Command == UnitCmds._1011);
            Assert.IsTrue(msg.Port == stocker.Port);
            Assert.IsTrue(msg.UnitAddr == stocker.Address);
        }

        [TestMethod]
        public void DynamicInletTest ()
        {
            var testSample = new Sample
            {
                SampleID = "0001",
                Rack = RackType.Bypass
            };

            var dynamic = (DynamicInlet)RouterService.FindTargetUnit("0000000002").First();

            dynamic.InitUnit();
            dynamic.EnqueueSample(testSample);

            RouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)RouterService.FindNextDestination(dynamic);
            var pendingCount = 0;

            for(var i = 0; i < 10; i++)
            {
                pendingCount = unit.PendingCount;
                Thread.Sleep(1000);
            }

            Assert.IsTrue(pendingCount == 1);
        }

        [TestMethod]
        public void HMOutletTest ()
        {
            var testSample1 = new Sample
            {
                SampleID = "0001",
                Rack = RackType.Bypass
            };
            var testSample2 = new Sample
            {
                SampleID = "0017",
                Rack = RackType.Bypass
            };

            var mhOut = (HMOutlet)RouterService.FindTargetUnit("0000000001").First();

            mhOut.ResetQueue();
            mhOut.InitUnit();
            mhOut.EnqueueSample(testSample1);

            RouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)RouterService.FindNextDestination(mhOut);

            Thread.Sleep(5000);
            Assert.IsTrue(unit.PendingCount == 1);

            //----------------------------------------

            mhOut.EnqueueSample(testSample2);
            Thread.Sleep(5000);
            Assert.IsTrue(mhOut.StoredCount == 1);
        }

        [TestMethod]
        public void CentrifugeTest ()
        {
            var testSample1 = new Sample
            {
                SampleID = "0001_0011",
                Rack = RackType.Bypass
            };

            var cent = (Centrifuge)RouterService.FindTargetUnit("0000000004").First();
            RouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)RouterService.FindNextDestination(cent);

            cent.InitUnit();
            cent.EnqueueSample(testSample1);

            Thread.Sleep(40000);

            Assert.IsTrue(unit.PendingCount == 1);

            var senderForUt = (MsgSenderForUT)TestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = senderForUt.MessageList.Where(t => t.UnitAddr == "0000000004");

            Assert.IsTrue(centMsg.Count(t => t.Command == "1022") == 3);
            Assert.IsTrue(centMsg.Count(t => t.Command == "101A") == 1);
        }

        [TestMethod]
        public void LevelDetectorTest ()
        {
            var testSample1 = new Sample {SampleID = "0001", Rack = RackType.Bypass};
            var testSample2 = new Sample {SampleID = "0002", Rack = RackType.Bypass};
            var testSample3 = new Sample {SampleID = "0003", Rack = RackType.Bypass};
            var testSample4 = new Sample {SampleID = "0004", Rack = RackType.Bypass};
            var testSample5 = new Sample {SampleID = "0005", Rack = RackType.Bypass};

            var ld = (LevelDetector)RouterService.FindTargetUnit("0000000010").First();
            RouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)RouterService.FindNextDestination(ld);

            ld.InitUnit();
            ld.EnqueueSample(testSample1);
            ld.EnqueueSample(testSample2);
            ld.EnqueueSample(testSample3);
            ld.EnqueueSample(testSample4);
            ld.EnqueueSample(testSample5);

            Thread.Sleep(10000);

            Assert.IsTrue(unit.PendingCount == 5);

            var senderForUt = (MsgSenderForUT)TestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = senderForUt.MessageList.Where(t => t.UnitAddr == "0000000010");

            Assert.IsTrue(centMsg.Count(t => t.Command == "1012") == 1);
        }

        [TestMethod]
        public void LabelerAndAliquoterTest ()
        {
            var testSample1 = new Sample {SampleID = "0001", Rack = RackType.Bypass};

            var lbr = (Labeler)RouterService.FindTargetUnit("0000000020").First();
            var aqr = (Aliquoter)RouterService.FindTargetUnit("0000000040").First();
            RouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)RouterService.FindNextDestination(aqr);

            lbr.InitUnit();
            aqr.InitUnit();
            lbr.EnqueueSample(testSample1);

            Thread.Sleep(10000);

            var senderForUt = (MsgSenderForUT)TestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = senderForUt.MessageList.Where(t => t.UnitAddr == "0000000020" || t.UnitAddr == "0000000040");

            Assert.IsTrue(centMsg.Count(t => t.Command == "1011") == 4);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1013") == 1);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1014") == 1);
            Assert.IsTrue(unit.PendingCount == 2);
        }

        [TestMethod]
        public void GCTest ()
        {
            var testSample1 = new Sample
                {SampleID = "00011", Rack = RackType.Bypass, DcToken = "AAA", IsSubTube = true};

            var gc = (GC)RouterService.FindTargetUnit("0000000100").First();
            RouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)RouterService.FindNextDestination(gc);

            gc.InitUnit();
            gc.EnqueueSample(testSample1);

            Thread.Sleep(5000);

            var senderForUt = (MsgSenderForUT)TestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = senderForUt.MessageList.Where(t => t.UnitAddr == "0000000100");

            Assert.IsTrue(centMsg.Count(t => t.Command == "1011") == 3);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1015") == 1);
            Assert.IsTrue(unit.PendingCount == 1);
        }

        [TestMethod]
        public void DxCTest ()
        {
            var testSample1 = new Sample
                {SampleID = "00011", Rack = RackType.Bypass, DcToken = "AAA", IsSubTube = true};

            var dxc = (DxC)RouterService.FindTargetUnit("0000200000").First();
            RouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)RouterService.FindNextDestination(dxc);

            dxc.InitUnit();
            dxc.EnqueueSample(testSample1);

            Thread.Sleep(5000);

            var senderForUt = (MsgSenderForUT)TestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = senderForUt.MessageList.Where(t => t.UnitAddr == "0000200000");

            Assert.IsTrue(centMsg.Count(t => t.Command == "1011") == 2);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1015") == 1);
            Assert.IsTrue(unit.PendingCount == 1);
        }

        [TestMethod]
        public void StockerTest ()
        {
            var testSample1 = new Sample {SampleID = "0001", Rack = RackType.Bypass};
            var testSample2 = new Sample {SampleID = "0002", Rack = RackType.Bypass};

            var stocker = (Stocker)RouterService.FindTargetUnit("0002000000").First();
            RouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            var unit = (UnitBase)RouterService.FindNextDestination(stocker);

            stocker.InitUnit();
            stocker.EnqueueSample(testSample1);
            stocker.EnqueueSample(testSample2);

            Thread.Sleep(10000);

            var senderForUt = (MsgSenderForUT)TestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = senderForUt.MessageList.Where(t => t.UnitAddr == "0002000000");
            Assert.IsTrue(centMsg.Count(t => t.Command == "1011") == 4);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1026") == 1);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1015") == 1);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1019") == 1);
            Assert.IsTrue(unit.PendingCount == 2);
        }

        [TestMethod]
        public void OutletTest ()
        {
            var testSample1 = new Sample {SampleID = "0001", Rack = RackType.Bypass};

            var outlet = (Outlet)RouterService.FindTargetUnit("0010000000").First();
            //mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();
            //var unit = (UnitBase)mRouterService.FindNextDestination(stocker);

            outlet.InitUnit();
            outlet.EnqueueSample(testSample1);

            Thread.Sleep(10000);

            var senderForUt = (MsgSenderForUT)TestContainer.Resolve<ISendMsgBehavior>();

            var centMsg = senderForUt.MessageList.Where(t => t.UnitAddr == "0010000000");
            Assert.IsTrue(centMsg.Count(t => t.Command == "1011") == 1);
            Assert.IsTrue(centMsg.Count(t => t.Command == "1015") == 1);
            Assert.IsTrue(outlet.StoredCount == 1);
        }
    }
}