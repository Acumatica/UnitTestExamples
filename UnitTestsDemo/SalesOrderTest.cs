using System;

using Autofac;
using Xunit;

using PX.Data;
using PX.Data.Unit;

using PX.Objects.GL;
using PX.Objects.SO;
using PX.Objects.IN;
using PX.Objects.AR;

using UnitTestsDemo;
using PX.Objects.CS;

namespace VirtualDevConf2021Demo
{
    public class SalesOrderTest : UnitTestWithSOSetup
    {
        private SOOrderEntry PrepareGraph()
        {
            AddGLSetup<SOOrderEntry>();
            SetupSOOrderTypeAndTypeOperation<SOOrderEntry>();
            SetupOrganizationAndBranch<SOOrderEntry>();
            Setup<SOOrderEntry>(new SOSetup { }, new ARSetup());

            var graph = PXGraph.CreateInstance<SOOrderEntry>();

            return graph;
        }


        [Fact]
        public void Test_Calculation_UnitPrice()
        {
            SOOrderEntry graph = PrepareGraph();
            graph.Caches[typeof(INUnit)].Insert(
           new INUnit
           {
               UnitType = INUnitType.Global,
               InventoryID = 0,
               FromUnit = "KG",
               ToUnit = "KG",
               UnitMultDiv = "M",
               UnitRate = 1m
           });
            graph.Caches[typeof(INUnit)].Insert(
             new INUnit
             {
                 UnitType = INUnitType.Global,
                 InventoryID = 0,
                 FromUnit = "LBS",
                 ToUnit = "LBS",
                 UnitMultDiv = "M",
                 UnitRate = 1m
             });

            var stockItem = (InventoryItem)graph.Caches[typeof(InventoryItem)].Insert(
                 new InventoryItem
                 {
                     InventoryCD = "TESTITEM",
                     BasePrice = 500,
                     BaseUnit = "KG",
                     SalesUnit = "LBS"
                 });
           
            graph.Caches[typeof(INUnit)].Insert(
                new INUnit
                {
                    UnitType = INUnitType.InventoryItem,
                    InventoryID = stockItem.InventoryID,
                    FromUnit = "KG",
                    ToUnit = "LBS",
                    UnitMultDiv = "M",
                    UnitRate = 2.2m
                });

            graph.Caches[typeof(INUnit)].Insert(
            new INUnit
            {
                UnitType = INUnitType.InventoryItem,
                InventoryID = stockItem.InventoryID,
                FromUnit = "LBS",
                ToUnit = "KG",
                UnitMultDiv = "D",
                UnitRate = 2.2m
            });

            Customer customer = InsertCustomer(graph, "TestCustmer");

            SOOrder order = new SOOrder()
            {
                OrderType = "SO",
                OrderNbr = "TEST"
            };
            AutoNumberAttribute.SetUserNumbering<SOOrder.orderNbr>(graph.Caches<SOOrder>());
            order = graph.Document.Insert(order);
            order.CustomerID = customer.BAccountID;
            order.CustomerLocationID = customer.DefLocationID;
            graph.Document.Update(order);

            var orderLine = graph.Transactions.Insert();
            orderLine.InventoryID = stockItem.InventoryID;
            orderLine.Qty = 1;
            orderLine.UOM = "LBS";
            orderLine = graph.Transactions.Update(orderLine);

            Assert.Equal("LBS", orderLine.UOM);
            Assert.InRange(orderLine.CuryUnitPrice.Value, Math.Floor(500m / 2.2m), Math.Ceiling(500m / 2.2m));
        }
    }
}
