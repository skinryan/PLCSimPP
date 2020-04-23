using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Communication.Execption;

namespace PLCSimPP.Service.Devicies.StandardResponds
{

    public static class RespondsFactory
    {
        public static IResponds GetRespondsHandler(string cmd)
        {
            switch (cmd)
            {
                case LcCmds._0004:
                    return new ReplyMsg_1001();
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
