using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities.views
{
    [Serializable()]
    public class GetBasicConceptView : BusinessView
    {

        public BusinessCollection Concept
        {
            get
            {
                return this["Concept"];
            }
        }

        public BusinessCollection BasicConcept
        {
            get
            {
                return this["BasicConcept"];
            }
        }

        public BusinessCollection BasicConceptCheck
        {
            get
            {
                return this["BasicConceptCheck"];
            }
        }
    }
}

