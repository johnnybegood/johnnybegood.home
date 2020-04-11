namespace JOHNNYbeGOOD.Home.Api.Contracts.Models
{
    /// <summary>
    /// Slot of a <see cref="ScheduleDTO"/>
    /// </summary>
    public class ScheduleResponseSlot
    {
        /// <summary>
        /// Identifier for slot
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Hour scheduled
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// Minutes scheduled
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// Active on Monday
        /// </summary>
        public bool OnMonday { get; set; }

        /// <summary>
        /// Active on Tuesday
        /// </summary>
        public bool OnTuesday { get; set; }

        /// <summary>
        /// Active on Wednesday
        /// </summary>
        public bool OnWednesday { get; set; }

        /// <summary>
        /// Active on Thursday
        /// </summary>
        public bool OnThursday { get; set; }

        /// <summary>
        /// Active on Friday
        /// </summary>
        public bool OnFriday { get; set; }

        /// <summary>
        /// Active on Saturday
        /// </summary>
        public bool OnSaturday { get; set; }

        /// <summary>
        /// Active on Sunday
        /// </summary>
        public bool OnSunday { get; set; }
    }
}