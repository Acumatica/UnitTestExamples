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
        public virtual SOOrderType CreateSOOrderTypeAndTypeOperation(PXGraph graph, string orderType, string behavior, string template, string operation, string iNDocType, string aRDocType, short invtMult)
        {
            Insert<SOOrderTypeOperation>(graph, new SOOrderTypeOperation
            {
                OrderType = orderType,
                Operation = operation,
                INDocType = iNDocType,
                InvtMult = invtMult,
            });

            Insert<SOOrderTypeT>(graph, new SOOrderTypeT
            {
                OrderType = orderType,
                Behavior = behavior,
            });

            return Insert<SOOrderType>(graph, new SOOrderType
            {
                OrderType = orderType,
                Active = true,
                DaysToKeep = 0,
                Template = template,
                IsSystem = true,
                Behavior = behavior,
                DefaultOperation = operation,
                INDocType = iNDocType,
                ARDocType = aRDocType,
            });
        }

    }
}
