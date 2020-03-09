using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PLCSimPP.Communication.Models;
using static System.Net.Mime.MediaTypeNames;

namespace PLCSimPP.Communication.Support
{
    public class DataHelper
    {
        public static CheckResult CheckData(byte[] rawData)
        {
            CheckResult result = new CheckResult() { Result = ResultType.InvalidCmd };

            /* Ingore first 2 bytes,it is header 
               next 2 bytes convert to int as data length
               next 5 bytes convert to hex string as unit address
               next 2 bytes convert to hex string as command
            */
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
            if (!Commands.ReceivedCmds.Contains(command))
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

        public static byte[] BuildSendData(CmdMsg msg)
        {
            byte[] head = { 0x60, 0x00 };
            byte[] unitAddr = EncoderHelper.HexStringToByteArray(msg.Unit);
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

        //public static string ConvertToString(Stream source)
        //{
        //    if (source == null)
        //    {
        //        throw (new ArgumentNullException("source"));
        //    }

        //    byte[] Buffer = new byte[System.Convert.ToInt32(source.Length - 1) + 1];

        //    source.Read(Buffer, 0, Buffer.Length);
        //    return System.Text.Encoding.ASCII.GetString(Buffer);
        //}

        //public static void Copy(Stream source, Stream target)
        //{
        //    byte[] Buffer = new byte[4096];
        //    int cnt;

        //    if (source == null)
        //    {
        //        throw (new ArgumentNullException("source"));
        //    }

        //    if (target == null)
        //    {
        //        throw (new ArgumentNullException("target"));
        //    }

        //    cnt = source.Read(Buffer, 0, Buffer.Length);
        //    while (cnt > 0)
        //    {
        //        target.Write(Buffer, 0, cnt);
        //        cnt = source.Read(Buffer, 0, Buffer.Length);
        //    }
        //}


        //public static string ConvertToCodePage437String(Stream source)
        //{
        //    if (source == null)
        //    {
        //        throw (new ArgumentNullException("source"));
        //    }

        //    byte[] Buffer = new byte[System.Convert.ToInt32(source.Length - 1) + 1];

        //    source.Read(Buffer, 0, Buffer.Length);
        //    return EncoderService.GetString(Buffer, 0, Buffer.Length);
        //}




        //public static void WriteLong(long data, Stream target)
        //{
        //    byte[] Buffer = new byte[8];

        //    if (target == null)
        //    {
        //        throw (new ArgumentNullException("target"));
        //    }

        //    Buffer[0] = System.Convert.ToByte((data & 0xFF) / Math.Pow(256, 0));
        //    Buffer[1] = System.Convert.ToByte((data & 0xFF00) / Math.Pow(256, 1));
        //    Buffer[2] = System.Convert.ToByte((data & 0xFF0000) / Math.Pow(256, 2));
        //    Buffer[3] = System.Convert.ToByte((data & 0xFF000000) / Math.Pow(256, 3));
        //    Buffer[4] = System.Convert.ToByte((data & 0xFF00000000) / Math.Pow(256, 4));
        //    Buffer[5] = System.Convert.ToByte((data & 0xFF0000000000) / Math.Pow(256, 5));
        //    Buffer[6] = System.Convert.ToByte((data & 0xFF000000000000) / Math.Pow(256, 6));
        //    Buffer[7] = System.Convert.ToByte(((System.Convert.ToUInt64(data)) & 0xFF00000000000000) / Math.Pow(256, 7));  // converted data to ulong// RT
        //    target.Write(Buffer, 0, 8);
        //}

        //public static long ReadLong(Stream source)
        //{
        //    if (source == null)
        //    {
        //        throw (new ArgumentNullException("source"));
        //    }

        //    byte[] Buffer = new byte[8];
        //    source.Read(Buffer, 0, 8);

        //    long Data = 0;
        //    Data += System.Convert.ToInt32(Buffer[0] * Math.Pow(256, 0));
        //    Data += System.Convert.ToInt32(Buffer[1] * Math.Pow(256, 1));
        //    Data += System.Convert.ToInt32(Buffer[2] * Math.Pow(256, 2));
        //    Data += System.Convert.ToInt32(Buffer[3] * Math.Pow(256, 3));
        //    Data += System.Convert.ToInt32(Buffer[4] * Math.Pow(256, 4));
        //    Data += System.Convert.ToInt32(Buffer[5] * Math.Pow(256, 5));
        //    Data += System.Convert.ToInt32(Buffer[6] * Math.Pow(256, 6));
        //    Data += System.Convert.ToInt32(Buffer[7] * Math.Pow(256, 7));
        //    return Data;
        //}

        //public static void WriteObject(object obj, Stream target)
        //{
        //    MemoryStream buffer = null;

        //    try
        //    {
        //        buffer = new MemoryStream();
        //        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        //        formatter.Serialize(buffer, obj);
        //        buffer.Position = 0;

        //        long cnt = buffer.Length;
        //        WriteLong(cnt, target);
        //        buffer.WriteTo(target);
        //    }
        //    finally
        //    {
        //        if (buffer != null)
        //        {
        //            buffer.Close();
        //        }
        //    }
        //}

        //public static object ReadObject(Stream source)
        //{
        //    if (source == null)
        //    {
        //        throw (new ArgumentNullException("source"));
        //    }

        //    MemoryStream memStream = null;
        //    object @out = null;

        //    try
        //    {
        //        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter Formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        //        long cnt = ReadLong(source);
        //        byte[] Buffer = new byte[System.Convert.ToInt32(cnt - 1) + 1];

        //        source.Read(Buffer, 0, (int)cnt);
        //        memStream = new MemoryStream(Buffer);
        //        @out = Formatter.Deserialize(memStream);

        //    }
        //    finally
        //    {
        //        if (memStream != null)
        //        {
        //            memStream.Close();
        //        }
        //    }

        //    return @out;
        //}

    }
}
