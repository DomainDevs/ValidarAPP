using Sistran.Core.Application.PropertyEndorsementCancellationService.EEProvider;
using System;
using sp = Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Co.Application.Data;

namespace Sistran.Core.Application.PropertyEndorsementCancellationService3GProvider.DAOs
{
    public class CancellationDAO
    {
        /// <summary>
        /// Crear temporal de cancelacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <param name="cancellationFactor">factor de cancelacion</param>
        /// <returns>Id temporal</returns>
        public int CreateTemporalEndorsementCancellation(Policy policy, int cancellationFactor, string userName)
        {
            NameValue[] parameters = new NameValue[7];

            parameters[0] = new NameValue("POLICY_ID", policy.Endorsement.PolicyId);
            parameters[1] = new NameValue("USER_ID", policy.UserId);
            if (string.IsNullOrEmpty(policy.Endorsement.Text.TextBody))
            {
                parameters[2] = new NameValue("CONDITION_TEXT", DBNull.Value);
            }
            else
            {
                parameters[2] = new NameValue("CONDITION_TEXT", policy.Endorsement.Text.TextBody);
            }
            parameters[3] = new NameValue("ENDO_REASON_CD", policy.Endorsement.EndorsementReasonId);
            if (string.IsNullOrEmpty(policy.Endorsement.Text.Observations))
            {
                parameters[4] = new NameValue("ANNOTATIONS", DBNull.Value);
            }
            else
            {
                parameters[4] = new NameValue("ANNOTATIONS", policy.Endorsement.Text.Observations);
            }
            parameters[5] = new NameValue("CANCELL", cancellationFactor);
            parameters[6] = new NameValue("USER_NAME", userName);

            object result = 0;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPScalar("TMP.CANCELLATION_POLICY_LOCATION", parameters);
            }
            return Convert.ToInt32(result);
        }
    }
}
