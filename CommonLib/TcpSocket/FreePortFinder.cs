using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;

namespace CommonLib.TcpSocket
{
    /// <summary>
    /// Find a port that is unused/free.
    /// If no port numbers are passed to the constructor, the private ports will be searched.
    /// </summary>
    public class FreePortFinder
    {
        private const int MIN_PRIVATE_PORT_NUMBER = 49152;
        private const int MAX_PRIVATE_PORT_NUMBER = 65535;

        private readonly int mBeginningPortNumber;
        private readonly int mEndingPortNumber;

        // Constructor(s) ==============================================================
        
        public FreePortFinder(
            int beginningPortNumberArg = MIN_PRIVATE_PORT_NUMBER, 
            int endingPortNumberArg = MAX_PRIVATE_PORT_NUMBER)
        {
            mBeginningPortNumber = beginningPortNumberArg;
            mEndingPortNumber = endingPortNumberArg;
        }

        // Methods(s) - Public =========================================================

        /// <summary>
        /// Find the first free port.
        /// Note that even if port x is currently available, it may be unavailable
        /// by the time the client tries to use it, so the client may have to requery.
        /// </summary>
        /// <returns>the free port or 0 if none found</returns>
        public int GetAnAvailablePort()
        {
            var unavailablePorts = GetSortedUnavailablePorts(mBeginningPortNumber);
            for (int portNum = mBeginningPortNumber; portNum <= mEndingPortNumber; portNum++)
            {
                int index = unavailablePorts.BinarySearch(portNum);
                if (index < 0) // not found
                {
                    return portNum;
                }
            }

            return 0;
        }

        // Methods(s) - Private ========================================================
        
        /// <summary>
        /// Find the unavaible ports.  Returns the sorte list of port numbers.
        /// </summary>
        /// <param name="startingPort"></param>
        /// <returns></returns>
        private List<int> GetSortedUnavailablePorts(int startingPort)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            List<int> resultUnavailablePorts = new List<int>();

            //getting active connections
            TcpConnectionInformation[] connections = ipGlobalProperties.GetActiveTcpConnections();
            resultUnavailablePorts.AddRange(from n in connections
                                            where n.LocalEndPoint.Port >= startingPort
                                            select n.LocalEndPoint.Port);

            //getting active tcp listners
            IPEndPoint[] endPoints = ipGlobalProperties.GetActiveTcpListeners();
            resultUnavailablePorts.AddRange(from n in endPoints
                                            where n.Port >= startingPort
                                            select n.Port);

            //getting active udp listeners
            endPoints = ipGlobalProperties.GetActiveUdpListeners();
            resultUnavailablePorts.AddRange(from n in endPoints
                                            where n.Port >= startingPort
                                            select n.Port);

            resultUnavailablePorts.Sort();

            return resultUnavailablePorts;
        }
    }
}