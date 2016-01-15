using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gothandy.DateTime.Test
{
    [TestClass]
    public class WeekTests
    {
        [TestMethod]
        public void GetStartOfWeek()
        {
            var dt = new System.DateTime(2016, 1, 14);

            Assert.AreEqual(new System.DateTime(2016, 1, 11), dt.GetStartOfWeek());
        }

        [TestMethod]
        public void GetStartOfWeekSunday()
        {
            var dt = new System.DateTime(2016, 1, 14);

            Assert.AreEqual(new System.DateTime(2016, 1, 10), dt.GetStartOfWeek(DayOfWeek.Sunday));
        }

        [TestMethod]
        public void AddWeeks()
        {
            var dt = new System.DateTime(2016, 1, 14);

            Assert.AreEqual(new System.DateTime(2016, 2, 4), dt.AddWeeks(3));
        }
    }
}
