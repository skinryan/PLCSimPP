using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Service.Devices;
using BCI.PLCSimPP.Service.Router;
using BCI.PLCSimPP.TestTool;

namespace BCI.PLCSimPP.Test.ServiceTest
{
    [TestClass]
    public class LayoutTests
    {
        [TestMethod]
        public void TestUnitFind()
        {
            ObservableCollection<IUnit> layout = new ObservableCollection<IUnit>(TestDataSource.GetLayout());

            RouterService serv = new RouterService();
            serv.SetSiteMap(layout);

            IUnit unit1 = serv.FindNextDestination(layout.First());
            Assert.IsTrue(unit1.Address == "0000000001");

            IUnit unit2 = serv.FindNextDestination(layout.First().Children.Last());
            Assert.IsTrue(unit2.Address == "0000000100");

            IUnit unit3 = serv.FindNextDestination(layout[1].Children.Last());
            Assert.IsTrue(unit3.Address == "0000200000");

            var units = serv.FindTargetUnit("000000007F");
            Assert.IsTrue(units.Count == 7);
        }

       

    }
}
