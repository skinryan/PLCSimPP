using BCI.PLCSimPP.Communication.Support;

namespace BCI.PLCSimPP.Communication.Interface
{
    public interface ITcpIpServerConnection
    {
        OpenStatus Open(string name, string address, int port);
    }
}