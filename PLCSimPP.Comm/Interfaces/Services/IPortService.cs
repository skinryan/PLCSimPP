using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.PLCSimPP.Comm.Interfaces.Services
{
    /// <summary>
    /// Port service interface
    /// </summary>
    public interface IPortService
    {
        /// <summary>
        /// Start ports service
        /// </summary>
        void Connect();

        /// <summary>
        /// Stop ports service
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Send message to LC
        /// </summary>
        /// <param name="msg">message instance</param>
        void SendMsg(IMessage msg);

        /// <summary>
        /// Try get received message from LC 
        /// </summary>
        /// <param name="msg">message instance</param>
        /// <returns></returns>
        bool TryDequeue(out IMessage msg);
    }
}
