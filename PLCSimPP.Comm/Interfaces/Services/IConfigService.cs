using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Comm.Interfaces.Services
{
    /// <summary>
    /// Config service interface
    /// </summary>
    public interface IConfigService
    {       
        /// <summary>
        /// read site map setting file
        /// </summary>
        /// <returns>return unit collection</returns>
        IEnumerable<IUnit> ReadSiteMap();

        /// <summary>
        /// read site map setting file
        /// </summary>
        /// <returns>return unit collection</returns>
        IEnumerable<IUnit> ReadSiteMap(string filePath);

        /// <summary>
        /// save site map file 
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="unitCollection">content to save</param>
        void SaveSiteMap(string path, IEnumerable<IUnit> unitCollection);

        /// <summary>
        /// Read system settings file
        /// </summary>
        /// <returns>system information</returns>
        SystemInfo ReadSysConfig();

        /// <summary>
        /// Save system setting file
        /// </summary>
        /// <param name="cfg">system information instance</param>
        void SaveSysConfig(SystemInfo cfg);

    }
}
