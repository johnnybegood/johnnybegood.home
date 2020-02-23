namespace JOHNNYbeGOOD.Home.Model.Devices
{
    public interface IDependedDevice<TDevice> : IDevice
        where TDevice : IDevice
    {
        void Use(TDevice connector);
    }
}
