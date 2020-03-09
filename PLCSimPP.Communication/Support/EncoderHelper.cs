using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using PLCSimPP.Communication.Execption;
using PLCSimPP.Communication.Models;

namespace PLCSimPP.Communication.Support
{
    public static class EncoderHelper
    {
        // public const int CODE_PAGE = 437;

        public static byte[] GetBytes(string s)
        {
            //Encoding extendedAsciiEncoding = Encoding.GetEncoding(CODE_PAGE);
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            return bytes;
        }


        public static int GetBytes(string s, int index, int count, byte[] bytes, int indexByte)
        {
            //Encoding extendedAsciiEncoding = Encoding.GetEncoding(CODE_PAGE);
            return Encoding.ASCII.GetBytes(s, index, count, bytes, indexByte);
        }


        //public static char[] GetChars(byte[] bytes, int index, int count)
        //{
        //    Encoding extendedAsciiEncoding = Encoding.GetEncoding(CODE_PAGE);
        //    return extendedAsciiEncoding.GetChars(bytes, index, count);
        //}

        //public static string GetString(byte[] bytes)
        //{
        //    Encoding extendedAsciiEncoding = Encoding.GetEncoding(CODE_PAGE);
        //    return IgnoreAfterNullString(extendedAsciiEncoding.GetString(bytes, 0, bytes.Length));
        //}

        /// <summary>
        /// Ignore after null string
        /// </summary>
        /// <param name="str"></param>
        /// <returns>converted string ignore after null byte</returns>
        public static string IgnoreAfterNullString(string str)
        {
            var ret = str;
            if (str.IndexOf('\0') != -1)
            {
                ret = str.Substring(0, str.IndexOf('\0'));
            }
            return ret;
        }

        //public static string GetString(char[] bytes)
        //{
        //    return new string(bytes);
        //}

        public static string GetString(byte[] bytes, int index, int count)
        {
            //Encoding extendedAsciiEncoding = Encoding.GetEncoding(CODE_PAGE);
            return Encoding.ASCII.GetString(bytes, index, count);
        }

        public static string GetString(byte[] bytes)
        {
            //Encoding Encoding.ASCII.GetBytes = Encoding.GetEncoding(CODE_PAGE);
            return Encoding.ASCII.GetString(bytes);
        }

        public static int GetInt(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }

            // Convert string to current code page.
            byte[] bytes = GetBytes(s);

            // Return first character.
            return bytes[0];
        }

        public static int GetInt(byte[] bytes, int offset, int byteLength)
        {
            var converBytes = new byte[byteLength];
            Array.Copy(bytes, offset, converBytes, 0, byteLength);


            var hexString = new StringBuilder();
            foreach (var tmpByte in converBytes)
                hexString.Append(tmpByte.ToString("X2"));

            var intValue = int.Parse(hexString.ToString(), NumberStyles.AllowHexSpecifier);
            return intValue;
        }

        public static int GetInt(byte[] bytes)
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            var converBytes = new byte[bytes.Length];
            Array.Copy(bytes, 0, converBytes, 0, bytes.Length);


            var hexString = new StringBuilder();
            foreach (var tmpByte in converBytes) hexString.Append(tmpByte.ToString("X2"));

            var intValue = int.Parse(hexString.ToString(), NumberStyles.AllowHexSpecifier);
            return intValue;
        }
        public static bool GetBool(byte[] bytes)
        {
            return GetInt(bytes) != 0;
        }

        public static Int16 GetInt16(byte[] bytes)
        {
            var intValue = BitConverter.ToInt16(bytes, 0);

            return intValue;
        }
        public static Int16 GetIntOneByte(byte[] bytes)
        {
            var tempBytes = new byte[] { bytes[0], 0 };
            var intValue = BitConverter.ToInt16(tempBytes, 0);

            return intValue;
        }



        public static byte GetByte(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }

            // Convert string to current code page.
            byte[] bytes = GetBytes(s);

            // Return first character.
            return bytes[0];
        }


        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }



        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE 00 CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                var strB = new StringBuilder();

                foreach (var t in bytes)
                {
                    strB.Append(t.ToString("X2") + " ");
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        public static string ToHexStringWithoutSpace(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                var strB = new StringBuilder();

                foreach (var t in bytes)
                {
                    strB.Append(t.ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        public static string Hex2Bin(string strHex)
        {
            var decNumber = Convert.ToInt64(strHex, 16);
            return Convert.ToString(decNumber, 2);
        }
        public static string Bin2Hex(string strBin)
        {
            var decNumber = Convert.ToInt64(strBin, 2);
            return "0x" + Convert.ToString(decNumber, 16);
        }
        public static string Hex2Bin(uint hex)
        {
            return Convert.ToString(hex, 2);
        }

        public static uint HexString2Uint(string hexString)
        {
            return Convert.ToUInt32(hexString, 16);
        }
        public static void Hex2Bit(byte[] bytes, int validIndex, int validLength, int offset, ref List<int> bitCodes)
        {
            //var ret = new List<int>();
            var hexString = ToHexStringWithoutSpace(bytes);
            hexString = hexString.Substring(validIndex * 2, validLength * 2);
            var b = Hex2Bin(hexString);
            var arr = b.ToCharArray();
            Array.Reverse(arr);
            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] == '1')
                {
                    bitCodes.Add(i + offset);
                }
            }
            //return ret;
        }

        public static List<int> Hex2Bit(byte[] bytes, int validLength, int offset)
        {
            var ret = new List<int>();
            var hexString = ToHexStringWithoutSpace(bytes);
            hexString = hexString.Substring(0, validLength * 2);
            var b = Hex2Bin(hexString);
            var arr = b.ToCharArray();
            Array.Reverse(arr);
            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] == '1')
                {
                    ret.Add(i + offset);
                }
            }
            return ret;
        }


        public static List<int> Hex2Bit(uint hex, int offset)
        {
            var ret = new List<int>();

            var aBin = Convert.ToString(hex, 2);
            //var aBinString = aBin.PadLeft(32, '0');
            var arr = aBin.ToCharArray();
            Array.Reverse(arr);
            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] == '1')
                {
                    ret.Add(i + offset);
                }
            }
            return ret;
        }

        public static CmdMsg ConvertToMsg(byte[] rawData)
        {
            if (rawData == null)
            {
                throw (new ArgumentNullException("source"));
            }

            CmdMsg result = new CmdMsg();

            try
            {
                /* 
                   Ingore first 2 bytes,it is header 
                   next 2 bytes convert to int as data length
                   next 5 bytes convert to hex string as unit address
                   next 2 bytes convert to hex string as command
                */
                byte[] lenBuffer = new byte[2];

                Array.Copy(rawData, 2, lenBuffer, 0, 2);
                Array.Reverse(lenBuffer);
                int length = EncoderHelper.GetInt(lenBuffer) * 2;

                byte[] contentBuffer = new byte[length];
                Array.Copy(rawData, 4, contentBuffer, 0, length);

                byte[] addressBuffer = new byte[5];
                Array.Copy(contentBuffer, 0, addressBuffer, 0, 5);

                byte[] cmdBuffer = new byte[2];
                Array.Copy(contentBuffer, 5, cmdBuffer, 0, 2);
                Array.Reverse(cmdBuffer);

                byte[] paramBuffer = new byte[length - 7];
                Array.Copy(contentBuffer, 7, paramBuffer, 0, length - 7);

                result.Unit = ToHexStringWithoutSpace(addressBuffer);
                result.Command = ToHexStringWithoutSpace(cmdBuffer);
                result.Param = Encoding.ASCII.GetString(paramBuffer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return result;
        }


    }
}
