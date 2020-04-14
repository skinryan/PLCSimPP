using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;
using PLCSimPP.Communication.Models;

namespace PLCSimPP.Communication.Interface
{
    internal interface IServer
    {
        void ConnectTo(TcpClient client);
        void SendToClient(IMessage msg);
        void Close();
    }
}
