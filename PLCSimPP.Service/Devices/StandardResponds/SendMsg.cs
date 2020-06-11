using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Enums;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Devices;

namespace BCI.PLCSimPP.Service.Devices.StandardResponds
{
    /// <summary>
    /// Build send message helper
    /// </summary>
    public class SendMsg
    {
        /// <summary>
        /// Key Pad Operation
        /// </summary>
        /// <param name="unit">unit</param>
        /// <param name="activePanel">panel number</param>
        /// <param name="operation"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static IMessage GetMsg1002(IUnit unit, string activePanel, string operation, string sid)
        {
            return new MsgCmd
            {
                Command = UnitCmds._1002,
                Port = unit.Port,
                UnitAddr = unit.Address,
                Param = activePanel + operation + sid.PadRight(15)
            };
        }

        /// <summary>
        /// sample arrived notification
        /// </summary>
        /// <param name="unit">unit</param>
        /// <param name="bcr">bar code reader No</param>
        /// <returns></returns>
        public static IMessage GetMsg1011(IUnit unit, string bcr)
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
        /// <param name="unit">unit</param>
        /// <param name="errType">error type</param>
        /// <param name="errMsg">error message</param>
        /// <returns></returns>
        public static IMessage GetMsg10E0(IUnit unit, string errType, string errMsg)
        {
            return new MsgCmd
            {
                Command = UnitCmds._10E0,
                Port = unit.Port,
                UnitAddr = unit.Address,
                Param = errType + errMsg.PadRight(8)
            };
        }

        /// <summary>
        /// Holder Request
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="condition">'0': Holder Shortage cleared, '1': Holder Shortage occurred</param>
        /// <returns></returns>
        public static IMessage GetMsg1026(IUnit unit, string condition)
        {
            return new MsgCmd
            {
                Command = UnitCmds._1026,
                Port = unit.Port,
                UnitAddr = unit.Address,
                Param = condition
            };
        }

        /// <summary>
        /// The first command when the sample online
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static IMessage GetMsg1024(IUnit unit)
        {
            string rack = unit.CurrentSample.Rack == RackType.Unrecognized ? "  " : ((int)unit.CurrentSample.Rack).ToString();

            return new MsgCmd
            {
                Command = UnitCmds._1024,
                Port = unit.Port,
                UnitAddr = unit.Address,
                Param = ParamConst.BCR_1 + unit.CurrentSample.SampleID.PadRight(15) + rack
            };
        }

        /// <summary>
        /// rack exchange
        /// </summary>
        /// <param name="unit">unit</param>
        /// <param name="floor">floor</param>
        /// <param name="rack">rack</param>
        /// <returns></returns>
        public static IMessage GetMsg1016(IUnit unit, string floor, string rack)
        {
            return new MsgCmd
            {
                Command = UnitCmds._1016,
                Port = unit.Port,
                UnitAddr = unit.Address,
                Param = "01" + floor + rack
            };
        }

        /// <summary>
        /// 1015 for stockyard
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="recvParamFrom0017"></param>
        /// <returns></returns>
        public static IMessage GetMsg1015(IUnit unit, string recvParamFrom0017)
        {
            string sid = recvParamFrom0017.Substring(1, 15).Trim();
            string floor = recvParamFrom0017.Substring(16, 1);
            string rack = recvParamFrom0017.Substring(17, 1);
            string position = recvParamFrom0017.Substring(18, 3);
            //string rackType = recvParamFrom0017.Substring(21, 2);
            string cassette = recvParamFrom0017.Substring(23, 1);

            string param = sid.PadRight(15) + floor + rack + position + (int)Flag.Normal + cassette;
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
        public static IMessage GetMsg1015(IUnit unit)
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
