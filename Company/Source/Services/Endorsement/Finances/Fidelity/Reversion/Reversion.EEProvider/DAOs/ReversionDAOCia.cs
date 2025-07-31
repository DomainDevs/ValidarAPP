using Sistran.Co.Application.Data;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework;
using System;
using System.Data;

namespace Sistran.Company.Application.FidelityReversionService.EEProvider.DAOs
{
    public class ReversionDAOCia
    {
        public CompanyPolicy CreateEndorsementReversion(CompanyPolicy policy, string userName)
        {

            return policy;
        }

        public CompanyPolicy CreateEndorsementReversion(CompanyPolicy policy)
        {
            //policy.InfringementPolicies = ValidateAuthorizationPolicies(policy);

            if (policy.InfringementPolicies == null || policy.InfringementPolicies.Count == 0)
            {
                NameValue[] parameters = new NameValue[6];

                parameters[0] = new NameValue("@POLICY_ID", policy.Endorsement.PolicyId);
                parameters[1] = new NameValue("@USER_ID", policy.UserId);
                if (string.IsNullOrEmpty(policy.Endorsement.Text.TextBody))
                {
                    parameters[2] = new NameValue("@CONDITION_TEXT", DBNull.Value,DbType.String);
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
                parameters[5] = new NameValue("@ISSUE_DATE", policy.IssueDate);

                DataTable dataTable;

                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    dataTable = pdb.ExecuteSPDataTable("TMP.CANCELLATION_ENDORSEMENT_FIDELITY", parameters);
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

        //public List<PoliciesAut> ValidateAuthorizationPolicies(Policy policy)
        //{
        //    var key = policy.Prefix.Id + "," + (int)policy.Product.CoveredRisk.CoveredRiskType;
        //    List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
        //    IList facadeList = new List<FacadeBasic>();

        //    FacadeGeneral facadeGeneral = EntityAssembler.CreateFacadeGeneral(policy);
        //    facadeList.Add(facadeGeneral);

        //    policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, key, facadeList, FacadesType.FacadeGeneral));

        //    return policiesAuts;
        //}

    }
}