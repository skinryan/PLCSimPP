using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.Devicies;
using PLCSimPP.Service.Config;
using System.Linq;

namespace PLCSimPP.Service.Router
{
    public class RouterService : IRouterService
    {
        private ObservableCollection<IUnit> mUnitCollection;

        public RouterService()
        {

        }

        public IUnit FindNextDestination(IUnit current)
        {
            if (current.IsMaster)
            {
                return current.Children.First();
            }

            var master = current.Parent;
            var index = master.Children.IndexOf(current);
            if (index + 1 < master.Children.Count)
            {
                return master.Children[index + 1];
            }

            var masterIndex = mUnitCollection.IndexOf(current);

            return mUnitCollection[masterIndex + 1].Children.First();
        }

        public void SetSiteMap(ObservableCollection<IUnit> unitColleciton)
        {
            mUnitCollection = unitColleciton;
        }


    }
}
