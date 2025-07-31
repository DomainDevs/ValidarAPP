using Sistran.Co.Application.Data;
using Sistran.Core.Application.SuretyEndorsementReversionService.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.SuretyEndorsementReversionService.EEProvider.DAOs
{
    public class ReversionDAO
    {
        public List<Policy> CreateTemporalsEndorsementReversion(Policy policy, string userName)
        {
            ConcurrentBag<Policy> policies = new ConcurrentBag<Policy>();

            int[] temporalIds = CreateTemporalEndorsementReversion(policy, userName);
            var imaper = ModelAssembler.CreateMapPolicy();
            TP.Parallel.ForEach(temporalIds, temporalId =>
            {
                Policy temporalPolicy = new Policy();
                temporalPolicy = imaper.Map<Policy, Policy>(policy);
                temporalPolicy.Id = temporalId;
                policies.Add(temporalPolicy);
            });

            return policies.ToList();
        }

        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        private int[] CreateTemporalEndorsementReversion(Policy policy, string userName)
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
            object result = null;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPScalar("TMP.CANCELLATION_ENDORSEMENT_SURETY", parameters);
            }

            string[] temporals = result.ToString().Split(',');

            return Array.ConvertAll(temporals, int.Parse);
        }

        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
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
                    dataTable = pdb.ExecuteSPDataTable("TMP.CANCELLATION_ENDORSEMENT_SURETY", parameters);
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
    class Temp
    {
        public int Id { get; set; }
    }
}
