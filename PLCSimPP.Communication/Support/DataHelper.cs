using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Communication.Models;
using static System.Net.Mime.MediaTypeNames;

namespace BCI.PLCSimPP.Communication.Support
{
    public class DataHelper
    {
        public static CheckResult CheckData(byte[] rawData)
        {
            CheckResult result = new CheckResult() { Result = ResultType.InvalidCmd };

            //length ==2 ,think of it as a confirm msg
            if (rawData.Length == 2)
            {
                result.Result = ResultType.Confirm;
                return result;
            }

            /* Ingore first 2 bytes,it is header 
               next 2 bytes convert to int as data length
               next 5 bytes convert to hex string as unit address
               next 2 bytes convert to hex string as command */
            byte[] lenBuffer = new byte[2];
            Array.Copy(rawData, 2, lenBuffer, 0, 2);
            Array.Reverse(lenBuffer);

            int length = EncoderHelper.GetInt(lenBuffer) * 2;

            //if length less then 8 ,think of it as a heartbeat
            if (length < 8)
            {
                result.Result = ResultType.Heartbeat;
                return result;
            }

            //check length
            if (rawData.Length != length + 4)
            {
                result.Result = ResultType.InvalidLength;
                result.ExpectLength = length;
                result.ActualLength = rawData.Length - 4;
                return result;
            }

            byte[] contentBuffer = new byte[length];
            Array.Copy(rawData, 4, contentBuffer, 0, length);

            //check command 
            byte[] cmdBuffer = new byte[2];
            Array.Copy(contentBuffer, 5, cmdBuffer, 0, 2);
            Array.Reverse(cmdBuffer);

            string command = EncoderHelper.ToHexStringWithoutSpace(cmdBuffer);
            if (!LcCmds.ReceivedCmds.Contains(command))
            {
                result.Result = ResultType.InvalidCmd;
                result.InvalidCommand = command;
                return result;
            }

            // normal result
            result.Result = ResultType.RawData;
            result.Content = contentBuffer;
            return result;
        }

        public static byte[] BuildSendData(IMessage msg)
        {
            byte[] head = { 0x60, 0x00 };
            byte[] unitAddr = EncoderHelper.HexStringToByteArray(msg.UnitAddr);
            byte[] cmd = EncoderHelper.HexStringToByteArray(msg.Command);
            byte[] param = EncoderHelper.GetBytes(msg.Param);

            Int16 totalLen = Convert.ToInt16(unitAddr.Length + cmd.Length + param.Length);
            if (totalLen % 2 != 0)
            {
                totalLen += 1;
            }

            byte[] result = new byte[totalLen + 4];

            //write header
            Array.Copy(head, 0, result, 0, head.Length);

            //write length 
            byte[] length = System.BitConverter.GetBytes(totalLen / 2);
            Array.Copy(length, 0, result, 2, length.Length);

            //write addr
            Array.Copy(unitAddr, 0, result, 4, unitAddr.Length);

            //write cmd
            Array.Reverse(cmd);
            Array.Copy(cmd, 0, result, 9, cmd.Length);

            //write param
            Array.Copy(param, 0, result, 11, param.Length);

            return result;
        }

        
    }
}
