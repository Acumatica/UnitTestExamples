using PX.Data;
using PX.Objects.AP;
using PX.Objects.CS;

using System;
using System.Collections.Generic;

namespace UnitTestsDemo.Setup
{
    public abstract class UnitTestWithAPSetup : UnitTestWithGLSetup
    {
		private class MockAutoNumberings : AutoNumberAttribute.Numberings
		{
			public MockAutoNumberings() : base()
			{
				_items = new Dictionary<string, string>();
				_items["APBILL"] = "<NEW>";
			}
		}
		protected virtual void SetupAP<TGraph>()
             where TGraph : PXGraph, new()
        {
            SetupGL<TGraph>();
            Setup<TGraph>(
                new APSetup
				{
					InvoiceNumberingID = "APBILL",
					RequireControlTotal = false,
                    RequireVendorRef = false
                }, 
                new NumberingSequence() { NumberingID = "APBILL", NbrStep = 1, LastNbr = "000033", StartDate = new DateTime(1, 1, 1) },
				new Numbering()
				{
					Descr = "Bill number",
					UserNumbering = false,
					NewSymbol = "<NEW>",
					NumberingID = "APBILL"
				}
				);
        }

        protected virtual TGraph PrepareGraph<TGraph>()
            where TGraph : PXGraph, new()
        {
            SetupAP<TGraph>();
            var graph = PXGraph.CreateInstance<TGraph>();
			Slot<AutoNumberAttribute.Numberings>(new MockAutoNumberings());
			return graph;
        }
    }
}
