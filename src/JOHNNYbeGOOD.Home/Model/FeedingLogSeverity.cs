namespace JOHNNYbeGOOD.Home.Models
{
    /// <summary>
    /// Severity for <see cref="FeedingLogg""/>
    /// </summary>
    public enum FeedingLogResult
    {
        /// <summary>
        /// Severity unknown
        /// </summary>
        Unkown = 0,

        /// <summary>
        /// Successfull feeding
        /// </summary>
        Successfull = 1,

        /// <summary>
        /// Failed feeding
        /// </summary>
        Failed = 2,
    }
}