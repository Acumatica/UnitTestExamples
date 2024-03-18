using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PX.Data;

namespace UnitTestsDemo.Helpers
{
    internal class TestHelper
    {
        /// <summary>
        /// Inserts record in the cache without checking for AllowInsert
        /// </summary>
        /// <typeparam name="TDAC">Type of the record to insert</typeparam>
        /// <param name="graph">Graph that the record neds to be inserted into</param>
        /// <param name="record">The record to be inserted</param>
        /// <returns>Inserted Data record</returns>
        /// <exception cref="PXException">Throws excption if failed to insert the record</exception>

        public static TDAC InsertDataRecord<TDAC>(PXGraph graph, TDAC record, bool triggerBusinessLogic = true)
            where TDAC : IBqlTable, new()
        {
            PXCache cache = graph.Caches[typeof(TDAC)];

            if (!triggerBusinessLogic)
            {
                cache.SetStatus(record, PXEntryStatus.Inserted);
                return record;
            }

            bool allowInsert = cache.AllowInsert;
            //set AllowInsert in case Insert is not allowed
            //(Unit tests Framework respects AllowInsert/AllowUpdate/AllowDelete properties)
            cache.AllowInsert = true;

            try
            {
                TDAC result = (TDAC)cache.Insert(record);
                if (result == null)
                {
                    throw new PXException($"Failed to insert {typeof(TDAC).FullName} record");
                }

                return result;
            }
            finally
            {
                //restore AllowInsert to its initial value to not affect graph behavior
                cache.AllowInsert = allowInsert;
            }
        }
    }
}
