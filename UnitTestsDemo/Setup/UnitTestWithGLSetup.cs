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


namespace UnitTestsDemo
{
	public abstract class UnitTestWithGLSetup : TestBase
	{
		protected override void RegisterServices(ContainerBuilder builder)
		{
			base.RegisterServices(builder);
			builder.RegisterType<PX.Objects.Unit.FinPeriodServiceMock>().As<IFinPeriodRepository>();
			builder.RegisterType<PX.Objects.Unit.CurrencyServiceMock>().As<IPXCurrencyService>();
		}

		protected void SetupGL<Graph>()
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
		protected virtual void SetupOrganizationAndBranch<Graph>()
			where Graph : PXGraph
		{
			Setup<Graph>(
				new Organization()
				{
					OrganizationCD = "MockOrganization",
					OrganizationID = -1
				},
				new Branch()
				{
					BranchCD = "MockBranch",
					OrganizationID = -1
				});
		}
	
		protected Table Insert<Table>(PXGraph graph, Table row)
			where Table : class, IBqlTable, new()
		{
			var cache = graph.Caches<Table>();
			var insertedRow = (Table)cache.Insert(row);
			Assert.NotNull(insertedRow);
			return insertedRow;
		}
		protected Table Update<Table>(PXGraph graph, Table row)
			where Table : class, IBqlTable, new()
		{
			var cache = graph.Caches<Table>();
			var updatedRow = (Table)cache.Update(row);
			Assert.NotNull(updatedRow);
			return updatedRow;
		}
	}
}
