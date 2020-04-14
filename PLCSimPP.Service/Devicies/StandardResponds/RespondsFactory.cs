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
                    return new ReplyMsg_0004();
                case LcCmds._0005:
                    return new ReplyMsg_0005();
                case LcCmds._0006:
                    return new ReplyMsg_0006();

                //case MsgConst.OPERATION_MODE_CONTROL0002:
                //    return new RespondsMsg0002();
                //case MsgConst.CONVEYOR_CONTROL0003:
                //    return new RespondsMsg0003();
                //case MsgConst.ALARMING_CONTROL0005:
                //    return new RespondsMsg0005();
                //case MsgConst.EMPTY_CARRIER_FEEDING_ORDER0006:
                //    return new RespondsMsg0006();
                //case MsgConst.BARCODE_REREAD_REQUEST0010:
                //    return new RespondsMsg0010();
                //case MsgConst.STORAGE_POSITIONING_ORDER0030:
                //    return new RespondsMsg0030();
                //case MsgConst.RACK_EXCHANGE_REQUEST0031:
                //    return new RespondsMsg0031();
                //case MsgConst.LABEL_FORMAT_INFORMATION0050:
                //    return new RespondsMsg0050();
                case LcCmds._0001:
                case LcCmds._0002:
                case LcCmds._0003:
                    return new ReplyEmpty();
                default:
                    throw new InvalidCommandExecption(cmd);
            }
        }



    }



}
