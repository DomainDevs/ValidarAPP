using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Assemblers;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Resources;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Views;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using IQUO = Sistran.Core.Application.Quotation.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Rules = Sistran.Core.Framework.Rules;
using UTILITES = Company.UnderwritingUtilities;
using Sistran.Core.Application.Utilities.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Business
{
    public class AircraftBusiness
    {
        Rules.Facade Facade = new Rules.Facade();
        
        /// <summary>
        /// Crear el Aircrafte temporal
        /// </summary>
        /// <param name="companyAircraft"></param>
        /// <param name="isMassive"></param>
        /// <returns></returns>
        public CompanyAircraft CreateCompanyAircraftTemporal(CompanyAircraft companyAircraft)
        {

            companyAircraft.Risk.InfringementPolicies = ValidateAuthorizationPolicies(companyAircraft);
            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";
            PendingOperation pendingOperation = new PendingOperation();
            CompanyPolicy policy = companyAircraft.Risk.Policy;
            companyAircraft.Risk.Policy = null;

            if (companyAircraft.Risk.Id == 0)
            {
                pendingOperation.CreationDate = DateTime.Now;
                pendingOperation.ParentId = policy.Id;
                pendingOperation.Operation = JsonConvert.SerializeObject(companyAircraft);

                pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);

            }
            else
            {
                pendingOperation.ModificationDate = DateTime.Now;
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(companyAircraft.Risk.Id);
                if (pendingOperation != null)
                {
                    //****************************GUARDAR TEMPORAL*********************************//
                    //companyAircraft = SavecompanyAircraftTemporalTables(companyAircraft, policy);
                    //****************************************************************************//

                    pendingOperation.Operation = JsonConvert.SerializeObject(companyAircraft);

                    DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);


                }
                else
                {
                    throw new Exception("Error obteniendo el Temporal");
                }
            }

            companyAircraft.Risk.Id = pendingOperation.Id;
            companyAircraft.Risk.Policy = policy;
            ////****************************GUARDAR TEMPORAL*********************************//

            int riskId = SaveCompanyAircraftTemporalTables(companyAircraft);
            if (companyAircraft.Risk.Policy.Endorsement.EndorsementType != Core.Application.UnderwritingServices.Enums.EndorsementType.Modification)
            {
                companyAircraft.Risk.RiskId = riskId;
            }

            ////****************************************************************************//

            pendingOperation.Operation = JsonConvert.SerializeObject(companyAircraft);

            DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);

            companyAircraft.Risk.Id = pendingOperation.Id;
            return companyAircraft;
        }

        public int SaveCompanyAircraftTemporalTables(CompanyAircraft companyAircraft)
        {


            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
            UTILITES.GetDatatables d = new UTILITES.GetDatatables();

            UTILITES.CommonDataTables dts = d.GetcommonDataTablesMa(companyAircraft.Risk);

            DataTable dataTable;
            NameValue[] parameters = new NameValue[11];

            DataTable dtTempRisk = dts.dtTempRisk;
            parameters[0] = new NameValue(dtTempRisk.TableName, dtTempRisk);

            DataTable dtCOTempRisk = dts.dtCOTempRisk;
            parameters[1] = new NameValue(dtCOTempRisk.TableName, dtCOTempRisk);

            DataTable dtBeneficary = dts.dtBeneficary;
            parameters[2] = new NameValue(dtBeneficary.TableName, dtBeneficary);

            DataTable dtRiskPayer = dts.dtRiskPayer;
            parameters[3] = new NameValue(dtRiskPayer.TableName, dtRiskPayer);

            DataTable dtClause = dts.dtClause;
            parameters[4] = new NameValue(dtClause.TableName, dtClause);

            DataTable dtRiskClause = dts.dtRiskClause;
            parameters[5] = new NameValue(dtRiskClause.TableName, dtRiskClause);

            DataTable dtDeduct = dts.dtDeduct;
            parameters[6] = new NameValue(dtDeduct.TableName, dtDeduct);

            DataTable dtCoverClause = dts.dtCoverClause;
            parameters[7] = new NameValue(dtCoverClause.TableName, dtCoverClause);

            DataTable dtDynamic = dts.dtDynamic;
            parameters[8] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_RISK", dtDynamic);

            DataTable dtDynamicCoverage = dts.dtDynamicCoverage;
            parameters[9] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicCoverage);

            DataTable dtTempRiskMarine = ModelAssembler.GetDataTableRiskAirCraft(companyAircraft);
            parameters[10] = new NameValue(dtTempRiskMarine.TableName, dtTempRiskMarine);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {

                dataTable = pdb.ExecuteSPDataTable("TMP.SAVE_TEMPORAL_RISK_AIRCRAFT_TEMP", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (companyAircraft.Risk.Policy.Endorsement.EndorsementType != Core.Application.UnderwritingServices.Enums.EndorsementType.Modification)
                {
                    companyAircraft.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return companyAircraft.Risk.RiskId;
            }
            else
            {
                throw new ValidationException(Errors.ErrorCreateTemporalCompanyAircraft);
            }
        }

        public CompanyAircraft UpdateCompanyAircraftTemporal(CompanyAircraft companyAircraft)
        {
            companyAircraft.Risk.InfringementPolicies = ValidateAuthorizationPolicies(companyAircraft);

            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";
            PendingOperation pendingOperation = new PendingOperation();
            CompanyPolicy policy = companyAircraft.Risk.Policy;
            companyAircraft.Risk.Policy = null;

            pendingOperation.ModificationDate = DateTime.Now;
            pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(companyAircraft.Risk.Id);
            if (pendingOperation != null)
            {
                pendingOperation.Operation = JsonConvert.SerializeObject(companyAircraft);
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);

            }
            else
            {
                throw new Exception("Error obteniendo el Temporal");
            }

            companyAircraft.Risk.Id = pendingOperation.Id;
            companyAircraft.Risk.Policy = policy;

            return companyAircraft;
        }

        public bool DeleteCompanyAircraftTemporal(int riskId)
        {
            bool deleted = false;
            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";
            List<PendingOperation> pendingOperation = new List<PendingOperation>();

            if (boolUseReplicatedDatabase)
            {
                //Se guarda el JSON en la base de datos de réplica
            }
            else
            {
                deleted = DelegateService.utilitiesServiceCore.DeletePendingOperation(riskId);

                if (deleted)
                {
                    NameValue[] parameters = new NameValue[1];
                    parameters[0] = new NameValue("OPERATION_ID", riskId);

                    object result = false;

                    using (DynamicDataAccess pdb = new DynamicDataAccess())
                    {
                        result = pdb.ExecuteSPScalar("TMP.DELETE_TEMP_RISK", parameters);
                    }

                    return Convert.ToBoolean(result);
                }
            }

            return deleted;
        }

        /// <summary>
        /// Obtiene el riesgo
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        public CompanyAircraft GetCompanyAircraftTemporalByRiskId(int riskId)
        {
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(riskId);

            if (pendingOperation != null)
            {
                CompanyAircraft companyAircraft = JsonConvert.DeserializeObject<CompanyAircraft>(pendingOperation.Operation);
                companyAircraft.Risk.Id = pendingOperation.Id;
                companyAircraft.Risk.IsPersisted = true;
                companyAircraft.Risk.Policy = new CompanyPolicy
                {
                    Id = pendingOperation.ParentId
                };

                if (companyAircraft.InsuredObjects == null && companyAircraft?.Risk?.Coverages != null && companyAircraft.Risk.Coverages.Any())
                {
                    companyAircraft.InsuredObjects = companyAircraft.Risk.Coverages.GroupBy(z => new { z.InsuredObject.Id, z.InsuredObject.Description }).Select(x => new InsuredObject { Id = x.Key.Id, Description = x.Key.Description, Premium = x.Sum(m => m.PremiumAmount), Amount = x.Sum(m => m.LimitAmount) }).ToList();
                }

                companyAircraft.InsuredObjects = GetCompanyAircraftRiskByRiskId(companyAircraft.InsuredObjects, companyAircraft.Risk.Coverages);

                return companyAircraft;
            }
            else
            {
                return null;
            }
        }
        public List<InsuredObject> GetCompanyAircraftRiskByRiskId(List<InsuredObject> InsuredObjects, List<CompanyCoverage> companyCoverages)
        {
            if (InsuredObjects != null && InsuredObjects.Any() && companyCoverages != null && companyCoverages.Any())
            {
                TP.Parallel.ForEach(InsuredObjects, insuredObject =>
                {
                    var insuredObjectData = insuredObject;
                    insuredObjectData.Premium = companyCoverages.Where(x => x.InsuredObject.Id == insuredObjectData.Id).Sum(y => y.PremiumAmount);
                    insuredObjectData.Amount = companyCoverages.Where(x => x.InsuredObject.Id == insuredObjectData.Id).Sum(y => y.LimitAmount);
                });
                return InsuredObjects;
            }
            else
            {
                return InsuredObjects;
            }

        }

        public List<CompanyAircraft> GetCompanyAircraftsByTemporalId(int temporalId)
        {
            List<CompanyAircraft> companyAircrafts = new List<CompanyAircraft>();
            List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);

            foreach (PendingOperation pendingOperation in pendingOperations)
            {
                CompanyAircraft companyAircraft = JsonConvert.DeserializeObject<CompanyAircraft>(pendingOperation.Operation);
                companyAircraft.Risk.Id = pendingOperation.Id;
                companyAircraft.Risk.IsPersisted = true;
                companyAircrafts.Add(companyAircraft);
            }

            return companyAircrafts;
        }

        /// <summary>
        /// Obtener Poliza de Aircraftes
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <returns>Listado de Trasnportes - Riesgos</returns>
        public List<CompanyAircraft> GetCompanyAircraftsByPolicyId(int policyId)
        {
            List<CompanyAircraft> companyAircrafts = new List<CompanyAircraft>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name);
            filter.IsNotNull();
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.In();
            filter.ListValue();
            filter.Constant((int)RiskStatusType.Excluded);
            filter.Constant((int)RiskStatusType.Cancelled);
            filter.EndList();




            RiskAircraftsView view = new RiskAircraftsView();
            ViewBuilder builder = new ViewBuilder("RiskAircraftsView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.EndorsementOperations.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.RiskCoverage> RiskCoverage = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();

                foreach (ISSEN.EndorsementOperation entityEndorsementOperation in view.EndorsementOperations)
                {
                    CompanyAircraft companyAircraft = new CompanyAircraft();
                    companyAircraft = JsonConvert.DeserializeObject<CompanyAircraft>(entityEndorsementOperation.Operation);

                    companyAircraft.Risk.Id = 0;
                    companyAircraft.Risk.RiskId = entityEndorsementRisks.First(x => x.RiskNum == entityEndorsementOperation.RiskNumber).RiskId;

                    companyAircraft.Risk.OriginalStatus = companyAircraft.Risk.Status;
                    companyAircraft.Risk.Status = RiskStatusType.NotModified;

                    companyAircraft.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
                    companyAircraft.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.NotModified);

                    companyAircraft.Risk.Premium = 0;
                    companyAircraft.Risk.AmountInsured = 0;
                    foreach (var coverage in companyAircraft.Risk.Coverages)
                    {
                        if (coverage.OriginalLimitAmount == 0)
                            coverage.OriginalLimitAmount = coverage.LimitAmount;

                        if (coverage.OriginalSubLimitAmount == 0)
                            coverage.OriginalSubLimitAmount = coverage.SubLimitAmount;
                    }
                    companyAircraft.Risk.Beneficiaries.ForEach(x => x.CustomerType = (Core.Services.UtilitiesServices.Enums.CustomerType)CustomerType.Individual);

                    foreach (ISSEN.RiskCoverage riskCoverages in view.RiskCoverages)
                    {
                        companyAircraft.Risk.Coverages.ForEach(x => x.RiskCoverageId = x.RiskCoverageId);
                    }
                    companyAircrafts.Add(companyAircraft);
                }
            }
            else
            {
                ObjectCriteriaBuilder filterR1 = new ObjectCriteriaBuilder();
                filterR1.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
                filterR1.Equal();
                filterR1.Constant(policyId);
                filterR1.And();
                filterR1.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
                filterR1.Equal();
                filterR1.Constant(true);
                filterR1.And();
                filterR1.Not();
                filterR1.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                filterR1.In();
                filterR1.ListValue();
                filterR1.Constant((int)RiskStatusType.Excluded);
                filterR1.Constant((int)RiskStatusType.Cancelled);
                filterR1.EndList();


                RiskAircraftsviewR1 viewr1 = new RiskAircraftsviewR1();
                ViewBuilder builderR1 = new ViewBuilder("RiskAircraftsviewR1");
                builderR1.Filter = filterR1.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builderR1, viewr1);

                List<ISSEN.Risk> risks = viewr1.CompanyAircrafts.Cast<ISSEN.Risk>().ToList();
                companyAircrafts.AddRange(GetRisks(policyId, risks, viewr1));
            }

            return companyAircrafts;
        }

        internal bool saveInsuredObject(int riskId, InsuredObject insuredObject, int tempId, int groupCoverageId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);
                CompanyAircraft companyAircraft = GetCompanyAircraftTemporalByRiskId(riskId);
                if (companyAircraft == null)
                {
                    return false;
                }
                companyAircraft.Risk.IsPersisted = true;
                List<CompanyCoverage> coverages_ = companyAircraft.Risk?.Coverages?.Where(u => u.InsuredObject.Id == insuredObject.Id)?.ToList();
                if (coverages_ != null && coverages_.Any())
                {
                    coverages_.ForEach(u => u.InsuredObject.Amount = insuredObject.Amount);
                    if (companyPolicy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.Modification)
                    {
                        foreach (var item in coverages_)
                        {
                            if (item.CoverStatus == CoverageStatusType.NotModified)
                            {
                                string coverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Modified);
                                item.CoverStatus = CoverageStatusType.Modified;
                                item.CoverStatusName = coverStatusName;
                            }
                        }
                    }
                }
                else
                {
                    coverages_ = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(
                    insuredObject.Id, companyAircraft.Risk.GroupCoverage.Id, companyPolicy.Product.Id);
                    coverages_.RemoveAll(x => !x.IsSelected);
                    foreach (var item in coverages_)
                    {
                        item.CurrentFrom = companyPolicy.CurrentFrom;
                        item.CurrentTo = companyPolicy.CurrentTo;
                        item.InsuredObject = new CompanyInsuredObject
                        {
                            Amount = insuredObject.Amount,
                            Description = insuredObject.Description,
                            Id = insuredObject.Id,
                            ExtendedProperties = insuredObject.ExtendedProperties,
                            IsDeclarative = insuredObject.IsDeclarative,
                            IsMandatory = insuredObject.IsMandatory,
                            IsSelected = insuredObject.IsSelected,
                            ParametrizationStatus = insuredObject.ParametrizationStatus,
                            Premium = insuredObject.Premium,
                            SmallDescription = insuredObject.SmallDescription
                        };
                        if (companyPolicy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.Emission || companyPolicy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.Renewal)
                        {
                            item.CoverStatus = CoverageStatusType.Original;
                            item.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                            item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original);
                        }
                        else
                        {
                            if (companyPolicy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.Modification)
                            {
                                item.CoverStatus = CoverageStatusType.Included;
                                item.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Included);
                            }
                        }
                        if (companyAircraft.InsuredObjects.Exists(x => x.Id == item.InsuredObject.Id))
                        {

                        }
                        else
                        {
                            companyAircraft.InsuredObjects.Add(item.InsuredObject);
                        }
                    }
                    if (companyAircraft.Risk.Coverages == null)
                    {
                        companyAircraft.Risk.Coverages = new List<CompanyCoverage>();
                    }
                    companyAircraft.Risk.Coverages.AddRange(coverages_);
                }
                companyAircraft.Risk.IsPersisted = true;
                companyAircraft.Risk.Policy = companyPolicy;
                companyAircraft = QuotateAircraft(companyAircraft, true, true);
                UpdateCompanyAircraftTemporal(companyAircraft);
                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al Guardar Objeto Asegurado"), ex);
            }
        }

        internal List<CompanyCoverage> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            try
            {
                CompanyAircraft risk = GetCompanyAircraftTemporalByRiskId(riskId);
                if (risk != null && risk.Risk != null)
                {
                    List<CompanyCoverage> coverages = new List<CompanyCoverage>();
                    if (risk.Risk.Coverages != null && risk.Risk.Coverages.Any())
                    {
                        if (insuredObjectId != 0)
                        {
                            coverages = risk.Risk.Coverages.Where(x => x.InsuredObject.Id == insuredObjectId)?.ToList();
                        }
                        else
                        {
                            coverages = risk.Risk.Coverages;
                        }
                        if (coverages != null && coverages.Any())
                        {
                            return coverages;
                        }
                        else
                        {
                            throw new Exception(Errors.ErrorCoverages);
                        }
                    }
                    else
                    {
                        return coverages;
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorCoverages);
                }
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorCoverages);
            }
        }

        internal bool DeleteCompanyRisk(int temporalId, int riskId)
        {
            try
            {
                bool result = false;
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                if (companyPolicy != null)
                {
                    CompanyAircraft companyAircraft = GetCompanyAircraftTemporalByRiskId(riskId);
                    if (companyAircraft != null)
                    {
                        if (companyPolicy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.Emission || companyPolicy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.Renewal || companyAircraft.Risk.Status == RiskStatusType.Included || companyAircraft.Risk.Status == null || companyAircraft.Risk.Status == RiskStatusType.Original)
                        {
                            result = DelegateService.underwritingService.DeleteCompanyRisksByRiskId(riskId, false);
                        }
                        else
                        {
                            companyAircraft.Risk.Status = RiskStatusType.Excluded;
                            companyAircraft.Risk.Description = string.Format("{0} ({1})", companyAircraft.Risk.Description, Errors.ResourceManager.GetString(EnumHelper.GetItemName<RiskStatusType>(companyAircraft.Risk.Status)));
                            companyAircraft.Risk.IsPersisted = true;
                            companyAircraft.Risk.Policy = companyPolicy;
                            companyAircraft = QuotateAircraft(companyAircraft, false, false);
                            companyAircraft.Risk.Coverages.AsParallel().ForAll(x => x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus));
                            result = true;
                        }
                        if (result)
                        {
                            return true;
                        }
                        else
                        {
                            throw new BusinessException(Errors.ErrorDeleteRisk);
                        }
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSearchRisk);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorSearchRisk);
                }
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorSearchRisk);
            }
        }

        public bool GetLeapYear()
        {
            return DelegateService.underwritingService.GetLeapYear();
        }


        /// <summary>
        /// metodo ExcludeCompanyAircraft para Aircraft en Company 
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <param name="riskAircraftId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public CompanyAircraft ExcludeCompanyAircraft(int temporalId, int riskId)
        {
            CompanyAircraft companyAircraft = GetCompanyAircraftTemporalByRiskId(riskId); //GetCompanyAircraftByRiskId(riskId);
            companyAircraft.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

            companyAircraft.Risk.Status = RiskStatusType.Excluded;
            companyAircraft.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.Excluded);
            companyAircraft = QuotateAircraft(companyAircraft, false, false);

            return companyAircraft;
        }

        public CompanyAircraft RunRulesRisk(CompanyAircraft companyAircraft, int ruleId)
        {
            if (!companyAircraft.Risk.Policy.IsPersisted)
            {
                companyAircraft.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyAircraft.Risk.Policy.Id, false);
            }

            return RunRules(companyAircraft, ruleId);
        }

        private CompanyAircraft RunRules(CompanyAircraft companyAircraft, int ruleId)
        {
            UnderwritingServices.Assembler.ModelAssembler.CreateFacadeGeneral(companyAircraft.Risk.Policy, Facade);
            EntityAssembler.CreateFacadeRiskAircraft(Facade, companyAircraft);

            Facade = RulesEngineDelegate.ExecuteRules(ruleId, Facade);

            ModelAssembler.CreateCompanyAircraft(companyAircraft, Facade);
            return companyAircraft;
        }

        public CompanyAircraft QuotateAircraft(CompanyAircraft companyAircraft, bool runRulesPre, bool runRulesPost)
        {
            bool updatePolicy = false;

            if (!companyAircraft.Risk.Policy.IsPersisted)
            {
                companyAircraft.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyAircraft.Risk.Policy.Id, false);
                updatePolicy = true;
            }

            if (runRulesPost && companyAircraft.Risk.Policy.Product.CoveredRisk.RuleSetId.GetValueOrDefault() > 0)
            {
                companyAircraft = RunRules(companyAircraft, companyAircraft.Risk.Policy.Product.CoveredRisk.RuleSetId.Value);
            }

            if (companyAircraft.Risk.Status == RiskStatusType.Excluded)
            {
                companyAircraft.Risk.Coverages = companyAircraft.Risk.Coverages.Where(x => x.CoverStatus != CoverageStatusType.Included).ToList();
                companyAircraft.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.Excluded);
            }

            companyAircraft.Risk.Premium = 0;
            companyAircraft.Risk.AmountInsured = 0;
            List<CompanyCoverage> quotateCoverages = new List<CompanyCoverage>();
            //companyAircraft.Risk.Coverages.AsParallel().ForAll(z =>
            //{
            //    z.EndorsementType = companyAircraft.Risk.Policy.Endorsement.EndorsementType;
            //});


            foreach (CompanyCoverage coverage in companyAircraft.Risk.Coverages)
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                quotateCoverages.Add(coverageBusiness.Quotate(companyAircraft, coverage, runRulesPre, runRulesPost));
                coverageBusiness.UpdateFacadeConcepts(Facade);
            }

            companyAircraft.Risk.Coverages = quotateCoverages;

            //Eliminar Clausulas Poliza
            companyAircraft.Risk.Policy.Clauses = DelegateService.underwritingService.RemoveClauses(companyAircraft.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove));

            //Agregar Clausulas Poliza
            companyAircraft.Risk.Policy.Clauses = DelegateService.underwritingService.AddClauses(companyAircraft.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd));

            //Eliminar Clausulas Riesgo
            companyAircraft.Risk.Clauses = DelegateService.underwritingService.RemoveClauses(companyAircraft.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove));

            //Agregar Clausulas Riesgo
            companyAircraft.Risk.Clauses = DelegateService.underwritingService.AddClauses(companyAircraft.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd));

            //Eliminar Coberturas
            companyAircraft.Risk.Coverages = DelegateService.underwritingService.RemoveCoverages(companyAircraft.Risk.Coverages, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove));

            //Agregar Coberturas
            if (Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd) != null)
            {
                foreach (int coverageId in Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd))
                {
                    if (!companyAircraft.Risk.Coverages.Exists(x => x.Id == coverageId))
                    {
                        CompanyCoverage quotateCoverage = new CompanyCoverage();
                        quotateCoverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, companyAircraft.Risk.Policy.Product.Id, companyAircraft.Risk.GroupCoverage.Id);

                        CoverageBusiness coverageBusiness = new CoverageBusiness();
                        companyAircraft.Risk.Coverages.Add(coverageBusiness.Quotate(companyAircraft, quotateCoverage, true, true));
                        coverageBusiness.UpdateFacadeConcepts(Facade);
                    }
                }
            }

            //Deducibles
            companyAircraft.Risk.Coverages = DelegateService.underwritingService.GetDeductiblesByCompanyCoverages(companyAircraft.Risk.Coverages);

            foreach (CompanyCoverage coverage in companyAircraft.Risk.Coverages)
            {
                if (coverage.Deductible != null)
                {
                    DelegateService.underwritingService.CalculateCompanyPremiumDeductible(coverage);
                }
            }

            //Prima Mínima
            if (companyAircraft.Risk.Policy.CalculateMinPremium == true && companyAircraft.Risk.DynamicProperties?.Count > 0)
            {
                decimal minimumPremiumAmount = DelegateService.underwritingService.GetMinimumPremiumAmountByModelDynamicConcepts(companyAircraft.Risk.DynamicProperties);

                if (minimumPremiumAmount > 0)
                {
                    bool prorate = DelegateService.underwritingService.GetProrateMinimumPremiumByModelDynamicConcepts(companyAircraft.Risk.DynamicProperties);
                    companyAircraft.Risk.Coverages = DelegateService.underwritingService.CalculateMinimumPremiumRatePerCoverage(companyAircraft.Risk.Coverages, minimumPremiumAmount, prorate, false);
                }
            }
            //Prima Mínima

            companyAircraft.Risk.Premium = companyAircraft.Risk.Coverages.Sum(x => x.PremiumAmount);
            companyAircraft.Risk.AmountInsured = companyAircraft.Risk.Coverages.Sum(x => x.LimitAmount);

            if (updatePolicy)
            {
                DelegateService.underwritingService.CreatePolicyTemporal(companyAircraft.Risk.Policy, false);
            }

            return companyAircraft;
        }

        public List<CompanyAircraft> QuotateAircrafts(CompanyPolicy companyPolicy, List<CompanyAircraft> companyPropertyRisks, bool runRulesPre, bool runRulesPost)
        {
            if (!companyPolicy.IsPersisted)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Id, false);
            }

            foreach (CompanyAircraft companyAircraft in companyPropertyRisks)
            {
                companyAircraft.Risk.Policy = companyPolicy;
                QuotateAircraft(companyAircraft, runRulesPre, runRulesPost);
            }

            return companyPropertyRisks;
        }

        /// <summary>
        /// Politicas
        /// </summary>
        /// <param name="companyAircraft"></param>
        /// <returns></returns>
        private List<PoliciesAut> ValidateAuthorizationPolicies(CompanyAircraft companyAircraft)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAircraft = new List<PoliciesAut>();
            companyAircraft.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyAircraft.Risk.Policy.Id, false);
            if (companyAircraft != null && companyAircraft.Risk.Policy != null)
            {
                var key = companyAircraft.Risk.Policy.Prefix.Id + "," + (int)companyAircraft.Risk.Policy.Product.CoveredRisk.CoveredRiskType;
                EntityAssembler.CreateFacadeGeneral(facade, companyAircraft.Risk.Policy);
                EntityAssembler.CreateFacadeRiskAircraft(facade, companyAircraft);
                /*Politica del riesgo*/
                policiesAircraft.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_RISK));
                /*Politicas de cobertura*/
                if (companyAircraft.Risk.Coverages != null)
                {
                    foreach (CompanyCoverage coverage in companyAircraft.Risk.Coverages)
                    {
                        EntityAssembler.CreateFacadeCoverage(facade, coverage);
                        policiesAircraft.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_COVERAGE));
                    }
                }
            }
            return policiesAircraft;
        }
        private void ValidateInfringementPolicies(CompanyPolicy companyPolicy, List<CompanyAircraft> companyAircrafts)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            infringementPolicies.AddRange(companyPolicy.InfringementPolicies);

            if (companyAircrafts != null && companyAircrafts.Any())
            {
                companyAircrafts.ForEach(x =>
                {
                    if (x.Risk.InfringementPolicies != null && x.Risk.InfringementPolicies.Count > 0)
                    {
                        infringementPolicies.AddRange(x.Risk.InfringementPolicies);
                    }
                });
            }

            companyPolicy.InfringementPolicies = DelegateService.authorizationPoliciesService.ValidateInfringementPolicies(infringementPolicies);
        }

        private void ValidateAuthorizationPolicies(CompanyPolicy companyPolicy, List<CompanyAircraft> companyAircrafts)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            infringementPolicies.AddRange(companyPolicy.InfringementPolicies);

            companyAircrafts.ForEach(x =>
            {
                if (x.Risk.InfringementPolicies != null && x.Risk.InfringementPolicies.Count > 0)
                {
                    infringementPolicies.AddRange(x.Risk.InfringementPolicies);
                }
            });

            companyPolicy.InfringementPolicies = DelegateService.authorizationPoliciesService.ValidateInfringementPolicies(infringementPolicies);
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyAircraft> companyAircrafts)
        {
            if (companyPolicy == null || companyAircrafts == null || companyAircrafts.Count == 0)
            {
                throw new ValidationException(Errors.ErrorPolicyOrRisksEmpty);
            }
            ValidateInfringementPolicies(companyPolicy, companyAircrafts);
            if (companyPolicy?.InfringementPolicies?.Count == 0)
            {
                companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
                if (companyPolicy != null)
                {
                    int maxRiskCount = companyPolicy.Summary.RiskCount;
                    int policyId = companyPolicy.Endorsement.PolicyId;
                    int endorsementId = companyPolicy.Endorsement.Id;
                    int endorsementTypeId = (int)companyPolicy.Endorsement.EndorsementType;
                    Core.Application.UnderwritingServices.Enums.EndorsementType endorsementType = (Core.Application.UnderwritingServices.Enums.EndorsementType)endorsementTypeId;
                    try
                    {
                        TP.Parallel.ForEach(companyAircrafts, companyAircraft =>
                        {
                            companyAircraft.Risk.Policy = companyPolicy;
                            if (companyAircraft.Risk.Status == RiskStatusType.Original ||
                                companyAircraft.Risk.Status == RiskStatusType.Included || 
                                companyAircraft.Risk.Status == RiskStatusType.Excluded)
                            {
                                companyAircraft.Risk.Number = ++maxRiskCount;
                            }
                        });
                        //se comenta el IF preguntar a elver si aplica, (si se deja activo no genera registros en la tabla EndorsementRisk)
                        //if (companyPolicy.Product.IsCollective)
                        //{
                        ConcurrentBag<string> errors = new ConcurrentBag<string>();
                        Parallel.ForEach(companyAircrafts, ParallelHelper.DebugParallelFor(), companyAircraft =>
                        {
                            try
                            {
                                CreateEndorsementRisk(companyPolicy, companyAircraft);
                            }
                            catch (Exception ex)
                            {

                                errors.Add(ex.Message);

                            }
                            finally
                            {
                                DataFacadeManager.Dispose();
                            }
                        });
                        if (errors != null && errors.Any())
                        {
                            throw new ValidationException(string.Join(" ", errors));
                        }
                        //}
                        DelegateService.underwritingService.CreateCompanyPolicyPayer(companyPolicy);
                        try
                        {
                            DelegateService.underwritingService.DeleteTemporalByOperationId(companyPolicy.Id, 0, 0, 0);
                        }
                        catch (Exception)
                        {

                            throw new ValidationException("Error tratando de borrar temporal.");
                        }
                    }
                    catch (Exception ex)
                    {

                        DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(policyId, endorsementId, endorsementType);
                        throw ex;
                    }
                }
                else
                {
                    DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, companyPolicy.Endorsement.EndorsementType.Value);
                }
            }
            return companyPolicy;
        }

        private void CreateEndorsementRisk(CompanyPolicy companyPolicy, CompanyAircraft companyAircraft)
        {
            NameValue[] parameters = new NameValue[42];

            parameters[0] = new NameValue("@ENDORSEMENT_ID", companyPolicy.Endorsement.Id);
            parameters[1] = new NameValue("@POLICY_ID", companyPolicy.Endorsement.PolicyId);
            parameters[2] = new NameValue("@PAYER_ID", companyPolicy.Holder.IndividualId);
            parameters[3] = new NameValue("@AIRCRAFT_TYPE_CD", companyAircraft.TypeId);
            parameters[4] = new NameValue("@AIRCRAFT_MAKE_CD", companyAircraft.MakeId);
            parameters[5] = new NameValue("@AIRCRAFT_MODEL_CD", companyAircraft.ModelId);
            parameters[6] = new NameValue("@AIRCRAFT_REGISTER_CD", companyAircraft.RegisterId);
            parameters[7] = new NameValue("@AIRCRAFT_TERRITORY_CD", DBNull.Value);
            parameters[8] = new NameValue("@AIRCRAFT_OPERATOR_CD", companyAircraft.OperatorId);
            parameters[9] = new NameValue("@AIRCRAFT_USE_CD", companyAircraft.UseId);
            parameters[10] = new NameValue("@MOTOR_TYPE_CD", DBNull.Value);
            parameters[11] = new NameValue("@MATERIAL_HULL_CD", DBNull.Value);
            parameters[12] = new NameValue("@PASSENGER_QTY", DBNull.Value);
            parameters[13] = new NameValue("@CREW_QTY", DBNull.Value);
            parameters[14] = new NameValue("@OPERATIONS_BASE", DBNull.Value);
            parameters[15] = new NameValue("@SERIAL_NO", DBNull.Value);
            parameters[16] = new NameValue("@AIRCRAFT_YEAR", DBNull.Value);
            parameters[17] = new NameValue("@AIRCRAFT_DESCRIPTION", DBNull.Value);
            parameters[18] = new NameValue("@LOAD_CAPACITY", DBNull.Value);
            parameters[19] = new NameValue("@OVERHAULING", false);
            parameters[20] = new NameValue("@REGISTER_NO", DBNull.Value);
            parameters[21] = new NameValue("@INSURED_ID", companyAircraft.Risk.MainInsured.IndividualId);
            parameters[22] = new NameValue("@COVERED_RISK_TYPE_CD", (int)companyAircraft.Risk.CoveredRiskType);
            parameters[23] = new NameValue("@RISK_STATUS_CD", (int)companyAircraft.Risk.Status);
            parameters[24] = new NameValue("@COMM_RISK_CLASS_CD", DBNull.Value);
            parameters[25] = new NameValue("@RISK_COMMERCIAL_TYPE_CD", DBNull.Value);

            if (companyAircraft.Risk.Text == null)
            {
                parameters[26] = new NameValue("@CONDITION_TEXT", DBNull.Value);
            }
            else
            {
                parameters[26] = new NameValue("@CONDITION_TEXT", companyAircraft.Risk.Text.TextBody);
            }

            parameters[27] = new NameValue("@RATING_ZONE_CD", DBNull.Value);
            parameters[28] = new NameValue("@COVER_GROUP_ID", companyAircraft.Risk.GroupCoverage.Id);
            parameters[29] = new NameValue("@IS_FACULTATIVE", false);
            parameters[30] = new NameValue("@LIMITS_RC_CD", DBNull.Value);
            parameters[31] = new NameValue("@LIMIT_RC_SUM", DBNull.Value);
            parameters[32] = new NameValue("@SINISTER_PCT", DBNull.Value);
            parameters[33] = new NameValue("@SECONDARY_INSURED_ID", DBNull.Value);


            DataTable dtBeneficiaries = new DataTable("PARAM_TEMP_RISK_BENEFICIARY");
            dtBeneficiaries.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICIARY_ID", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICIARY_TYPE_CD", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICT_PCT", typeof(decimal));
            dtBeneficiaries.Columns.Add("NAME_NUM", typeof(int));

            foreach (CompanyBeneficiary item in companyAircraft.Risk.Beneficiaries)
            {
                DataRow dataRow = dtBeneficiaries.NewRow();
                dataRow["CUSTOMER_TYPE_CD"] = item.CustomerType;
                dataRow["BENEFICIARY_ID"] = item.IndividualId;
                dataRow["BENEFICIARY_TYPE_CD"] = item.BeneficiaryType.Id;
                dataRow["BENEFICT_PCT"] = item.Participation;
                //dataRow["NAME_NUM"] = item.CompanyName.NameNum;

                dtBeneficiaries.Rows.Add(dataRow);
            }

            parameters[34] = new NameValue("@INSERT_TEMP_RISK_BENEFICIARY", dtBeneficiaries);

            DataTable dtCoverages = new DataTable("PARAM_TEMP_RISK_COVERAGE");
            dtCoverages.Columns.Add("COVERAGE_ID", typeof(int));
            dtCoverages.Columns.Add("IS_DECLARATIVE", typeof(bool));
            dtCoverages.Columns.Add("IS_MIN_PREMIUM_DEPOSIT", typeof(bool));
            dtCoverages.Columns.Add("FIRST_RISK_TYPE_CD", typeof(int));
            dtCoverages.Columns.Add("CALCULATION_TYPE_CD", typeof(int));
            dtCoverages.Columns.Add("DECLARED_AMT", typeof(decimal));
            dtCoverages.Columns.Add("PREMIUM_AMT", typeof(decimal));
            dtCoverages.Columns.Add("LIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("LIMIT_IN_EXCESS", typeof(decimal));
            dtCoverages.Columns.Add("LIMIT_OCCURRENCE_AMT", typeof(decimal));
            dtCoverages.Columns.Add("LIMIT_CLAIMANT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("ACC_PREMIUM_AMT", typeof(decimal));
            dtCoverages.Columns.Add("ACC_LIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("ACC_SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("CURRENT_FROM", typeof(DateTime));
            dtCoverages.Columns.Add("RATE_TYPE_CD", typeof(int));
            dtCoverages.Columns.Add("RATE", typeof(decimal));
            dtCoverages.Columns.Add("CURRENT_TO", typeof(DateTime));
            dtCoverages.Columns.Add("COVER_NUM", typeof(int));
            dtCoverages.Columns.Add("RISK_COVER_ID", typeof(int));
            dtCoverages.Columns.Add("COVER_STATUS_CD", typeof(int));
            dtCoverages.Columns.Add("COVER_ORIGINAL_STATUS_CD", typeof(int));
            dtCoverages.Columns.Add("CONDITION_TEXT", typeof(string));
            dtCoverages.Columns.Add("ENDORSEMENT_LIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("ENDORSEMENT_SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("FLAT_RATE_PCT", typeof(decimal));
            dtCoverages.Columns.Add("CONTRACT_AMOUNT_PCT", typeof(decimal));
            dtCoverages.Columns.Add("DYNAMIC_PROPERTIES", typeof(byte[]));
            dtCoverages.Columns.Add("SHORT_TERM_PCT", typeof(decimal));
            dtCoverages.Columns.Add("PREMIUM_AMT_DEPOSIT_PERCENT", typeof(decimal));
            dtCoverages.Columns.Add("MAX_LIABILITY_AMT", typeof(decimal));

            DataTable dtDeductibles = new DataTable("PARAM_TEMP_RISK_COVER_DEDUCT");
            dtDeductibles.Columns.Add("COVERAGE_ID", typeof(int));
            dtDeductibles.Columns.Add("RATE_TYPE_CD", typeof(int));
            dtDeductibles.Columns.Add("RATE", typeof(decimal));
            dtDeductibles.Columns.Add("DEDUCT_PREMIUM_AMT", typeof(decimal));
            dtDeductibles.Columns.Add("DEDUCT_VALUE", typeof(decimal));
            dtDeductibles.Columns.Add("DEDUCT_UNIT_CD", typeof(int));
            dtDeductibles.Columns.Add("DEDUCT_SUBJECT_CD", typeof(int));
            dtDeductibles.Columns.Add("MIN_DEDUCT_VALUE", typeof(decimal));
            dtDeductibles.Columns.Add("MIN_DEDUCT_UNIT_CD", typeof(int));
            dtDeductibles.Columns.Add("MIN_DEDUCT_SUBJECT_CD", typeof(int));
            dtDeductibles.Columns.Add("MAX_DEDUCT_VALUE", typeof(decimal));
            dtDeductibles.Columns.Add("MAX_DEDUCT_UNIT_CD", typeof(int));
            dtDeductibles.Columns.Add("MAX_DEDUCT_SUBJECT_CD", typeof(int));
            dtDeductibles.Columns.Add("CURRENCY_CD", typeof(int));
            dtDeductibles.Columns.Add("ACC_DEDUCT_AMT", typeof(decimal));
            dtDeductibles.Columns.Add("DEDUCT_ID", typeof(int));

            companyAircraft.Risk.Coverages = companyAircraft.Risk.Coverages.OrderBy(x => x.Id).ToList();

            foreach (CompanyCoverage item in companyAircraft.Risk.Coverages)
            {
                DataRow dataRow = dtCoverages.NewRow();
                dataRow["RISK_COVER_ID"] = item.Id;
                dataRow["COVERAGE_ID"] = item.Id;
                dataRow["IS_DECLARATIVE"] = item.IsDeclarative;
                dataRow["IS_MIN_PREMIUM_DEPOSIT"] = item.IsMinPremiumDeposit;
                dataRow["FIRST_RISK_TYPE_CD"] = (int)FirstRiskType.None;
                dataRow["CALCULATION_TYPE_CD"] = item.CalculationType.Value;
                dataRow["DECLARED_AMT"] = item.DeclaredAmount;
                dataRow["PREMIUM_AMT"] = item.PremiumAmount;
                dataRow["LIMIT_AMT"] = item.LimitAmount;
                dataRow["SUBLIMIT_AMT"] = item.SubLimitAmount;
                dataRow["LIMIT_IN_EXCESS"] = item.ExcessLimit;
                dataRow["LIMIT_OCCURRENCE_AMT"] = item.LimitOccurrenceAmount;
                dataRow["LIMIT_CLAIMANT_AMT"] = item.LimitClaimantAmount;
                dataRow["ACC_PREMIUM_AMT"] = item.AccumulatedPremiumAmount;
                dataRow["ACC_LIMIT_AMT"] = item.AccumulatedLimitAmount;
                dataRow["ACC_SUBLIMIT_AMT"] = item.AccumulatedSubLimitAmount;
                dataRow["CURRENT_FROM"] = item.CurrentFrom;
                dataRow["RATE_TYPE_CD"] = item.RateType;

                if (item.Rate.HasValue)
                {
                    dataRow["RATE"] = item.Rate.Value;
                }

                dataRow["CURRENT_TO"] = item.CurrentTo;
                dataRow["COVER_NUM"] = item.Number;
                if (item.CoverStatus.HasValue)
                {
                    dataRow["COVER_STATUS_CD"] = item.CoverStatus.Value;
                }

                if (item.CoverageOriginalStatus.HasValue)
                {
                    dataRow["COVER_ORIGINAL_STATUS_CD"] = item.CoverageOriginalStatus.Value;
                }

                if (!string.IsNullOrEmpty(item.Text?.TextBody))
                {
                    dataRow["CONDITION_TEXT"] = item.Text.TextBody;
                }

                dataRow["ENDORSEMENT_LIMIT_AMT"] = item.EndorsementLimitAmount;
                dataRow["ENDORSEMENT_SUBLIMIT_AMT"] = item.EndorsementSublimitAmount;
                dataRow["FLAT_RATE_PCT"] = item.FlatRatePorcentage;
                dataRow["SHORT_TERM_PCT"] = item.ShortTermPercentage;
                dataRow["PREMIUM_AMT_DEPOSIT_PERCENT"] = item.DepositPremiumPercent;
                dataRow["MAX_LIABILITY_AMT"] = item.MaxLiabilityAmount;


                if (item.Deductible != null)
                {
                    DataRow dataRowDeductible = dtDeductibles.NewRow();
                    dataRowDeductible["COVERAGE_ID"] = item.Id;
                    dataRowDeductible["RATE_TYPE_CD"] = item.Deductible.RateType;
                    dataRowDeductible["RATE"] = (object)item.Deductible.Rate ?? DBNull.Value;
                    dataRowDeductible["DEDUCT_PREMIUM_AMT"] = item.Deductible.DeductPremiumAmount;
                    dataRowDeductible["DEDUCT_VALUE"] = item.Deductible.DeductValue;

                    if (item.Deductible.DeductibleUnit != null && item.Deductible.DeductibleUnit.Id != 0)
                    {
                        dataRowDeductible["DEDUCT_UNIT_CD"] = item.Deductible.DeductibleUnit.Id;
                    }

                    if (item.Deductible.DeductibleSubject != null)
                    {
                        dataRowDeductible["DEDUCT_SUBJECT_CD"] = item.Deductible.DeductibleSubject.Id;
                    }

                    if (item.Deductible.MinDeductValue.HasValue)
                    {
                        dataRowDeductible["MIN_DEDUCT_VALUE"] = item.Deductible.MinDeductValue.Value;
                    }

                    if (item.Deductible.MinDeductibleUnit != null && item.Deductible.MinDeductibleUnit.Id != 0)
                    {
                        dataRowDeductible["MIN_DEDUCT_UNIT_CD"] = item.Deductible.MinDeductibleUnit.Id;
                    }

                    if (item.Deductible.MinDeductibleSubject != null && item.Deductible.MinDeductibleSubject.Id != 0)
                    {
                        dataRowDeductible["MIN_DEDUCT_SUBJECT_CD"] = item.Deductible.MinDeductibleSubject.Id;
                    }

                    if (item.Deductible.MaxDeductValue.HasValue)
                    {
                        dataRowDeductible["MAX_DEDUCT_VALUE"] = item.Deductible.MaxDeductValue.Value;
                    }

                    if (item.Deductible.MaxDeductibleUnit != null && item.Deductible.MaxDeductibleUnit.Id != 0)
                    {
                        dataRowDeductible["MAX_DEDUCT_UNIT_CD"] = item.Deductible.MaxDeductibleUnit.Id;
                    }

                    if (item.Deductible.MaxDeductibleSubject != null && item.Deductible.MaxDeductibleSubject.Id != 0)
                    {
                        dataRowDeductible["MAX_DEDUCT_SUBJECT_CD"] = item.Deductible.MaxDeductibleSubject.Id;
                    }

                    if (item.Deductible.Currency != null)
                    {
                        dataRowDeductible["CURRENCY_CD"] = item.Deductible.Currency.Id;
                    }

                    dataRowDeductible["ACC_DEDUCT_AMT"] = item.Deductible.AccDeductAmt;
                    dataRowDeductible["DEDUCT_ID"] = item.Deductible.Id;

                    dtDeductibles.Rows.Add(dataRowDeductible);
                }

                dtCoverages.Rows.Add(dataRow);
            }

            parameters[35] = new NameValue("@INSERT_TEMP_RISK_COVERAGE", dtCoverages);
            parameters[36] = new NameValue("@INSERT_TEMP_RISK_COVER_DEDUCT", dtDeductibles);

            DataTable dtClauses = new DataTable("PARAM_TEMP_CLAUSE");
            dtClauses.Columns.Add("CLAUSE_ID", typeof(int));
            dtClauses.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtClauses.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dtClauses.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            if (companyAircraft.Risk.Clauses != null)
            {
                foreach (CompanyClause item in companyAircraft.Risk.Clauses)
                {
                    DataRow dataRow = dtClauses.NewRow();
                    dataRow["CLAUSE_ID"] = item.Id;
                    dataRow["CLAUSE_STATUS_CD"] = (int)ClauseStatuses.Original;
                    dtClauses.Rows.Add(dataRow);
                }
            }

            parameters[37] = new NameValue("@INSERT_TEMP_CLAUSE", dtClauses);
            parameters[38] = new NameValue("@RISK_NUM", companyAircraft.Risk.Number);
            parameters[39] = new NameValue("OPERATION", JsonConvert.SerializeObject(companyAircraft));
            parameters[40] = new NameValue("@RISK_INSP_TYPE_CD", 1);
            parameters[41] = new NameValue("@INSPECTION_ID", DBNull.Value);

            DataTable result;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("ISS.RECORD_RISK_AIRCRAFT", parameters);
            }

            if (!Convert.ToBoolean(result.Rows[0][0].ToString()))
            {
                throw new BusinessException(result.Rows[0][1].ToString());
            }
        }

        public List<CompanyAircraft> GetRisks(int policyId, List<ISSEN.Risk> risks, RiskAircraftsviewR1 viewR1)
        {
            if (risks == null || risks.Count < 1 || viewR1.CompanyAircrafts == null || viewR1.CompanyAircrafts.Count < 1)
            {
                throw new ArgumentException(Errors.ErrorRiskEmpty);
            }
            try
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = viewR1.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.RiskAircraft> riskAircrafts = viewR1.RiskAircrafts.Cast<ISSEN.RiskAircraft>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = viewR1.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<ISSEN.RiskPayer> RiskPayers = viewR1.RiskPayers.Cast<ISSEN.RiskPayer>().ToList();
                List<ISSEN.RiskClause> riskClauses = viewR1.RiskClause.Cast<ISSEN.RiskClause>().ToList();

                List<CompanyAircraft> Aircrafts = new List<CompanyAircraft>();
                foreach (ISSEN.Risk item in risks)
                {
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.LoadDynamicProperties(item);
                    }
                    CompanyAircraft Aircraft = new CompanyAircraft();
                    Aircraft = ModelAssembler.CreateAircraft(item, riskAircrafts.First(x => x.RiskId == item.RiskId), endorsementRisks.First(x => x.RiskId == item.RiskId));

                    CompanyIssuanceInsured companyInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(Aircraft.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                    if (companyInsured == null || companyInsured.IndividualId < 1)
                    {
                        throw new Exception(Errors.ErrorInsuredMain);
                    }

                    Aircraft.Risk.MainInsured = companyInsured;
                    Aircraft.Risk.MainInsured.Name = Aircraft.Risk.MainInsured.Name + " " + Aircraft.Risk.MainInsured.Surname + " " + Aircraft.Risk.MainInsured.SecondSurname;
                    ConcurrentBag<CompanyClause> clauses = new ConcurrentBag<CompanyClause>();
                    //clausulas
                    if (riskClauses != null && riskClauses.Count > 0)
                    {
                        TP.Parallel.ForEach(riskClauses.Where(x => x.RiskId == item.RiskId).ToList(), riskClause =>
                        {
                            clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                        });
                        Aircraft.Risk.Clauses = clauses.ToList();
                    }
                    CompanyName companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(Aircraft.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                    Aircraft.Risk.MainInsured.CompanyName = new IssuanceCompanyName
                    {
                        NameNum = companyName.NameNum,
                        TradeName = companyName.TradeName,
                        Address = new IssuanceAddress
                        {
                            Id = companyName.Address.Id,
                            Description = companyName.Address.Description,
                            City = companyName.Address.City
                        },
                        Phone = new IssuancePhone
                        {
                            Id = companyName.Phone.Id,
                            Description = companyName.Phone.Description
                        }
                    };
                    if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
                    {
                        Aircraft.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                        Object objlock = new object();
                        var imappers = ModelAssembler.CreateMapCompanyBeneficiary();
                        TP.Parallel.ForEach(riskBeneficiaries.Where(x => x.RiskId == item.RiskId), riskBeneficiary =>
                        {
                            CompanyBeneficiary CiaBeneficiary = new CompanyBeneficiary();
                            var beneficiaryRisk = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), (Core.Services.UtilitiesServices.Enums.InsuredSearchType)InsuredSearchType.IndividualId).FirstOrDefault();
                            if (beneficiaryRisk != null)
                            {
                                CiaBeneficiary = imappers.Map<Beneficiary, CompanyBeneficiary>(beneficiaryRisk);
                                CiaBeneficiary.CustomerType = (Core.Services.UtilitiesServices.Enums.CustomerType)CustomerType.Individual;
                                CiaBeneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode };
                                CiaBeneficiary.Participation = riskBeneficiary.BenefitPercentage;
                                List<CompanyName> companyNames = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(CiaBeneficiary.IndividualId, (CustomerType)CiaBeneficiary.CustomerType);
                                companyName = new CompanyName();
                                if (companyNames.Exists(x => x.NameNum == 0 && x.IsMain))
                                {
                                    companyName = companyNames.First(x => x.IsMain);
                                }
                                else
                                {
                                    companyName = companyNames.First();
                                }
                                CiaBeneficiary.CompanyName = new IssuanceCompanyName
                                {
                                    NameNum = companyName.NameNum,
                                    TradeName = companyName.TradeName,
                                    Address = new IssuanceAddress
                                    {
                                        Id = companyName.Address.Id,
                                        Description = companyName.Address.Description,
                                        City = companyName.Address.City
                                    },
                                    Phone = new IssuancePhone
                                    {
                                        Id = companyName.Phone.Id,
                                        Description = companyName.Phone.Description
                                    },
                                    Email = new IssuanceEmail
                                    {
                                        Id = companyName.Email.Id,
                                        Description = companyName.Email.Description
                                    }
                                };
                                lock (objlock)
                                {
                                    Aircraft.Risk.Beneficiaries.Add(CiaBeneficiary);
                                }
                            }
                        });
                    }

                    //coberturas
                    List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

                    companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId, item.RiskId);
                    Aircraft.Risk.Coverages = companyCoverages;
                    Aircrafts.Add(Aircraft);
                }
                return Aircrafts;
            }

            catch (Exception ex)
            {

                throw new BusinessException(ex.ToString());
            }
        }

        /// jhgomez
        //public List<CompanyAircraft> GetRisks(int policyId, List<ISSEN.Risk> risks, RiskAircraftsview view)
        //{
        //    if (risks == null || risks.Count < 1 || view.CompanyAircrafts == null || view.CompanyAircrafts.Count < 1)
        //    {
        //        throw new ArgumentException(Errors.ErrorRiskEmpty);
        //    }
        //    try
        //    {
        //        List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
        //        List<ISSEN.RiskAircraft> riskAircrafts = view.RiskAircrafts.Cast<ISSEN.RiskAircraft>().ToList();
        //        List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
        //        List<ISSEN.RiskPayer> RiskPayers = view.RiskPayers.Cast<ISSEN.RiskPayer>().ToList();
        //        List<ISSEN.RiskClause> riskClauses = view.RiskClause.Cast<ISSEN.RiskClause>().ToList();

        //        List<CompanyAircraft> Aircrafts = new List<CompanyAircraft>();
        //        foreach (ISSEN.Risk item in risks)
        //        {
        //            using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //            {
        //                daf.LoadDynamicProperties(item);
        //            }
        //            CompanyAircraft Aircraft = new CompanyAircraft();
        //            Aircraft = ModelAssembler.CreateAircraft(item, riskAircrafts.First(x => x.RiskId == item.RiskId), endorsementRisks.First(x => x.RiskId == item.RiskId));

        //            var companyInsured = DelegateService.uniquePersonService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(Aircraft.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
        //            if (companyInsured == null || companyInsured.IndividualId < 1)
        //            {
        //                throw new Exception(Errors.ErrorInsuredMain);
        //            }
        //            var imapper = ModelAssembler.CreateMapPersonInsured();
        //            Aircraft.Risk.MainInsured = imapper.Map<CompanyInsured, CompanyIssuanceInsured>(companyInsured);
        //            Aircraft.Risk.MainInsured.Name = Aircraft.Risk.MainInsured.Name + " " + Aircraft.Risk.MainInsured.Surname + " " + Aircraft.Risk.MainInsured.SecondSurname;
        //            ConcurrentBag<CompanyClause> clauses = new ConcurrentBag<CompanyClause>();
        //            //clausulas
        //            if (riskClauses != null && riskClauses.Count > 0)
        //            {
        //                Parallel.ForEach(riskClauses.Where(x => x.RiskId == item.RiskId).ToList(), riskClause =>
        //                {
        //                    clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
        //                });
        //                Aircraft.Risk.Clauses = clauses.ToList();
        //            }
        //            var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(Aircraft.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
        //            Aircraft.Risk.MainInsured.CompanyName = new IssuanceCompanyName
        //            {
        //                NameNum = companyName.NameNum,
        //                TradeName = companyName.TradeName,
        //                Address = new IssuanceAddress
        //                {
        //                    Id = companyName.Address.Id,
        //                    Description = companyName.Address.Description,
        //                    City = companyName.Address.City
        //                },
        //                Phone = new IssuancePhone
        //                {
        //                    Id = companyName.Phone.Id,
        //                    Description = companyName.Phone.Description
        //                }
        //            };
        //            if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
        //            {
        //                Aircraft.Risk.Beneficiaries = new List<CompanyBeneficiary>();
        //                Object objlock = new object();
        //                var imappers = ModelAssembler.CreateMapCompanyBeneficiary();
        //                Parallel.ForEach(riskBeneficiaries.Where(x => x.RiskId == item.RiskId), riskBeneficiary =>
        //                {
        //                    CompanyBeneficiary CiaBeneficiary = new CompanyBeneficiary();
        //                    var beneficiaryRisk = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), (Core.Services.UtilitiesServices.Enums.InsuredSearchType)InsuredSearchType.IndividualId).FirstOrDefault();
        //                    if (beneficiaryRisk != null)
        //                    {
        //                        CiaBeneficiary = imappers.Map<Beneficiary, CompanyBeneficiary>(beneficiaryRisk);
        //                        CiaBeneficiary.CustomerType = (Core.Services.UtilitiesServices.Enums.CustomerType)CustomerType.Individual;
        //                        CiaBeneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode };
        //                        CiaBeneficiary.Participation = riskBeneficiary.BenefitPercentage;
        //                        List<CompanyName> companyNames = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(CiaBeneficiary.IndividualId, (CustomerType)CiaBeneficiary.CustomerType);
        //                        companyName = new CompanyName();
        //                        if (companyNames.Exists(x => x.NameNum == 0 && x.IsMain))
        //                        {
        //                            companyName = companyNames.First(x => x.IsMain);
        //                        }
        //                        else
        //                        {
        //                            companyName = companyNames.First();
        //                        }
        //                        CiaBeneficiary.CompanyName = new IssuanceCompanyName
        //                        {
        //                            NameNum = companyName.NameNum,
        //                            TradeName = companyName.TradeName,
        //                            Address = new IssuanceAddress
        //                            {
        //                                Id = companyName.Address.Id,
        //                                Description = companyName.Address.Description,
        //                                City = companyName.Address.City
        //                            },
        //                            Phone = new IssuancePhone
        //                            {
        //                                Id = companyName.Phone.Id,
        //                                Description = companyName.Phone.Description
        //                            },
        //                            Email = new IssuanceEmail
        //                            {
        //                                Id = companyName.Email.Id,
        //                                Description = companyName.Email.Description
        //                            }
        //                        };
        //                        lock (objlock)
        //                        {
        //                            Aircraft.Risk.Beneficiaries.Add(CiaBeneficiary);
        //                        }
        //                    }
        //                });
        //            }

        //            //coberturas
        //            List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

        //            companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId, item.RiskId);
        //            Aircraft.Risk.Coverages = companyCoverages;
        //            Aircrafts.Add(Aircraft);
        //        }
        //        return Aircrafts;
        //    }

        //    catch (Exception ex)
        //    {

        //        throw new BusinessException(ex.ToString());
        //    }
        //}

        public List<CompanyAircraft> GetCompanyAircraftsByPolicyIdEndorsementId(int policyId, int endorsementId)
        {

            List<CompanyAircraft> companyAircrafts = new List<CompanyAircraft>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name);
            filter.IsNotNull();
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);
            filter.And();
            filter.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.In();
            filter.ListValue();
            filter.Constant((int)RiskStatusType.Excluded);
            filter.Constant((int)RiskStatusType.Cancelled);
            filter.EndList();


            RiskAircraftsviewR1 view = new RiskAircraftsviewR1();
            ViewBuilder builder = new ViewBuilder("RiskAircraftsViewR1");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.EndorsementOperations.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.RiskCoverage> RiskCoverage = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                foreach (ISSEN.EndorsementOperation entityEndorsementOperation in view.EndorsementOperations)
                {
                    CompanyAircraft companyAircraft = new CompanyAircraft();
                    companyAircraft = JsonConvert.DeserializeObject<CompanyAircraft>(entityEndorsementOperation.Operation);

                    companyAircraft.Risk.Id = entityEndorsementRisks.First(x => x.RiskNum == entityEndorsementOperation.RiskNumber).RiskId;
                    companyAircraft.Risk.RiskId = entityEndorsementRisks.First(x => x.RiskNum == entityEndorsementOperation.RiskNumber).RiskId;

                    companyAircraft.Risk.OriginalStatus = companyAircraft.Risk.Status;
                    companyAircraft.Risk.Status = RiskStatusType.NotModified;

                    companyAircraft.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
                    companyAircraft.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.NotModified);

                    foreach (var coverage in companyAircraft.Risk.Coverages)
                    {
                        if (coverage.OriginalLimitAmount == 0)
                            coverage.OriginalLimitAmount = coverage.LimitAmount;

                        if (coverage.OriginalSubLimitAmount == 0)
                            coverage.OriginalSubLimitAmount = coverage.SubLimitAmount;
                    }
                    companyAircraft.Risk.Policy.Endorsement.Id = entityEndorsementRisks.First(x => x.EndorsementId == entityEndorsementOperation.EndorsementId).EndorsementId;

                    foreach (ISSEN.RiskCoverage riskCoverages in view.RiskCoverages)
                    {
                        companyAircraft.Risk.Coverages.ForEach(x => x.RiskCoverageId = x.RiskCoverageId);
                    }

                    companyAircrafts.Add(companyAircraft);
                }
            }
            else
            {
                ObjectCriteriaBuilder filterR1 = new ObjectCriteriaBuilder();
                filterR1.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                filterR1.Equal();
                filterR1.Constant(policyId);
                filterR1.And();
                filterR1.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
                filterR1.Equal();
                filterR1.Constant(true);
                filterR1.And();
                filterR1.Not();
                filterR1.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                filterR1.In();
                filterR1.ListValue();
                filterR1.Constant((int)RiskStatusType.Excluded);
                filterR1.Constant((int)RiskStatusType.Cancelled);
                filterR1.EndList();

                RiskAircraftsviewR1 viewr1 = new RiskAircraftsviewR1();
                ViewBuilder builderR1 = new ViewBuilder("RiskAircraftsviewR1");
                builderR1.Filter = filterR1.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builderR1, viewr1);

                List<ISSEN.Risk> risks = viewr1.CompanyAircrafts.Cast<ISSEN.Risk>().ToList();
                companyAircrafts.AddRange(GetRisks(policyId, risks, viewr1));
            }


            return companyAircrafts;
        }


        public List<CompanyEndorsement> GetCompanyEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Endorsement.Properties.EndoTypeCode, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementTypeId);
            filter.And();
            filter.Property(ISSEN.Endorsement.Properties.PolicyId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.In();
            filter.ListValue();
            filter.Constant((int)RiskStatusType.Excluded);
            filter.Constant((int)RiskStatusType.Cancelled);
            filter.EndList();

            RiskAircraftsviewR1 viewr = new RiskAircraftsviewR1();
            ViewBuilder builder = new ViewBuilder("RiskAircraftsviewR1");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, viewr);


            List<ISSEN.Endorsement> entityEndorsements = viewr.Endorsements.Cast<ISSEN.Endorsement>().ToList();
            List<CompanyEndorsement> companyEndorsements = new List<CompanyEndorsement>();
            if (viewr.Endorsements.Count > 0)
            {
                if (entityEndorsements == null || entityEndorsements.Count < 1)
                {
                    throw new ArgumentException(Errors.ErrorEndorsementTypeIdEmpty);
                }
                try
                {
                    companyEndorsements = ModelAssembler.CreateCompanyEndorsements(entityEndorsements);
                }
                catch (Exception)
                {
                    throw new ArgumentException(Errors.ErrorEndorsementTypeIdEmpty);
                }
            }
            return companyEndorsements;
        }
        public List<CompanyCoverage> GetCoveragesByRiskId(int riskId, int temporalId)
        {
            List<CompanyCoverage> coverages = new List<CompanyCoverage>();
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationByIdParentId(riskId, temporalId);

            if (pendingOperation != null)
            {
                CompanyAircraft companyAircraft = JsonConvert.DeserializeObject<CompanyAircraft>(pendingOperation.Operation);
                companyAircraft.Risk.Id = pendingOperation.Id;
                companyAircraft.Risk.IsPersisted = true;
                coverages = companyAircraft.Risk.Coverages;
            }
            //            CompanyAircraft companyAircraft = GetCompanyAircraftTemporalByRiskId(temporalId);

            return coverages;

        }

        public List<CompanyCoverage> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name);
            filter.IsNotNull();
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(riskId);
            filter.And();
            filter.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.In();
            filter.ListValue();
            filter.Constant((int)RiskStatusType.Excluded);
            filter.Constant((int)RiskStatusType.Cancelled);
            filter.EndList();


            RiskAircraftsviewR1 view = new RiskAircraftsviewR1();
            ViewBuilder builder = new ViewBuilder("RiskAircraftsViewR1");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.EndorsementOperations.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.RiskCoverage> RiskCoverage = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                foreach (ISSEN.EndorsementOperation entityEndorsementOperation in view.EndorsementOperations)
                {
                    CompanyAircraft companyAircraft = new CompanyAircraft();
                    companyAircraft = JsonConvert.DeserializeObject<CompanyAircraft>(entityEndorsementOperation.Operation);

                    companyAircraft.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
                    companyAircraft.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.NotModified);
                    companyAircraft.Risk.Policy.Endorsement.Id = entityEndorsementRisks.First(x => x.EndorsementId == entityEndorsementOperation.EndorsementId).EndorsementId;

                    foreach (ISSEN.RiskCoverage riskCoverages in view.RiskCoverages)
                    {
                        companyAircraft.Risk.Coverages.ForEach(x => x.RiskCoverageId = x.RiskCoverageId);
                    }
                    CompanyCoverage companyCoverage = new CompanyCoverage();
                    companyCoverage.CoverageAllied = companyAircraft.Risk.Coverages;
                    companyCoverages.Add(companyCoverage);
                }

                companyCoverages = GetCoveragesById(companyCoverages);
            }
            return companyCoverages;
        }

        private List<CompanyCoverage> GetCoveragesById(List<CompanyCoverage> companyCoverages)
        {
            List<CompanyCoverage> companyCoverages2 = new List<CompanyCoverage>();
            companyCoverages2 = companyCoverages;
            List<int> listCoveragesId = new List<int>();
            foreach (var item in companyCoverages2)
            {
                foreach (var item3 in item.CoverageAllied)
                {
                    listCoveragesId.Add(item3.Id);
                }
            }

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(IQUO.Coverage.Properties.CoverageId, typeof(IQUO.Coverage).Name);
            filter.In();
            filter.ListValue();
            foreach (int value in listCoveragesId)
            {
                filter.Constant((int)value);
            }
            filter.EndList();

            CompanyAircraftsRiskCoveragesView view = new CompanyAircraftsRiskCoveragesView();
            ViewBuilder builder = new ViewBuilder("RiskCoveragesView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Coverages.Count > 0)
            {
                List<IQUO.Coverage> Coverages = view.Coverages.Cast<IQUO.Coverage>().ToList();
                foreach (var item in companyCoverages2)
                {
                    foreach (var coveredAllied in item.CoverageAllied)
                    {
                        coveredAllied.Description = Coverages.First(x => x.CoverageId == coveredAllied.Id).PrintDescription;
                    }
                }
            }
            return companyCoverages2;
        }



        public List<CompanyAircraft> GetCompanyAircraftsByEndorsementId(int endorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementOperation.Properties.EndorsementId, typeof(ISSEN.EndorsementOperation).Name).Equal().Constant(endorsementId);
            filter.And();
            filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name).IsNotNull();
            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                var result = daf.SelectObjects(typeof(ISSEN.EndorsementOperation), filter.GetPredicate());
                businessCollection = new BusinessCollection(result);
            }
            if (businessCollection != null && businessCollection.Any())
            {
                List<CompanyAircraft> companyAircrafts = ModelAssembler.CreateAircrafts(businessCollection);

                if (companyAircrafts.Count > 0)
                {
                    if (companyAircrafts[0].Risk.Coverages != null)
                    {
                        return companyAircrafts;
                    }
                    else
                    {
                        return GetCompanyAircraftsFromTables(endorsementId);
                    }
                }
                else
                {
                    return GetCompanyAircraftsFromTables(endorsementId);
                }
            }
            else
            {
                return null;
            }
        }


        private List<CompanyAircraft> GetCompanyAircraftsFromTables(int endorsementId)
        {
            List<CompanyAircraft> companyAircrafts = new List<CompanyAircraft>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            RiskAircraftsView view = new RiskAircraftsView();
            ViewBuilder builder = new ViewBuilder("RiskAircraftsview");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.RiskAircrafts.Count > 0)
            {
                List<ISSEN.Risk> risks = view.RiskAircrafts.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                //List<CompanyCoRisk> coRisks = view.CoRisks.Cast<CompanyCoRisk>().ToList();
                List<ISSEN.RiskAircraft> riskAircrafts = view.RiskAircrafts.Cast<ISSEN.RiskAircraft>().ToList();
                //List<CompanyCoRiskVehicle> coRiskVehicles = view.CoRiskVehicles.Cast<CompanyCoRiskVehicle>().ToList();
                //List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                //List<COMMEN.VehicleModel> vehicleModels = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
                //List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                //List<COMMEN.VehicleVersion> vehicleVersions = view.VehicleVersions.Cast<COMMEN.VehicleVersion>().ToList();
                Parallel.ForEach(risks, ParallelHelper.DebugParallelFor(), item =>
                {
                    DataFacadeManager.Instance.GetDataFacade().LoadDynamicProperties(item);
                    CompanyAircraft aircraft = new CompanyAircraft();
                    //CompanyVehicle aircraft = new CompanyVehicle();

                    aircraft = ModelAssembler.CreateAircraft(item,
                        //coRisks.Where(x => x.RiskId == item.RiskId).First(),
                        riskAircrafts.Where(x => x.RiskId == item.RiskId).First(),
                        //coRiskVehicles.Where(x => x.RiskId == item.RiskId).First(),
                        endorsementRisks.Where(x => x.RiskId == item.RiskId).First()
                        );

                    //aircraft.Make.Description = vehicleMakes.Where(x => x.VehicleMakeCode == aircraft.Make.Id).FirstOrDefault().SmallDescription;
                    //aircraft.Model.Description = vehicleModels.Where(x => x.VehicleModelCode == aircraft.Model.Id).FirstOrDefault().SmallDescription;
                    //aircraft.Version.Description = vehicleVersions.Where(x => x.VehicleVersionCode == aircraft.Version.Id).FirstOrDefault().Description;

                    int insuredNameNum = aircraft.Risk.MainInsured.CompanyName.NameNum;
                    aircraft.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(aircraft.Risk.MainInsured.IndividualId.ToString(), Sistran.Core.Services.UtilitiesServices.Enums.InsuredSearchType.IndividualId, Sistran.Core.Services.UtilitiesServices.Enums.CustomerType.Individual);

                    var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(aircraft.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                    aircraft.Risk.MainInsured.CompanyName = new IssuanceCompanyName
                    {
                        NameNum = companyName.NameNum,
                        TradeName = companyName.TradeName,
                        Address = new IssuanceAddress
                        {
                            Id = companyName.Address.Id,
                            Description = companyName.Address.Description,
                            City = companyName.Address.City
                        },
                        Phone = new IssuancePhone
                        {
                            Id = companyName.Phone.Id,
                            Description = companyName.Phone.Description
                        },
                        Email = new IssuanceEmail
                        {
                            Id = companyName.Email.Id,
                            Description = companyName.Email.Description
                        }
                    };
                    aircraft.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                    //foreach (ISSEN.RiskBeneficiary riskBeneficiary in riskBeneficiaries.Where(x => x.RiskId == item.RiskId))
                    //{
                    //    CompanyBeneficiary beneficiary = new CompanyBeneficiary();
                    //    beneficiary = ModelAssembler.CreateBeneficiary(riskBeneficiary);

                    //    int beneficiaryNameNum = beneficiary.CompanyName.NameNum;
                    //    List<CompanyName> companyNames = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(beneficiary.IndividualId, (CustomerType)beneficiary.CustomerType);
                    //    companyName = new CompanyName();
                    //    if (beneficiaryNameNum == 0)
                    //    {
                    //        companyName = companyNames.First(x => x.IsMain);
                    //    }
                    //    else
                    //    {
                    //        companyName = companyNames.First(x => x.NameNum == beneficiaryNameNum);
                    //    }
                    //    beneficiary.CompanyName = new IssuanceCompanyName
                    //    {
                    //        NameNum = companyName.NameNum,
                    //        TradeName = companyName.TradeName,
                    //        Address = new IssuanceAddress
                    //        {
                    //            Id = companyName.Address.Id,
                    //            Description = companyName.Address.Description,
                    //            City = companyName.Address.City
                    //        },
                    //        Phone = new IssuancePhone
                    //        {
                    //            Id = companyName.Phone.Id,
                    //            Description = companyName.Phone.Description
                    //        },
                    //        Email = new IssuanceEmail
                    //        {
                    //            Id = companyName.Email.Id,
                    //            Description = companyName.Email.Description
                    //        }
                    //    };
                    //    aircraft.Risk.Beneficiaries.Add(beneficiary);
                    //}

                    aircraft.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(endorsementRisks[0].PolicyId, endorsementId, item.RiskId);

                    DataFacadeManager.Dispose();
                    lock (companyAircrafts)
                    {
                        companyAircrafts.Add(aircraft);
                    }
                });
            }

            return companyAircrafts;
        }

        public List<CompanyAircraft> GetCompanyAircraftByInsuredId(int insuredId)
        {
            List<CompanyAircraft> companyAircrafts = new List<CompanyAircraft>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);
            filter.And();
            filter.Property(ISSEN.RiskAircraft.Properties.AircraftTypeCode, typeof(ISSEN.RiskAircraft).Name);
            filter.Equal();
            filter.Constant(1);

            ClaimRiskAircraftsView claimRiskAircraftsView = new ClaimRiskAircraftsView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskAircraftsView");
            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskAircraftsView);

            if (claimRiskAircraftsView.RiskAircrafts.Count > 0)
            {
                foreach (ISSEN.RiskAircraft entityRiskAircraft in claimRiskAircraftsView.RiskAircrafts)
                {
                    ISSEN.Risk entityRisk = claimRiskAircraftsView.Risks.Cast<ISSEN.Risk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                    ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskAircraftsView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                    ISSEN.Policy entityPolicy = claimRiskAircraftsView.Policies.Cast<ISSEN.Policy>().First(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                    CompanyAircraft companyAircraft = ModelAssembler.CreateAircraft(entityRisk, entityRiskAircraft, entityEndorsementRisk);

                    companyAircraft.Make = new Models.CompanyMake
                    {
                        Id = Convert.ToInt32(entityRiskAircraft.AircraftMakeCode)
                    };

                    companyAircraft.Model = new Models.CompanyModel
                    {
                        Id = Convert.ToInt32(entityRiskAircraft.AircraftModelCode)
                    };

                    companyAircraft.TypeId = Convert.ToInt32(entityRiskAircraft.AircraftTypeCode);
                    companyAircraft.UseId = Convert.ToInt32(entityRiskAircraft.AircraftUseCode);
                    companyAircraft.RegisterId = Convert.ToInt32(entityRiskAircraft.AircraftRegisterCode);
                    companyAircraft.OperatorId = Convert.ToInt32(entityRiskAircraft.AircraftOperatorCode);
                    companyAircraft.NumberRegister = entityRiskAircraft.RegisterNo;

                    companyAircraft.Risk.Description = entityRiskAircraft.RegisterNo + " - " + entityRiskAircraft.AircraftYear;

                    companyAircraft.Risk.Policy = new CompanyPolicy
                    {
                        Id = entityPolicy.PolicyId,
                        DocumentNumber = entityPolicy.DocumentNumber,
                        Endorsement = new CompanyEndorsement
                        {
                            Id = entityEndorsementRisk.EndorsementId
                        },
                        Prefix = new CommonServices.Models.CompanyPrefix
                        {
                            Id = entityPolicy.PrefixCode
                        }
                    };

                    companyAircrafts.Add(companyAircraft);
                }
            }

            return companyAircrafts;
        }

        public CompanyAircraft GetCompanyAircraftByRiskId(int riskId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.RiskId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(riskId);

            CompanyAircraft companyAircraft = new CompanyAircraft();

            ClaimRiskAircraftsView claimRiskAircraftsView = new ClaimRiskAircraftsView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskAircraftsView");
            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskAircraftsView);

            if (claimRiskAircraftsView.RiskAircrafts.Count > 0)
            {
                ISSEN.RiskAircraft entityRiskAircraft = claimRiskAircraftsView.RiskAircrafts.Cast<ISSEN.RiskAircraft>().First();
                ISSEN.Risk entityRisk = claimRiskAircraftsView.Risks.Cast<ISSEN.Risk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskAircraftsView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                ISSEN.Policy entityPolicy = claimRiskAircraftsView.Policies.Cast<ISSEN.Policy>().First(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                companyAircraft = ModelAssembler.CreateAircraft(entityRisk, entityRiskAircraft, entityEndorsementRisk);

                companyAircraft.Make = new Models.CompanyMake
                {
                    Id = Convert.ToInt32(entityRiskAircraft.AircraftMakeCode)
                };

                companyAircraft.Model = new Models.CompanyModel
                {
                    Id = Convert.ToInt32(entityRiskAircraft.AircraftModelCode)
                };

                companyAircraft.TypeId = Convert.ToInt32(entityRiskAircraft.AircraftTypeCode);
                companyAircraft.UseId = Convert.ToInt32(entityRiskAircraft.AircraftUseCode);
                companyAircraft.RegisterId = Convert.ToInt32(entityRiskAircraft.AircraftRegisterCode);
                companyAircraft.OperatorId = Convert.ToInt32(entityRiskAircraft.AircraftOperatorCode);
                companyAircraft.NumberRegister = entityRiskAircraft.RegisterNo;

                companyAircraft.Risk.Description = entityRiskAircraft.RegisterNo + " - " + entityRiskAircraft.AircraftYear;

                companyAircraft.Risk.Policy = new CompanyPolicy
                {
                    Id = entityPolicy.PolicyId,
                    DocumentNumber = entityPolicy.DocumentNumber,
                    Endorsement = new CompanyEndorsement
                    {
                        Id = entityEndorsementRisk.EndorsementId
                    },
                    Prefix = new CommonServices.Models.CompanyPrefix
                    {
                        Id = entityPolicy.PrefixCode
                    }
                };

                return companyAircraft;
            }

            return null;
        }

        public List<CompanyAircraft> GetCompanyClaimAircraftsByEndorsementId(int endorsemetId)
        {
            List<CompanyAircraft> companyAircrafts = new List<CompanyAircraft>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsemetId);

            ClaimRiskAircraftsView claimRiskAircraftsView = new ClaimRiskAircraftsView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskAircraftsView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskAircraftsView);

            if (claimRiskAircraftsView.RiskAircrafts.Count > 0)
            {
                foreach (ISSEN.RiskAircraft entityRiskAircraft in claimRiskAircraftsView.RiskAircrafts)
                {
                    CompanyAircraft companyAircraft = new CompanyAircraft();
                    ISSEN.Risk entityRisk = claimRiskAircraftsView.Risks.Cast<ISSEN.Risk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                    ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskAircraftsView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                    ISSEN.Policy entityPolicy = claimRiskAircraftsView.Policies.Cast<ISSEN.Policy>().First(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                    companyAircraft = ModelAssembler.CreateAircraft(entityRisk, entityRiskAircraft, entityEndorsementRisk);

                    companyAircraft.Make = new Models.CompanyMake
                    {
                        Id = Convert.ToInt32(entityRiskAircraft.AircraftMakeCode)
                    };

                    companyAircraft.Model = new Models.CompanyModel
                    {
                        Id = Convert.ToInt32(entityRiskAircraft.AircraftModelCode)
                    };

                    companyAircraft.TypeId = Convert.ToInt32(entityRiskAircraft.AircraftTypeCode);
                    companyAircraft.UseId = Convert.ToInt32(entityRiskAircraft.AircraftUseCode);
                    companyAircraft.RegisterId = Convert.ToInt32(entityRiskAircraft.AircraftRegisterCode);
                    companyAircraft.OperatorId = Convert.ToInt32(entityRiskAircraft.AircraftOperatorCode);
                    companyAircraft.NumberRegister = entityRiskAircraft.RegisterNo;

                    companyAircraft.Risk.Description = entityRiskAircraft.RegisterNo + " - " + entityRiskAircraft.AircraftYear;

                    companyAircraft.Risk.Policy = new CompanyPolicy
                    {
                        Id = entityPolicy.PolicyId,
                        DocumentNumber = entityPolicy.DocumentNumber,
                        Endorsement = new CompanyEndorsement
                        {
                            Id = entityEndorsementRisk.EndorsementId
                        },
                        Prefix = new CommonServices.Models.CompanyPrefix
                        {
                            Id = entityPolicy.PrefixCode
                        }
                    };

                    ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                    filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filterSumAssured.Equal();
                    filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                    SumAssuredAirCraftView assuredView = new SumAssuredAirCraftView();
                    ViewBuilder builderAssured = new ViewBuilder("SumAssuredAirCraftView");
                    builderAssured.Filter = filterSumAssured.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                    decimal insuredAmount = 0;

                    foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                    {
                        insuredAmount += item.LimitAmount;
                    }

                    companyAircraft.Risk.AmountInsured = insuredAmount;

                    companyAircrafts.Add(companyAircraft);
                }

                return companyAircrafts;
            }

            return companyAircrafts;
        }
    }
}

