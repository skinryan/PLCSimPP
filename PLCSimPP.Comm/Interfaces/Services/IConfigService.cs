using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Comm.Interfaces.Services
{
    public interface IConfigService
    {
        //IEnumerable<T> ReadFile<T>() where T : UnitBase;

        //void Write<T>(List<T> unitCollection) where T : UnitBase;

        List<IUnit> ReadSiteMap();

        void SaveSiteMap();

        void ReadSysConfig();

        void SaveSysConfig();

    }
}
