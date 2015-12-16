using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Formula.Test
{
    [TestClass]
    public class FromLabelsToInvoice
    {
        [TestMethod]
        public void InvoiceFromLabel()
        { 
            List<string> labels = new List<string> { "Invoice 2015 10 01", "BLOCKED" };

            Assert.AreEqual(new DateTime(2015, 10, 1), FromLabels.GetInvoice(labels, "Dev Done"));
        }

        [TestMethod]
        public void InvoiceFromList()
        {
            List<string> labels = new List<string> { "BLOCKED" };

            Assert.AreEqual(new DateTime(2015, 10, 1), FromLabels.GetInvoice(labels, "Invoice 2015 10 01"));
        }

        [TestMethod]
        public void NoInvocie()
        {
            List<string> labels = new List<string> { "BLOCKED" };

            Assert.IsNull(FromLabels.GetInvoice(labels, "Dev Done"));
        }
    }
}
