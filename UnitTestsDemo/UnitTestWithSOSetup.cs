using System;
using System.Collections;
using System.Linq;

using Autofac;
using Xunit;

using PX.Data;
using PX.Data.Unit;
using PX.Objects.AP;
using PX.Objects.CM.Extensions;
using PX.Objects.GL;
using PX.Objects.GL.FinPeriods;
using PX.Objects.GL.DAC;
using PX.Objects.SO;
using PX.Objects.IN;

namespace UnitTestsDemo
{
	public abstract class UnitTestWithSOSetup : UnitTestWithARSetup
    {
        public virtual void SetupSOOrderTypeAndTypeOperation<Graph>()
            where Graph : PXGraph
        {
            Setup<Graph>(
                new SOOrderTypeOperation
                {
                    OrderType = "SO",
                    Operation = "I",
                },
                new SOOrderTypeT
                {
                    OrderType = "SO",
                    Behavior = "SO",
                },
                new SOOrderType
                {
                    OrderType = "SO",
                    Active = true,
                    DaysToKeep = 0,
                    Template = "SO",
                    IsSystem = true,
                    Behavior = "SO",
                    DefaultOperation = "I",
                });
        }

        protected static void InsertINUnit(PXGraph graph, string unit)
        {
            graph.Caches[typeof(INUnit)].Insert(
               new INUnit
               {
                   UnitType = INUnitType.Global,
                   InventoryID = 0,
                   FromUnit = unit,
                   ToUnit = unit,
                   UnitMultDiv = "M",
                   UnitRate = 1m
               });
        }
    }
}
