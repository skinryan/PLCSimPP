using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Enums;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Devices;

namespace BCI.PLCSimPP.TestTool
{
    public class TestDataSource
    {
        public static List<IUnit> GetLayout()
        {
            List<IUnit> UnitCollection = new List<IUnit>();

            DynamicInlet di = new DynamicInlet() { Address = "0000000002", Port = 1, DisplayName = "InletErrLane", IsMaster = true };
            HMOutlet hmoutlet = new HMOutlet() { Address = "0000000001", Port = 1, DisplayName = "HMOutlet", Parent = di };
            Centrifuge cent1 = new Centrifuge() { Address = "0000000004", Port = 1, DisplayName = "Centrifuge#1", Parent = di };
            Centrifuge cent2 = new Centrifuge() { Address = "0000000008", Port = 1, DisplayName = "Centrifuge#2", Parent = di };
            LevelDetector ld = new LevelDetector() { Address = "0000000010", Port = 1, DisplayName = "SerumLevel", Parent = di };
            Labeler laber = new Labeler() { Address = "0000000020", Port = 1, DisplayName = "Labeler", Parent = di };
            Aliquoter ali = new Aliquoter() { Address = "0000000040", Port = 1, DisplayName = "Aliquoter", Parent = di };

            UnitCollection.Add(di);
            di.Children.Add(hmoutlet);
            di.Children.Add(cent1);
            di.Children.Add(cent2);
            di.Children.Add(ld);
            di.Children.Add(laber);
            di.Children.Add(ali);

            HLane HLane = new HLane() { Address = "0000000080", Port = 2, DisplayName = "HLane", IsMaster = true };
            Service.Devices.GC gc1 = new Service.Devices.GC() { Address = "0000000100", Port = 2, DisplayName = "Generic #1", Parent = HLane };
            Service.Devices.GC gc2 = new Service.Devices.GC() { Address = "0000000200", Port = 2, DisplayName = "Generic #2", Parent = HLane };
            Service.Devices.GC gc3 = new Service.Devices.GC() { Address = "0000000400", Port = 2, DisplayName = "Generic #3", Parent = HLane };
            Service.Devices.GC gc4 = new Service.Devices.GC() { Address = "0000000800", Port = 2, DisplayName = "Generic #4", Parent = HLane };
            Service.Devices.GC gc5 = new Service.Devices.GC() { Address = "0000001000", Port = 2, DisplayName = "Generic #5", Parent = HLane };
            Service.Devices.GC gc6 = new Service.Devices.GC() { Address = "0000002000", Port = 2, DisplayName = "Generic #6", Parent = HLane };
            Service.Devices.GC gc7 = new Service.Devices.GC() { Address = "0000004000", Port = 2, DisplayName = "Generic #7", Parent = HLane };
            Service.Devices.GC gc8 = new Service.Devices.GC() { Address = "0000008000", Port = 2, DisplayName = "Generic #8", Parent = HLane };
            Service.Devices.GC gc9 = new Service.Devices.GC() { Address = "0000010000", Port = 2, DisplayName = "Generic #9", Parent = HLane };
            Service.Devices.GC gc10 = new Service.Devices.GC() { Address = "0000020000", Port = 2, DisplayName = "Generic #10", Parent = HLane };
            Service.Devices.GC gc11 = new Service.Devices.GC() { Address = "0000040000", Port = 2, DisplayName = "Generic #11", Parent = HLane };
            Service.Devices.GC gc12 = new Service.Devices.GC() { Address = "0000080000", Port = 2, DisplayName = "Generic #12", Parent = HLane };
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

            ILane ilane = new ILane() { Address = "0000100000", Port = 3, DisplayName = "ILane", IsMaster = true };
            DxC dxc1 = new DxC() { Address = "0000200000", Port = 3, DisplayName = "DxC#1", Parent = ilane };
            DxC dxc2 = new DxC() { Address = "0000400000", Port = 3, DisplayName = "DxC#2", Parent = ilane };
            DxC dxc3 = new DxC() { Address = "0000800000", Port = 3, DisplayName = "DxC#3", Parent = ilane };
            DxC dxc4 = new DxC() { Address = "0001000000", Port = 3, DisplayName = "DxC#4", Parent = ilane };
            Stocker stockyard1 = new Stocker() { Address = "0002000000", Port = 3, DisplayName = "Stockyard#1", Parent = ilane };
            Stocker stockyard2 = new Stocker() { Address = "0004000000", Port = 3, DisplayName = "Stockyard#2", Parent = ilane };
            Stocker stockyard3 = new Stocker() { Address = "0008000000", Port = 3, DisplayName = "Stockyard#3", Parent = ilane };
            Outlet outlet1 = new Outlet() { Address = "0010000000", Port = 3, DisplayName = "SigleOutlet#1", Parent = ilane };
            Outlet outlet2 = new Outlet() { Address = "0020000000", Port = 3, DisplayName = "SigleOutlet#2", Parent = ilane };
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


        public static List<ISample> GetSample()
        {
            List<ISample> SampleCollection = new List<ISample>();

            for (int i = 0; i < 20; i++)
            {
                SampleCollection.Add(new Sample { SampleID = "TEST_" + i, Rack = RackType.Remap, DcToken = i.ToString(), DxCToken = i.ToString() });
            }

            return SampleCollection;
        }
    }
}
