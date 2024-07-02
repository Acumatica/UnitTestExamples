using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PX.Common;
using PX.Data;
using PX.Objects.CA;
using PX.Objects.GL;

using UnitTestsDemo.Setup;

using Xunit;

namespace UnitTestsDemo.Tests
{
    public class JournalEntryTest : UnitTestWithGLSetup
    {
        private JournalEntry PrepareGraph()
        {
            SetupGL<JournalEntry>();
            var graph = PXGraph.CreateInstance<JournalEntry>();
            return graph;
        }

        [Fact]

        public void Test_Defaulting_GLTranAmount()
        {
            JournalEntry graph = PrepareGraph();


            Account glAccount =
             (Account)graph.Caches[typeof(Account)].Insert(
                 new Account());
            Sub subaccount =
            (Sub)graph.Caches[typeof(Sub)].Insert(new Sub
                {
                    SubCD = "000"
                });
            Sub subaccount2 =
           (Sub)graph.Caches[typeof(Sub)].Insert(new Sub
               {
                   SubCD = "111"
               });
          

            graph.BatchModule.Insert();
            graph.GLTranModuleBatNbr.Insert(new GLTran()
            {
                AccountID = glAccount.AccountID,
                CuryDebitAmt = 100,
                CuryCreditAmt = 0,
                SubID = subaccount.SubID
            });
            var gltran2 = (GLTran)graph.GLTranModuleBatNbr.Insert(new GLTran()
            {
                AccountID = glAccount.AccountID,
                SubID = subaccount2.SubID,
            });
            Assert.Equal(100, gltran2.CuryCreditAmt);
        }
    }
}
