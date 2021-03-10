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
			SetupSO<SOOrderEntry>();
            SetupSOOrderTypeAndTypeOperation<SOOrderEntry>();
            SetupOrganizationAndBranch<SOOrderEntry>();

            return PXGraph.CreateInstance<SOOrderEntry>();
        }

        [Fact]
        public void Test_Calculation_UnitPrice()
		{
			// Prepare Data
			SOOrderEntry graph = PrepareGraph();
			InsertINUnit(graph, "KG");
			InsertINUnit(graph, "LBS");

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
					FromUnit = "LBS",
					ToUnit = "KG",
					UnitMultDiv = "D",
					UnitRate = 2.2m
				});
			Customer customer = InsertCustomer(graph, "TestCustmer");

			// Execute Action
			SOOrder order = graph.Document.Insert(
				new SOOrder()
				{
					CustomerID = customer.BAccountID,
					CustomerLocationID = customer.DefLocationID
				});

			SOLine orderLine = graph.Transactions.Insert(
				new SOLine()
				{
					InventoryID = stockItem.InventoryID,
					Qty = 1
				});

			//Check the result
			Assert.Equal("LBS", orderLine.UOM);
			Assert.InRange(orderLine.CuryUnitPrice.Value, Math.Floor(500m / 2.2m), Math.Ceiling(500m / 2.2m));
		}
	}
}
