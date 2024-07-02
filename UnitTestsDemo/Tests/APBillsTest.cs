using System;
using System.Collections;
using System.Linq;

using Autofac;
using Xunit;

using PX.Data;
using PX.Tests.Unit;

using PX.Objects.GL;
using PX.Objects.AP;

using UnitTestsDemo;
using UnitTestsDemo.Setup;

namespace UnitTestsDemo.Tests
{
    public class APBillsTest : UnitTestWithAPSetup
    {       

        [Fact]
        public void Test_Defaulting_CuryIDFromVendor()
        {
            // Create and set up object you are going to test
            APInvoiceEntry graph = PrepareGraph<APInvoiceEntry>();

            graph.Caches[typeof(PX.Objects.CM.CurrencyList)].Insert(
             new PX.Objects.CM.CurrencyList
             {
                 CuryID = "SGD",
                 IsFinancial = true
             });

            Vendor vendor =
             (Vendor)graph.Caches[typeof(Vendor)].Insert(
                 new Vendor { 
                     AcctCD= "TESTVENDOR",
                     AcctName = "Test Vendor",
                     CuryID = "SGD" 
                 });


            // Execute the action that you will test
            var bill = graph.Document.Insert();
            bill.VendorID = vendor.BAccountID;
            bill = graph.Document.Update(bill);


            // Check the result
            Assert.Equal("SGD", bill.CuryID);
        }
    }
}
