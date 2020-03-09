using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.Devicies;
using PLCSimPP.Service.Config;

namespace PLCSimPP.Service.Router
{
    public class RouterService : IRouterService
    {
       // private IConfigService mLayoutServ;

        public RouterService()
        {
            //mLayoutServ = layout;
        }

        

        private void FindNextDestination()
        {
            throw new System.NotImplementedException();
        }

        public void MoveSampleToTargetUnit()
        {
            throw new System.NotImplementedException();
        }
    }
}
