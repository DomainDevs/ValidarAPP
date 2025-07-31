using Sistran.Core.Application.VehicleEndorsementReversionService.EEProvider;
using System;
using sp = Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Linq;
using Sistran.Co.Application.Data;
using System.Collections.Generic;
using System.Collections;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using AutoMapper;
using System.Data;
using Sistran.Core.Framework;

namespace Sistran.Core.Application.VehicleEndorsementReversionService3GProvider.DAOs
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
                    dataTable = pdb.ExecuteSPDataTable("TMP.CANCELLATION_ENDORSEMENT_VEHICLE", parameters);
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