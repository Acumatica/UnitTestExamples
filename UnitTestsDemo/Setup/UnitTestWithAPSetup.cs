using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PX.Data;
using PX.Objects.AP;

namespace UnitTestsDemo.Setup
{
    public abstract class UnitTestWithAPSetup : UnitTestWithGLSetup
    {
        protected virtual void SetupAP<TGraph>()
             where TGraph : PXGraph, new()
        {
            SetupGL<TGraph>();
            Setup<TGraph>(
                new APSetup
                {
                    RequireControlTotal = false,
                    RequireVendorRef = false
                });
        }

        protected virtual TGraph PrepareGraph<TGraph>()
            where TGraph : PXGraph, new()
        {
            SetupAP<TGraph>();
            var graph = PXGraph.CreateInstance<TGraph>();
            return graph;
        }
    }
}
