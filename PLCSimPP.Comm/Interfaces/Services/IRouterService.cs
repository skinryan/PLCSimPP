using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Comm.Interfaces.Services
{
    public interface IRouterService
    {
        //ObservableCollection<UnitBase> GetLayout();
        public void SetSiteMap(ObservableCollection<IUnit> unitColleciton);

        public List<IUnit> FindTargetUnit(string targetAddr);

        public IUnit FindNextDestination(IUnit current);

       // public IUnit FindNextPort(IUnit current);
    }
}
