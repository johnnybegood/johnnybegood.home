using System;
using DateTimeExtensions;

namespace JOHNNYbeGOOD.Home.Client.Helpers
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Compare the given date to now and get difference in natural text
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ComparedToNow(this DateTime dateTime)
        {
            return dateTime.ToNaturalText(DateTime.Now);
        }
    }
}
