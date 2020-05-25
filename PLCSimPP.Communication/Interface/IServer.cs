using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Communication.Models;

namespace BCI.PLCSimPP.Communication.Interface
{
    internal interface IServer
    {
        void ConnectTo(TcpClient client);
        void SendToClient(IMessage msg);
        void Close();
    }
}
