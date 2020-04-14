using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Service.Devicies.StandardResponds
{
    public class SendMsg
    {
        /// <summary>
        /// Key Pad Operation
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static IMessage GetMsg_1002(IUnit unit, string operation, string sid)
        {
            MsgCmd msg = new MsgCmd();
            msg.Command = UnitCmds._1002;
            msg.Port = unit.Port;
            msg.UnitAddr = unit.Address;
            msg.Param = operation + sid.PadRight(15);
            return msg;
        }

        public static IMessage GetMsg_1011(IUnit unit, string bcr)
        {
            string param = bcr + unit.CurrentSample.SampleID.PadRight(15);
            return new MsgCmd()
            {
                Command = UnitCmds._1011,
                Param = param,
                Port = unit.Port,
                UnitAddr = unit.Address
            };
        }

        /// <summary>
        /// Error Notification
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static IMessage GetMsg_10E0(IUnit unit, string errType, string errMsg)
        {
            MsgCmd msg = new MsgCmd();
            msg.Command = UnitCmds._10E0;
            msg.Port = unit.Port;
            msg.UnitAddr = unit.Address;
            msg.Param = errType + errMsg.PadRight(8);
            return msg;
        }


        /// <summary>
        /// The first command when the sample online
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static IMessage GetMsg_1024(IUnit unit, ISample sample)
        {
            MsgCmd msg = new MsgCmd();
            msg.Command = UnitCmds._1024;
            msg.Port = unit.Port;
            msg.UnitAddr = unit.Address;
            string rack = sample.Rack == Comm.RackType.Unrecognized ? "  " : ((int)sample.Rack).ToString();
            msg.Param = ParamConst.BCR_1 + sample.SampleID.PadRight(15) + rack;
            return msg;
        }

        /// <summary>
        /// Request for holders
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="request">'0': Holder Shortage cleared, '1': Holder Shortage occurred</param>
        /// <returns></returns>
        public static IMessage GetMsg_1026(IUnit unit, bool request)
        {
            MsgCmd msg = new MsgCmd();
            msg.Command = UnitCmds._1026;
            msg.Port = unit.Port;
            msg.UnitAddr = unit.Address;
            msg.Param = request ? "1" : "0";
            return msg;
        }

        /// <summary>
        /// rack exchange
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IMessage GetMsg_1016(IUnit unit, string floor, string rack)
        {
            MsgCmd msg = new MsgCmd();
            msg.Command = UnitCmds._1016;
            msg.Port = unit.Port;
            msg.UnitAddr = unit.Address;
            msg.Param = "01" + floor + rack;
            return msg;
        }

        /// <summary>
        /// 1015 for stockyard
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="recvParamFrom1017"></param>
        /// <returns></returns>
        public static IMessage GetMsg_1015(IUnit unit, string recvParamFrom1017)
        {
            string sid = recvParamFrom1017.Substring(1, 15).Trim();
            string floor = recvParamFrom1017.Substring(16, 1);
            string rack = recvParamFrom1017.Substring(17, 1);
            string position = recvParamFrom1017.Substring(18, 3);
            string rackType = recvParamFrom1017.Substring(21, 2);
            string cassette = recvParamFrom1017.Substring(23, 1);

            string param = sid.PadRight(15) + floor + rack + position + (int)Flag.Normal + "0";//Fixed value of 0
            return new MsgCmd()
            {
                Command = UnitCmds._1015,
                Param = param,
                Port = unit.Port,
                UnitAddr = unit.Address
            };
        }

        /// <summary>
        /// 1015 for connection
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static IMessage GetMsg_1015(IUnit unit)
        {
            //Fixed value when the instrument use the message
            var floor = "0";
            var rack = "1";
            var position = "001";

            string param = unit.CurrentSample.SampleID.PadRight(15) + floor + rack + position + (int)Flag.Normal + "0";//Fixed value of 0
            return new MsgCmd()
            {
                Command = UnitCmds._1015,
                Param = param,
                Port = unit.Port,
                UnitAddr = unit.Address
            };
        }
    }
}
