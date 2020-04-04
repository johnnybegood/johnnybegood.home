using System;
using DateTimeExtensions;

namespace JOHNNYbeGOOD.Home.Client.Helpers
{
    public static class DateTimeExtensions
    {
        public static string ComparedToNow(this DateTime dateTime)
        {
            return dateTime.ToNaturalText(DateTime.Now, true);
        }
    }
}
