using Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class CoInsuranceDAO
    {

        public List<CoInsuranceAssigned> GetCoInsuranceByPolicyIdByEndorsementId(int policyId, int endorsementId)
        {
            List<CoInsuranceAssigned> issuanceCoInsuranceCompanies = new List<CoInsuranceAssigned>(); 
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CoinsuranceAssigned.Properties.PolicyId, typeof(CoinsuranceAssigned).Name, policyId);
            filter.And();
            filter.PropertyEquals(CoinsuranceAssigned.Properties.EndorsementId, typeof(CoinsuranceAssigned).Name, endorsementId);

            //Se consulta la Tabla por filter
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(CoinsuranceAssigned), filter.GetPredicate());

            if (businessCollection != null)
            {
                return ModelAssembler.CreateCoinsuranceAssigneds(businessCollection);
            }
            return null;
        }
    }
}
