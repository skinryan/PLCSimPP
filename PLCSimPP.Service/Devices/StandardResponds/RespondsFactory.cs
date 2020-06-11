using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Communication.Exceptions;

namespace BCI.PLCSimPP.Service.Devices.StandardResponds
{
    /// <summary>
    /// Common responds factory
    /// </summary>
    public static class RespondsFactory
    {
        public static IResponds GetRespondsHandler(string cmd)
        {
            switch (cmd)
            {
                case LcCmds._0004:
                    return new ReplyMsg0004();
                case LcCmds._0005:
                    return new ReplyMsg0005();
                case LcCmds._0006:
                    return new ReplyMsg0006();
                default:
                    return new ReplyEmpty();
            }
        }
    }
}
