using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using PLCSimPP.Comm.Configuration;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Models;
using Dapper;
using PLCSimPP.Comm.Interfaces.Services;
using Microsoft.Data.SqlClient;
using log4net.Config;
using log4net.Repository;
using log4net;
using PLCSimPP.Communication.Support;

namespace PLCSimPP.Service.Log
{
    /// <summary>
    /// Logger service
    /// </summary>
    public class LogService : ILogService
    {
        private ILog sysLogger;
        private ILog dbLogger;
        private ILog rawDataLogger;

        public LogService()
        {
            //var connString = AppConfig.Configuration.GetConnectionString("PLCSimPP");

            ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

            sysLogger = LogManager.GetLogger(repository.Name, "SYSTEM");
            dbLogger = LogManager.GetLogger(repository.Name, "COMMUNICATION");
            rawDataLogger = LogManager.GetLogger(repository.Name, "ROWDATA");
        }

        /// <summary>
        /// Write the system log to log file
        /// </summary>
        /// <param name="content">Text content</param>
        /// <param name="ex">Exception</param>
        public void LogSys(string content, Exception ex)
        {
            sysLogger.Debug(content, ex);
        }

        /// <summary>
        /// Write the system log to log file
        /// </summary>
        /// <param name="content">Text content</param>
        public void LogSys(string content)
        {
            sysLogger.Debug(content);
        }

        public void LogSys(Exception ex, string content, params string[] strs)
        {
            sysLogger.Debug(string.Format(content, strs), ex);
        }

        /// <summary>
        /// Write the message log to database
        /// </summary>
        /// <param name="message">message object</param>
        public void LogSendMsg(MsgLog message)
        {
            log4net.LogicalThreadContext.Properties["Address"] = message.Address;
            log4net.LogicalThreadContext.Properties["Command"] = message.Command;
            log4net.LogicalThreadContext.Properties["Direction"] = CommConst.SEND;
            log4net.LogicalThreadContext.Properties["Details"] = message.Details;
            log4net.LogicalThreadContext.Properties["Token"] = message.Token;
            dbLogger.Info(message);
        }

        /// <summary>
        /// Write the message log to database
        /// </summary>
        /// <param name="address">unit address</param>
        /// <param name="command">command code</param>
        /// <param name="details">content</param>
        /// <param name="token">id token</param>
        public void LogSendMsg(string address, string command, string details, string token)
        {
            log4net.LogicalThreadContext.Properties["Address"] = address;
            log4net.LogicalThreadContext.Properties["Command"] = command;
            log4net.LogicalThreadContext.Properties["Direction"] = CommConst.SEND;
            log4net.LogicalThreadContext.Properties["Details"] = details;
            log4net.LogicalThreadContext.Properties["Token"] = token;
            dbLogger.Info("");
        }

        /// <summary>
        /// Write the message log to database
        /// </summary>
        /// <param name="message">message object</param>
        public void LogRecvMsg(MsgLog message)
        {
            log4net.LogicalThreadContext.Properties["Address"] = message.Address;
            log4net.LogicalThreadContext.Properties["Command"] = message.Command;
            log4net.LogicalThreadContext.Properties["Direction"] = CommConst.RECV;
            log4net.LogicalThreadContext.Properties["Details"] = message.Details;
            log4net.LogicalThreadContext.Properties["Token"] = message.Token;
            dbLogger.Info(message);
        }

        /// <summary>
        /// Write the message log to database
        /// </summary>
        /// <param name="address">unit address</param>
        /// <param name="command">command code</param>
        /// <param name="details">content</param>
        /// <param name="token">id token</param>
        public void LogRecvMsg(string address, string command, string details, string token)
        {
            log4net.LogicalThreadContext.Properties["Address"] = address;
            log4net.LogicalThreadContext.Properties["Command"] = command;
            log4net.LogicalThreadContext.Properties["Direction"] = CommConst.RECV;
            log4net.LogicalThreadContext.Properties["Details"] = details;
            log4net.LogicalThreadContext.Properties["Token"] = token;
            dbLogger.Info("");
        }

        public void Dispose()
        {

        }

        public void LogRawData(string addrAndPort, byte[] dataContent)
        {
            rawDataLogger.Debug(string.Format("{0} {1}", addrAndPort, EncoderHelper.ToHexString(dataContent)));
        }
    }
}
