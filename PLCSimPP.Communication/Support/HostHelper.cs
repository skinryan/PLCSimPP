using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PLCSimPP.Communication.Support
{
    public sealed class HostHelper
    {
        #region "[CStr]"

        public static string @CStr(string str)
        {
            if (str == null)
            {
                return "String is null.";
            }
            else if (str.Length == 0)
            {
                return "String is empty.";
            }
            else
            {
                return string.Format(CultureInfo.InvariantCulture, "({0}) {1}", str.Length, CString(str));
            }
        }

        #endregion

        #region "CStrings"
        public static string CString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            var translatedData = new StringBuilder();

            for (var i = 0; i <= str.Length - 1; i++)
            {
                char data = char.Parse(Convert.ToString(str[i]));
                translatedData.Append(CString(data));
            }

            return translatedData.ToString();
        }

        public static string CString(char val)
        {
            switch (val)
            {
                case '\0':
                    return "<NUL>";
                case '\u0001':
                    return "<SOH>";
                case '\u0002':
                    return "<STX>";
                case '\u0003':
                    return "<ETX>";
                case '\u0004':
                    return "<EOT>";
                case '\u0005':
                    return "<ENQ>";
                case '\u0006':
                    return "<ACK>";
                case '\a':
                    return "<BEL>";
                case '\b':
                    return "<BS>";
                case '\t':
                    return "<HT>";
                case '\n':
                    return "<LF>";
                case '\v':
                    return "<VT>";
                case '\f':
                    return "<FF>";
                case '\r':
                    return "<CR>";
                case '\u000E':
                    return "<SO>";
                case '\u000F':
                    return "<SI>";
                case '\u0010':
                    return "<DLE>";
                case '\u0011':
                    return "<DC1>";
                case '\u0012':
                    return "<DC2>";
                case '\u0013':
                    return "<DC3>";
                case '\u0014':
                    return "<DC4>";
                case '\u0015':
                    return "<NAK>";
                case '\u0016':
                    return "<SYN>";
                case '\u0017':
                    return "<ETB>";
                case '\u0018':
                    return "<CAN>";
                case '\u0019':
                    return "<EM>";
                case '\u007F':
                    return "<DEL>";
                default:
                    return val.ToString();
            }
        }
        #endregion
    }
}
