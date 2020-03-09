using PLCSimPP.Communication.Support;

namespace PLCSimPP.Communication.Interface
{
    public interface ITcpIpServerConnection
    {
        OpenStatus Open(string name, string address, int port);
    }
}