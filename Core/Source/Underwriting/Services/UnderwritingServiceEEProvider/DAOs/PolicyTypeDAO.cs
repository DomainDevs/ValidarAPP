using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class PolicyTypeDAO
    {
        public List<PolicyType> GetPolicyTypeAll()
        {
            BusinessCollection policyType = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CoPolicyType)));
            List<PolicyType> policyTypeModel = ModelAssembler.CreatePolicyTypes(policyType);
            return policyTypeModel;
        }
    }
}
