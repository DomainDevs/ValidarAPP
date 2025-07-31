using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities.views
{
    [Serializable()]
    public class GetRangeConceptView : BusinessView
    {

        public BusinessCollection Concept
        {
            get
            {
                return this["Concept"];
            }
        }

        public BusinessCollection RangeConcept
        {
            get
            {
                return this["RangeConcept"];
            }
        }

        public BusinessCollection RangeEntity
        {
            get
            {
                return this["RangeEntity"];
            }
        }

        public BusinessCollection RangeEntityValue
        {
            get
            {
                return this["RangeEntityValue"];
            }
        }
    }
}

