using Sistran.Co.Application.Data;
using Sistran.Core.Application.BaseEndorsementService.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using TMPEN = Sistran.Core.Application.Temporary.Entities;

namespace Sistran.Core.Application.BaseEndorsementService.EEProvider.DAOs
{
    public class EndorsementDAO
    {
        /// <summary>
        /// Obtener temporal de una póliza
        /// </summary>
        /// <param name="policyId">Id póliza</param>
        /// <returns>modelo Endorsement</returns>
        public Endorsement GetTemporalEndorsementByPolicyId(int policyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TMPEN.TempSubscription.Properties.PolicyId, typeof(TMPEN.TempSubscription).Name);
            filter.Equal();
            filter.Constant(policyId);
            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = daf.List(typeof(TMPEN.TempSubscription), filter.GetPredicate());
            }

            if (businessCollection.Count > 0)
            {
                return ModelAssembler.CreateEndorsementByTempSubscription((TMPEN.TempSubscription)businessCollection[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Grabar endoso de una póliza
        /// </summary>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>Número de endoso</returns>
        public int CreateEndorsement(int temporalId)
        {
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("OPERATION_ID", temporalId);
            object result = "";
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPScalar("ISS.RECORD_POLICY_ENDORSEMENT", parameters);
            }
            Int32 endorsementNumber = 0;
            if (Int32.TryParse(result.ToString(), out endorsementNumber))
            {
                return endorsementNumber;
            }
            else
            {
                throw new Exception(result.ToString());
            }
        }
    }
}
