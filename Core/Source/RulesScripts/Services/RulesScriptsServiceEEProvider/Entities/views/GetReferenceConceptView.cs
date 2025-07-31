using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities.views
{
    [Serializable()]
    public class GetReferenceConceptView : BusinessView
    {

        public BusinessCollection Concept
        {
            get
            {
                return this["Concept"];
            }
        }

        public BusinessCollection ReferenceConcept
        {
            get
            {
                return this["ReferenceConcept"];
            }
        }

        public BusinessCollection Entity
        {
            get
            {
                return this["Entity"];
            }
        }

        public BusinessCollection ConceptDependency
        {
            get
            {
                return this["ConceptDependency"];
            }
        }
    }
}

