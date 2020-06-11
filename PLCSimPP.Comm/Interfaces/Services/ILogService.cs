using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Models;
using Prism.Logging;

namespace BCI.PLCSimPP.Comm.Interfaces.Services
{
    /// <summary>
    /// Log service interface
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Write system log
        /// </summary>
        /// <param name="content">log content</param>
        /// <param name="ex">exception</param>
        void LogSys(string content, Exception ex);

        /// <summary>
        /// Write system log
        /// </summary>
        /// <param name="content">log content</param>
        void LogSys(string content);

        /// <summary>
        /// Write communication raw data
        /// </summary>
        /// <param name="addrAndPort">address and port</param>
        /// <param name="dataContent">data</param>
        void LogRawData(string addrAndPort, byte[] dataContent);

        /// <summary>
        /// Write PLCSim sent message
        /// </summary>
        /// <param name="message">message instance</param>
        void LogSendMsg(MsgLog message);

        /// <summary>
        /// Write PLCSim sent message
        /// </summary>
        /// <param name="address">address </param>
        /// <param name="command">command</param>
        /// <param name="details">command param</param>
        /// <param name="token">token</param>
        void LogSendMsg(string address, string command, string details, string token = "");

        /// <summary>
        /// Write PLCSim received message
        /// </summary>
        /// <param name="message">message instance</param>
        void LogRecvMsg(MsgLog message);

        /// <summary>
        /// Write PLCSim received message
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="command">command</param>
        /// <param name="details">command param</param>
        /// <param name="token">token</param>
        void LogRecvMsg(string address, string command, string details, string token = "");
    }
}
