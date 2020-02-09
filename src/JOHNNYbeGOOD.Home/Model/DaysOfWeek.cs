using System;

namespace JOHNNYbeGOOD.Home.Model
{
    [Flags]
    public enum DaysOfWeek
    {
        /// <summary>
        /// No days selected
        /// </summary>
        None = 0,
		/// <summary>Indicates Monday.</summary>
		Monday = 1,
		/// <summary>Indicates Tuesday.</summary>
		Tuesday = 2,
		/// <summary>Indicates Wednesday.</summary>
		Wednesday = 4,
		/// <summary>Indicates Thursday.</summary>
		Thursday = 8,
		/// <summary>Indicates Friday.</summary>
		Friday = 16,
		/// <summary>Indicates Saturday.</summary>
		Saturday = 32,
		/// <summary>Indicates Sunday.</summary>
		Sunday = 64,

        EveryDay = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday
	}
}