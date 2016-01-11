using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Vincente.Formula.Test
{
    [TestClass]
    public class FromLablelsToEpic
    {
        [TestMethod]
        public void SimpleTest()
        {
            List<string> labels = new List<string> { "eJ Rebooking", "BLOCKED"};

            Assert.AreEqual("Rebooking", FromLabels.GetEpic(labels));
        }

        [TestMethod]
        public void TwoLabels()
        {
            List<string> labels = new List<string> { "eJ Rebooking", "eJ Redesign" };

            Assert.IsNull(FromLabels.GetEpic(labels));
        }


        [TestMethod]
        public void NoEpics()
        {
            List<string> labels = new List<string> { "Resuse DA", "BLOCKED" };

            Assert.IsNull(FromLabels.GetEpic(labels));
        }

        [TestMethod]
        public void GetEpicsOK()
        {
            List<string> labels = new List<string> { "eJ Project 1", "BLOCKED", "eJ Project 2" };

            Assert.AreEqual(2, FromLabels.GetEpics(labels).Count);
        }
    }
}
