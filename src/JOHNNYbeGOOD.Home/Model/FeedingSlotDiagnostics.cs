namespace JOHNNYbeGOOD.Home.Model
{
    /// <summary>
    /// Diagnostics of the feedingslots
    /// </summary>
    public class FeedingSlotDiagnostics
    {
        /// <summary>
        /// Id of the feedingslot
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Flag to indicate of the slot can be opened or is open
        /// </summary>
        public bool CanOpen { get; set; }
    }
}
