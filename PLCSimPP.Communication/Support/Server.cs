using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using PLCSimPP.Communication.Interface;
using PLCSimPP.Communication.Models;

namespace PLCSimPP.Communication.Support
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

        public void SendToClient(CmdMsg msg)
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
