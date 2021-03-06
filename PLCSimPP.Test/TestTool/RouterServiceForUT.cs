﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;

namespace BCI.PLCSimPP.Test.TestTool
{
    public class RouterServiceForUT : IRouterService
    {
        public IUnit LastFindNextUnit { get; set; }

        public IUnit LastFindTargetUnit { get; set; }

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


    }

}
