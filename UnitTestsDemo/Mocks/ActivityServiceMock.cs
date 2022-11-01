using PX.Common;
using PX.Data;
using PX.Data.EP;
using PX.Objects.CR;
using PX.Objects.EP;
using PX.SM;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace UnitTestsDemo.Mocks
{

    public class ActivityServiceMock : IActivityService
    {
        public void Cancel(string keys)
        {
        }

        public void Complete(string keys)
        {
        }

        public void CreateActivity(object refNoteID, string typeCode)
        {
        }

        public void CreateEmailActivity(object refNoteID, int EmailAccountID)
        {
        }

        public void CreateEvent(object refNoteID)
        {
        }

        public void CreateTask(object refNoteID)
        {
        }

        public void Defer(string keys, int minuts)
        {
        }

        public void Dismiss(string keys)
        {
        }

        public IEnumerable<PX.Data.EP.ActivityService.IActivityType> GetActivityTypes()
        {
            yield break;
        }

        public int GetCount(object refNoteID)
        {
            return 0;
        }

        public IEnumerable<PX.Data.EP.ActivityService.Total> GetCounts()
        {
            yield break;
        }

        public string GetEmailAddressesForCc(PXGraph graph, Guid? refNoteID)
        {
            return "a@b.c";
        }

        public DateTime? GetEndDate(object item)
        {
            return DateTime.MaxValue;
        }

        public string GetImage(object item)
        {
            return String.Empty;

        }

        public string GetKeys(object item)
        {
            return String.Empty;
        }

        public DateTime? GetStartDate(object item)
        {
            return DateTime.Now;
        }

        public string GetTitle(object item)
        {
            return String.Empty;
        }

        public bool IsViewed(object item)
        {
            return false;
        }

        public void Open(string keys)
        {
        }

        public void OpenMailPopup(string link)
        {
        }

        public IEnumerable Select(object refNoteID, int? filterId = null)
        {
            yield break;
        }

        public void ShowAll(object refNoteID)
        {
        }

        public bool ShowTime(object item)
        {
            return false;
        }
    }
}