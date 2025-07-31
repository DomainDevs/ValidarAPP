using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class CoverageParametrizationView : BusinessView
    {
        public BusinessCollection Coverages
        {
            get
            {
                return this["Coverage"];
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
