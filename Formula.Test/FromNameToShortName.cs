using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Vincente.Formula.Test
{
    [TestClass]
    public class FromNameToShortName
    {
        [TestMethod]
        public void Simple()
        {
            Assert.AreEqual(
                "Carousel2 - Destination Ads - Enable selection of 'Flag' groups",
                FromName.GetShortName("20141110.1 - Carousel2 - Destination Ads - Enable selection of 'Flag' groups"));
        }

        [TestMethod]
        public void WithTrelloEstimate()
        {
            Assert.AreEqual(
                "Widget pages - Third party Code",
                FromName.GetShortName("(2) 20150210.1 - Widget pages - Third party Code"));
        }

        [TestMethod]
        public void ApostropheReplace()
        {
            Assert.AreEqual(
                "Marketing Pages - Set Sitemap 'Tickbox' and 'Change Freq' to be shared fields",
                FromName.GetShortName("20151029.2 - Marketing Pages â€“ Set Sitemap â€˜Tickboxâ€™ and â€˜Change Freqâ€™ to be shared fields"));
        }

        //Empty out â€œejcms/cacheâ€ folder prior to deploying

        [TestMethod]
        public void HideEpics()
        {
            Assert.AreEqual(
                "Ancillaries",
                FromName.GetShortName("(3) 20151009.6 - Rebooking - Ancillaries", "Rebooking"));
        }
    }
}
