using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Communication.Execption;

namespace BCI.PLCSimPP.Service.Devicies.StandardResponds
{

    public static class RespondsFactory
    {
        public static IResponds GetRespondsHandler(string cmd)
        {
            switch (cmd)
            {
                case LcCmds._0004:
                    return new ReplyMsg_0004();
                case LcCmds._0005:
                    return new ReplyMsg_0005();
                case LcCmds._0006:
                    return new ReplyMsg_0006();
                default:
                    return new ReplyEmpty();
            }
        }



    }



}
