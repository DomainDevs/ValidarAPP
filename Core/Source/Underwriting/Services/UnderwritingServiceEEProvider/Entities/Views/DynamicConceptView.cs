using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable]
    public class DynamicConceptView : BusinessView
    {
        public BusinessCollection DynamicConceptValueList
        {
            get
            {
                return this["DynamicConceptValue"];
            }
        }

        public BusinessCollection DynamicConceptRelationList
        {
            get
            {
                return this["DynamicConceptRelation"];
            }
        }
        
        public BusinessCollection GetDynamicConceptValueListByDynamicConceptRelation(DynamicConceptRelation dynamicConceptRelation)
        {
            return this.GetObjectsByRelation("DynamicConceptRelationDynamicConceptValue", dynamicConceptRelation, false);
        }
    }
}
