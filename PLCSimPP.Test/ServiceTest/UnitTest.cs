using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Service.Devicies;
using PLCSimPP.Service.Router;

namespace PLCSimPP.Test.ServiceTest
{
    [TestClass]
    public class UnitTest
    {
        public List<IUnit> GetLayout()
        {
            List<IUnit> UnitCollection = new List<IUnit>();

            HMOutlet hmoutlet = new HMOutlet() { Address = "0000000001", Port = 1, DisplayName = "HMOutlet" };
            DynamicInlet di = new DynamicInlet() { Address = "0000000002", Port = 1, DisplayName = "InletErrLane" };
            Centrifuge cent1 = new Centrifuge() { Address = "0000000004", Port = 1, DisplayName = "Centrifuge#1" };
            Centrifuge cent2 = new Centrifuge() { Address = "0000000008", Port = 1, DisplayName = "Centrifuge#2" };
            LevelDetector ld = new LevelDetector() { Address = "0000000010", Port = 1, DisplayName = "SerumLevel" };
            Labeler laber = new Labeler() { Address = "0000000020", Port = 1, DisplayName = "Labeler" };
            Aliquoter ali = new Aliquoter() { Address = "0000000040", Port = 1, DisplayName = "Aliquoter" };

            UnitCollection.Add(hmoutlet);
            hmoutlet.Children.Add(di);
            hmoutlet.Children.Add(cent1);
            hmoutlet.Children.Add(cent2);
            hmoutlet.Children.Add(ld);
            hmoutlet.Children.Add(laber);
            hmoutlet.Children.Add(ali);

            HLane HLane = new HLane() { Address = "0000000080", Port = 2, DisplayName = "HLane" };
            Service.Devicies.GC gc1 = new Service.Devicies.GC() { Address = "0000000100", Port = 2, DisplayName = "Generic #1" };
            Service.Devicies.GC gc2 = new Service.Devicies.GC() { Address = "0000000200", Port = 2, DisplayName = "Generic #2" };
            Service.Devicies.GC gc3 = new Service.Devicies.GC() { Address = "0000000400", Port = 2, DisplayName = "Generic #3" };
            Service.Devicies.GC gc4 = new Service.Devicies.GC() { Address = "0000000800", Port = 2, DisplayName = "Generic #4" };
            Service.Devicies.GC gc5 = new Service.Devicies.GC() { Address = "0000001000", Port = 2, DisplayName = "Generic #5" };
            Service.Devicies.GC gc6 = new Service.Devicies.GC() { Address = "0000002000", Port = 2, DisplayName = "Generic #6" };
            Service.Devicies.GC gc7 = new Service.Devicies.GC() { Address = "0000004000", Port = 2, DisplayName = "Generic #7" };
            Service.Devicies.GC gc8 = new Service.Devicies.GC() { Address = "0000008000", Port = 2, DisplayName = "Generic #8" };
            Service.Devicies.GC gc9 = new Service.Devicies.GC() { Address = "0000010000", Port = 2, DisplayName = "Generic #9" };
            Service.Devicies.GC gc10 = new Service.Devicies.GC() { Address = "0000020000", Port = 2, DisplayName = "Generic #10" };
            Service.Devicies.GC gc11 = new Service.Devicies.GC() { Address = "0000040000", Port = 2, DisplayName = "Generic #11" };
            Service.Devicies.GC gc12 = new Service.Devicies.GC() { Address = "0000080000", Port = 2, DisplayName = "Generic #12" };
            UnitCollection.Add(HLane);
            HLane.Children.Add(gc1);
            HLane.Children.Add(gc2);
            HLane.Children.Add(gc3);
            HLane.Children.Add(gc4);
            HLane.Children.Add(gc5);
            HLane.Children.Add(gc6);
            HLane.Children.Add(gc7);
            HLane.Children.Add(gc8);
            HLane.Children.Add(gc9);
            HLane.Children.Add(gc10);
            HLane.Children.Add(gc11);
            HLane.Children.Add(gc12);

            ILane ilane = new ILane() { Address = "0000100000", Port = 3, DisplayName = "ILane" };
            DxC dxc1 = new DxC() { Address = "0000200000", Port = 3, DisplayName = "DxC#1" };
            DxC dxc2 = new DxC() { Address = "0000400000", Port = 3, DisplayName = "DxC#2" };
            DxC dxc3 = new DxC() { Address = "0000800000", Port = 3, DisplayName = "DxC#3" };
            DxC dxc4 = new DxC() { Address = "0001000000", Port = 3, DisplayName = "DxC#4" };
            Stocker stockyard1 = new Stocker() { Address = "0002000000", Port = 3, DisplayName = "Stockyard#1" };
            Stocker stockyard2 = new Stocker() { Address = "0004000000", Port = 3, DisplayName = "Stockyard#2" };
            Stocker stockyard3 = new Stocker() { Address = "0008000000", Port = 3, DisplayName = "Stockyard#3" };
            Outlet outlet1 = new Outlet() { Address = "0010000000", Port = 3, DisplayName = "SigleOutlet#1" };
            Outlet outlet2 = new Outlet() { Address = "0020000000", Port = 3, DisplayName = "SigleOutlet#2" };
            UnitCollection.Add(ilane);
            ilane.Children.Add(dxc1);
            ilane.Children.Add(dxc2);
            ilane.Children.Add(dxc3);
            ilane.Children.Add(dxc4);
            ilane.Children.Add(stockyard1);
            ilane.Children.Add(stockyard2);
            ilane.Children.Add(stockyard3);
            ilane.Children.Add(outlet1);
            ilane.Children.Add(outlet2);

            return UnitCollection;
        }


        [TestMethod]
        public void TestUnitFind()
        {
            string[] ss = new string[] { "0000000001",
                                       "0000000002",
                                       "0000000004",
                                       "0000000008",
                                       "0000000010",
                                       "0000000020",
                                       "0000000040", };

            int count = 0;
            foreach (var unit in ss)
            {
                int targetValue = int.Parse("0000000004", System.Globalization.NumberStyles.HexNumber);
                int unitValue = int.Parse(unit, System.Globalization.NumberStyles.HexNumber);
                if ((unitValue | targetValue) == targetValue)
                {
                    count += 1;
                }
            }

            //var result = from p in GetLayout() ;
            //var units = UnitHelper.FindTargetUnit(result, "000000007F");

            Assert.IsTrue(count == 1);
        }
    }
}
