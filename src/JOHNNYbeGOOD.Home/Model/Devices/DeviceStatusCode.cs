namespace JOHNNYbeGOOD.Home.Model.Devices
{
    /// <summary>
    /// Code to indicate state of <see cref="IDevice"/>
    /// </summary>
    public enum DeviceStateCode
    {
        /// <summary>
        /// Device status unkown
        /// </summary>
        Unkown = 0,

        /// <summary>
        /// Faulted/Error
        /// </summary>
        Error = 1,

        /// <summary>
        /// Closed/Locked
        /// </summary>
        Closed = 1,

        /// <summary>
        /// Open/Unlocked
        /// </summary>
        Open = 2,

        /// <summary>
        /// Transitioning between states
        /// </summary>
        Transitioning = 3,
    }
}