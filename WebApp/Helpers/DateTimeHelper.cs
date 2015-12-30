using System;

namespace Vincente.WebApp.Helpers
{
    public class DateTimeHelper
    {
        public static string Format(DateTime? dateTime)
        {
            return string.Format("{0:d}", dateTime);
        }
        public static string Format(DateTime dateTime)
        {
            return string.Format("{0:d}", dateTime);
        }

        public static string GetPeriodFromNow(DateTime past)
        {
            TimeSpan ts = DateTime.Now.Subtract(past);

            if (ts.Days != 0) return GetPeriod(ts.Days, "Day");
            if (ts.Hours != 0) return GetPeriod(ts.Hours, "Hour");
            if (ts.Minutes != 0) return GetPeriod(ts.Minutes, "Minute");
            return GetPeriod(ts.Seconds, "Second");
        }

        private static string GetPeriod(int count, string period)
        {
            var format = (count == 1) ? "{0} {1}" : "{0} {1}s";

            return string.Format(format, count, period);
        }
    }
}