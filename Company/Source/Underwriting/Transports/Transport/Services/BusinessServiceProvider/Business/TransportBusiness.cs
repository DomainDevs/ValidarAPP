using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.Queries;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Framework;
using System.Collections.Concurrent;
using Sistran.Core.Application.Utilities.Helper;
using System.Data;
using Sistran.Core.Application.Transports.TransportBusinessService.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Views;
using DAFENG = Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.UnderwritingServices;
using IQUO = Sistran.Core.Application.Quotation.Entities;
using PAREN = Sistran.Core.Application.Parameters.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Company.Application.Transports.TransportBusinessService.Models;
using Sistran.Company.Application.Transports.TransportBusinessService.Enums;
using UTILITIES = Company.UnderwritingUtilities;
using Sistran.Core.Application.Utilities.Constants;
using UTIENUMS = Sistran.Core.Services.UtilitiesServices.Enums;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using Sistran.Core.Application.CommonService.Models;
using TP = Sistran.Core.Application.Utilities.Utility;
using System.Diagnostics;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Enums;

namespace Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Business
{
    public class TransportBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        /// <summary>
        /// Crear el transporte temporal
        /// </summary>
        /// <param name="companyTransport"></param>
        /// <param name="isMassive"></param>
        /// <returns></returns>
        public CompanyTransport CreateCompanyTransportTemporal(CompanyTransport companyTransport)
        {
            companyTransport.Risk.InfringementPolicies = ValidateAuthorizationPolicies(companyTransport);

            PendingOperation pendingOperation = new PendingOperation();
            //CompanyPolicy policy = companyTransport.Risk.Policy;
            //companyTransport.Risk.Policy = null;

            if (companyTransport.Risk.Id == 0)
            {
                pendingOperation.CreationDate = DateTime.Now;
                pendingOperation.ParentId = companyTransport.Risk.Policy.Id;
                pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyTransport);

                pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
            }
            else
            {
                pendingOperation.ModificationDate = DateTime.Now;
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(companyTransport.Risk.Id);
                if (pendingOperation != null)
                {
                    pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyTransport);
                    DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);

                }
                else
                {
                    throw new Exception("Error obteniendo el Temporal");
                }
            }

            companyTransport.Risk.Id = pendingOperation.Id;
            //companyTransport.Risk.IsPersisted = true;
            //companyTransport.Risk.Policy = policy;

            if (companyTransport.Risk.Policy.TemporalType != TemporalType.Endorsement)
            {
                companyTransport = SaveCompanyTransportTemporalTables(companyTransport);
            }

            return companyTransport;
        }

        public CompanyTransport UpdateCompanyTransportTemporal(CompanyTransport companyTransport)
        {
            companyTransport.Risk.InfringementPolicies = ValidateAuthorizationPolicies(companyTransport);

            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";
            PendingOperation pendingOperation = new PendingOperation();
            //CompanyPolicy policy = companyTransport.Risk.Policy;
            //companyTransport.Risk.Policy = null;

            pendingOperation.ModificationDate = DateTime.Now;
            pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(companyTransport.Risk.Id);
            if (pendingOperation != null)
            {
                pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyTransport);
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);

            }
            else
            {
                throw new Exception("Error obteniendo el Temporal");
            }

            companyTransport.Risk.Id = pendingOperation.Id;
            //companyTransport.Risk.Policy = policy;

            if (companyTransport.Risk.Policy.TemporalType != TemporalType.Endorsement)
            {
                companyTransport = SaveCompanyTransportTemporalTables(companyTransport);
            }

            return companyTransport;
        }

        public CompanyTransport SaveCompanyTransportTemporalTables(CompanyTransport companyTransport)
        {
            DAFENG.IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DAFENG.DynamicPropertiesSerializer();

            DataTable dataTable;
            NameValue[] parameters = new NameValue[11];

            UTILITIES.GetDatatables dts = new UTILITIES.GetDatatables();
            UTILITIES.CommonDataTables datatables = dts.GetcommonDataTables(companyTransport.Risk);


            DataTable dtTempRisk = datatables.dtTempRisk;
            parameters[0] = new NameValue(dtTempRisk.TableName, dtTempRisk);

            DataTable dtCOTempRisk = datatables.dtCOTempRisk;
            parameters[1] = new NameValue(dtCOTempRisk.TableName, dtCOTempRisk);

            DataTable dtBeneficary = datatables.dtBeneficary;
            parameters[2] = new NameValue(dtBeneficary.TableName, dtBeneficary);

            DataTable dtRiskPayer = datatables.dtRiskPayer;
            parameters[3] = new NameValue(dtRiskPayer.TableName, dtRiskPayer);

            DataTable dtRiskClause = datatables.dtRiskClause;
            parameters[4] = new NameValue(dtRiskClause.TableName, dtRiskClause);

            DataTable dtRiskCoverage = datatables.dtRiskCoverage; //UTILITIES.ModelAssembler.GetDataTableRisCoverage(liabilityRisk.Risk);
            parameters[5] = new NameValue(dtRiskCoverage.TableName, dtRiskCoverage);

            DataTable dtDeduct = datatables.dtDeduct;
            parameters[6] = new NameValue(dtDeduct.TableName, dtDeduct);

            DataTable dtCoverClause = datatables.dtCoverClause;
            parameters[7] = new NameValue(dtCoverClause.TableName, dtCoverClause);

            DataTable dtDynamicRisk = datatables.dtDynamic;
            parameters[8] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_RISK", dtDynamicRisk);

            DataTable dtDynamicCoverage = datatables.dtDynamicCoverage;
            parameters[9] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicCoverage);

            DataTable dtTempRiskTransport = ModelAssembler.GetDataTableTempRiskTransport(companyTransport);
            parameters[10] = new NameValue(dtTempRiskTransport.TableName, dtTempRiskTransport);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("TMP.CIA_SAVE_TEMPORAL_RISK_TRANSPORT_TEMP", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (companyTransport.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    companyTransport.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return companyTransport;
            }
            else
            {
                //wmw
                //throw new ValidationException(Errors.ErrorCreateTemporalCompanyLiability);//ErrrRecordTemporal "error al grabar riesgo
                throw new ValidationException("Error temporal riesgo");
            }

        }

        public bool DeleteCompanyTransportTemporal(int riskId)
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
        public CompanyTransport GetCompanyTransportTemporalByRiskId(int riskId)
        {
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(riskId);

            if (pendingOperation != null)
            {
                CompanyTransport companyTransport = COMUT.JsonHelper.DeserializeJson<CompanyTransport>(pendingOperation.Operation);
                companyTransport.Risk.Id = pendingOperation.Id;
                companyTransport.Risk.IsPersisted = true;
                if (companyTransport.Risk.Policy == null)
                {
                    companyTransport.Risk.Policy = DelegateService.underwritingService.GetPolicyByPendingOperation(pendingOperation.Id);
                }

                if (companyTransport.InsuredObjects == null && companyTransport?.Risk?.Coverages != null && companyTransport.Risk.Coverages.Any())
                {
                    companyTransport.InsuredObjects = companyTransport.Risk.Coverages.GroupBy(z => new { z.InsuredObject.Id, z.InsuredObject.Description }).Select(x => new CompanyInsuredObject { Id = x.Key.Id, Description = x.Key.Description, Premium = x.Sum(m => m.PremiumAmount), Amount = x.Sum(m => m.LimitAmount) }).ToList();
                }

                companyTransport.InsuredObjects = GetCompanyTransportRiskByRiskId(companyTransport.InsuredObjects, companyTransport.Risk.Coverages);

                return companyTransport;
            }
            else
            {
                return null;
            }
        }
        public List<CompanyInsuredObject> GetCompanyTransportRiskByRiskId(List<CompanyInsuredObject> InsuredObjects, List<CompanyCoverage> companyCoverages)
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

        public List<CompanyTransport> GetCompanyTransportsByTemporalId(int temporalId)
        {
            List<CompanyTransport> companyTransports = new List<CompanyTransport>();
            List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);

            foreach (PendingOperation pendingOperation in pendingOperations)
            {
                CompanyTransport companyTransport = COMUT.JsonHelper.DeserializeJson<CompanyTransport>(pendingOperation.Operation);
                companyTransport.Risk.Id = pendingOperation.Id;
                companyTransport.Risk.IsPersisted = true;
                companyTransports.Add(companyTransport);
            }

            return companyTransports;
        }

        /// <summary>
        /// Obtener Poliza de transportes
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <returns>Listado de Trasnportes - Riesgos</returns>
        public List<CompanyTransport> GetCompanyTransportsByPolicyId(int policyId)
        {
            List<CompanyTransport> companyTransports = new List<CompanyTransport>();

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

            //filter.EndList();


            RiskTransportsView view = new RiskTransportsView();
            DAFENG.ViewBuilder builder = new DAFENG.ViewBuilder("RiskTransportsView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            DataFacadeManager.Dispose();

            if (view.EndorsementOperations.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.RiskCoverage> RiskCoverage = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                List<ISSEN.TransportViaType> transportViaTypes = view.TransportViaTypes.Cast<ISSEN.TransportViaType>().ToList();
                List<ISSEN.TransportMean> transportMeans = view.TransportMeans.Cast<ISSEN.TransportMean>().ToList();
                List<ISSEN.TransportPackagingType> transportPackagingTypes = view.TransportPackagingTypes.Cast<ISSEN.TransportPackagingType>().ToList();
                foreach (ISSEN.EndorsementOperation entityEndorsementOperation in view.EndorsementOperations)
                {
                    CompanyTransport companyTransport = new CompanyTransport();
                    companyTransport = COMUT.JsonHelper.DeserializeJson<CompanyTransport>(entityEndorsementOperation.Operation);
                    if (companyTransport.Risk.Status != RiskStatusType.Excluded)
                    {
                        companyTransport.Risk.Id = 0;
                        companyTransport.Risk.RiskId = entityEndorsementRisks.First(x => x.RiskNum == entityEndorsementOperation.RiskNumber).RiskId;
                        companyTransport.Risk.OriginalStatus = companyTransport.Risk.Status;
                        companyTransport.Risk.Status = RiskStatusType.NotModified;
                        companyTransport.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
                        companyTransport.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.NotModified);
                        List<CompanyCoverage> companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, entityEndorsementOperation.EndorsementId, companyTransport.Risk.RiskId);
                        foreach (var item in companyCoverages)
                        {
                            companyTransport.Risk.Coverages.Where(x => x.Id == item.Id).First().RiskCoverageId = item.RiskCoverageId;
                        }
                        companyTransport.Risk.Beneficiaries.ForEach(x => x.CustomerType = (Core.Services.UtilitiesServices.Enums.CustomerType)CustomerType.Individual);

                        foreach (ISSEN.TransportViaType viaType in transportViaTypes)
                        {
                            companyTransport.ViaType.Description = viaType.Description;
                        }
                        foreach (ISSEN.TransportPackagingType Packaging in transportPackagingTypes)
                        {
                            companyTransport.PackagingType.Description = Packaging.Description;
                        }

                        if (transportMeans.Count > 0)
                        {

                            foreach (var item in companyTransport.Types)
                            {
                                item.Description = transportMeans.First(x => x.TransportMeanCode == item.Id).Description;
                            }
                        }
                        companyTransports.Add(companyTransport);
                    }
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


                RiskTransportsviewR1 viewr1 = new RiskTransportsviewR1();
                DAFENG.ViewBuilder builderR1 = new DAFENG.ViewBuilder("RiskTransportsviewR1");
                builderR1.Filter = filterR1.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builderR1, viewr1);
                DataFacadeManager.Dispose();
                List<ISSEN.Risk> risks = viewr1.CompanyTransports.Cast<ISSEN.Risk>().ToList();
                companyTransports.AddRange(GetRisks(policyId, risks, viewr1));
            }

            return companyTransports;
        }

        internal bool SaveInsuredObject(int riskId, CompanyInsuredObject insuredObject, int tempId, int groupCoverageId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);
                PolicyType policyType = DelegateService.commonService.GetPolicyTypesByPrefixIdById(companyPolicy.Prefix.Id, companyPolicy.PolicyType.Id);
                CompanyTransport companyTransport = GetCompanyTransportTemporalByRiskId(riskId);
                if (companyTransport == null)
                {
                    return false;
                }
                companyTransport.Risk.IsPersisted = true;

                companyTransport.InsuredObjects.Where(x => x.Id == insuredObject.Id).Select(x =>
                {
                    x.Amount = insuredObject.Amount;
                    x.DepositPremiunPercent = Convert.ToDecimal(insuredObject.DepositPremiunPercent);
                    x.Rate = insuredObject.Rate;
                    x.Premium = insuredObject.Premium;
                    return x;

                }).ToList();

                List<CompanyCoverage> coverages_ = companyTransport.Risk?.Coverages?.Where(u => u.InsuredObject.Id == insuredObject.Id)?.ToList();
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
                    insuredObject.Id, companyTransport.Risk.GroupCoverage.Id, companyPolicy.Product.Id);
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
                        if (companyTransport.InsuredObjects.Exists(x => x.Id == item.InsuredObject.Id))
                        {

                        }
                        else
                        {
                            companyTransport.InsuredObjects.Add(item.InsuredObject);
                        }
                    }
                    if (companyTransport.Risk.Coverages == null)
                    {
                        companyTransport.Risk.Coverages = new List<CompanyCoverage>();
                    }
                    companyTransport.Risk.Coverages.AddRange(coverages_);
                }
                companyTransport.Risk.IsPersisted = true;
                companyTransport.Risk.Policy = companyPolicy;
                companyTransport = QuotateTransport(companyTransport, true, true);
                UpdateCompanyTransportTemporal(companyTransport);
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
                CompanyTransport risk = GetCompanyTransportTemporalByRiskId(riskId);
                if (risk != null && risk.Risk != null)
                {
                    List<CompanyCoverage> coverages = new List<CompanyCoverage>();
                    CompanyInsuredObject insuredObject = new CompanyInsuredObject();
                    if (risk.Risk.Coverages != null && risk.Risk.Coverages.Any())
                    {
                        if (insuredObjectId != 0)
                        {
                            insuredObject = risk.InsuredObjects.Where(x => x.Id == insuredObjectId).FirstOrDefault();
                            if (insuredObject == null)
                            {
                                coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Product.Id).Where(u => u.IsSelected == true).ToList();
                            }
                            else
                            {
                                coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Product.Id).ToList();
                                foreach (CompanyCoverage item in risk.Risk.Coverages.Where(x => x.InsuredObject.Id == insuredObjectId))
                                {
                                    item.IsPrimary = coverages.First(x => x.Id == item.Id).IsPrimary;
                                    item.CoverNum = coverages.First(x => x.Id == item.Id).CoverNum;
                                    item.Number = coverages.First(x => x.Id == item.Id).CoverNum;
                                    item.MainCoverageId = coverages.First(x => x.Id == item.Id).MainCoverageId.GetValueOrDefault();
                                    item.SublimitPercentage = coverages.First(x => x.Id == item.Id).SublimitPercentage;
                                    item.AllyCoverageId = coverages.First(x => x.Id == item.Id).AllyCoverageId;
                                }
                                coverages = risk.Risk.Coverages.Where(x => x.InsuredObject.Id == insuredObject.Id).Select(item =>
                                {
                                    item.InsuredObject = insuredObject;
                                    return item;
                                }).ToList();
                            }
                            if (coverages?.Count < 1)
                            {
                                coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Product.Id).Where(u => u.IsSelected == true).ToList();
                            }
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
                        coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Product.Id).Where(u => u.IsSelected == true).ToList();
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
                    CompanyTransport companyTransport = GetCompanyTransportTemporalByRiskId(riskId);
                    if (companyTransport != null)
                    {

                        if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Emission || companyPolicy.Endorsement.EndorsementType == EndorsementType.Renewal || companyTransport.Risk.Status == RiskStatusType.Included || companyTransport.Risk.Status == null || companyTransport.Risk.Status == RiskStatusType.Original)
                        {
                            //result = DelegateService.utilitiesServiceCore.DeletePendingOperation(companyTransport.Risk.Id);
                            //DelegateService.underwritingService.DeleteRisk(companyTransport.Risk.Id);
                            result = DelegateService.underwritingService.DeleteCompanyRisksByRiskId(riskId, false);
                        }
                        else
                        {
                            if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Modification)
                            {
                                foreach (var item in companyTransport.Risk.Coverages)
                                {
                                    item.EndorsementType = EndorsementType.Modification;
                                }
                            }
                            companyTransport.Risk.Status = RiskStatusType.Excluded;
                            companyTransport.Risk.Description = companyTransport.Risk.Description + " (" + EnumHelper.GetItemName<RiskStatusType>(RiskStatusType.Excluded) + ")";
                            companyTransport.Risk.IsPersisted = true;
                            companyTransport.Risk.Policy = companyPolicy;
                            companyTransport = QuotateTransport(companyTransport, false, false);
                            companyTransport.Risk.Coverages.AsParallel().ForAll(x => x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus));
                            CreateCompanyTransportTemporal(companyTransport);
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
        /// metodo ExcludeCompanyTransport para Transport en Company 
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <param name="risktransportId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public CompanyTransport ExcludeCompanyTransport(int temporalId, int riskId)
        {
            CompanyTransport companyTransport = GetCompanyTransportTemporalByRiskId(riskId); //GetCompanyTransportByRiskId(riskId);
            companyTransport.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

            companyTransport.Risk.Status = RiskStatusType.Excluded;
            companyTransport.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.Excluded);
            companyTransport = QuotateTransport(companyTransport, false, false);

            return companyTransport;
        }

        public CompanyTransport RunRulesRisk(CompanyTransport companyTransport, int ruleId)
        {
            if (!companyTransport.Risk.Policy.IsPersisted)
            {
                companyTransport.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyTransport.Risk.Policy.Id, false);
            }

            return RunRules(companyTransport, ruleId);
        }

        private CompanyTransport RunRules(CompanyTransport companyTransport, int ruleId)
        {
            UnderwritingServices.Assembler.ModelAssembler.CreateFacadeGeneral(companyTransport.Risk.Policy, Facade);
            EntityAssembler.CreateFacadeRiskTransport(Facade, companyTransport);

            Facade = RulesEngineDelegate.ExecuteRules(ruleId, Facade);

            ModelAssembler.CreateCompanyTransport(companyTransport, Facade);
            return companyTransport;
        }

        public CompanyTransport QuotateTransport(CompanyTransport companyTransport, bool runRulesPre, bool runRulesPost)
        {
            bool updatePolicy = false;

            if (!companyTransport.Risk.Policy.IsPersisted)
            {
                companyTransport.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyTransport.Risk.Policy.Id, false);
                updatePolicy = true;
            }

            if (runRulesPost && companyTransport.Risk.Policy.Product.CoveredRisk.RuleSetId.GetValueOrDefault() > 0)
            {
                companyTransport = RunRules(companyTransport, companyTransport.Risk.Policy.Product.CoveredRisk.RuleSetId.Value);
            }

            //foreach (CompanyCoverage item in companyTransport.Risk.Coverages)
            //{
            //    if (item.MaxLiabilityAmount == 0)
            //    {
            //        item.MaxLiabilityAmount = item.LimitAmount;
            //    }
            //}

            if (companyTransport.Risk.Status == RiskStatusType.Excluded)
            {
                companyTransport.Risk.Coverages = companyTransport.Risk.Coverages.Where(x => x.CoverStatus != CoverageStatusType.Included).ToList();
                companyTransport.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.Excluded);
            }

            companyTransport.Risk.Premium = 0;
            companyTransport.Risk.AmountInsured = 0;
            List<CompanyCoverage> quotateCoverages = new List<CompanyCoverage>();

            foreach (CompanyCoverage coverage in companyTransport.Risk.Coverages)
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                quotateCoverages.Add(coverageBusiness.Quotate(companyTransport, coverage, runRulesPre, runRulesPost));
                coverageBusiness.UpdateFacadeConcepts(Facade);
            }

            companyTransport.Risk.Coverages = quotateCoverages;

            //Eliminar Clausulas Poliza
            companyTransport.Risk.Policy.Clauses = DelegateService.underwritingService.RemoveClauses(companyTransport.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove));

            //Agregar Clausulas Poliza
            companyTransport.Risk.Policy.Clauses = DelegateService.underwritingService.AddClauses(companyTransport.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd));

            //Eliminar Clausulas Riesgo
            companyTransport.Risk.Clauses = DelegateService.underwritingService.RemoveClauses(companyTransport.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove));

            //Agregar Clausulas Riesgo
            companyTransport.Risk.Clauses = DelegateService.underwritingService.AddClauses(companyTransport.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd));

            //Eliminar Coberturas
            companyTransport.Risk.Coverages = DelegateService.underwritingService.RemoveCoverages(companyTransport.Risk.Coverages, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove));

            //Agregar Coberturas
            if (Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd) != null)
            {
                foreach (int coverageId in Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd))
                {
                    if (!companyTransport.Risk.Coverages.Exists(x => x.Id == coverageId))
                    {
                        CompanyCoverage quotateCoverage = new CompanyCoverage();
                        quotateCoverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, companyTransport.Risk.Policy.Product.Id, companyTransport.Risk.GroupCoverage.Id);

                        CoverageBusiness coverageBusiness = new CoverageBusiness();
                        companyTransport.Risk.Coverages.Add(coverageBusiness.Quotate(companyTransport, quotateCoverage, true, true));
                        coverageBusiness.UpdateFacadeConcepts(Facade);
                    }
                }
            }

            //Deducibles
            companyTransport.Risk.Coverages = DelegateService.underwritingService.GetDeductiblesByCompanyCoverages(companyTransport.Risk.Coverages);

            foreach (CompanyCoverage coverage in companyTransport.Risk.Coverages)
            {
                if (coverage.Deductible != null)
                {
                    DelegateService.underwritingService.CalculateCompanyPremiumDeductible(coverage);
                }
            }

            //Prima Mínima
            if (companyTransport.Risk.Policy.CalculateMinPremium == true)
            {
                decimal minimumPremiumAmount = DelegateService.underwritingService.GetMinimumPremiumAmountByModelDynamicConcepts(companyTransport.Risk.DynamicProperties);

                if (minimumPremiumAmount > 0)
                {
                    bool prorate = DelegateService.underwritingService.GetProrateMinimumPremiumByModelDynamicConcepts(companyTransport.Risk.DynamicProperties);
                    companyTransport.Risk.Coverages = DelegateService.underwritingService.CalculateMinimumPremiumRatePerCoverage(companyTransport.Risk.Coverages, minimumPremiumAmount, prorate, false);
                }
            }
            //Prima Mínima

            companyTransport.Risk.Premium = companyTransport.Risk.Coverages.Sum(x => x.PremiumAmount);
            companyTransport.Risk.AmountInsured = companyTransport.Risk.Coverages.Sum(x => x.LimitAmount);

            if (updatePolicy)
            {
                DelegateService.underwritingService.CreatePolicyTemporal(companyTransport.Risk.Policy, false);
            }

            return companyTransport;
        }

        public List<CompanyTransport> QuotateTransports(CompanyPolicy companyPolicy, List<CompanyTransport> companyPropertyRisks, bool runRulesPre, bool runRulesPost)
        {
            if (!companyPolicy.IsPersisted)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Id, false);
            }

            foreach (CompanyTransport companyTransport in companyPropertyRisks)
            {
                companyTransport.Risk.Policy = companyPolicy;
                QuotateTransport(companyTransport, runRulesPre, runRulesPost);
            }

            return companyPropertyRisks;
        }

        /// <summary>
        /// Politicas
        /// </summary>
        /// <param name="companyTransport"></param>
        /// <returns></returns>
        private List<PoliciesAut> ValidateAuthorizationPolicies(CompanyTransport companyTransport)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesTrans = new List<PoliciesAut>();
            companyTransport.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyTransport.Risk.Policy.Id, false);
            if (companyTransport != null && companyTransport.Risk.Policy != null)
            {
                var key = companyTransport.Risk.Policy.Prefix.Id + "," + (int)companyTransport.Risk.Policy.Product.CoveredRisk.CoveredRiskType;
                facade = DelegateService.underwritingService.CreateFacadeGeneral(companyTransport.Risk.Policy);
                EntityAssembler.CreateFacadeRiskTransport(facade, companyTransport);
                /*Politica del riesgo*/
                policiesTrans.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_RISK));
                /*Politicas de cobertura*/
                if (companyTransport.Risk.Coverages != null)
                {
                    foreach (CompanyCoverage coverage in companyTransport.Risk.Coverages)
                    {
                        EntityAssembler.CreateFacadeCoverage(facade, coverage);
                        policiesTrans.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_COVERAGE));
                    }
                }
            }
            return policiesTrans;
        }
        private void ValidateInfringementPolicies(CompanyPolicy companyPolicy, List<CompanyTransport> companyTransports)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            infringementPolicies.AddRange(companyPolicy.InfringementPolicies);

            if (companyTransports != null && companyTransports.Any())
            {
                companyTransports.ForEach(x =>
                {
                    if (x.Risk.InfringementPolicies != null && x.Risk.InfringementPolicies.Count > 0)
                    {
                        infringementPolicies.AddRange(x.Risk.InfringementPolicies);
                    }
                });
            }

            companyPolicy.InfringementPolicies = DelegateService.authorizationPoliciesService.ValidateInfringementPolicies(infringementPolicies);
        }

        private void ValidateAuthorizationPolicies(CompanyPolicy companyPolicy, List<CompanyTransport> companyTransports)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            infringementPolicies.AddRange(companyPolicy.InfringementPolicies);

            companyTransports.ForEach(x =>
            {
                if (x.Risk.InfringementPolicies != null && x.Risk.InfringementPolicies.Count > 0)
                {
                    infringementPolicies.AddRange(x.Risk.InfringementPolicies);
                }
            });

            companyPolicy.InfringementPolicies = DelegateService.authorizationPoliciesService.ValidateInfringementPolicies(infringementPolicies);
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyTransport> companyTransports)
        {
            if (companyPolicy == null || companyTransports == null || companyTransports.Count == 0)
            {
                throw new ValidationException(Errors.ErrorPolicyOrRisksEmpty);
            }
            ValidateInfringementPolicies(companyPolicy, companyTransports);
            if (companyPolicy?.InfringementPolicies?.Count == 0)
            {
                companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
                if (companyPolicy != null)
                {
                    int maxRiskCount = companyPolicy.Summary.RiskCount;
                    int policyId = companyPolicy.Endorsement.PolicyId;
                    int endorsementId = companyPolicy.Endorsement.Id;
                    int endorsementTypeId = (int)companyPolicy.Endorsement.EndorsementType;
                    int operationId = companyPolicy.Id;

                    Core.Application.UnderwritingServices.Enums.EndorsementType endorsementType = (Core.Application.UnderwritingServices.Enums.EndorsementType)endorsementTypeId;
                    try
                    {
                        foreach (var companyTransport in companyTransports)
                        {
                            companyTransport.Risk.Policy = companyPolicy;
                            if (companyTransport.Risk.Status == RiskStatusType.Original ||
                                companyTransport.Risk.Status == RiskStatusType.Included)
                            {
                                companyTransport.Risk.Number = ++maxRiskCount;
                            }
                        }
                        if (companyPolicy.Product.IsCollective)
                        {
                            ConcurrentBag<string> errors = new ConcurrentBag<string>();
                            TP.Parallel.ForEach(companyTransports, companyTransport =>
                           {
                               try
                               {
                                   CreateEndorsementRisk(companyPolicy, companyTransport);
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
                        }
                        else
                        {
                            foreach (var companyTransport in companyTransports)
                            {
                                CreateEndorsementRisk(companyPolicy, companyTransport);
                            }
                        }
                        DelegateService.underwritingService.CreateCompanyPolicyPayer(companyPolicy);
                        try
                        {
                            DelegateService.underwritingService.DeleteTemporalByOperationId(companyPolicy.Id, 0, 0, 0);
                            try
                            {
                                DelegateService.underwritingService.SaveControlPolicy(policyId, endorsementId, operationId, (int)companyPolicy.PolicyOrigin);
                            }
                            catch (Exception)
                            {
                                EventLog.WriteEntry("Application", Errors.ErrorRegisterIntegration);
                            }
                        }
                        catch (Exception)
                        {

                            throw new ValidationException(Errors.ErrorDeleteCompanyTransportTemporal);
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

        private void CreateEndorsementRisk(CompanyPolicy companyPolicy, CompanyTransport companyTransport)
        {
            try
            {


                NameValue[] parameters = new NameValue[49];

                parameters[0] = new NameValue("@ENDORSEMENT_ID", companyPolicy.Endorsement.Id);
                parameters[1] = new NameValue("@POLICY_ID", companyPolicy.Endorsement.PolicyId);
                parameters[2] = new NameValue("@PAYER_ID", companyPolicy.Holder.IndividualId);
                parameters[3] = new NameValue("@TRANSPORT_CARGO_TYPE_CD", companyTransport.CargoType.Id);
                parameters[4] = new NameValue("@TRANSPORT_PACKAGING_TYPE_CD", companyTransport.PackagingType.Id);

                if (companyPolicy.PolicyType.IsFloating)
                {
                    parameters[5] = new NameValue("@DECLARATION_PERIOD_CD", companyTransport.DeclarationPeriod.Id);
                    parameters[6] = new NameValue("@ADJUST_PERIOD_CD", companyTransport.AdjustPeriod.Id);
                    parameters[7] = new NameValue("@SOURCE", companyTransport.Source);
                    parameters[8] = new NameValue("@DESTINY", companyTransport.Destiny);
                    parameters[9] = new NameValue("@RELEASE_AMT", companyTransport.ReleaseAmount);
                    if (companyTransport.FreightAmount == null)
                    {
                        parameters[10] = new NameValue("@FREIGHT_AMT", DBNull.Value, DbType.Decimal);
                    }
                    else
                    {
                        parameters[10] = new NameValue("@FREIGHT_AMT", companyTransport.FreightAmount);

                    }
                    if (companyTransport.LimitMaxReleaseAmount == null)
                    {
                        parameters[11] = new NameValue("@LIMIT_MAX_RELEASE_AMT", DBNull.Value, DbType.Decimal);
                    }
                    else
                    {
                        parameters[11] = new NameValue("@LIMIT_MAX_RELEASE_AMT", companyTransport.LimitMaxReleaseAmount);
                    }
                    if (companyTransport.ReleaseDate > DateTime.MinValue)
                    {
                        parameters[12] = new NameValue("@RELEASE_DATE", companyTransport.ReleaseDate);
                    }
                    else
                    {
                        parameters[12] = new NameValue("@RELEASE_DATE", DBNull.Value, DbType.DateTime);
                    }

                    parameters[13] = new NameValue("@COUNTRY_FROM_CD", DBNull.Value, DbType.Int16);
                    parameters[14] = new NameValue("@STATE_FROM_CD", DBNull.Value, DbType.Int16);
                    parameters[15] = new NameValue("@CITY_FROM_CD", DBNull.Value, DbType.Int16);
                    parameters[16] = new NameValue("@COUNTRY_TO_CD", DBNull.Value, DbType.Int16);
                    parameters[17] = new NameValue("@STATE_TO_CD", DBNull.Value, DbType.Int16);
                    parameters[18] = new NameValue("@CITY_TO_CD", DBNull.Value, DbType.Int16);
                    parameters[19] = new NameValue("@TRANSPORT_VIA_TYPE_CD", DBNull.Value, DbType.Int16);
                }
                else
                {
                    parameters[5] = new NameValue("@DECLARATION_PERIOD_CD", DBNull.Value, DbType.Double);
                    parameters[6] = new NameValue("@ADJUST_PERIOD_CD", DBNull.Value, DbType.Double);
                    parameters[7] = new NameValue("@SOURCE", companyTransport.Source, DbType.String);
                    parameters[8] = new NameValue("@DESTINY", companyTransport.Destiny, DbType.String);
                    parameters[9] = new NameValue("@RELEASE_AMT", DBNull.Value, DbType.Decimal);
                    parameters[10] = new NameValue("@FREIGHT_AMT", DBNull.Value, DbType.Decimal);
                    parameters[11] = new NameValue("@LIMIT_MAX_RELEASE_AMT", DBNull.Value, DbType.Decimal);
                    parameters[12] = new NameValue("@RELEASE_DATE", DBNull.Value, DbType.DateTime);
                    parameters[13] = new NameValue("@COUNTRY_FROM_CD", companyTransport.CityFrom.State.Country.Id);
                    parameters[14] = new NameValue("@STATE_FROM_CD", companyTransport.CityFrom.State.Id);
                    parameters[15] = new NameValue("@CITY_FROM_CD", companyTransport.CityFrom.Id);
                    parameters[16] = new NameValue("@COUNTRY_TO_CD", companyTransport.CityTo.State.Country.Id);
                    parameters[17] = new NameValue("@STATE_TO_CD", companyTransport.CityTo.State.Id);
                    parameters[18] = new NameValue("@CITY_TO_CD", companyTransport.CityTo.Id);
                    parameters[19] = new NameValue("@TRANSPORT_VIA_TYPE_CD", companyTransport.ViaType.Id);
                }

                //parameters[20] = new NameValue("@IS_RETENTION", DBNull.Value);
                parameters[20] = new NameValue("@IS_RETENTION", companyTransport.Risk.IsRetention);
                parameters[21] = new NameValue("@HOLDER_TYPE_CD", companyTransport.HolderType.Id);
                parameters[22] = new NameValue("@ANNOUNCEMENT_DATE", DBNull.Value, DbType.DateTime);
                parameters[23] = new NameValue("@CUSTOMS_BROKER", DBNull.Value, DbType.String);
                parameters[24] = new NameValue("@CUSTOMS_AGENT", DBNull.Value, DbType.String);
                parameters[25] = new NameValue("@MINIMUM_PREMIUM", companyTransport.MinimumPremium);
                parameters[26] = new NameValue("@INSURED_ID", companyTransport.Risk.MainInsured.IndividualId);
                parameters[27] = new NameValue("@COVERED_RISK_TYPE_CD", Convert.ToInt16(companyTransport.Risk.CoveredRiskType));
                parameters[28] = new NameValue("@RISK_STATUS_CD", Convert.ToInt16(companyTransport.Risk.Status));
                parameters[29] = new NameValue("@COMM_RISK_CLASS_CD", companyTransport.Risk.RiskActivity.Id);
                parameters[30] = new NameValue("@RISK_COMMERCIAL_TYPE_CD", DBNull.Value, DbType.Int16);

                if (companyTransport.Risk.Text == null)
                {
                    parameters[31] = new NameValue("@CONDITION_TEXT", DBNull.Value, DbType.String);
                }
                else
                {
                    parameters[31] = new NameValue("@CONDITION_TEXT", companyTransport.Risk.Text.TextBody, DbType.String);
                }

                parameters[32] = new NameValue("@RATING_ZONE_CD", DBNull.Value, DbType.Int16);
                parameters[33] = new NameValue("@COVER_GROUP_ID", companyTransport.Risk.GroupCoverage.Id);
                parameters[34] = new NameValue("@IS_FACULTATIVE", companyTransport.Risk.IsFacultative);
                parameters[35] = new NameValue("@LIMITS_RC_CD", 0);
                parameters[36] = new NameValue("@LIMIT_RC_SUM", 0);
                parameters[37] = new NameValue("@SINISTER_PCT", DBNull.Value, DbType.Decimal);
                parameters[38] = new NameValue("@SECONDARY_INSURED_ID", DBNull.Value, DbType.Int32);

                DataTable dtTransportTypes = new DataTable("PARAM_TRANSPORT_TYPE");
                dtTransportTypes.Columns.Add("TRANSPORT_MEAN_CD", typeof(int));

                foreach (TransportType type in companyTransport.Types)
                {
                    DataRow dataRow = dtTransportTypes.NewRow();
                    dataRow["TRANSPORT_MEAN_CD"] = type.Id;

                    dtTransportTypes.Rows.Add(dataRow);
                }

                parameters[39] = new NameValue("@INSERT_TRANSPORT_TYPES", dtTransportTypes);

                DataTable dtBeneficiaries = new DataTable("PARAM_TEMP_RISK_BENEFICIARY");
                dtBeneficiaries.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
                dtBeneficiaries.Columns.Add("BENEFICIARY_ID", typeof(int));
                dtBeneficiaries.Columns.Add("BENEFICIARY_TYPE_CD", typeof(int));
                dtBeneficiaries.Columns.Add("BENEFICT_PCT", typeof(decimal));
                dtBeneficiaries.Columns.Add("NAME_NUM", typeof(int));

                foreach (CompanyBeneficiary item in companyTransport.Risk.Beneficiaries)
                {
                    DataRow dataRow = dtBeneficiaries.NewRow();
                    dataRow["CUSTOMER_TYPE_CD"] = Convert.ToDouble(item.CustomerType);
                    dataRow["BENEFICIARY_ID"] = item.IndividualId;
                    dataRow["BENEFICIARY_TYPE_CD"] = item.BeneficiaryType.Id;
                    dataRow["BENEFICT_PCT"] = item.Participation;
                    //dataRow["NAME_NUM"] = item.CompanyName.NameNum;

                    dtBeneficiaries.Rows.Add(dataRow);
                }

                parameters[40] = new NameValue("@INSERT_TEMP_RISK_BENEFICIARY", dtBeneficiaries);

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
                dtDeductibles.Columns.Add("DEDUCT_PREMIUM_AMT", typeof(int));
                dtDeductibles.Columns.Add("DEDUCT_VALUE", typeof(int));
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

                companyTransport.Risk.Coverages = companyTransport.Risk.Coverages.OrderBy(x => x.Id).ToList();

                foreach (CompanyCoverage item in companyTransport.Risk.Coverages)
                {
                    DataRow dataRow = dtCoverages.NewRow();
                    dataRow["RISK_COVER_ID"] = item.Id;
                    dataRow["COVERAGE_ID"] = item.Id;
                    dataRow["IS_DECLARATIVE"] = item.IsDeclarative;
                    dataRow["IS_MIN_PREMIUM_DEPOSIT"] = item.IsMinPremiumDeposit;
                    dataRow["FIRST_RISK_TYPE_CD"] = Convert.ToDouble(FirstRiskType.None);
                    dataRow["CALCULATION_TYPE_CD"] = Convert.ToInt16(item.CalculationType.Value);
                    dataRow["DECLARED_AMT"] = item.DeclaredAmount;
                    dataRow["PREMIUM_AMT"] = Math.Round(Convert.ToDecimal(item.PremiumAmount), 2);
                    dataRow["LIMIT_AMT"] = item.LimitAmount;
                    dataRow["SUBLIMIT_AMT"] = item.SubLimitAmount;
                    dataRow["LIMIT_IN_EXCESS"] = item.ExcessLimit;
                    dataRow["LIMIT_OCCURRENCE_AMT"] = item.LimitOccurrenceAmount;
                    dataRow["LIMIT_CLAIMANT_AMT"] = item.LimitClaimantAmount;
                    dataRow["ACC_PREMIUM_AMT"] = item.AccumulatedPremiumAmount;
                    dataRow["ACC_LIMIT_AMT"] = item.AccumulatedLimitAmount;
                    dataRow["ACC_SUBLIMIT_AMT"] = item.AccumulatedSubLimitAmount;
                    dataRow["CURRENT_FROM"] = item.CurrentFrom;
                    dataRow["RATE_TYPE_CD"] = Convert.ToDouble(item.RateType);

                    if (item.Rate.HasValue)
                    {
                        dataRow["RATE"] = item.Rate.Value;
                    }

                    dataRow["CURRENT_TO"] = item.CurrentTo;
                    dataRow["COVER_NUM"] = item.Number;
                    if (item.CoverStatus.HasValue)
                    {
                        dataRow["COVER_STATUS_CD"] = Convert.ToInt16(item.CoverStatus.Value);
                    }

                    if (item.CoverageOriginalStatus.HasValue)
                    {
                        dataRow["COVER_ORIGINAL_STATUS_CD"] = Convert.ToInt16(item.CoverageOriginalStatus.Value);
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

                    if (item.Deductible != null)
                    {
                        DataRow dataRowDeductible = dtDeductibles.NewRow();
                        dataRowDeductible["COVERAGE_ID"] = item.Id;
                        dataRowDeductible["RATE_TYPE_CD"] = Convert.ToDouble(item.Deductible.RateType);
                        dataRowDeductible["RATE"] = (object)item.Deductible.Rate ?? DBNull.Value;
                        dataRowDeductible["DEDUCT_PREMIUM_AMT"] = item.Deductible.DeductPremiumAmount;
                        dataRowDeductible["DEDUCT_VALUE"] = item.Deductible.DeductValue;

                        if (item.Deductible.DeductibleUnit != null && item.Deductible.DeductibleUnit.Id > -1)
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

                        if (item.Deductible.MinDeductibleUnit != null && item.Deductible.MinDeductibleUnit.Id > -1)
                        {
                            dataRowDeductible["MIN_DEDUCT_UNIT_CD"] = item.Deductible.MinDeductibleUnit.Id;
                        }

                        if (item.Deductible.MinDeductibleSubject != null && item.Deductible.MinDeductibleSubject.Id > -1)
                        {
                            dataRowDeductible["MIN_DEDUCT_SUBJECT_CD"] = item.Deductible.MinDeductibleSubject.Id;
                        }

                        if (item.Deductible.MaxDeductValue.HasValue)
                        {
                            dataRowDeductible["MAX_DEDUCT_VALUE"] = item.Deductible.MaxDeductValue.Value;
                        }

                        if (item.Deductible.MaxDeductibleUnit != null && item.Deductible.MaxDeductibleUnit.Id > -1)
                        {
                            dataRowDeductible["MAX_DEDUCT_UNIT_CD"] = item.Deductible.MaxDeductibleUnit.Id;
                        }

                        if (item.Deductible.MaxDeductibleSubject != null && item.Deductible.MaxDeductibleSubject.Id > -1)
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
                    dataRow["MAX_LIABILITY_AMT"] = item.MaxLiabilityAmount;

                    dtCoverages.Rows.Add(dataRow);
                }

                parameters[41] = new NameValue("@INSERT_TEMP_RISK_COVERAGE", dtCoverages);
                parameters[42] = new NameValue("@INSERT_TEMP_RISK_COVER_DEDUCT", dtDeductibles);

                DataTable dtClauses = new DataTable("PARAM_TEMP_CLAUSE");
                dtClauses.Columns.Add("CLAUSE_ID", typeof(int));
                dtClauses.Columns.Add("ENDORSEMENT_ID", typeof(int));
                dtClauses.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
                dtClauses.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

                if (companyTransport.Risk.Clauses != null)
                {
                    foreach (CompanyClause item in companyTransport.Risk.Clauses)
                    {
                        DataRow dataRow = dtClauses.NewRow();
                        dataRow["CLAUSE_ID"] = item.Id;
                        dataRow["CLAUSE_STATUS_CD"] = Convert.ToDouble(ClauseStatuses.Original);
                        dtClauses.Rows.Add(dataRow);
                    }
                }

                parameters[43] = new NameValue("@INSERT_TEMP_CLAUSE", dtClauses);
                DataTable dtInsuredObjects = new DataTable("PARAM_TEMP_RISK_INSURED_OBJECT");
                dtInsuredObjects.Columns.Add("INSURED_OBJECT_ID", typeof(int));
                dtInsuredObjects.Columns.Add("INSURED_VALUE", typeof(decimal));
                dtInsuredObjects.Columns.Add("INSURED_PCT", typeof(decimal));
                dtInsuredObjects.Columns.Add("INSURED_RATE", typeof(decimal));

                foreach (CompanyCoverage item in companyTransport.Risk.Coverages)
                {
                    if (dtInsuredObjects.AsEnumerable().All(x => x.Field<int>("INSURED_OBJECT_ID") != item.InsuredObject.Id))
                    {
                        DataRow dataRow = dtInsuredObjects.NewRow();
                        dataRow["INSURED_OBJECT_ID"] = item.InsuredObject.Id;
                        dataRow["INSURED_VALUE"] = item.InsuredObject.Amount;

                        if (companyTransport.Risk.MainInsured != null && companyTransport.Risk.Coverages != null && companyTransport.Risk.Coverages.Any())
                        {
                            CompanyCoverage companyCoverage = companyTransport.Risk.Coverages.Find(x => x.Id == item.Id);
                            if (companyCoverage != null)
                            {
                                CompanyInsuredObject companyInsuredObject = companyCoverage.InsuredObject;
                                dataRow["INSURED_PCT"] = Convert.ToDecimal(companyInsuredObject.DepositPremiunPercent);
                                dataRow["INSURED_RATE"] = Convert.ToDecimal(companyInsuredObject.Rate);
                            }
                        }
                        dtInsuredObjects.Rows.Add(dataRow);
                    }
                }

                parameters[44] = new NameValue("@INSERT_TEMP_RISK_INSURED_OBJECT", dtInsuredObjects);

                parameters[45] = new NameValue("@RISK_NUM", companyTransport.Risk.Number);
                parameters[46] = new NameValue("OPERATION", COMUT.JsonHelper.SerializeObjectToJson(companyTransport));
                parameters[47] = new NameValue("@RISK_INSP_TYPE_CD", 1);
                parameters[48] = new NameValue("@INSPECTION_ID", DBNull.Value, DbType.Int32);
                DataTable result = new DataTable();

                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataTable("ISS.RECORD_RISK_TRANSPORT", parameters);
                }

                if (!Convert.ToBoolean(result.Rows[0][0]))
                {
                    throw new BusinessException(result.Rows[0][1].ToString());
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<CompanyTransport> GetRisks(int policyId, List<ISSEN.Risk> risks, RiskTransportsviewR1 viewR1)
        {
            if (risks == null || risks.Count < 1 || viewR1.CompanyTransports == null || viewR1.CompanyTransports.Count < 1)
            {
                throw new ArgumentException(Errors.ErrorRiskEmpty);
            }
            try
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = viewR1.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.RiskTransport> risktransports = viewR1.RiskTransports.Cast<ISSEN.RiskTransport>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = viewR1.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<ISSEN.RiskPayer> RiskPayers = viewR1.RiskPayers.Cast<ISSEN.RiskPayer>().ToList();
                List<ISSEN.RiskClause> riskClauses = viewR1.RiskClause.Cast<ISSEN.RiskClause>().ToList();
                List<ISSEN.RiskTransportMean> riskTransportMean = viewR1.RiskTransportMeans.Cast<ISSEN.RiskTransportMean>().ToList();
                List<ISSEN.TransportMean> transportMeans = viewR1.TransportMeans.Cast<ISSEN.TransportMean>().ToList();
                List<COMMEN.TransportCargoType> transportsCargoTypes = viewR1.TransportsCargoTypes.Cast<COMMEN.TransportCargoType>().ToList();
                List<ISSEN.TransportViaType> transportViaTypes = viewR1.TransportViaTypes.Cast<ISSEN.TransportViaType>().ToList();
                List<ISSEN.TransportPackagingType> transportPackagingTypes = viewR1.TransportPackagingTypes.Cast<ISSEN.TransportPackagingType>().ToList();
                List<ISSEN.RiskCoverage> RiskCoverages = viewR1.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                List<ISSEN.Policy> policies = viewR1.Policies.Cast<ISSEN.Policy>().ToList();

                List<CompanyTransport> transports = new List<CompanyTransport>();
                foreach (ISSEN.Risk item in risks)
                {
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.LoadDynamicProperties(item);
                    }
                    CompanyTransport transport = new CompanyTransport();
                    ISSEN.RiskTransport riskTransport = risktransports.First(x => x.RiskId == item.RiskId);
                    int[] valores = new int[3];
                    valores[0] = policies[0].ProductId.Value;//Producto
                    valores[1] = policies[0].PrefixCode;// Prefix
                    valores[2] = policies[0].BranchCode;//branch
                    transport = ModelAssembler.CreateTransport(item,
                    riskTransport, endorsementRisks.First(x => x.RiskId == item.RiskId), valores);
                    //-----------------------------
                    transport.CargoType = new CargoType
                    {
                        Id = riskTransport.TransportCargoTypeCode,
                    };
                    List<CargoType> cargoTypes = new List<CargoType>();
                    cargoTypes = DelegateService.transportBusinessService.GetCargoTypes();
                    CargoType cargo = cargoTypes.Where(x => x.Id == transport.CargoType.Id).FirstOrDefault();
                    transport.CargoType.Description = cargo.Description;
                    transport.CargoType.SmallDescription = cargo.SmallDescription;

                    transport.CityFrom = new City
                    {
                        Id = riskTransport.CityFromCode != null ? (int)riskTransport.CityFromCode : 0,
                        State = new State
                        {
                            Id = riskTransport.StateFromCode != null ? (int)riskTransport.StateFromCode : 0,
                            Country = new Country
                            {
                                Id = riskTransport.CountryFromCode != null ? (int)riskTransport.CountryFromCode : 0
                            }
                        }
                    };

                    transport.CityTo = new City
                    {
                        Id = riskTransport.CityToCode != null ? (int)riskTransport.CityToCode : 0,
                        State = new State
                        {
                            Id = riskTransport.StateToCode != null ? (int)riskTransport.StateToCode : 0,
                            Country = new Country
                            {
                                Id = riskTransport.CountryToCode != null ? (int)riskTransport.CountryToCode : 0
                            }
                        }
                    };
                    transport.Destiny = riskTransport.Destiny;
                    transport.Source = riskTransport.Source;

                    transport.LimitMaxReleaseAmount = riskTransport.LimitMaxReleaseAmount != null ? riskTransport.LimitMaxReleaseAmount : 0;
                    transport.FreightAmount = riskTransport.FreightAmount != null ? riskTransport.FreightAmount : 0;
                    transport.ReleaseAmount = riskTransport.ReleaseAmount != null ? (decimal)riskTransport.ReleaseAmount : 0;
                    transport.ViaType = new ViaType
                    {
                        Id = riskTransport.TransportViaTypeCode != null ? (int)riskTransport.TransportViaTypeCode : 0
                    };
                    transport.AdjustPeriod = new AdjustPeriod
                    {
                        Id = ValidateAdjustPeriodR1(riskTransport.AdjustPeriodCode != null ? (int)riskTransport.AdjustPeriodCode : 0)
                    };
                    transport.MinimumPremium = riskTransport.MinimumPremium != null ? (decimal)riskTransport.MinimumPremium : 0;
                    transport.DeclarationPeriod = new DeclarationPeriod
                    {
                        Id = riskTransport.DeclarationPeriodCode != null ? (int)riskTransport.DeclarationPeriodCode : 0,
                    };
                    List<CompanyRiskCommercialClass> companyRiskCommercialClasses = new List<CompanyRiskCommercialClass>();
                    companyRiskCommercialClasses = GetRiskCommercialClasses(transport.Risk.RiskActivity.Id.ToString());
                    if (companyRiskCommercialClasses.Count == 1)
                    {
                        if (transport.Risk.RiskActivity.Id == companyRiskCommercialClasses[0].Id)
                        {
                            transport.Risk.RiskActivity.Description = companyRiskCommercialClasses[0].Description;
                            transport.Risk.RiskActivity.SmallDescription = companyRiskCommercialClasses[0].SmallDescription;
                        }
                    }
                    //-----------------------------
                    transport.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(transport.Risk.MainInsured.IndividualId.ToString(), Core.Services.UtilitiesServices.Enums.InsuredSearchType.IndividualId, Core.Services.UtilitiesServices.Enums.CustomerType.Individual);// Mapper.Map<CompanyInsured, CompanyIssuanceInsured>(companyInsured);

                    transport.Risk.MainInsured.Name = transport.Risk.MainInsured.Name + " " + transport.Risk.MainInsured.Surname + " " + transport.Risk.MainInsured.SecondSurname;
                    transport.Risk.Description = transportMeans.First(x => x.TransportMeanCode == riskTransportMean.First(z => z.RiskId == item.RiskId).TransportMeanCode).Description + "-" + transportsCargoTypes.First(x => x.TransportCargoTypeCode == risktransports.First(r => r.RiskId == item.RiskId).TransportCargoTypeCode).Description;

                    if (transport.Risk.RiskActivity.Description == null)
                    {
                        string RiskActivityId = transport.Risk.RiskActivity.Id.ToString();
                        List<CompanyRiskCommercialClass> riskCommercialClasses = GetRiskCommercialClasses(RiskActivityId);
                        transport.Risk.RiskActivity.Description = riskCommercialClasses[0].Description;
                    };
                    List<TransportType> TypesTransport = new List<TransportType>();
                    if (riskTransportMean != null && riskTransportMean.Count > 0)
                    {
                        TP.Parallel.ForEach(riskTransportMean.Where(x => x.RiskId == item.RiskId).ToList(), TransportMean =>
                         {
                             TypesTransport.Add(new TransportType
                             {
                                 Id = TransportMean.TransportMeanCode
                             });
                         });
                        transport.Types = TypesTransport.ToList();
                    }
                    ConcurrentBag<CompanyClause> clauses = new ConcurrentBag<CompanyClause>();
                    transport.HolderType = new CompanyHolderType();
                    transport.HolderType.Id = (int)risktransports.First(x => x.RiskId == item.RiskId).HolderTypeCode;
                    //clausulas
                    if (riskClauses != null && riskClauses.Count > 0)
                    {
                        TP.Parallel.ForEach(riskClauses.Where(x => x.RiskId == item.RiskId).ToList(), riskClause =>
                        {
                            clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                        });
                        transport.Risk.Clauses = clauses.ToList();
                    }
                    CompanyName companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(transport.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                    transport.Risk.MainInsured.CompanyName = new IssuanceCompanyName
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
                        Email = companyName.Email == null ? null : new IssuanceEmail
                        {
                            Id = companyName.Email.Id,
                            Description = companyName.Email.Description
                        }
                    };
                    if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
                    {
                        transport.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                        Object objlock = new object();
                        var imapper = ModelAssembler.CreateMapCompanyBeneficiary();
                        TP.Parallel.ForEach(riskBeneficiaries.Where(x => x.RiskId == item.RiskId), riskBeneficiary =>
                        {
                            CompanyBeneficiary CiaBeneficiary = new CompanyBeneficiary();
                            var beneficiaryRisk = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), (Core.Services.UtilitiesServices.Enums.InsuredSearchType)InsuredSearchType.IndividualId).FirstOrDefault();
                            if (beneficiaryRisk != null)
                            {
                                CiaBeneficiary = imapper.Map<Beneficiary, CompanyBeneficiary>(beneficiaryRisk);
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
                                    Address = companyName.Address == null ? null : new IssuanceAddress
                                    {
                                        Id = companyName.Address.Id,
                                        Description = companyName.Address.Description,
                                        City = companyName.Address.City
                                    },
                                    Phone = companyName.Phone == null ? null : new IssuancePhone
                                    {
                                        Id = companyName.Phone.Id,
                                        Description = companyName.Phone.Description
                                    },
                                    Email = companyName.Email == null ? null : new IssuanceEmail
                                    {
                                        Id = companyName.Email != null ? companyName.Email.Id : 0,
                                        Description = companyName.Email != null ? companyName.Email.Description : ""
                                    }
                                };
                                lock (objlock)
                                {
                                    transport.Risk.Beneficiaries.Add(CiaBeneficiary);
                                }
                            }
                        });
                    }
                    if (transportViaTypes.Count == 1)
                    {
                        transport.ViaType = new ViaType
                        {
                            Id = transportViaTypes[0].TransportViaTypeCode,
                            Description = transportViaTypes[0].Description,
                            SmallDescription = transportViaTypes[0].SmallDescription,
                            IsEnabled = transportViaTypes[0].Enabled
                        };
                    }
                    if (transportPackagingTypes.Count == 1)
                    {
                        transport.PackagingType = new PackagingType
                        {
                            Id = transportPackagingTypes[0].TransportPackagingTypeCode,
                            Description = transportPackagingTypes[0].Description,
                            SmallDescription = transportPackagingTypes[0].SmallDescription,
                            IsEnabled = transportPackagingTypes[0].Enabled
                        };
                    }
                    if (transportMeans.Count > 0)
                    {
                        List<TransportType> types = new List<TransportType>();
                        types = DelegateService.transportBusinessService.GetTransportTypes();
                        transport.Types = new List<TransportType>();
                        foreach (var type in types)
                        {
                            foreach (var mean in transportMeans)
                            {
                                if (type.Id == mean.TransportMeanCode)
                                {
                                    transport.Types.Add(type);
                                    break;
                                }
                            }
                        }
                    }
                    transport.Risk.Description = transport.CargoType.Description + " - " + transportMeans.FirstOrDefault().Description;

                    //coberturas
                    List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();
                   
                    companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId, item.RiskId);
                    foreach (var coverage in RiskCoverages)
                    {
                        if (companyCoverages.FirstOrDefault(x => x.Id == coverage.CoverageId) != null)
                        {
                            companyCoverages.FirstOrDefault(x => x.Id == coverage.CoverageId).DepositPremiumPercent = coverage.PremiumAmtDepositPercent.GetValueOrDefault();
                            companyCoverages.FirstOrDefault(x => x.Id == coverage.CoverageId).Rate = (decimal)coverage.Rate.GetValueOrDefault();
                        }
                    };
                    transport.Risk.Coverages = companyCoverages;
                    transport.Risk.OriginalStatus = transport.Risk.Status;
                    transport.Risk.Status = RiskStatusType.NotModified;
                    transport.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
                    transport.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.NotModified);
                    if (transport.InsuredObjects == null)
                    {
                        var insuredObjects = DelegateService.underwritingService.GetInsuredObjectsByProductIdGroupCoverageId(valores[0], transport.Risk.GroupCoverage.Id, valores[1]);
                        transport.InsuredObjects = new List<CompanyInsuredObject>();
                        foreach (var coverage in transport.Risk.Coverages)
                        {
                            if (coverage.InsuredObject != null)
                            {
                                CompanyInsuredObject companyInsuredObject = transport.InsuredObjects.Where(x => x.Id == coverage.InsuredObject.Id)?.FirstOrDefault();
                                if (companyInsuredObject == null)
                                {
                                    var insuredObject = insuredObjects.Where(x => x.Id == coverage.InsuredObject.Id)?.FirstOrDefault();
                                    if (insuredObject != null)
                                    {
                                        companyInsuredObject = new CompanyInsuredObject
                                        {
                                            Id = insuredObject.Id,
                                            Description = insuredObject.Description,
                                            IsMandatory = insuredObject.IsMandatory,
                                            IsSelected = insuredObject.IsSelected,
                                        };
                                        transport.InsuredObjects.Add(companyInsuredObject);
                                    }
                                }
                            }
                        }
                        transport.InsuredObjects.ForEach(x =>
                        {
                            x.Amount = transport.Risk.Coverages.Where(z => z.InsuredObject.Id == x.Id).Select(y => y.DeclaredAmount).Sum();
                            x.DepositPremiunPercent = transport.Risk.Coverages.Where(z => z.InsuredObject.Id == x.Id && z.IsPrimary && z.IsSelected).FirstOrDefault().DepositPremiumPercent;
                            x.Rate = transport.Risk.Coverages.Where(z => z.InsuredObject.Id == x.Id && z.IsPrimary && z.IsSelected).FirstOrDefault().Rate;
                        });
                    }
                    transports.Add(transport);
                }
                return transports;
            }

            catch (Exception ex)
            {

                throw new BusinessException(ex.ToString());
            }
        }
        public int ValidateAdjustPeriodR1(int AdjustPeriodR1)
        {
            switch (AdjustPeriodR1)
            {
                case 1:
                    AdjustPeriodR1 = (int)EnumsAdjustmentPeriod.Monthly;//mensual
                    break;
                case 2:
                    AdjustPeriodR1 = (int)EnumsAdjustmentPeriod.BiMonthly;//Bimestral
                    break;
                case 3:
                    AdjustPeriodR1 = (int)EnumsAdjustmentPeriod.Quarterly;//Trimestral
                    break;
                case 4:
                    AdjustPeriodR1 = (int)EnumsAdjustmentPeriod.FourMonths;//Cuatrimestral
                    break;
                case 5:
                    AdjustPeriodR1 = (int)EnumsAdjustmentPeriod.Biannual;//Semestral
                    break;
                case 6:
                    AdjustPeriodR1 = (int)EnumsAdjustmentPeriod.Annual;//Anual
                    break;
                default:
                    AdjustPeriodR1 = 0;
                    break;
            }
            return AdjustPeriodR1;
        }
        public List<CompanyTransport> GetCompanyTransportsByPolicyIdEndorsementId(int policyId, int endorsementId)
        {

            List<CompanyTransport> companyTransports = new List<CompanyTransport>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name);
            filter.IsNotNull();
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
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
            RiskTransportsView view = new RiskTransportsView();
            DAFENG.ViewBuilder builder = new DAFENG.ViewBuilder("RiskTransportsView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.EndorsementOperations.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.RiskCoverage> RiskCoverage = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                List<ISSEN.TransportViaType> transportViaTypes = view.TransportViaTypes.Cast<ISSEN.TransportViaType>().ToList();

                foreach (ISSEN.EndorsementOperation entityEndorsementOperation in view.EndorsementOperations)
                {
                    CompanyTransport companyTransport = new CompanyTransport();
                    companyTransport = COMUT.JsonHelper.DeserializeJson<CompanyTransport>(entityEndorsementOperation.Operation);
                    if (entityEndorsementOperation.EndorsementId == endorsementId)
                    {
                        companyTransport.Risk.Id = entityEndorsementRisks.First(x => x.EndorsementId == entityEndorsementOperation.EndorsementId
                       ).RiskId;
                        companyTransport.Risk.RiskId = entityEndorsementRisks.First(x => x.EndorsementId == entityEndorsementOperation.EndorsementId).RiskId;

                        companyTransport.Risk.OriginalStatus = companyTransport.Risk.Status;
                        companyTransport.Risk.Status = RiskStatusType.NotModified;

                        companyTransport.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
                        companyTransport.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.NotModified);
                        companyTransport.Risk.Policy.Endorsement.Id = entityEndorsementRisks.First(x => x.EndorsementId == entityEndorsementOperation.EndorsementId).EndorsementId;

                        foreach (ISSEN.RiskCoverage riskCoverages in view.RiskCoverages)
                        {
                            companyTransport.Risk.Coverages.ForEach(x => x.RiskCoverageId = x.RiskCoverageId);
                        }
                        companyTransports.Add(companyTransport);
                    }
                }
            }
            else
            {
                ObjectCriteriaBuilder filterR1 = new ObjectCriteriaBuilder();
                filterR1.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                filterR1.Equal();
                filterR1.Constant(policyId);
                filterR1.And();
                filterR1.And();
                filterR1.Not();
                filterR1.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                filterR1.In();
                filterR1.ListValue();
                filterR1.Constant((int)RiskStatusType.Excluded);
                filterR1.Constant((int)RiskStatusType.Cancelled);
                filterR1.EndList();

                RiskTransportsviewR1 viewr1 = new RiskTransportsviewR1();
                DAFENG.ViewBuilder builderR1 = new DAFENG.ViewBuilder("RiskTransportsviewR1");
                builderR1.Filter = filterR1.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builderR1, viewr1);

                List<ISSEN.Risk> risks = viewr1.CompanyTransports.Cast<ISSEN.Risk>().ToList();
                companyTransports.AddRange(GetRisks(policyId, risks, viewr1));
            }


            return companyTransports;
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

            RiskTransportsviewR1 viewr = new RiskTransportsviewR1();
            DAFENG.ViewBuilder builder = new DAFENG.ViewBuilder("RiskTransportsviewR1");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, viewr);


            List<ISSEN.Endorsement> entityEndorsements = viewr.Endorsements.Cast<ISSEN.Endorsement>().ToList();
            List<ISSEN.EndorsementRisk> entityEndorsementRisks = viewr.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

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

                    foreach (CompanyEndorsement item in companyEndorsements)
                    {
                        item.RiskId = entityEndorsementRisks.First(x => x.EndorsementId == item.Id).RiskId;
                    }

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
                CompanyTransport companyTransport = COMUT.JsonHelper.DeserializeJson<CompanyTransport>(pendingOperation.Operation);
                companyTransport.Risk.Id = pendingOperation.Id;
                companyTransport.Risk.IsPersisted = true;
                coverages = companyTransport.Risk.Coverages;
            }
            //            CompanyTransport companyTransport = GetCompanyTransportTemporalByRiskId(temporalId);

            return coverages;

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

            CompanyTransportsRiskCoveragesView view = new CompanyTransportsRiskCoveragesView();
            DAFENG.ViewBuilder builder = new DAFENG.ViewBuilder("RiskCoveragesView");
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

        public List<CompanyRiskCommercialClass> GetRiskCommercialClasses(string description)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 id = 0;
            Int64.TryParse(description, out id);

            if (id > 0)
            {
                filter.Property(PAREN.RiskCommercialClass.Properties.RiskCommercialClassCode, typeof(PAREN.RiskCommercialClass).Name);
                filter.Equal();
                filter.Constant(id);
            }
            else
            {
                filter.Property(PAREN.RiskCommercialClass.Properties.Description, typeof(PAREN.RiskCommercialClass).Name);
                filter.Like();
                filter.Constant(description + "%");
            }

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PAREN.RiskCommercialClass), filter.GetPredicate());

            return ModelAssembler.CreateCompanyRiskCommercialClasses(businessCollection);
        }

        public List<CompanyHolderType> GetHolderTypes()
        {
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(COMMEN.HolderType));
            return ModelAssembler.CreateCompanyHolderTypes(businessCollection);
        }

       
        public List<CompanyTransport> GetCompanyTransportByEndorsementIdModuleType(int endorsementId, Core.Services.UtilitiesServices.Enums.ModuleType moduleType)
        {
            switch (moduleType)
            {
                case Core.Services.UtilitiesServices.Enums.ModuleType.Emission:
                    return new List<CompanyTransport>();
                case Core.Services.UtilitiesServices.Enums.ModuleType.Claim:

                    List<CompanyTransport> companyTransports = new List<CompanyTransport>();

                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Equal();
                    filter.Constant(endorsementId);

                    ClaimRiskTransportView claimRiskTransportView = new ClaimRiskTransportView();
                    DAFENG.ViewBuilder builder = new DAFENG.ViewBuilder("ClaimRiskTransportView");
                    builder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, claimRiskTransportView);

                    if (claimRiskTransportView.RiskTransports.Count > 0)
                    {
                        foreach (ISSEN.RiskTransport entityRiskTransport in claimRiskTransportView.RiskTransports)
                        {
                            CompanyTransport companyTransport = new CompanyTransport();
                            companyTransport = ModelAssembler.CreateCompanyTransportsByRiskTransport(entityRiskTransport);
                            ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskTransportView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == companyTransport.Risk.RiskId);
                            ISSEN.Policy entityPolicy = claimRiskTransportView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                            ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                            filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                            filterSumAssured.Equal();
                            filterSumAssured.Constant(endorsementId);

                            SumAssuredTransportView assuredView = new SumAssuredTransportView();
                            DAFENG.ViewBuilder builderAssured = new DAFENG.ViewBuilder("SumAssuredTransportView");
                            builderAssured.Filter = filterSumAssured.GetPredicate();
                            DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                            decimal insuredAmount = 0;

                            foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                            {
                                insuredAmount += item.LimitAmount;
                            }

                            List<COMMEN.TransportMean> transportMeans = claimRiskTransportView.TransportMeans.Cast<COMMEN.TransportMean>().ToList();
                            List<COMMEN.City> cities = claimRiskTransportView.Cities.Cast<COMMEN.City>().ToList();
                            List<COMMEN.TransportViaType> transportsViaTypes = claimRiskTransportView.TransportsViaTypes.Cast<COMMEN.TransportViaType>().ToList();
                            List<COMMEN.TransportCargoType> transportsCargoTypes = claimRiskTransportView.TransportsCargoTypes.Cast<COMMEN.TransportCargoType>().ToList();
                            List<COMMEN.TransportPackagingType> transportsPackagingTypes = claimRiskTransportView.TransportsPackagingTypes.Cast<COMMEN.TransportPackagingType>().ToList();
                            List<ISSEN.Risk> entityRisks = claimRiskTransportView.Risks.Cast<ISSEN.Risk>().ToList();

                            companyTransport.CargoType.Description = transportsCargoTypes.FirstOrDefault(x => x.TransportCargoTypeCode == companyTransport.CargoType.Id).Description;
                            companyTransport.PackagingType.Description = transportsPackagingTypes.FirstOrDefault(x => x.TransportPackagingTypeCode == companyTransport.PackagingType.Id).Description;
                            companyTransport.CityFrom.Description = cities.FirstOrDefault(x => x.CityCode == companyTransport.CityFrom.Id)?.Description;
                            companyTransport.CityTo.Description = cities.FirstOrDefault(x => x.CityCode == companyTransport.CityTo.Id)?.Description;
                            companyTransport.ViaType.Description = transportsViaTypes.FirstOrDefault(x => x.TransportViaTypeCode == companyTransport.ViaType.Id)?.Description;
                            companyTransport.ReleaseAmount = insuredAmount;
                            companyTransport.Risk.Description = companyTransport.CargoType.Description + " - " + transportMeans.FirstOrDefault().Description;
                            companyTransport.Risk.CoveredRiskType = (CoveredRiskType)entityRisks.Where(x => x.RiskId == companyTransport.Risk.RiskId).FirstOrDefault()?.CoveredRiskTypeCode;
                            companyTransport.Risk.MainInsured = new CompanyIssuanceInsured
                            {
                                IndividualId = Convert.ToInt32(entityRisks.Where(x => x.RiskId == companyTransport.Risk.RiskId).FirstOrDefault()?.InsuredId)
                            };
                            companyTransport.Risk.Policy = new CompanyPolicy
                            {
                                Id = entityPolicy.PolicyId,
                                DocumentNumber = entityPolicy.DocumentNumber,
                                Endorsement = new CompanyEndorsement
                                {
                                    Id = entityEndorsementRisk.EndorsementId
                                }
                            };

                            companyTransports.Add(companyTransport);
                        }
                    }

                    return companyTransports;
                default:
                    return new List<CompanyTransport>();
            }
        }

        public List<CompanyTransport> GetCompanyTransportsByInsuredId(int insuredId)
        {
            List<CompanyTransport> companyTransports = new List<CompanyTransport>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);

            ClaimRiskTransportView claimRiskTransportView = new ClaimRiskTransportView();
            DAFENG.ViewBuilder builder = new DAFENG.ViewBuilder("ClaimRiskTransportView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, claimRiskTransportView);

            if (claimRiskTransportView.RiskTransports.Count > 0)
            {
                foreach (ISSEN.RiskTransport entityRiskTransport in claimRiskTransportView.RiskTransports)
                {
                    CompanyTransport companyTransport = new CompanyTransport();
                    companyTransport = ModelAssembler.CreateCompanyTransportsByRiskTransport(entityRiskTransport);
                    ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskTransportView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == companyTransport.Risk.RiskId);
                    ISSEN.Policy entityPolicy = claimRiskTransportView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                    ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                    filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filterSumAssured.Equal();
                    filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                    SumAssuredTransportView assuredView = new SumAssuredTransportView();
                    DAFENG.ViewBuilder builderAssured = new DAFENG.ViewBuilder("SumAssuredTransportView");
                    builderAssured.Filter = filterSumAssured.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                    decimal insuredAmount = 0;

                    foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                    {
                        insuredAmount += item.LimitAmount;
                    }

                    List<COMMEN.TransportMean> transportMeans = claimRiskTransportView.TransportMeans.Cast<COMMEN.TransportMean>().ToList();
                    List<COMMEN.City> cities = claimRiskTransportView.Cities.Cast<COMMEN.City>().ToList();
                    List<COMMEN.TransportViaType> transportsViaTypes = claimRiskTransportView.TransportsViaTypes.Cast<COMMEN.TransportViaType>().ToList();
                    List<COMMEN.TransportCargoType> transportsCargoTypes = claimRiskTransportView.TransportsCargoTypes.Cast<COMMEN.TransportCargoType>().ToList();
                    List<COMMEN.TransportPackagingType> transportsPackagingTypes = claimRiskTransportView.TransportsPackagingTypes.Cast<COMMEN.TransportPackagingType>().ToList();
                    List<ISSEN.Risk> entityRisks = claimRiskTransportView.Risks.Cast<ISSEN.Risk>().ToList();

                    companyTransport.CargoType.Description = transportsCargoTypes.FirstOrDefault(x => x.TransportCargoTypeCode == companyTransport.CargoType.Id).Description;
                    companyTransport.PackagingType.Description = transportsPackagingTypes.FirstOrDefault(x => x.TransportPackagingTypeCode == companyTransport.PackagingType.Id).Description;
                    companyTransport.CityFrom.Description = cities.FirstOrDefault(x => x.CityCode == companyTransport.CityFrom.Id)?.Description;
                    companyTransport.CityTo.Description = cities.FirstOrDefault(x => x.CityCode == companyTransport.CityTo.Id)?.Description;
                    companyTransport.ViaType.Description = transportsViaTypes.FirstOrDefault(x => x.TransportViaTypeCode == companyTransport.ViaType.Id)?.Description;
                    companyTransport.ReleaseAmount = insuredAmount;
                    companyTransport.Risk.Description = companyTransport.CargoType.Description + " - " + transportMeans.FirstOrDefault().Description;
                    companyTransport.Risk.CoveredRiskType = (CoveredRiskType)entityRisks.Where(x => x.RiskId == companyTransport.Risk.RiskId).FirstOrDefault()?.CoveredRiskTypeCode;
                    companyTransport.Risk.MainInsured = new CompanyIssuanceInsured
                    {
                        IndividualId = Convert.ToInt32(entityRisks.Where(x => x.RiskId == companyTransport.Risk.RiskId).FirstOrDefault()?.InsuredId)
                    };
                    companyTransport.Risk.Policy = new CompanyPolicy
                    {
                        Id = entityPolicy.PolicyId,
                        DocumentNumber = entityPolicy.DocumentNumber,
                        Endorsement = new CompanyEndorsement
                        {
                            Id = entityEndorsementRisk.EndorsementId
                        }
                    };

                    companyTransports.Add(companyTransport);
                }
            }
            return companyTransports;
        }

        /// <summary>
        /// Genera la información de la vigencia para el próximo endoso de ajuste
        ///     para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Endoso de ajuste</returns>
        public CompanyEndorsement GetNextAdjustmentEndorsementByPolicyId(int policyId)
        {
            CompanyTransport companyTransport = GetCompanyTransportsByPolicyId(policyId).FirstOrDefault();
            List<CompanyEndorsement> endorsements = GetEndorsements(companyTransport.Risk.Policy);

            var currentFrom = GetCurrentFromForNextAdjustment(endorsements);
            int months = GetMothsByAdjustmentPeriod(companyTransport.AdjustPeriod.Id);

            return new CompanyEndorsement
            {
                CurrentFrom = currentFrom,
                CurrentTo = currentFrom.AddMonths(months),
                EndorsementType = Core.Application.UnderwritingServices.Enums.EndorsementType.AdjustmentEndorsement
            };
        }

        /// <summary>
        /// Obtiene la cantidad de meses que tiene un periodo de declaración
        /// </summary>
        /// <param name="declarationId">Identificador del periodo de declaración</param>
        /// <returns>Canitdad de meses</returns>
        private int GetMothsByDeclarationPeriod(int declarationId)
        {
            EnumsDeclarationPeriod declarationPeriod = (EnumsDeclarationPeriod)declarationId;

            switch (declarationPeriod)
            {
                case EnumsDeclarationPeriod.Monthly:
                    return 1;
                case EnumsDeclarationPeriod.BiMonthly:
                    return 2;
                case EnumsDeclarationPeriod.Quarterly:
                    return 3;
                case EnumsDeclarationPeriod.FourMonths:
                    return 4;
                case EnumsDeclarationPeriod.Biannual:
                    return 6;
                case EnumsDeclarationPeriod.Annual:
                    return 12;
            }
            return 1;
        }

        /// <summary>
        /// Obtiene la cantidad de meses que tiene un periodo de ajuste
        /// </summary>
        /// <param name="adjustmentId">Identificador del periodo de ajuste</param>
        /// <returns>Cantidad de meses</returns>
        private int GetMothsByAdjustmentPeriod(int adjustmentId)
        {
            EnumsAdjustmentPeriod adjustmentPeriod = (EnumsAdjustmentPeriod)adjustmentId;

            switch (adjustmentPeriod)
            {
                case EnumsAdjustmentPeriod.Monthly:
                    return 1;
                case EnumsAdjustmentPeriod.BiMonthly:
                    return 2;
                case EnumsAdjustmentPeriod.Quarterly:
                    return 3;
                case EnumsAdjustmentPeriod.FourMonths:
                    return 4;
                case EnumsAdjustmentPeriod.Biannual:
                    return 6;
                case EnumsAdjustmentPeriod.Annual:
                    return 12;
            }
            return 1;
        }

        /// <summary>
        /// Determina la fecha de inicio de vigencia para el próximo endoso de ajuste
        /// </summary>
        /// <param name="endorsements">Listado de endosos</param>
        /// <returns>Fecha de inicio de vigencia</returns>
        private DateTime GetCurrentFromForNextAdjustment(List<CompanyEndorsement> endorsements)
        {
            endorsements = endorsements.OrderByDescending(x => x.Id).ToList();

            try
            {
                return endorsements.Where(x => Core.Application.UnderwritingServices.Enums.EndorsementType.AdjustmentEndorsement == x.EndorsementType.Value).First().CurrentTo.Date;
            }
            catch (Exception)
            {
            }
            return endorsements.Last().CurrentFrom.Date;
        }

        /// <summary>
        /// Determina la fecha de inicio de vigencia para el próximo endoso de declaración
        /// </summary>
        /// <param name="endorsements">Listado de endosos</param>
        /// <returns>Fecha de inicio de vigencia</returns>
        private DateTime GetCurrentFromForNextDeclaration(List<CompanyEndorsement> endorsements)
        {
            endorsements = endorsements.OrderByDescending(x => x.Id).ToList();

            foreach (var endorsement in endorsements)
            {
                if (endorsement.EndorsementType.Value == Core.Application.UnderwritingServices.Enums.EndorsementType.DeclarationEndorsement
                    || endorsement.EndorsementType.Value == Core.Application.UnderwritingServices.Enums.EndorsementType.AdjustmentEndorsement)
                {
                    return endorsement.CurrentTo.Date;
                }
            }
            return endorsements.Last().CurrentFrom.Date;

        }

        /// <summary>
        /// Consultar el listado endosos de declaración más recientes, si hay endosos de 
        ///     ajuste, retorna las declaraciones posteriores a dicho ajuste, si no, trae
        ///     las declaraciones desde la emisión
        /// </summary>
        /// <param name="endorsements"></param>
        /// <returns></returns>
        public List<CompanyEndorsement> GetLastDeclarationEndorsements(List<CompanyEndorsement> endorsements)
        {
            // Se verifica si hay endoso de ajuste anterior
            if (endorsements.Where(x => x.EndorsementType.Value == Core.Application.UnderwritingServices.Enums.EndorsementType.AdjustmentEndorsement).Count() > 0)
            {
                // Se toma el endoso de ajuste más reciente
                CompanyEndorsement adjustmentEndorsement = endorsements.OrderByDescending(x => x.Id).
                    First(x => x.EndorsementType.Value == Core.Application.UnderwritingServices.Enums.EndorsementType.AdjustmentEndorsement);

                List<CompanyEndorsement> endorsementsList = endorsements.Where(x => x.Id > adjustmentEndorsement.Id).ToList();
                endorsements = (endorsementsList.Count() > 0) ? endorsementsList : endorsements;
            }

            return endorsements.Where(x => x.EndorsementType.Value == Core.Application.UnderwritingServices.Enums.EndorsementType.DeclarationEndorsement).ToList();
        }

        /// <summary>
        /// Retorna el listado de todas las declaraciones en un listado de endosos
        /// </summary>
        /// <param name="endorsements">Listado de endosos</param>
        /// <returns>Listado de declaraciones</returns>
        public List<CompanyEndorsement> GetAllDeclarationEndorsements(List<CompanyEndorsement> endorsements)
        {
            return endorsements.Where(x => x.EndorsementType.Value == Core.Application.UnderwritingServices.Enums.EndorsementType.DeclarationEndorsement).ToList();
        }

        /// <summary>
        /// Valida si en la poliza actual se pueden realizar endosos de ajuste 
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public bool CanMakeAdjustmentEndorsement(int policyId)
        {
            try
            {
                CompanyTransport companyTransport = GetCompanyTransportsByPolicyId(policyId).FirstOrDefault();
                List<CompanyEndorsement> endorsements = GetEndorsements(companyTransport.Risk.Policy);

                if (endorsements.OrderByDescending(x => x.Id).FirstOrDefault().EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.AdjustmentEndorsement)
                    return false;

                var quantityofAdjustment = GetNumberofDeclarationsExpected(companyTransport.AdjustPeriod.Id, companyTransport.DeclarationPeriod.Id);
                List<CompanyEndorsement> declarationEndorsements = GetLastDeclarationEndorsements(endorsements);

                if (declarationEndorsements == null)
                {
                    return false;
                }
                return declarationEndorsements.Count == quantityofAdjustment;
            }
            catch (Exception)
            {
                throw new BusinessException("Error al consultar los endosos de declaración");
            }
        }

        /// <summary>
        /// Valida si en la poliza actual se pueden realizar endosos de declaracion
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public bool CanMakeDeclarationEndorsement(int policyId)
        {
            try
            {
                CompanyTransport companyTransport = GetCompanyTransportsByPolicyId(policyId).FirstOrDefault();
                List<CompanyEndorsement> endorsements = GetEndorsements(companyTransport.Risk.Policy);

                var quantityofDeclaration = GetNumberofDeclarationsExpected(companyTransport.AdjustPeriod.Id, companyTransport.DeclarationPeriod.Id);
                List<CompanyEndorsement> declarationEndorsements = GetLastDeclarationEndorsements(endorsements);

                if (declarationEndorsements == null)
                {
                    return false;
                }
                return declarationEndorsements.Count < quantityofDeclaration;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al consultar los endosos de declaración");
            }
        }

        /// <summary>
        /// obtiene el numero de declaraciones que tiene que realizarse para hacer un ajuste.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <param name="declarationId"></param>
        /// <returns></returns>
        private int GetNumberofDeclarationsExpected(int adjustmentId, int declarationId)
        {

            var monthsForAdjustment = GetMothsByAdjustmentPeriod(adjustmentId);
            var monthsForDeclaration = GetMothsByDeclarationPeriod(declarationId);

            return monthsForAdjustment / monthsForDeclaration;

        }

        /// <summary>
        /// Rertorna el listado de los endosos para una póliza
        /// </summary>
        /// <param name="companyPolicy">Póliza</param>
        /// <returns>Listado de endosos</returns>
        public List<CompanyEndorsement> GetEndorsements(CompanyPolicy companyPolicy)
        {
            return DelegateService.underwritingService.GetCoPolicyEndorsementsByPrefixIdBranchIdPolicyNumber(
                   companyPolicy.Prefix.Id,
                   companyPolicy.Branch.Id,
                   companyPolicy.DocumentNumber);
        }

        /// <summary>
        /// Determina la prima en depósito para una cobertura
        /// </summary>
        /// <param name="endorsements">Listado de endosos</param>
        /// <param name="coverageId">Identificador de la cobertura</param>
        /// <returns>Prima en depósito para una cobertura</returns>
        public decimal GetDepositPremiumByCoverageId(CompanyEndorsement companyEndorsement, int coverageId, CompanyRisk companyRisk)
        {

            CoverageBusiness coverageBusiness = new CoverageBusiness();

            foreach (var coverage in companyRisk.Coverages)
            {
                if (coverage.Id == coverageId)
                {
                    coverage.SubLimitAmount = coverage.DeclaredAmount;
                    CompanyCoverage _coverage = DelegateService.underwritingService.QuotateCompanyCoverage(coverage, companyEndorsement.PolicyId, companyRisk.Id, 2);
                    return (_coverage.PremiumAmount * (coverage.DepositPremiumPercent / 100)) * (companyEndorsement.EndorsementDays / 365);
                }
            }

            return 0;
        }

        /// <summary>
        /// Determina si un riesgo tiene prima en depósito
        /// El riesgo a validar debería ser el de la emisión de la póliza
        /// </summary>
        /// <param name="companyTransport">Riesgo a validar</param>
        /// <returns>True, si alguna de las coberturas del riesgo tiene tasa mayor a 0</returns>
        public bool HasDepositPremium(CompanyTransport companyTransport)
        {
            foreach (var coverage in companyTransport.Risk.Coverages)
            {
                if (coverage.DepositPremiumPercent > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Indica si la prima en depósito ha sido excedida
        /// </summary>
        /// <param name="endorsements">Listado de endosos para calcular si se ha excedido la prima en depósito</param>
        /// <returns>True, si ya se excedió la prima en depósito</returns>
        public bool HasBeenDepositPremiumOverflowed(List<CompanyEndorsement> endorsements)
        {
            // Se listan los endosos de ajuste
            List<CompanyEndorsement> adjustmentEndorsements = endorsements.Where(
                x => x.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.AdjustmentEndorsement).ToList();

            if (adjustmentEndorsements == null || adjustmentEndorsements.Count == 0)
                return false;

            adjustmentEndorsements = adjustmentEndorsements.OrderByDescending(x => x.Id).ToList();

            // Se recorren los endosos de ajuste
            foreach (var adjustmentEndorsement in adjustmentEndorsements)
            {
                List<CompanyRisk> corisks = DelegateService.underwritingService.GetRiskByPolicyIdEndorsmentId(adjustmentEndorsement.PolicyId,
                    adjustmentEndorsement.Id);

                foreach (var risk in corisks)
                {
                    foreach (var coverage in risk.Coverages)
                    {
                        // Si cualquier cobertura cobró prima, quiere decir que desbordó la prima en depósito
                        if (coverage.PremiumAmount > 0)
                            return true;
                    }
                }
            }
            return false;
        }

        public CompanyTransport GetCompanyTransportByRiskId(int riskId)
        {
            CompanyTransport companyTransport = new CompanyTransport();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskTransport.Properties.RiskId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(riskId);

            ClaimRiskTransportView claimRiskTransportView = new ClaimRiskTransportView();
            DAFENG.ViewBuilder builder = new DAFENG.ViewBuilder("ClaimRiskTransportView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, claimRiskTransportView);

            if (claimRiskTransportView.RiskTransports.Count > 0)
            {

                ISSEN.RiskTransport entityRiskTransport = claimRiskTransportView.RiskTransports.Cast<ISSEN.RiskTransport>().First();

                companyTransport = ModelAssembler.CreateCompanyTransportsByRiskTransport(entityRiskTransport);
                ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskTransportView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == companyTransport.Risk.RiskId);
                ISSEN.Policy entityPolicy = claimRiskTransportView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                filterSumAssured.Equal();
                filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                SumAssuredTransportView assuredView = new SumAssuredTransportView();
                DAFENG.ViewBuilder builderAssured = new DAFENG.ViewBuilder("SumAssuredTransportView");
                builderAssured.Filter = filterSumAssured.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                decimal insuredAmount = 0;

                foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                {
                    insuredAmount += item.LimitAmount;
                }

                List<COMMEN.TransportMean> transportMeans = claimRiskTransportView.TransportMeans.Cast<COMMEN.TransportMean>().ToList();
                List<COMMEN.City> cities = claimRiskTransportView.Cities.Cast<COMMEN.City>().ToList();
                List<COMMEN.TransportViaType> transportsViaTypes = claimRiskTransportView.TransportsViaTypes.Cast<COMMEN.TransportViaType>().ToList();
                List<COMMEN.TransportCargoType> transportsCargoTypes = claimRiskTransportView.TransportsCargoTypes.Cast<COMMEN.TransportCargoType>().ToList();
                List<COMMEN.TransportPackagingType> transportsPackagingTypes = claimRiskTransportView.TransportsPackagingTypes.Cast<COMMEN.TransportPackagingType>().ToList();
                List<ISSEN.Risk> entityRisks = claimRiskTransportView.Risks.Cast<ISSEN.Risk>().ToList();

                companyTransport.CargoType.Description = transportsCargoTypes.FirstOrDefault(x => x.TransportCargoTypeCode == companyTransport.CargoType.Id).Description;
                companyTransport.PackagingType.Description = transportsPackagingTypes.FirstOrDefault(x => x.TransportPackagingTypeCode == companyTransport.PackagingType.Id).Description;
                companyTransport.CityFrom.Description = cities.FirstOrDefault(x => x.CityCode == companyTransport.CityFrom.Id)?.Description;
                companyTransport.CityTo.Description = cities.FirstOrDefault(x => x.CityCode == companyTransport.CityTo.Id)?.Description;
                companyTransport.ViaType.Description = transportsViaTypes.FirstOrDefault(x => x.TransportViaTypeCode == companyTransport.ViaType.Id)?.Description;
                companyTransport.ReleaseAmount = insuredAmount;
                companyTransport.Risk.Description = companyTransport.CargoType.Description + " - " + transportMeans.FirstOrDefault().Description;
                companyTransport.Risk.CoveredRiskType = (CoveredRiskType)entityRisks.Where(x => x.RiskId == companyTransport.Risk.RiskId).FirstOrDefault()?.CoveredRiskTypeCode;
                companyTransport.Risk.MainInsured = new CompanyIssuanceInsured
                {
                    IndividualId = Convert.ToInt32(entityRisks.Where(x => x.RiskId == companyTransport.Risk.RiskId).FirstOrDefault()?.InsuredId)
                };
                companyTransport.Risk.Policy = new CompanyPolicy
                {
                    Id = entityPolicy.PolicyId,
                    DocumentNumber = entityPolicy.DocumentNumber,
                    Endorsement = new CompanyEndorsement
                    {
                        Id = entityEndorsementRisk.EndorsementId
                    }
                };
            }

            return companyTransport;
        }

        /// <summary>
        /// Genera la información de la vigencia para el próximo endoso de declaración
        ///     para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Endoso de declaración</returns>
        public CompanyEndorsement GetNextDeclarationEndorsementByPolicyId(int policyId)
        {
            CompanyTransport companyTransport = GetCompanyTransportsByPolicyId(policyId).FirstOrDefault();
            List<CompanyEndorsement> endorsements = GetEndorsements(companyTransport.Risk.Policy);

            var currentFrom = GetCurrentFromForNextDeclaration(endorsements);
            int months = GetMothsByDeclarationPeriod(companyTransport.DeclarationPeriod.Id);

            foreach (CompanyEndorsement endorsement in endorsements)
            {
                endorsement.CurrentFrom = currentFrom;
                endorsement.CurrentTo = currentFrom.AddMonths(months);
                endorsement.EndorsementType = Core.Application.UnderwritingServices.Enums.EndorsementType.DeclarationEndorsement;
            }

            return endorsements.FirstOrDefault();

            /*return new CompanyEndorsement
            {
                CurrentFrom = currentFrom,
                CurrentTo = currentFrom.AddMonths(months),
                EndorsementType = Core.Application.UnderwritingServices.Enums.EndorsementType.DeclarationEndorsement,
                RiskId = companyTransport.Risk.RiskId
            };*/
        }

        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="temporalType">Type of the temporal.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies)
        {
            try
            {
                CompanyPolicyResult companyPolicyResult = new CompanyPolicyResult();
                companyPolicyResult.IsError = false;
                companyPolicyResult.Errors = new List<ErrorBase>();
                List<CompanyEndorsement> companyEndorsements = new List<CompanyEndorsement>();
                string message = string.Empty;
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy.Summary != null && policy.Summary.Risks.Count > 0 && temporalType == TempType.Endorsement)
                {
                    policy.Summary.Risks.ForEach(x => companyEndorsements.Add(x.Policy.Endorsement));

                }
                else
                {
                    policy.Summary.Risks = new List<CompanyRisk>();
                }

                List<CompanyTransport> companyTransports = null;
                //Businesscontext llega null en el endoso
                policy.UserId = (BusinessContext.Current != null) ? BusinessContext.Current.UserId : policy.UserId;
                policy.Errors = new List<ErrorBase>();
                if (policy == null)
                {
                    companyPolicyResult.IsError = true;
                    companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorTemporalNotFound });

                }
                else
                {

                    if (temporalType != TempType.Quotation)
                    {
                        ValidateHolder(ref policy);
                    }
                    if (policy.Errors != null && !policy.Errors.Any() && policy.Product.CoveredRisk != null)
                    {
                        companyTransports = GetCompanyTransportsByTemporalId(policy.Id);

                        if (companyTransports != null && companyTransports.Any())
                        {
                            if (clearPolicies)
                            {
                                policy.InfringementPolicies.Clear();
                                companyTransports.ForEach(x => x.Risk.InfringementPolicies.Clear());
                            }

                            policy = CreateEndorsement(policy, companyTransports);
                            //se agrega la validación para el caso en que tenga un evento de autorzación
                            if (policy?.InfringementPolicies?.Count == 0)
                            {
                                //if (policy.Endorsement.PolicyId != 0 && policy.Endorsement.Id != 0)

                                DelegateService.underwritingService.SaveTextLarge(policy.Endorsement.PolicyId, policy.Endorsement.Id);
                            }
                        }
                        else
                        {
                            throw new ArgumentException(Errors.NoExistRisk);
                        }

                        if (temporalType != TempType.Quotation)
                        {
                            if (policy.InfringementPolicies.Any())
                            {
                                companyPolicyResult.TemporalId = policy.Id;
                                companyPolicyResult.InfringementPolicies = policy.InfringementPolicies;
                            }
                            else
                            {
                                if (policy.Endorsement.EndorsementType == EndorsementType.Emission)
                                {
                                    if (policy.PaymentPlan.PremiumFinance == null)
                                    {
                                        companyPolicyResult.Message = string.Format(Errors.PolicyNumber, policy.DocumentNumber);
                                    }
                                    else
                                    {
                                        companyPolicyResult.Message = string.Format(Errors.PolicyNumber, policy.DocumentNumber +
                                       " \n\r " + Errors.LabelPay + policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode +
                                       " \n\r " + Errors.LabelUser + policy.UserId.ToString());
                                    }
                                }
                                else
                                {
                                    string additionalFinancing = "";

                                    if (policy.PaymentPlan.PremiumFinance != null)
                                    {
                                        companyPolicyResult.PromissoryNoteNumCode = policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode;
                                        additionalFinancing = string.Format(Errors.PromissoryNote, policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode, policy.User.UserId);
                                    }

                                    companyPolicyResult.Message = string.Format(Errors.EndorsementNumber, policy.DocumentNumber, policy.Endorsement.Number, policy.Endorsement.Id, additionalFinancing);
                                }

                                companyPolicyResult.DocumentNumber = policy.DocumentNumber;
                                companyPolicyResult.EndorsementId = policy.Endorsement.Id;
                                companyPolicyResult.EndorsementNumber = policy.Endorsement.Number;
                            }
                        }
                        else
                        {
                            companyPolicyResult.Message = string.Format(Errors.QuotationNumber, policy.Endorsement.QuotationId.ToString());
                            companyPolicyResult.DocumentNumber = Convert.ToDecimal(policy.Endorsement.QuotationId);
                        }
                    }
                    else
                    {
                        companyPolicyResult.IsError = true;
                        companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = string.Join(" - ", policy.Errors) });
                    }

                }
                List<CompanyEndorsementDetail> companyEndorsementDetails = new List<CompanyEndorsementDetail>();
                if (policy.PolicyType.IsFloating && temporalType == TempType.Policy)
                {
                    SaveCompanyEndorsementPeriod(ModelAssembler.CreateCompanyEndorsementPeriod(policy, companyTransports.FirstOrDefault(), companyPolicyResult.DocumentNumber));
                }
                else if (temporalType == TempType.Endorsement)
                {
                    if (policy.Endorsement.EndorsementType == EndorsementType.Renewal || policy.Endorsement.EndorsementType == EndorsementType.EffectiveExtension)
                    {
                        SaveCompanyEndorsementPeriod(ModelAssembler.CreateCompanyEndorsementPeriod(policy, companyTransports.FirstOrDefault(), companyPolicyResult.DocumentNumber));
                    }
                    else
                    {
                        CompanyEndorsementPeriod endorsementPeriod = GetEndorsementPeriodByPolicyId(policy.Endorsement.PolicyId);
                        companyEndorsementDetails = SaveEndorsementDetailS(ModelAssembler.CreateCompanyEndorsementDetails(companyEndorsements, companyTransports, companyPolicyResult.DocumentNumber, endorsementPeriod));
                    }

                }
                return companyPolicyResult;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Validates the holder.
        /// </summary>
        /// <param name="policy">The policy.</param>
        public void ValidateHolder(ref CompanyPolicy policy)
        {
            if (policy.Holder != null)
            {
                if (policy.Holder.CustomerType == UTIENUMS.CustomerType.Prospect)
                {
                    policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorHolderNoInsuredRole });
                }
                else
                {
                    List<Holder> holders = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), UTIENUMS.InsuredSearchType.IndividualId, policy.Holder.CustomerType);

                    if (holders != null && holders.Count == 1)
                    {
                        if (holders[0].InsuredId == 0)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderWithoutRol });
                        }
                        else if (holders[0]?.DeclinedDate > DateTime.MinValue)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderDisabled });
                        }
                    }
                    else
                    {
                        policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorConsultPolicyholder });
                    }

                    if (policy.Holder.PaymentMethod != null)
                    {
                        if (policy.Holder.PaymentMethod.Id == 0)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderDefaultPaymentPlan });
                        }
                    }

                    //Validación asegurado principal como prospecto
                    switch (policy.Product.CoveredRisk.CoveredRiskType)
                    {
                        case CoveredRiskType.Transport:
                            List<CompanyTransport> companyTransports = GetCompanyTransportsByTemporalId(policy.Id);
                            if (companyTransports != null && companyTransports.Any())
                            {
                                var result = companyTransports.Select(x => x.Risk).Where(z => z.MainInsured?.CustomerType == UTIENUMS.CustomerType.Prospect).Count();
                                if (result > 0)
                                {
                                    policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorInsuredNoInsuredRole });
                                }
                            }
                            break;
                    }
                }
            }

        }

        private decimal GetMonthsByVigency(DateTime currentFrom, DateTime currentTo)
        {
            //var result = currentFrom;
            //var monthCount = 0;
            //while (result < currentTo)
            //{
            //    result = result.AddMonths(1);
            //    monthCount++;
            //}
            //return monthCount;
            int referenceDay = currentFrom.Day;
            var result = currentFrom;
            var monthCount = 0;
            while (result < currentTo)
            {
                result = result.AddMonths(1);
                if (result.Day < referenceDay)
                {
                    DateTime tempDate;
                    bool validDate = false;
                    validDate = DateTime.TryParse(string.Format("{0}/{1}/{2}", result.Year, result.Month, referenceDay), out tempDate);
                    if (validDate)
                    {
                        result = tempDate;
                    }
                }
                monthCount++;
            }
            return monthCount;
        }

        public bool CanMakeEndorsement(int policyId, out Dictionary<string, object> endorsementValidate)
        {
            try
            {
                bool result = false;
                endorsementValidate = new Dictionary<string, object>();
                CompanyTransport companyTransport = GetCompanyTransportsByPolicyId(policyId).FirstOrDefault();
                List<CompanyEndorsement> endorsements = GetEndorsements(companyTransport.Risk.Policy);
                //decimal monthsVigency = GetMonthsByVigency(companyTransport.Risk.Policy.CurrentFrom, companyTransport.Risk.Policy.CurrentTo);
                decimal monthsVigency = GetMonthsByVigency((endorsements.Where(x => x.EndorsementType == EndorsementType.Emission || x.EndorsementType == EndorsementType.Renewal || x.EndorsementType == EndorsementType.EffectiveExtension).OrderByDescending(x => x.Id).FirstOrDefault().CurrentFrom),
                        (endorsements.Where(x => x.EndorsementType == EndorsementType.Emission || x.EndorsementType == EndorsementType.Renewal || x.EndorsementType == EndorsementType.EffectiveExtension).OrderByDescending(x => x.Id).FirstOrDefault().CurrentTo));
                decimal QuantityOfDeclaration = 0;
                if (companyTransport.DeclarationPeriod.Id != 0)
                {
                    QuantityOfDeclaration = Math.Ceiling(monthsVigency / GetMothsByDeclarationPeriod(companyTransport.DeclarationPeriod.Id));
                }
                decimal QuantityOfAdjustment = 0;
                if (companyTransport.AdjustPeriod.Id != 0)
                {
                    QuantityOfAdjustment = Math.Floor(monthsVigency / GetMothsByAdjustmentPeriod(companyTransport.AdjustPeriod.Id));
                    if (monthsVigency < 12 && QuantityOfAdjustment == 0)
                    {
                        QuantityOfAdjustment = 1;
                    }
                }
                decimal declarationEndorsmentCount = endorsements.Where(x => x.EndorsementType == EndorsementType.DeclarationEndorsement && x.CurrentFrom >= (endorsements.Where(y => y.EndorsementType == EndorsementType.Emission || y.EndorsementType == EndorsementType.Renewal || y.EndorsementType == EndorsementType.EffectiveExtension).OrderByDescending(y => y.Id).FirstOrDefault().CurrentFrom)).Count();
                decimal ajustmentEndorsmentCount = endorsements.Where(x => x.EndorsementType == EndorsementType.AdjustmentEndorsement && x.CurrentFrom >= (endorsements.Where(y => y.EndorsementType == EndorsementType.Emission || y.EndorsementType == EndorsementType.Renewal || y.EndorsementType == EndorsementType.EffectiveExtension).OrderByDescending(y => y.Id).FirstOrDefault().CurrentFrom)).Count();
                decimal QuantityOfDeclarationbyAjust = 0;
                if (QuantityOfAdjustment != 0 && QuantityOfDeclaration != 0)
                {
                    QuantityOfDeclarationbyAjust = QuantityOfDeclaration / QuantityOfAdjustment;
                }

                List<decimal> declarationByAjustment = new List<decimal>(); ;
                decimal aux = QuantityOfDeclaration;
                decimal eval = 0;
                do
                {
                    eval = QuantityOfDeclaration - QuantityOfDeclarationbyAjust;
                    if (eval > QuantityOfDeclarationbyAjust && ((QuantityOfDeclaration - eval) > 0))
                    {
                        declarationByAjustment.Add(QuantityOfDeclaration - eval);
                        QuantityOfDeclaration = eval;
                    }
                    else
                    {
                        declarationByAjustment.Add(QuantityOfDeclaration);
                    }

                } while (eval > QuantityOfDeclarationbyAjust);
                QuantityOfDeclaration = aux;

                decimal endorsementeLimit = 0;

                if (declarationEndorsmentCount < QuantityOfDeclaration || ajustmentEndorsmentCount < QuantityOfAdjustment)
                {
                    if (declarationByAjustment.Count == 1)
                    {
                        endorsementeLimit = declarationByAjustment[0];
                    }
                    else
                    {
                        endorsementeLimit = ((declarationByAjustment[(int)ajustmentEndorsmentCount] * ajustmentEndorsmentCount) + QuantityOfDeclarationbyAjust);
                    }
                    if (declarationEndorsmentCount < endorsementeLimit)
                    {

                        endorsementValidate.Add("AllowEndorsement", EndorsementType.DeclarationEndorsement);
                        endorsementValidate.Add("Message", "Endoso de declaracion pendiente");
                    }
                    else
                    {
                        endorsementValidate.Add("AllowEndorsement", EndorsementType.AdjustmentEndorsement);
                        endorsementValidate.Add("Message", "Endoso de ajuste pendiente");

                    }
                    result = true;
                }
                else
                {
                    endorsementValidate.Add("AllowEndorsement", 0);
                    endorsementValidate.Add("Message", "Ya se han realizado todos los endosos (Ajuste/Declaracion) para esta Póliza");
                    result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al consultar los endosos");
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyCoverage> GetCoveragesByCoveragesAdd(int productId, int coverageGroupId, int prefixId, string coveragesAdd, int insuredObjectId)
        {
            List<CompanyCoverage> coverages = new List<CompanyCoverage>();

            if (!string.IsNullOrEmpty(coveragesAdd))
            {
                string[] idCoverages = coveragesAdd.Split(',');

                try
                {
                    coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, coverageGroupId, prefixId);
                    coverages = coverages.Where(c => c.InsuredObject.Id == insuredObjectId).ToList();
                    coverages = coverages.Where(c => (!idCoverages.Any(x => Convert.ToInt32(x) == c.Id)) && c.IsVisible == true && c.InsuredObject.Id == insuredObjectId).ToList();
                    coverages.RemoveAll(u => u.MainCoverageId != 0);
                    if (coverages != null)
                    {
                        coverages.OrderBy(x => x.Description);
                        return coverages;
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
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }
        }


        #region Persistencia Emision (Ajuste/Declaracion)
        private DatatableToList DatatableToList = new DatatableToList();
        private bool tryAgain = true;
        public CompanyEndorsementPeriod SaveCompanyEndorsementPeriod(CompanyEndorsementPeriod companyEndorsementPeriod)
        {
            try
            {
                NameValue[] parameters = new NameValue[8];
                CompanyEndorsementPeriod resultEndorsementPeriod = new CompanyEndorsementPeriod();
                decimal monthsVigency = GetMonthsByVigency(companyEndorsementPeriod.CurrentFrom, companyEndorsementPeriod.CurrentTo);
                companyEndorsementPeriod.DeclarationPeriod = GetMothsByDeclarationPeriod(companyEndorsementPeriod.DeclarationPeriod);
                companyEndorsementPeriod.AdjustPeriod = GetMothsByAdjustmentPeriod(companyEndorsementPeriod.AdjustPeriod);
                companyEndorsementPeriod.TotalAdjustment = (int)Math.Floor(monthsVigency / companyEndorsementPeriod.AdjustPeriod);
                companyEndorsementPeriod.TotalDeclarations = (int)Math.Ceiling(monthsVigency / companyEndorsementPeriod.DeclarationPeriod);
                if (monthsVigency < 12 && companyEndorsementPeriod.TotalAdjustment == 0)
                {
                    companyEndorsementPeriod.TotalAdjustment = 1;
                }
                //int rowcount = 0;
                parameters[0] = new NameValue("@CURRENT_FROM", companyEndorsementPeriod.CurrentFrom);
                parameters[1] = new NameValue("@CURRENT_TO", companyEndorsementPeriod.CurrentTo);
                parameters[2] = new NameValue("@ADJUST_PERIOD", companyEndorsementPeriod.AdjustPeriod);
                parameters[3] = new NameValue("@DECLARATION_PERIOD", companyEndorsementPeriod.DeclarationPeriod);
                parameters[4] = new NameValue("@POLICY_ID", companyEndorsementPeriod.PolicyId);
                parameters[5] = new NameValue("@TOTAL_DECLARATIONS", companyEndorsementPeriod.TotalDeclarations);
                parameters[6] = new NameValue("@TOTAL_ADJUSTMENT", companyEndorsementPeriod.TotalAdjustment);
                parameters[7] = new NameValue("@VERSION", companyEndorsementPeriod.Version);
                DataSet result;
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataSet("ISS.SAVE_ENDORSEMENT_COUNT_PERIOD", parameters);
                }
                if (result != null)
                {
                    return resultEndorsementPeriod;
                    //resultEndorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementPeriod>(result.Tables[0]).FirstOrDefault();
                }
                return new CompanyEndorsementPeriod();
            }
            catch (Exception ex)
            {

                EventLog.WriteEntry("SaveCompanyEndorsementPeriod", String.Format("Error Persistiendo Datos de la poliza en ISS.ENDORSEMENT_COUNT_PERIOD DETALLES {0} : {1}", ex.Message, JsonConvert.SerializeObject(companyEndorsementPeriod)));
                if (tryAgain)
                {
                    tryAgain = false;
                    SaveCompanyEndorsementPeriod(companyEndorsementPeriod);

                }
                throw new Exception(String.Format("Error Persistiendo Datos de la poliza en ISS.ENDORSEMENT_COUNT_PERIOD DETALLES {0}", ex.Message));
            }
        }
        public List<CompanyEndorsementDetail> SaveEndorsementDetailS(List<CompanyEndorsementDetail> endorsementDetails)
        {

            List<CompanyEndorsementDetail> ValidEndtrsementDetails = new List<CompanyEndorsementDetail>();
            try
            {
                foreach (CompanyEndorsementDetail item in endorsementDetails)
                {
                    ValidEndtrsementDetails.Add(SaveEndorsementDetail(item));
                }

                return ValidEndtrsementDetails;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public CompanyEndorsementDetail SaveEndorsementDetail(CompanyEndorsementDetail model)
        {

            CompanyEndorsementDetail resultEndorsementPeriod = new CompanyEndorsementDetail();
            NameValue[] parameters = new NameValue[12];
            parameters[0] = new NameValue("@POLICY_ID", model.PolicyId);
            parameters[1] = new NameValue("@ENDORSEMENT_TYPE", model.EndorsementType);
            parameters[2] = new NameValue("@RISK_NUM", model.RiskNum);
            parameters[3] = new NameValue("@INSURED_OBJECT_ID", model.InsuredObjectId);
            parameters[4] = new NameValue("@VERSION", model.Version);
            parameters[5] = new NameValue("@ENDORSEMENT_DATE", model.EndorsementDate);
            if (model.DeclarationValue != null)
            {
                parameters[6] = new NameValue("@DECLARATION_VALUE", model.DeclarationValue);
            }
            else
            {
                parameters[6] = new NameValue("@DECLARATION_VALUE", DBNull.Value, DbType.Decimal);
            }
            parameters[7] = new NameValue("@PREMIUM_AMOUNT", model.PremiumAmount);
            if (model.DeductibleAmmount != null)
            {
                parameters[8] = new NameValue("@DEDUCTIBLE_AMOUNT", model.DeductibleAmmount);
            }
            else
            {
                parameters[8] = new NameValue("@DEDUCTIBLE_AMOUNT", DBNull.Value, DbType.Int32);
            }
            if (model.Taxes != null)
            {
                parameters[9] = new NameValue("@TAXES", model.Taxes);
            }
            else
            {
                parameters[9] = new NameValue("@TAXES", DBNull.Value, DbType.Int32);
            }
            if (model.Surchanges != null)
            {
                parameters[10] = new NameValue("@SURCHANGE", model.Surchanges);
            }
            else
            {
                parameters[10] = new NameValue("@SURCHANGE", DBNull.Value, DbType.Int32);
            }
            if (model.Expenses != null)
            {
                parameters[11] = new NameValue("@EXPENSES", model.Expenses);
            }
            else
            {
                parameters[11] = new NameValue("@EXPENSES", DBNull.Value, DbType.Int32);
            }



            DataSet result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataSet("ISS.SAVE_ENDORSEMENT_COUNT_DETAIL", parameters);
            }
            if (result != null)
            {
                //resultEndorsementPeriod;
                return resultEndorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementDetail>(result.Tables[0]).FirstOrDefault();
            }

            return new CompanyEndorsementDetail();
        }

        public CompanyEndorsementPeriod GetEndorsementPeriodByPolicyId(decimal policyId)
        {
            CompanyEndorsementPeriod endorsementPeriod = new CompanyEndorsementPeriod();
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@POLICY_ID", policyId);
            DataSet result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataSet("ISS.GET_ENDORSEMENT_COUNT_PERIOD", parameters);
            }
            if (result != null)
            {

                endorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementPeriod>(result.Tables[0]).FirstOrDefault();
            }
            return endorsementPeriod;
        }

        public List<CompanyEndorsementDetail> GetEndorsementDetailsListByPolicyId(decimal policyId, decimal version)
        {
            try
            {
                List<CompanyEndorsementDetail> endorsementPeriod = new List<CompanyEndorsementDetail>();
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("@POLICY_ID", policyId);
                parameters[1] = new NameValue("@VERSION", version);
                DataSet result;
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataSet("ISS.GET_ENDORSEMENT_COUNT_DETAIL", parameters);
                }
                if (result != null)
                {

                    endorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementDetail>(result.Tables[0]);
                }
                return endorsementPeriod;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public bool CanMakeEndorsementByRiskByInsuredObjectId(decimal policyId, decimal riskId, decimal insuredObjectId, EndorsementType endorsementType)
        {

            try
            {

                CompanyEndorsementPeriod period = GetEndorsementPeriodByPolicyId(policyId);
                List<CompanyEndorsementDetail> detailsList = GetEndorsementDetailsListByPolicyId(policyId, period.Version);
                int endorsementCount = detailsList.Where(x => x.PolicyId == policyId && x.RiskNum == riskId && x.InsuredObjectId == insuredObjectId && x.Version == period.Version && x.EndorsementType == (int)endorsementType).Count();
                switch (endorsementType)
                {
                    case EndorsementType.DeclarationEndorsement:
                        return (endorsementCount < period.TotalDeclarations) ? true : false;
                        break;
                    case EndorsementType.AdjustmentEndorsement:
                        return (endorsementCount < period.TotalAdjustment) ? true : false;
                        break;
                    default:
                        return false;
                        break;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        #endregion
    }
}