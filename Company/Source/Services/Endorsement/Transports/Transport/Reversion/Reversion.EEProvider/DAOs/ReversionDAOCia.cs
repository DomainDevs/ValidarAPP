using Sistran.Co.Application.Data;
using Sistran.Company.Application.TranportReversionService.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Application.Utilities.Enums;
namespace Sistran.Company.Application.TranportReversionService.EEProvider.DAOs
{
    public class ReversionDAOCia
    {

        public CompanyPolicy CreateEndorsementReversion(CompanyPolicy policy, bool clearPolicies)
        {
            policy.InfringementPolicies = !clearPolicies ? this.ValidateAuthorizationPolicies(policy) : new List<PoliciesAut>();

            if (policy.InfringementPolicies == null || policy.InfringementPolicies.Count == 0)
            {
                NameValue[] parameters = new NameValue[5];

                parameters[0] = new NameValue("@POLICY_ID", policy.Endorsement.PolicyId);
                parameters[1] = new NameValue("@USER_ID", policy.UserId);
                if (string.IsNullOrEmpty(policy.Endorsement.Text.TextBody))
                {
                    parameters[2] = new NameValue("@CONDITION_TEXT", DBNull.Value, DbType.String);
                }
                else
                {
                    parameters[2] = new NameValue("@CONDITION_TEXT", policy.Endorsement.Text.TextBody);
                }
                parameters[3] = new NameValue("@ENDO_REASON_CD", policy.Endorsement.EndorsementReasonId);
                if (string.IsNullOrEmpty(policy.Endorsement.Text.Observations))
                {
                    parameters[4] = new NameValue("@ANNOTATIONS", DBNull.Value, DbType.String);
                }
                else
                {
                    parameters[4] = new NameValue("@ANNOTATIONS", policy.Endorsement.Text.Observations);
                }

                DataTable dataTable;

                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    dataTable = pdb.ExecuteSPDataTable("TMP.CANCELLATION_ENDORSEMENT_TRANSPORT", parameters);
                }

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dataTable.Rows[0][0]))
                    {
                        policy.Endorsement.Number = Convert.ToInt32(dataTable.Rows[0][1]);
                        policy.Endorsement.Id = Convert.ToInt32(dataTable.Rows[0][2]);

                        DelegateService.underwritingService.DeleteTemporalByOperationId(policy.Id, 0, 0, 0);
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
            else
            {
                Endorsement endorsement = DelegateService.baseEndorsementService.GetTemporalEndorsementByPolicyId(policy.Endorsement.PolicyId);
                if (endorsement == null)
                {
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                }
                else
                {
                    policy.Id = endorsement.TemporalId;
                }
            }

            return policy;
        }

        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyPolicy policy)
        {
            var key = policy.Prefix.Id + "," + (int)policy.Product.CoveredRisk.CoveredRiskType;
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
            Rules.Facade facade = new Rules.Facade();

            EntityAssembler.CreateFacadeGeneral(policy, facade);

            policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_GENERAL));

            return policiesAuts.Where(x => x.Type != Core.Application.AuthorizationPoliciesServices.Enums.TypePolicies.Notification).ToList();
        }
    }
}