using Sistran.Co.Application.Data;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using AutoMapper;
using Sistran.Core.Framework;
using System.Data;

namespace Sistran.Core.Application.LiabilityEndorsementReversionService3GProvider.DAOs
{
    public class ReversionDAO
    {
        public Policy CreateEndorsementReversion(Policy policy, string userName)
        {
            

            if (policy.InfringementPolicies.Count == 0)
            {
                NameValue[] parameters = new NameValue[6];

                parameters[0] = new NameValue("@POLICY_ID", policy.Endorsement.PolicyId);
                parameters[1] = new NameValue("@USER_ID", policy.UserId);
                if (string.IsNullOrEmpty(policy.Endorsement.Text.TextBody))
                {
                    parameters[2] = new NameValue("@CONDITION_TEXT", DBNull.Value);
                }
                else
                {
                    parameters[2] = new NameValue("@CONDITION_TEXT", policy.Endorsement.Text.TextBody);
                }
                parameters[3] = new NameValue("@ENDO_REASON_CD", policy.Endorsement.EndorsementReasonId);
                if (string.IsNullOrEmpty(policy.Endorsement.Text.Observations))
                {
                    parameters[4] = new NameValue("@ANNOTATIONS", DBNull.Value);
                }
                else
                {
                    parameters[4] = new NameValue("@ANNOTATIONS", policy.Endorsement.Text.Observations);
                }
                parameters[5] = new NameValue("@USER_NAME", userName);

                DataTable dataTable;

                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    dataTable = pdb.ExecuteSPDataTable("TMP.CANCELLATION_ENDORSEMENT_LOCATION", parameters);
                }

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dataTable.Rows[0][0]))
                    {
                        policy.Endorsement.Number = Convert.ToInt32(dataTable.Rows[0][1]);
                    }
                    else
                    {
                        throw new ValidationException(dataTable.Rows[0][1].ToString());
                    }
                }
                else
                {
                    throw new ValidationException("Error creando endoso");
                }
            }

            return policy;
        }

        
    }
}