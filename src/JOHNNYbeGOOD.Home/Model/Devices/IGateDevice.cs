using System;
using System.Threading.Tasks;

namespace JOHNNYbeGOOD.Home.Model.Devices
{
    /// <summary>
    /// Gate control device
    /// </summary>
    public interface IGateDevice : IDevice
    {
        /// <summary>
        /// Open the gate.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the gate cannot be opened</exception>
        /// <returns></returns>
        Task OpenGateAsync();
    }
}
