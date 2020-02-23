namespace JOHNNYbeGOOD.Home.Model.Devices
{
    public interface II2cWriterDevice : IDevice
    {
        void SendCommand(byte[] command);
    }
}
