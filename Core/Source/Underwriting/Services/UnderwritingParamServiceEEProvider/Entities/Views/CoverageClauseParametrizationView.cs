using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class CoverageClauseParametrizationView : BusinessView
    {
   

      
        public BusinessCollection Coverages
        {
            get
            {
                return this["Coverage"];
            }
        }

        public BusinessCollection ClauseLevels
        {
            get
            {
                return this["ClauseLevel"];
            }
        }

     
        public BusinessCollection InsuredObjects
        {
            get
            {
                return this["InsuredObject"];
            }
        }

        public BusinessCollection  Perils
        {
            get
            {
                return this["Peril"];
            }
        }
    }
}
