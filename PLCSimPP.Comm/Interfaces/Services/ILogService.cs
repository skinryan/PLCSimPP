using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Models;
using Prism.Logging;

namespace PLCSimPP.Comm.Interfaces.Services
{
    public interface ILogService
    {
        void LogSys(string content, Exception ex);

        void LogSys(Exception ex, string content, params string[] strs);

        //void LogSys(Category logLevel, Exception ex, string content);

        void LogSys(string content);

        void LogRawData(string addrAndPort, byte[] dataContent);

        void LogSendMsg(MsgLog message);

        void LogSendMsg(string address, string command, string details, string token);

        void LogRecvMsg(MsgLog message);

        void LogRecvMsg(string address, string command, string details, string token);
    }
}
