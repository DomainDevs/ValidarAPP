using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using commonEntitiesCore = Sistran.Core.Application.Common.Entities;
using commonEntitiesCompany = Sistran.Company.Application.Common.Entities;

namespace Sistran.Company.Application.CommonParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class CompanyPrefixParametrizationView : BusinessView
    {
        public BusinessCollection Prefix
        {
            get
            {
                return this["Prefix"];
            }
        }        
    }
}
