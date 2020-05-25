using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Communication.Interface;
using BCI.PLCSimPP.Communication.Models;

namespace BCI.PLCSimPP.Communication.Support
{
    internal sealed class Server : SmartConnection, IServer
    {
        #region "Constructor"

        public Server()
        {
        }

        #endregion

        #region "IServer"
        
        public void ServerConnect(TcpClient client)
        {
            base.ConnectTo(client);
        }

        public void SendToClient(IMessage msg)
        {
            base.Send(msg);
        }

        public void Close()
        {
            base.Disconnect();
        }

        #endregion // IServer
    }
}
