using System;

namespace Gothandy.DateTime
{
    public static class WeekExtensions
    {
        public static System.DateTime GetStartOfWeek(this System.DateTime self, DayOfWeek startOfWeek)
        {
            int diff = self.DayOfWeek - startOfWeek;

            if (diff < 0) diff += 7;

            return self.AddDays(-1 * diff).Date;
        }

        public static System.DateTime GetStartOfWeek(this System.DateTime self)
        {
            return self.GetStartOfWeek(DayOfWeek.Monday);
        }

        public static System.DateTime AddWeeks(this System.DateTime self, double value)
        {
            return self.AddDays(7 * value);
        }
    }
}
