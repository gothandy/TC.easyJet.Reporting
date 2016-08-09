using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vincente.Formula.Test
{
    [TestClass]
    public class FromNameDomIDTests
    {
        [TestMethod]
        public void NoDomID()
        {
            Assert.IsNull(Formula.FromName.GetDomID("No Dom ID"));
        }

        [TestMethod]
        public void HappyPath()
        {
            Assert.AreEqual("D20141110.1", Formula.FromName.GetDomID("20141110.1 - Carousel2 - Destination Ads - Enable selection of 'Flag' groups"));
        }

        [TestMethod]
        public void WithTrelloEstimates()
        {
            Assert.AreEqual("D20150624.2", FromName.GetDomID("(3) 20150624.2 - Live Release (inc. Hotac)"));
        }

        [TestMethod]
        public void DoubleDigits()
        {
            Assert.AreEqual("D20150715.18", FromName.GetDomID("(5) 20150715.18 - Rebooking - Confirmation Page"));
        }

        [TestMethod]
        public void DateTooShort()
        {
            Assert.IsNull(FromName.GetDomID("(5) 2015071.18 - Rebooking - Confirmation Page"));
        }

        [TestMethod]
        public void DateTooLong()
        {
            Assert.IsNull(FromName.GetDomID("(5) 201507152.1 - Rebooking - Confirmation Page"));
        }

        [TestMethod]
        public void IncrementTooLong()
        {
            Assert.IsNull(FromName.GetDomID("(5) 20150715.183 - Rebooking - Confirmation Page"));
        }

        [TestMethod]
        public void NoIncrement()
        {
            Assert.IsNull(FromName.GetDomID("(5) 20150715. - Rebooking - Confirmation Page"));
        }

        [TestMethod]
        public void DateOnly()
        {
            Assert.IsNull(FromName.GetDomID("(5) 20150715 - Rebooking - Confirmation Page"));
        }

        [TestMethod]
        public void NullReturnsNull()
        {
            Assert.IsNull(FromName.GetDomID(null));
        }

        [TestMethod]
        public void DomIDOnly()
        {
            Assert.AreEqual("D20160805.2", FromName.GetDomID("20160805.2"));
        }
    }
}
