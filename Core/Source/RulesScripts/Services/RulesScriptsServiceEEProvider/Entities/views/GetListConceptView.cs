using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities.views
{
    [Serializable()]
    public class GetListConceptView : BusinessView
    {

        public BusinessCollection Concept
        {
            get
            {
                return this["Concept"];
            }
        }

        public BusinessCollection ListConcept
        {
            get
            {
                return this["ListConcept"];
            }
        }

        public BusinessCollection ListEntity
        {
            get
            {
                return this["ListEntity"];
            }
        }

        public BusinessCollection ListEntityValue
        {
            get
            {
                return this["ListEntityValue"];
            }
        }
    }
}

