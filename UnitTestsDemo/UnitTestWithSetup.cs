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


namespace UnitTestsDemo
{
	public abstract class UnitTestWithSetup : TestBase
	{
        protected override void RegisterServices(ContainerBuilder builder)
        {
            base.RegisterServices(builder);
            builder.RegisterType<PX.Objects.Unit.FinPeriodServiceMock>().As<IFinPeriodRepository>();
            builder.RegisterType<PX.Objects.Unit.CurrencyServiceMock>().As<IPXCurrencyService>();
        }

		protected void AddGLSetup<Graph>()
			where Graph : PXGraph
		{
			Setup<Graph>(
				new GLSetup
				{
					YtdNetIncAccountID = int.MaxValue - 2,
					RetEarnAccountID = int.MaxValue - 1,
					RequireControlTotal = false
				});
		}
	}
}
