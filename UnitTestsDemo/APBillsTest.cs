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

namespace VirtualDevConfDemo
{
    public class APBillsTest : TestBase
    {       
        protected override void RegisterServices(ContainerBuilder builder)
        {
            base.RegisterServices(builder);
            builder.RegisterType<PX.Objects.Unit.FinPeriodServiceMock>().As<IFinPeriodRepository>();
            builder.RegisterType<PX.Objects.Unit.CurrencyServiceMock>().As<IPXCurrencyService>();
        }

        private APInvoiceEntry PrepareGraph()
        {
            Setup<APInvoiceEntry>(
                new GLSetup
                {
                    RequireControlTotal = false
                });
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
