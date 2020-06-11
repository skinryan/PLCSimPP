using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Comm.Interfaces.Services
{
    /// <summary>
    /// Router Service 
    /// </summary>
    public interface IRouterService
    {
        /// <summary>
        /// Set loaded units setting to router service
        /// </summary>
        /// <param name="unitCollection">unit settings</param>
        void SetSiteMap(ObservableCollection<IUnit> unitCollection);

        /// <summary>
        /// Find target unit by address
        /// </summary>
        /// <param name="targetAddr">target address</param>
        /// <returns>matching units</returns>
        List<IUnit> FindTargetUnit(string targetAddr);

        /// <summary>
        /// Find sample next destination
        /// </summary>
        /// <param name="current">current unit</param>
        /// <returns>next unit</returns>
        IUnit FindNextDestination(IUnit current);

        
    }
}
