using System;
using Autofac;
using System.Collections;
using System.Linq;

using Xunit;

using PX.Data;
using PX.Data.Unit;

using PX.Objects.GL;
using PX.Objects.AP;
using PX.Objects.GL.FinPeriods;
using PX.Objects.CM.Extensions;
using UnitTestsDemo;

namespace VirtualDevConf2020Demo
{
    public class APBillsTest : UnitTestWithSetup
    {       
        private APInvoiceEntry PrepareGraph()
        {
            AddGLSetup<APInvoiceEntry>();
            Setup<APInvoiceEntry>(
                new APSetup
                {
                    RequireControlTotal = false
                });
            var graph = PXGraph.CreateInstance<APInvoiceEntry>();
            return graph;
        }

        [Fact]
        public void Test_Defaulting_CuryIDFromVendor()
        {
            // Create and set up object you are going to test
            APInvoiceEntry graph = PrepareGraph();

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
