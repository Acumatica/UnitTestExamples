using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using PX.Data;
using PX.Data.EP;
using PX.Objects.AP;
using PX.Objects.PO;

using UnitTestsDemo.Mocks;

namespace UnitTestsDemo.Setup
{
    public abstract class UnitTestWithPOSetup : UnitTestWithAPSetup
    {
        protected override void RegisterServices(ContainerBuilder builder)
        {
            base.RegisterServices(builder);
            builder.RegisterType<ActivityServiceMock>().As<IActivityService>();
        }

        protected virtual void SetupPO<TGraph>()
            where TGraph : PXGraph, new()
        {
            SetupGL<TGraph>();
            SetupAP<TGraph>();
            Setup<TGraph>(
                new POSetup
                {
                    RequireReceiptControlTotal = false
                });
        }
        protected virtual TGraph PrepareGraph<TGraph>()
            where TGraph : PXGraph, new()
        {
            SetupPO<TGraph>();
            var graph = PXGraph.CreateInstance<TGraph>();
            return graph;
        }
    }
}
