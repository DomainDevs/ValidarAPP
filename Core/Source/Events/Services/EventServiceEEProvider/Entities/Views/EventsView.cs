using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.EventsServices.EEProvider.Views
{
    [Serializable()]
    public class EventsView : BusinessView
    {
        public BusinessCollection CoEventCompanies
        {
            get { return this["CoEventCompany"]; }
        }
        public BusinessCollection CoEventsConditionGroup
        {
            get { return this["CoEventConditionGroup"]; }
        }
        public BusinessCollection CoEventsGroup
        {
            get { return this["CoEventGroup"]; }
        }
        public BusinessCollection CoEventGroupPrefixes
        {
            get { return this["CoEventGroupPrefix"]; }
        }
    }
}
