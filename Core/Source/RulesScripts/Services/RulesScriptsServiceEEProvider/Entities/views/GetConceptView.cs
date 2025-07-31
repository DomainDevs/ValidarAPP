using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities.views
{
    [Serializable()]
    public class GetConceptView : BusinessView
    {

        public BusinessCollection Concept
        {
            get
            {
                return this["Concept"];
            }
        }

        public BusinessCollection ConceptDependency
        {
            get
            {
                return this["ConceptDependency"];
            }
        }

        public BusinessCollection Concept2
        {
            get
            {
                return this["Concept2"];
            }
        }
    }
}

