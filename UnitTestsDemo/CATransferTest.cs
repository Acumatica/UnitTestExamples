using System;
using Autofac;
using Xunit;
using PX.Data;
using PX.Data.Unit;
using PX.Objects.GL;
using PX.Objects.CA;
using PX.Objects.GL.FinPeriods;
using PX.Objects.CM.Extensions;
using UnitTestsDemo;

namespace Summit2020Demo
{
    public class CATransferTest : UnitTestWithSetup
    {       

        private CashTransferEntry PrepareGraph()
        {
            AddGLSetup<CashTransferEntry>();
            Setup<CashTransferEntry>(
                new CASetup
                {
                    RequireControlTotal = false
                });
            var graph = PXGraph.CreateInstance<CashTransferEntry>();
            return graph;
        }

        [Fact]
        public void Test_Defaulting_InCuryID()
        {
            CashTransferEntry graph = PrepareGraph();

            CashAccount cashaccount =
             (CashAccount)graph.Caches[typeof(CashAccount)].Insert(
                 new CashAccount { 
                     CashAccountCD = "102000", 
                     CuryID = "USD" 
                 });

            var transfer = graph.Transfer.Insert();
            transfer.InAccountID = cashaccount.CashAccountID;
            transfer = graph.Transfer.Update(transfer);

            Assert.Equal("USD", transfer.InCuryID);
        }
    }
}
