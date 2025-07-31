using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Temporary.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class TempSubscriptionDAO
    {
        public void CreateTempSubscription(CompanyPolicy companyPolicy)
        {
            TempSubscription entityTempSubscription = EntityAssembler.CreateTempSubscription(companyPolicy);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityTempSubscription);
        }
    }
}
