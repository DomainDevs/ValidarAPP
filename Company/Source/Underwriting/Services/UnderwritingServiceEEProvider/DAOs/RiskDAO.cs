using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class RiskDAO
    {
        /// <summary>
        /// Eliminar riesgo
        /// </summary>
        /// <param name="operationId">Id operacion</param>
        /// <returns>Resultado</returns>
        public bool DeleteRisk(int operationId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("OPERATION_ID", operationId);

            object result = false;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPScalar("TMP.DELETE_TEMP_RISK", parameters);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs.DeleteRisk");

            return Convert.ToBoolean(result);
        }

        public List<String> GetRiskByEndorsementDocumentNumber(int endorsementId)
        {
            List<String> risks = new List<String>();
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@ENDORSEMENT_ID", endorsementId);
            parameters[1] = new NameValue("@ONLY_POLICY", 0);
            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("REPORT.REPORT_GET_OPERATION", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow arrayItem in result.Rows)
                {
                    risks.Add(arrayItem[0].ToString());
                }
            }
            return risks;
        }

        public List<CompanyRisk> GetCompanyRisksByTemporalId(int temporalId, bool isMasive)
        {

            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "false";


            List<PendingOperation> pendingOperations = new List<PendingOperation>();
            if (isMasive && boolUseReplicatedDatabase)
            {
                //Se guarda el JSON en la base de datos de réplica
            }
            else
            {
                pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);
            }

            if (pendingOperations != null)
            {
                return CreateCompanyRisk(pendingOperations);
            }
            else
            {
                return null;
            }
        }
        private List<CompanyRisk> CreateCompanyRisk(List<PendingOperation> pendingOperations)
        {
            ConcurrentBag<CompanyRisk> companyRisks = new ConcurrentBag<CompanyRisk>();
            ConcurrentBag<string> companyErrors = new ConcurrentBag<string>();
            TP.Parallel.ForEach(pendingOperations, pendingOperation =>
            {
                JObject companyRisk = JObject.Parse(pendingOperation.Operation);
                JToken risk = companyRisk[RiskPropertyName.Risk];
                if (risk != null)
                {
                    CompanyRisk riskData = JsonConvert.DeserializeObject<CompanyRisk>(risk.ToString());
                    riskData.Id = pendingOperation.Id;
                    companyRisks.Add(riskData);
                }
                else
                {
                    companyErrors.Add(Errors.ErrorRiskNotFound + pendingOperation.Id.ToString());
                }
            }
            );
            if (companyErrors != null && companyErrors.Count > 0)
            {
                throw new Exception(string.Join(": ", companyErrors));
            }
            return companyRisks.ToList();
        }

        public bool DeleteCompanyRisksByRiskId(int riskId, bool isMasive)
        {
            bool deleted = false;
            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";


            List<PendingOperation> pendingOperation = new List<PendingOperation>();
            if (isMasive && boolUseReplicatedDatabase)
            {
                //Se guarda el JSON en la base de datos de réplica
            }
            else
            {
                deleted = DelegateService.utilitiesServiceCore.DeletePendingOperation(riskId);
                if (deleted)
                {
                    DeleteRisk(riskId);
                }
            }

            return deleted;
        }
        #region riesgo Endosos
        public List<CompanyRisk> GetCiaRiskByTemporalId(int temporalId, bool isMasive)
        {

            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";

            List<PendingOperation> pendingOperations = new List<PendingOperation>();
            if (boolUseReplicatedDatabase && isMasive)
            {
                //Se guarda el JSON en la base de datos de réplica
            }
            else
            {
                pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);
            }


            if (pendingOperations != null)
            {
                return CreateCiayRisk(pendingOperations);
            }
            else
            {
                return null;
            }
        }
        private List<CompanyRisk> CreateCiayRisk(List<PendingOperation> pendingOperations)
        {
            ConcurrentBag<CompanyRisk> companyRisks = new ConcurrentBag<CompanyRisk>();
            TP.Parallel.ForEach(pendingOperations, pendingOperation =>
            {
                CompanyRisk ciaRisk = new CompanyRisk();
                JObject companyRisk = JObject.Parse(pendingOperation.Operation);
                JToken risk = companyRisk[RiskPropertyName.Risk];
                if (risk != null)
                {
                    CompanyRisk riskData = JsonConvert.DeserializeObject<CompanyRisk>(risk.ToString());
                    riskData.Id = pendingOperation.Id;
                    ciaRisk.Id = riskData.Id;
                    ciaRisk.Description = riskData.Description;
                    companyRisks.Add(riskData);
                }
            }
            );

            return companyRisks.ToList();
        }

        public List<CompanyRisk> GetRiskByPolicyIdEndorsmentId(int policyId, int endorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(EndorsementRiskCoverage.Properties.PolicyId, typeof(EndorsementRiskCoverage).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(EndorsementRiskCoverage.Properties.EndorsementId, typeof(EndorsementRiskCoverage).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            CiaEndorsementRiskView view = new CiaEndorsementRiskView();
            ViewBuilder builder = new ViewBuilder("CiaEndorsementRiskView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            List<CompanyRisk> coRisks = new List<CompanyRisk>();
            if (view.RiskCoverages.Count > 0)
            {

                var endoRiskCoverages = (List<EndorsementRiskCoverage>)view.EndorsementRiskCoverages.Cast<EndorsementRiskCoverage>().ToList();
                var endoRiskCoveragesDistinct = new List<EndorsementRiskCoverage>();

                endoRiskCoveragesDistinct.Add(endoRiskCoverages[0]);
                int riskId = endoRiskCoverages[0].RiskId;
                foreach (var endoRiskCoverage in endoRiskCoverages)
                {
                    if (endoRiskCoverage.RiskId != riskId)
                    {
                        riskId = endoRiskCoverage.RiskId;
                        endoRiskCoveragesDistinct.Add(endoRiskCoverage);
                    }

                }
                
                List<CompanyCoverage> companyCoverages = ModelAssembler.CreateCompanyCoverages(view.RiskCoverages);
                CompanyRisk companyRisk;

                foreach (var endoRiskCoverage in endoRiskCoveragesDistinct)
                {
                    companyRisk = new CompanyRisk()
                    {
                        Id = endoRiskCoverage.RiskId,
                        // Esto sólo aplica para el una declaración
                        Coverages = companyCoverages,
                        Policy = new CompanyPolicy()
                        {
                            Id = policyId,
                            Endorsement = new CompanyEndorsement
                            {
                                Id = endorsementId
                            }
                        }
                    };
                    coRisks.Add(companyRisk);
                }
            }
            return coRisks;

        }


        #endregion riesgo endoso

    }
}
