using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.PLCSimPP.Comm.Interfaces.Services
{
    public interface IPortService
    {
        void Connect();

        void Disconnect();

        void SendMsg(IMessage msg);

        // void SetReceiveQueue(ConcurrentQueue<IMessage> queue);

        bool TryDequeue(out IMessage msg);
    }
}
