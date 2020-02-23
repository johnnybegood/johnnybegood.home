using JOHHNYbeGOOD.Home.Resources.Connectors;

namespace JOHHNYbeGOOD.Home.Resources.Devices
{
    internal interface IRpiDevice
    {
        void Connect(IRpiConnectionFactory factory);
    }
}