using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Comm.Interfaces.Services
{
    public interface IMsgService
    {
        void Connect();

        void Disconnect();

        void SendMsg(IMessage msg);

       // void SetReceiveQueue(ConcurrentQueue<IMessage> queue);

        bool TryDequeue(out IMessage msg);
    }
}
