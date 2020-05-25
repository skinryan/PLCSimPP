﻿using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Comm.Interfaces.Services
{
    public interface IConfigService
    {
        //IEnumerable<T> ReadFile<T>() where T : UnitBase;

        //void Write<T>(List<T> unitCollection) where T : UnitBase;

        IEnumerable<IUnit> ReadSiteMap();

        void SaveSiteMap(string path, IEnumerable<IUnit> unitCollection);

        SystemInfo ReadSysConfig();

        void SaveSysConfig(SystemInfo cfg);

    }
}
