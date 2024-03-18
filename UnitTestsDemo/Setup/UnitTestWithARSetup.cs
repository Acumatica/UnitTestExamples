using System;
using System.Collections;
using System.Linq;

using Autofac;
using Xunit;

using PX.Data;
using PX.Tests.Unit;
using PX.Objects.AP;
using PX.Objects.CM.Extensions;
using PX.Objects.GL;
using PX.Objects.GL.FinPeriods;
using PX.Objects.GL.DAC;
using PX.Objects.SO;
using PX.Objects.AR;
using PX.Objects.CR;

namespace UnitTestsDemo.Setup
{
	public abstract class UnitTestWithARSetup : UnitTestWithGLSetup
	{
		protected void SetupAR<Graph>()
			where Graph : PXGraph
		{
			SetupGL<Graph>();
			Setup<Graph>(
				new ARSetup
				{
					RequireControlTotal = false
				});
		}
		public virtual Customer InsertCustomer(PXGraph graph, string customerCD)
		{
            var baccountR = Insert<BAccountR>(graph, new BAccountR
            {
                AcctCD = customerCD,
                CuryID = "USD",
                Type = BAccountType.CustomerType,
                Status = CustomerStatus.Active,
            });
            
            var location = Insert<Location>(graph, new Location()
			{
				BAccountID = baccountR.BAccountID,
				LocationCD = "MockLocaltion",
				IsActive = true,
				IsDefault = true,
				LocType = LocTypeList.CustomerLoc
			});

            baccountR.DefLocationID = location.LocationID;
			baccountR = Update<BAccountR>(graph, baccountR);


            var customer = Insert<Customer>(graph, new Customer
            {
                BAccountID = baccountR.BAccountID,
                AcctCD = customerCD,
                CuryID = "USD",
                Type = BAccountType.CustomerType,
                Status = CustomerStatus.Active,
            });
            customer.DefLocationID = location.LocationID;

            return customer;
		}
	}
}
