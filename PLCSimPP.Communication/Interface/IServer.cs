using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using PLCSimPP.Communication.Models;

namespace PLCSimPP.Communication.Interface
{
    internal interface IServer
    {
        void ConnectTo(TcpClient client);
        void SendToClient(CmdMsg msg);
        void Close();
    }
}
