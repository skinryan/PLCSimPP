﻿using System;
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


        public void SetSiteMap(ObservableCollection<IUnit> unitColleciton)
        {
            mUnitCollection = unitColleciton;
        }

        public List<IUnit> FindTargetUnit(string targetAddr)
        {
            List<IUnit> result = new List<IUnit>();
            int targetValue = int.Parse(targetAddr, System.Globalization.NumberStyles.HexNumber);

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

        //public IUnit FindNextPort(IUnit current)
        //{
        //    int masterIndex;
        //    if (current.IsMaster)
        //    {
        //        masterIndex = mUnitCollection.IndexOf(current);
        //    }
        //    else
        //    {
        //        var master = current.Parent;
        //        masterIndex = mUnitCollection.IndexOf(master);
        //    }

        //    return mUnitCollection[masterIndex + 1].Children.First();
        //}
    }
}
