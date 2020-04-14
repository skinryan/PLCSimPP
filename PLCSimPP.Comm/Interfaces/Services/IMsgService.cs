using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Comm.Interfaces.Services
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
