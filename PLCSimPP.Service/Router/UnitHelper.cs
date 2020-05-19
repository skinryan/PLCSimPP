using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PLCSimPP.Comm.CustomExecption;
using PLCSimPP.Comm.Interfaces;

namespace PLCSimPP.Service.Router
{
    public static class UnitHelper
    {
        //find helper
        public static List<IUnit> FindTargetUnit(this IEnumerable<IUnit> units, string targetAddr)
        {
            List<IUnit> result = new List<IUnit>();
            int targetValue = int.Parse(targetAddr, System.Globalization.NumberStyles.HexNumber);

            foreach (var unit in units)
            {
                int unitValue = int.Parse(unit.Address, System.Globalization.NumberStyles.HexNumber);
                if ((unitValue | targetValue) == targetValue)
                {
                    result.Add(unit);

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
            }

            return result;
        }


        //public static string PadRight(this string smapleid)
        //{
        //    return smapleid.PadRight(15);
        //}

    }
}
