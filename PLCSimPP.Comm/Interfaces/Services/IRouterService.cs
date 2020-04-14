using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Comm.Interfaces.Services
{
    public interface IRouterService
    {
        //ObservableCollection<UnitBase> GetLayout();
        public void SetSiteMap(ObservableCollection<IUnit> unitColleciton);

        //public void MoveSampleToTargetUnit(IUnit current, IUnit target);
        public IUnit FindNextDestination(IUnit current);
    }
}
