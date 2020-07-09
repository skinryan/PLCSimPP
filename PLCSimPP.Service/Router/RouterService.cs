using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Devices;
using BCI.PLCSimPP.Service.Config;
using System.Linq;

namespace BCI.PLCSimPP.Service.Router
{
    public class RouterService : IRouterService
    {
        private ObservableCollection<IUnit> mUnitCollection;

        /// <summary>
        /// FindNextDestination of current unit
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public IUnit FindNextDestination(IUnit current)
        {
            if (mUnitCollection.Count < 2)
            {
                throw new Exception("Incorrect configuration");
            }

            if (current.IsMaster)
            {
                return current.Children.First();
            }

            //if in same port
            var master = current.Parent;
            var index = master.Children.IndexOf(current);
            if (index + 1 < master.Children.Count)
            {
                return master.Children[index + 1];
            }

            //move to next port
            var masterIndex = mUnitCollection.IndexOf(master);
            if (masterIndex + 1 < mUnitCollection.Count)
            {
                //jump the I-lane and H-lane 
                return mUnitCollection[masterIndex + 1].Children.First();
            }

            //add on return to first unit of Port2            
            return mUnitCollection[1].Children.First();
        }

        /// <summary>
        /// SetSiteMap
        /// </summary>
        /// <param name="unitCollection"></param>
        public void SetSiteMap(ObservableCollection<IUnit> unitCollection)
        {
            mUnitCollection = unitCollection;
        }

        /// <summary>
        /// FindTargetUnit
        /// </summary>
        /// <param name="targetAddress"></param>
        /// <returns></returns>
        public List<IUnit> FindTargetUnit(string targetAddress)
        {
            List<IUnit> result = new List<IUnit>();
            int targetValue = int.Parse(targetAddress, System.Globalization.NumberStyles.HexNumber);

            foreach (var unit in mUnitCollection)
            {
                int unitValue = int.Parse(unit.Address, System.Globalization.NumberStyles.HexNumber);
                if ((unitValue | targetValue) == targetValue)
                {
                    result.Add(unit);
                }

                if (unit.HasChild)
                {
                    foreach (var subUnit in unit.Children)
                    {
                        int subValue = int.Parse(subUnit.Address, System.Globalization.NumberStyles.HexNumber);

                        if ((subValue | targetValue) == targetValue)
                        {
                            result.Add(subUnit);
                        }
                    }
                }
            }

            return result;
        }

       
    }
}
