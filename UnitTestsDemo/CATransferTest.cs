using System;
using Autofac;
using Xunit;
using PX.Data;
using PX.Data.Unit;
using PX.Objects.GL;
using PX.Objects.CA;
using PX.Objects.GL.FinPeriods;
using PX.Objects.CM.Extensions;

namespace SummitDemo
{
    public class CATransferTest : TestBase
    {       
        protected override void RegisterServices(ContainerBuilder builder)
        {
            base.RegisterServices(builder);
            builder.RegisterType<PX.Objects.Unit.FinPeriodServiceMock>().As<IFinPeriodRepository>();
            builder.RegisterType<PX.Objects.Unit.CurrencyServiceMock>().As<IPXCurrencyService>();
        }

        private CashTransferEntry PrepareGraph()
        {
            Setup<CashTransferEntry>(
                new GLSetup
                {
                    YtdNetIncAccountID = int.MaxValue - 2,
                    RetEarnAccountID = int.MaxValue - 1,
                    RequireControlTotal = false
                });
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
