using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using ENUM = Sistran.Core.Services.UtilitiesServices.Enums;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Rules = Sistran.Core.Framework.Rules;
using SAR = Sistran.Company.Application.SarlaftApplicationServices.DTO;
using SCREN = Sistran.Core.Application.Script.Entities;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider
{
    public class UnderwritingRuleEngineCompatibilityServiceEEProvider
    {



        public void ValidateLimitEndorsementCoverage(Rules.Facade facade)
        {
            decimal endorsementLimitAmount = facade.GetConcept<decimal>(RuleConceptCoverage.EndorsementLimitAmount);
            decimal endorsementSubLimitAmount = facade.GetConcept<decimal>(RuleConceptCoverage.EndorsementSubLimitAmount);

            if (endorsementLimitAmount != endorsementSubLimitAmount)
            {
                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
            }
        }

        //funciones de R1
        /// <summary>
        /// Tasa unica Antigua
        /// </summary>
        /// <param name="facade"></param>
        public void GetFlatRatePct(Rules.Facade facade)
        {
            const int parameterId = 2119;
            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.CoRiskVehicle.Properties.RiskId).Equal().Constant(facade.GetConcept<int?>(CompanyRuleConceptRisk.RiskId).GetValueOrDefault());
                BusinessCollection<ISSEN.CoRiskVehicle> businessColection = DataFacadeManager.Instance.GetDataFacade().List<ISSEN.CoRiskVehicle>(filter.GetPredicate());
                if (businessColection.Count > 0)
                {
                    decimal FlatRatePct = businessColection[0].FlatRatePercentage ?? 0;
                    facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value), FlatRatePct);
                }
            }

        }

        /// <summary>
        /// Grupo Coberturas Antigua
        /// </summary>
        /// <param name="facade"></param>
        public void GetGroupCoverage(Rules.Facade facade)
        {
            const int parameterId = 2120;
            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.Risk.Properties.RiskId).Equal().Constant(facade.GetConcept<int?>(CompanyRuleConceptRisk.RiskId).GetValueOrDefault());
                BusinessCollection<ISSEN.Risk> businessCollection = DataFacadeManager.Instance.GetDataFacade().List<ISSEN.Risk>(filter.GetPredicate());
                if (businessCollection.Count > 0 && parameter.NumberParameter.Value > 0)
                {
                    int GroupCoverage = businessCollection[0].CoverGroupId ?? 0;
                    facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value), GroupCoverage);
                }
            }
        }

        /// <summary>
        /// Ultimo Deducible nivel riesgo
        /// </summary>
        /// <param name="facade"></param>
        public void GetLastRiskDeductId(Rules.Facade facade)
        {
            const int paramCoverageId = 2125;
            const int paramDeductibleId = 2124;

            List<Parameter> parametersList = new List<Parameter>
                {
                    new Parameter
                    {
                        Id = paramCoverageId
                    },
                    new Parameter
                    {
                        Id = paramDeductibleId
                    }
                };
            List<Parameter> parameters = DelegateService.commonService.GetParametersByParameterIds(parametersList);
            Parameter coverageParameter = parameters.FirstOrDefault(x => x.Id == paramCoverageId);
            Parameter deductParameter = parameters.FirstOrDefault(x => x.Id == paramDeductibleId);

            decimal deductId = 0;
            if (coverageParameter != null && coverageParameter.NumberParameter.HasValue)
            {
                int coverageId = facade.GetConcept<int>(CompanyRuleConceptRisk.DynamicConcept(coverageParameter.NumberParameter.Value));
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(ISSEN.RiskCoverDeduct.Properties.RiskCoverId, coverageId);
                BusinessCollection<ISSEN.RiskCoverDeduct> businessCollection = DataFacadeManager.Instance.GetDataFacade().List<ISSEN.RiskCoverDeduct>(filter.GetPredicate());
                if (businessCollection != null && businessCollection.Count > 0)
                {
                    deductId = businessCollection[0].DeductId ?? 0;
                }
            }
            facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(deductParameter.NumberParameter.Value), deductId);

        }

        public void ValidateBasicCoverage(Rules.Facade facade)
        {
            //Coverturas asociadas al riesgo
            List<CompanyCoverage> riskcoverages = facade.GetConcept<List<CompanyCoverage>>(RuleConceptRisk.Coverages);

            /*valida que al menos exista una cobertura Basica*/
            if (riskcoverages != null)
            {
                if (!riskcoverages.Any(c => c.IsPrimary))
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }
            }
        }

        /// <summary>
        /// Tiene Cobertura Lucro
        /// </summary>
        /// <param name="facade"></param>
        public void GetIslucre(Rules.Facade facade)
        {
            const int parameterId = 2115;
            const int coverageId = 105;
            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int IsLucre = 0;

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRiskCoverage.Properties.PolicyId).Equal().Constant(facade.GetConcept<string>(CompanyRuleConceptGeneral.PolicyId));
                filter.And();
                filter.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId).Equal().Constant(facade.GetConcept<string>(CompanyRuleConceptGeneral.EndorsementId));
                filter.And();
                filter.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskNum).Equal().Constant(facade.GetConcept<string>(CompanyRuleConceptRisk.RiskNumber));
                filter.And();
                filter.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskCoverId).Equal().Constant(coverageId);
                filter.And();
                filter.Property(ISSEN.EndorsementRiskCoverage.Properties.CoverStatusCode).Distinct().Constant(CoverageStatusType.Excluded);
                filter.And();
                filter.Property(ISSEN.EndorsementRiskCoverage.Properties.CoverStatusCode).Distinct().Constant(CoverageStatusType.Cancelled);

                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List<ISSEN.EndorsementRiskCoverage>(filter.GetPredicate());
                if (businessCollection != null && businessCollection.Count > 0)
                {
                    IsLucre = 1;
                }
                facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value), IsLucre);
            }
        }

        public void SetExistsDelegate(Rules.Facade facade)
        {
            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(10024);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                if (facade.GetConcept<int>(CompanyRuleConceptGeneral.UserId) != 0)
                {
                    PrimaryKey primaryKey = UUEN.UniqueUsers.CreatePrimaryKey(facade.GetConcept<int>(CompanyRuleConceptGeneral.UserId));

                    UUEN.UniqueUsers entityUniqueUsers = (UUEN.UniqueUsers)DataFacadeManager.GetObject(primaryKey);
                    if (entityUniqueUsers != null)
                    {
                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                        filter.PropertyEquals(UPEN.IndividualRelationApp.Properties.ParentIndividualId, typeof(UPEN.IndividualRelationApp).Name, entityUniqueUsers.PersonId);

                        BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(UPEN.IndividualRelationApp), filter.GetPredicate());

                        if (businessCollection.Count > 0)
                        {
                            facade.SetConcept(CompanyRuleConceptGeneral.DynamicConcept(parameter.NumberParameter.Value), true);
                        }
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorUser);
                    }

                }
                else
                {
                    throw new Exception(Errors.ErrorUser);

                }
            }
        }

        /// <summary>
        /// Obtiene valor prima
        /// </summary>
        /// <param name="facade"></param>
        public void GetTechnicalPremium(Rules.Facade facade)
        {
            int parameterId = 2231;
            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                facade.SetConcept(CompanyRuleConceptCoverage.DynamicConcept(parameter.NumberParameter.Value), facade.GetConcept<List<Coverage>>(CompanyRuleConceptGeneral.Premium));
            }
        }

        /// <summary>
        /// Código de Rango de Peso
        /// </summary>
        /// <param name="facade"></param>
        public void GetWeightRangeCode(Rules.Facade facade)
        {
            const int parameterId = 2145;
            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(WeightRange.Properties.LimitRange).Equal().Constant(facade.GetConcept<int>(CompanyRuleConceptRisk.TonsQty));
                BusinessCollection<WeightRange> businessCollection = DataFacadeManager.Instance.GetDataFacade().List<WeightRange>(filter.GetPredicate());
                if (businessCollection.Count > 0 && parameter.NumberParameter.Value > 0)
                {
                    int WeightRangeCod = businessCollection[0].WeightRangeCode;
                    facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value), WeightRangeCod);
                }

            }
        }

        //Eventos
        /// <summary>
        /// 
        /// </summary>
        /// <param name="facade"></param>
        public void GetRiskQuantityByStatus(Rules.Facade facade)
        {
            int riskquantity = 0;
            List<Risk> risks = facade.GetConcept<List<Risk>>(CompanyRuleConceptGeneral.Risks);

            riskquantity = risks.Count(x => x.Status != RiskStatusType.Excluded && x.Status != RiskStatusType.Cancelled);

            if (riskquantity > 0)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.RisksQuantity, riskquantity);
            }
        }

        /// <summary>
        /// Asigna Prima Mínima para cumplimiento
        /// </summary>
        /// <param name="facade"></param>
        public void GetMinimumPremiumCoverage(Rules.Facade facade)
        {
            int parameterId = 2206;
            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(parameterId);
            int minPremiumRelId = 0;
            int minPremiumRelIds = 0;
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                var coverageid = facade.GetConcept<int>(CompanyRuleConceptCoverage.CoverageId);
                List<CompanyCoverage> listTempRiskCoverage = facade.GetConcept<List<CompanyCoverage>>(CompanyRuleConceptRisk.Coverages);
                CompanyCoverage tempRiskCoverage = null;
                if (facade.GetConcept<List<CompanyCoverage>>(CompanyRuleConceptRisk.Coverages).FirstOrDefault(x => x.Id == coverageid) != null)
                {
                    tempRiskCoverage = facade.GetConcept<List<CompanyCoverage>>(CompanyRuleConceptRisk.Coverages).FirstOrDefault(x => x.Id == coverageid);
                }
                if (tempRiskCoverage.PremiumAmount != 0)
                {
                    facade.SetConcept(CompanyRuleConceptCoverage.DynamicConcept(parameter.NumberParameter.Value),
                    facade.GetConcept<int>(CompanyRuleConceptCoverage.MinimumPremiumCoverage));

                    //Se agrega codigo de la funcion de R1
                    //Se obtienen los valores para realizar la consulta
                    int prefixId = facade.GetConcept<int>(CompanyRuleConceptGeneral.PrefixCode);
                    int productId = facade.GetConcept<int>(CompanyRuleConceptGeneral.ProductId);
                    int currencyCode = facade.GetConcept<int>(CompanyRuleConceptGeneral.CurrencyCode);
                    int endorsementType = facade.GetConcept<int>(CompanyRuleConceptGeneral.EndorsementTypeCode);

                    //Se crea consulta
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(MinPremiumRelation.Properties.PrefixCode).Equal().Constant(prefixId);
                    filter.And();
                    filter.Property(MinPremiumRelation.Properties.Key1).Equal().Constant(productId);
                    filter.And();
                    filter.Property(MinPremiumRelation.Properties.CurrencyCode).Equal().Constant(currencyCode);
                    filter.And();
                    filter.Property(MinPremiumRelation.Properties.EndoTypeCode).Equal().Constant(endorsementType);
                    BusinessCollection<MinPremiumRelation> response = DataFacadeManager.Instance.GetDataFacade().List<MinPremiumRelation>(filter.GetPredicate());

                    if (listTempRiskCoverage?.Count > 0 && response?.Count > 0)
                    {

                        List<CompanyCoverage> listTempRiskCoverageEdit = new List<CompanyCoverage>();



                        if (tempRiskCoverage.IsEnabledMinimumPremium)
                        {
                            minPremiumRelId = (from o in response.Cast<MinPremiumRelation>().AsEnumerable()
                                               where o.SubsMinPremium.Value <= tempRiskCoverage.MinimumPremiumCoverage
                                               && o.RiskMinPremium >= tempRiskCoverage.MinimumPremiumCoverage
                                               select o.MinPremiumRelId).Sum();

                            List<MinPremiumRelation> editedRisks = (from o in response.Cast<MinPremiumRelation>()
                                                                    where o.MinPremiumRelId == minPremiumRelId
                                                                    select o).ToList();
                            if (editedRisks.Count > 0)
                            {
                                if (tempRiskCoverage.SubLimitAmount != 0)
                                {
                                    if (editedRisks[0].Key2 == 1)
                                    {
                                        tempRiskCoverage.PremiumAmount = editedRisks[0].RiskMinPremium.Value;
                                        tempRiskCoverage.RateType = ENUM.RateType.FixedValue;
                                        tempRiskCoverage.CalculationType = ENUM.CalculationType.Direct;
                                        tempRiskCoverage.Rate = tempRiskCoverage.PremiumAmount;
                                        tempRiskCoverage.IsEnabledMinimumPremium = true;
                                    }
                                    if (editedRisks[0].Key2 == 2)
                                    {
                                        tempRiskCoverage.PremiumAmount = tempRiskCoverage.MinimumPremiumCoverage.Value;
                                        tempRiskCoverage.RateType = ENUM.RateType.FixedValue;
                                        tempRiskCoverage.CalculationType = ENUM.CalculationType.Direct;
                                        tempRiskCoverage.Rate = tempRiskCoverage.PremiumAmount;
                                        tempRiskCoverage.IsEnabledMinimumPremium = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            minPremiumRelIds = (from o in response.Cast<MinPremiumRelation>().AsEnumerable()
                                                where o.SubsMinPremium.Value <= tempRiskCoverage.PremiumAmount
                                                && o.RiskMinPremium >= tempRiskCoverage.PremiumAmount
                                                select o.MinPremiumRelId).Sum();

                            List<MinPremiumRelation> editedRisk = (from o in response.Cast<MinPremiumRelation>()
                                                                   where o.MinPremiumRelId == minPremiumRelIds
                                                                   select o).ToList();

                            if (editedRisk.Count > 0)
                            {
                                if (tempRiskCoverage.SubLimitAmount != 0)
                                {
                                    if (editedRisk[0].Key2 == 1) //RANGO DE PRIMA ENTRE 0 Y 20000 SI LA PRIMA ES MENOR A 20000 Y EXISTE MAS DE UNA COBERTURA LA PRIMA DEBE VALER 20000
                                    {
                                        tempRiskCoverage.MinimumPremiumCoverage = editedRisk[0].RiskMinPremium.Value;
                                        tempRiskCoverage.PremiumAmount = editedRisk[0].RiskMinPremium.Value;
                                        tempRiskCoverage.RateType = ENUM.RateType.FixedValue;
                                        tempRiskCoverage.CalculationType = ENUM.CalculationType.Direct;
                                        tempRiskCoverage.Rate = editedRisk[0].RiskMinPremium.Value;
                                        tempRiskCoverage.IsEnabledMinimumPremium = true;
                                    }

                                    if (editedRisk[0].Key2 == 2) //RANGO DE PRIMA ENTRE 20000 Y 25000 SI LA PRIMA ESTA ENTRE 20000 Y 25000 Y EXISTE MAS DE UNA COBERTURA LA PRIMA DEBE SER LA PRIMA GURDADA EN EL CAMPOS EnabledMinimumPremiumAmount
                                    {
                                        tempRiskCoverage.MinimumPremiumCoverage = tempRiskCoverage.PremiumAmount;
                                        tempRiskCoverage.PremiumAmount = tempRiskCoverage.PremiumAmount;
                                        tempRiskCoverage.RateType = ENUM.RateType.FixedValue;
                                        tempRiskCoverage.CalculationType = ENUM.CalculationType.Direct;
                                        tempRiskCoverage.Rate = tempRiskCoverage.PremiumAmount;
                                        tempRiskCoverage.IsEnabledMinimumPremium = false;
                                    }

                                    if (editedRisk[0].Key2 == 3)
                                    {
                                        tempRiskCoverage.PremiumAmount = tempRiskCoverage.PremiumAmount;
                                        tempRiskCoverage.IsEnabledMinimumPremium = false;
                                    }

                                }
                            }
                        }
                        facade.SetConcept(RuleConceptCoverage.PremiumAmount, tempRiskCoverage.PremiumAmount);
                        facade.SetConcept(RuleConceptCoverage.RateTypeCode, tempRiskCoverage.RateType);
                        facade.SetConcept(RuleConceptCoverage.MinimumPremiumCoverage, tempRiskCoverage.MinimumPremiumCoverage);
                        facade.SetConcept(RuleConceptCoverage.CalculationTypeCode, tempRiskCoverage.CalculationType);
                        facade.SetConcept(RuleConceptCoverage.IsEnabledMinimumPremium, tempRiskCoverage.IsEnabledMinimumPremium);
                        facade.SetConcept(RuleConceptCoverage.Rate, tempRiskCoverage.Rate);
                        facade.SetConcept(CompanyRuleConceptRisk.ExecuteMinimumPremiumCoverageFunction, true);
                    }
                }
            }
        }

        public int GetInsuredId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.Insured.Properties.IndividualId, typeof(UPEN.Insured).Name);
            filter.Equal();
            filter.Constant(individualId);
            var result = DataFacadeManager.Instance.GetDataFacade().List<UPEN.Insured>(filter.GetPredicate());
            if (result.Count > 0)
            {
                return result[0].InsuredCode;
            }
            return 0;
        }

        public void IncreaseInsuredValue(Rules.Facade facade)
        {
            if (facade.GetConcept<int>(CompanyRuleConceptGeneral.EndorsementTypeCode) != (int)EndorsementType.Emission)
            {
                Func<SCREN.Concept, bool> entityPredicate = (str) => str == null ? false : str.ConceptName == "Porcentaje Suma Asegurada" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_RISK).ToString());
                SCREN.Concept entityConcept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                if (entityConcept != null)
                {
                    int valor = facade.GetConcept<int>(new KeyValuePair<int, int>(entityConcept.ConceptId, entityConcept.EntityId));

                    int branchId = facade.GetConcept<int>(CompanyRuleConceptGeneral.BranchCode);
                    int prefixId = facade.GetConcept<int>(CompanyRuleConceptGeneral.PrefixCode);
                    decimal documentNum = facade.GetConcept<decimal>(CompanyRuleConceptGeneral.DocumentNumber);
                    var policyId = facade.GetConcept<decimal>(CompanyRuleConceptGeneral.PolicyId);

                    if (branchId != 0 && prefixId != 0 && documentNum != 0)
                    {
                        List<Endorsement> endorsements = DelegateService.underwritingService.GetEndorsementsByPrefixIdBranchIdPolicyNumber(prefixId, branchId, documentNum);

                        if (endorsements?.Count > 0)
                        {
                            if (endorsements.Count(x => x.IsCurrent && x.Id != 0) == 1)
                            {
                                Endorsement endorsementCurrent = endorsements.First(x => x.IsCurrent && x.Id != 0);

                                CompanyPolicy lastCompanyPolicy = DelegateService.underwritingService.GetCiaCurrentStatusPolicyByEndorsementIdIsCurrentCompany(endorsementCurrent.Id, false);

                                decimal insuredValueAct = facade.GetConcept<decimal>(CompanyRuleConceptRisk.AmountInsured);
                                var calculateDifference = (insuredValueAct - lastCompanyPolicy.Summary.AmountInsured);
                                if (lastCompanyPolicy.Summary.AmountInsured > 0)
                                {
                                    var calculatePercentage = ((calculateDifference * 100) / lastCompanyPolicy.Summary.AmountInsured);

                                    if (calculatePercentage > valor)
                                    {
                                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                                    }
                                }

                            }
                        }

                    }

                }
            }
        }


        public void CoverageRate(Rules.Facade facade)
        {
            if (facade.GetConcept<int>(CompanyRuleConceptGeneral.EndorsementTypeCode) != (int)EndorsementType.Emission)
            {
                int branchId = facade.GetConcept<int>(CompanyRuleConceptGeneral.BranchCode);
                int prefixId = facade.GetConcept<int>(CompanyRuleConceptGeneral.PrefixCode);
                decimal documentNum = facade.GetConcept<decimal>(CompanyRuleConceptGeneral.DocumentNumber);
                var policyId = facade.GetConcept<int>(CompanyRuleConceptGeneral.PolicyId);

                if (branchId != 0 && prefixId != 0 && documentNum != 0)
                {
                    List<Endorsement> endorsements = DelegateService.underwritingService.GetEndorsementsByPrefixIdBranchIdPolicyNumber(prefixId, branchId, documentNum);

                    if (endorsements?.Count > 0)
                    {
                        if (endorsements.Count(x => x.IsCurrent && x.Id != 0) == 1)
                        {
                            Endorsement endorsementCurrent = endorsements.First(x => x.IsCurrent && x.Id != 0);

                            CompanyPolicy lastCompanyPolicy = DelegateService.underwritingService.GetCiaCurrentStatusPolicyByEndorsementIdIsCurrentCompany(endorsementCurrent.Id, false);

                            if (lastCompanyPolicy.Endorsement.Id != 0 && policyId != 0)
                            {
                                List<CompanyRisk> risks = DelegateService.underwritingService.GetRiskByPolicyIdEndorsmentId(policyId, lastCompanyPolicy.Endorsement.Id);

                                int riskId = facade.GetConcept<int>(RuleConceptRisk.RiskId);
                                int CoverageId = facade.GetConcept<int>(RuleConceptCoverage.CoverageId);
                                decimal RateModify = facade.GetConcept<decimal>(RuleConceptCoverage.Rate);
                                foreach (CompanyRisk risk in risks)
                                {

                                    TP.Parallel.ForEach(risk.Coverages.AsParallel().Where(x => CoverageId == x.Id && riskId == risk.Id)
                                        .Where(y => RateModify < y.Rate), z =>
                                                                 facade.SetConcept(RuleConceptPolicies.GenerateEvent, true));
                                    //foreach (CompanyCoverage coverage in risk.Coverages)
                                    //{
                                    //    if (CoverageId == coverage.Id && riskId == risk.Id)
                                    //    {
                                    //        if (RateModify < coverage.Rate)
                                    //        {
                                    //            facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                                    //        }
                                    //    }
                                    //}
                                }

                            }

                        }

                    }
                }

            }
        }

        public void EquivalenceOfCoverage(Rules.Facade facade)
        {
            int coverageId = facade.GetConcept<int>(CompanyRuleConceptCoverage.CoverageId);

            if (coverageId != 0)
            {
                List<IntCoEquivalenceCoverage> equivalences = DelegateService.underwritingServiceCore.GetCoEquivalenceCoverage(coverageId);

                if (equivalences.Count > 0)
                {

                    TP.Parallel.ForEach(equivalences.AsParallel().Where(x => x.Coverage2GId == 0 || x.Coverage3GId == 0 || x.CoverageId == 0), z =>
                                                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true));

                    //foreach (IntCoEquivalenceCoverage equivalence in equivalences)
                    //{
                    //    if (equivalence.Coverage2GId == 0 || equivalence.Coverage3GId == 0 || equivalence.CoverageId == 0)
                    //    {
                    //        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    //    }
                    //}
                }
            }

        }

        public void ValidateGetConsortiumCorrect(Rules.Facade facade)
        {

            decimal participation = 0;
            List<bool> principalList = new List<bool>();
            bool principal = false;
            int individual = facade.GetConcept<int>(RuleConceptGeneral.IndividualNumberHolder);
            int associationType = facade.GetConcept<int>(RuleConceptGeneral.AssociationType);
            List<ConsorciatedDTO> consortiums = new List<ConsorciatedDTO>();
            if (individual != 0 && individual > 0)
            {
                if (associationType != (int)AssociationTypes.Individual && associationType != 0 && associationType > 0)
                {
                    consortiums = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(individual);
                    if (consortiums != null && consortiums.Count > 0)
                    {
                        consortiums.Select(x => x.Enabled);
                        participation = consortiums.Sum(x => x.ParticipationRate);
                        principalList = consortiums.Select(x => x.IsMain).ToList();
                        principal = principalList.Find(x => x = true);

                        //foreach (ConsorciatedDTO consortium in consortiums)
                        //{
                        //    if (consortium.Enabled == true)
                        //    {
                        //        participation += consortium.ParticipationRate;
                        //        if (consortium.IsMain == true)
                        //        {
                        //            principal = true;
                        //        }

                        //    }

                        //}
                    }

                    if (participation < 100 || principal != true)
                    {
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    }

                }
            }

        }

        public void EconomicGroupMember(Rules.Facade facade)
        {
            Func<SCREN.Concept, bool> entityPredicate = (str) => str == null ? false : str.ConceptName == "IndividualId (Validar grupo economico)" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());
            SCREN.Concept entityConcept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            if (entityConcept != null)
            {
                int individualId = facade.GetConcept<int>(new KeyValuePair<int, int>(entityConcept.ConceptId, entityConcept.EntityId));

                if (individualId != 0)
                {
                    List<EconomicGroupDetail> ListEconomicGroupDetails = new List<EconomicGroupDetail>();
                    ListEconomicGroupDetails = DelegateService.uniquePersonService.GetEconomicGroupDetailByIndividual(individualId);


                    if (ListEconomicGroupDetails.Count > 0)
                    {
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    }
                }
            }
        }

        /// <summary>
        /// valida si el documento del concepto "DocumentToValidateListClinton", esta en la lista clinton
        /// </summary>
        /// <param name="parameters">facadeEventos, facadeGeneral</param>
        /// <returns></returns>
        public void ValidateRiskList(Rules.Facade facade)
        {
            try
            {
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());
                int entityRiskId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_RISK).ToString());

                Func<SCREN.Concept, bool> entityPredicate = concept => concept != null && concept.ConceptName == "Rol a Validar" && concept.EntityId == entityId;
                SCREN.Concept entityRolToValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Listas de riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Tipo Lista de Riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityTypeRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Beneficiarios en listas de riesgo" && concept.EntityId == entityRiskId;
                SCREN.Concept entityBeneficiariesRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                if (entityRolToValidate == null || entityRiskList == null)
                { return; }

                string documentNumber = string.Empty;
                int documentType = 0;
                string fullName = string.Empty;
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                int rolToValidate = facade.GetConcept<int>(RuleConceptGeneral.DynamicConcept(entityRolToValidate.ConceptId, entityRolToValidate.EntityId));

                switch (rolToValidate)
                {
                    case 1: //Tomador 
                        fullName = facade.GetConcept<string>(RuleConceptGeneral.NameHolder);
                        documentNumber = facade.GetConcept<string>(RuleConceptGeneral.DocumentoNumberHolder);
                        documentType = facade.GetConcept<int>(RuleConceptGeneral.TypeOfHolderDocument);
                        break;

                    case 2: //Asegurado
                        fullName = facade.GetConcept<string>(RuleConceptRisk.NameInsured);
                        documentNumber = facade.GetConcept<string>(RuleConceptRisk.InsuredDocumentNumber);
                        documentType = facade.GetConcept<int>(RuleConceptRisk.TypeOfInsuredDocument);
                        break;

                    case 3: //Beneficiario
                        beneficiaries = facade.GetConcept<List<CompanyBeneficiary>>(RuleConceptRisk.Beneficiaries);
                        break;

                    case 4://Afianzado
                        fullName = facade.GetConcept<string>(RuleConceptRisk.InsuredNameOfTheBond);
                        documentNumber = facade.GetConcept<string>(RuleConceptRisk.InsuredDocumentNumberOfTheBond);
                        documentType = facade.GetConcept<int>(RuleConceptRisk.TypeOfTheBondDocument);
                        break;

                    default:
                        return;
                }

                Func<string, int, string, List<ListRiskMatchDTO>> delegateFunc = (document, typeDocument, name) =>
                {
                    if (string.IsNullOrEmpty(document) || string.IsNullOrEmpty(name) || documentType == 0)
                    {
                        return new List<ListRiskMatchDTO>();
                    }

                    if (typeDocument == 2 && document.Length > 9)
                    {
                        document = document.Substring(0, 9);
                    }


                    int riskListType = facade.GetConcept<int>(RuleConceptGeneral.DynamicConcept(entityTypeRiskList.ConceptId));
                    return DelegateService.coreUniqueListRiskPersonService.ValidateListRiskPerson(document, name, riskListType);
                };
                if (rolToValidate != 3)
                {
                    List<ListRiskMatchDTO> riskList = delegateFunc(documentNumber, documentType, fullName);
                    if (riskList.Any())
                    {
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityRiskList.ConceptId, entityRiskList.EntityId), string.Join(",", riskList.Select(x => x.listType)));
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    }
                }
                else
                {
                    List<string> beneficiariesNames = new List<string>();
                    List<string> listRiskNames = new List<string>();

                    foreach (CompanyBeneficiary beneficiary in beneficiaries)
                    {
                        documentNumber = beneficiary.IdentificationDocument.Number;
                        documentType = beneficiary.IdentificationDocument.DocumentType.Id;
                        fullName = beneficiary.Name;
                        List<ListRiskMatchDTO> riskList = delegateFunc(documentNumber, documentType, fullName);

                        if (riskList.Any())
                        {
                            beneficiariesNames.Add(fullName);
                            listRiskNames.AddRange(riskList.Select(x => x.listType));
                        }
                    }

                    if (beneficiariesNames.Any() && listRiskNames.Any())
                    {
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityRiskList.ConceptId, entityRiskList.EntityId), string.Join(",", listRiskNames.Distinct()));
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityBeneficiariesRiskList.ConceptId, entityBeneficiariesRiskList.EntityId), string.Join(",", beneficiariesNames));
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidatePersonRiskList", ex);
            }
        }

        public void ValidateConsortiatesFiscalResponsibility(Rules.Facade facade)
        {
            try
            {
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());

                Func<SCREN.Concept, bool> entityPredicate = concept => concept != null && concept.ConceptName == "Integrantes del consorcio" && concept.EntityId == entityId;

                SCREN.Concept entityConsortiumPartnerRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                int associationType = 0;
                int individual = 0;

                associationType = facade.GetConcept<int>(RuleConceptGeneral.AssociationType);
                individual = facade.GetConcept<int>(RuleConceptGeneral.IndividualNumberHolder);

                if (associationType != 1 && associationType > 0)
                {
                    List<ConsorciatedDTO> consortiums = new List<ConsorciatedDTO>();
                    consortiums = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(individual);



                    if (consortiums != null && consortiums.Count > 0)
                    {

                        List<string> names = new List<string>();

                        foreach (ConsorciatedDTO consortiumPartner in consortiums)
                        {
                            List<CompanyDTO> consortiumIndividual = new List<CompanyDTO>();
                            consortiumIndividual = DelegateService.uniquePersonAplicationService.GetAplicationCompanyByDocument(consortiumPartner.PersonIdentificationNumber);

                            if (consortiumIndividual != null)
                            {

                                foreach (var consortiumEmail in consortiumIndividual)
                                {
                                    List<InsuredFiscalResponsibilityDTO> consortiumRF = new List<InsuredFiscalResponsibilityDTO>();
                                    consortiumRF = DelegateService.uniquePersonAplicationService.GetCompanyFiscalResponsibilityByIndividualId(consortiumEmail.Id);
                                    var EmailType = consortiumRF.ToList();

                                    if (!EmailType.Any())
                                    {
                                        names.Add($"{consortiumPartner.PersonIdentificationNumber}-{ consortiumPartner.FullName}");
                                    }
                                }
                            }
                        }

                        if (names.Any())
                        {
                            facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityConsortiumPartnerRiskList.ConceptId, entityConsortiumPartnerRiskList.EntityId), string.Join(",", names.Distinct()));
                            facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateConsortiatesElectronicBilling", ex);
            }
        }

        public void ValidateConsortiatesElectronicBilling(Rules.Facade facade)
        {
            try
            {
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());

                Func<SCREN.Concept, bool> entityPredicate = concept => concept != null && concept.ConceptName == "Integrantes del consorcio" && concept.EntityId == entityId;

                SCREN.Concept entityConsortiumPartnerRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                int associationType = 0;
                int individual = 0;

                associationType = facade.GetConcept<int>(RuleConceptGeneral.AssociationType);
                individual = facade.GetConcept<int>(RuleConceptGeneral.IndividualNumberHolder);

                if (associationType != 1 && associationType > 0)
                {
                    List<ConsorciatedDTO> consortiums = new List<ConsorciatedDTO>();
                    consortiums = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(individual);

                    if (consortiums != null && consortiums.Count > 0)
                    {
                        List<string> names = new List<string>();

                        foreach (ConsorciatedDTO consortiumPartner in consortiums)
                        {
                            List<CompanyDTO> consortiumIndividual = new List<CompanyDTO>();
                            consortiumIndividual = DelegateService.uniquePersonAplicationService.GetAplicationCompanyByDocument(consortiumPartner.PersonIdentificationNumber);

                            if (consortiumIndividual != null)
                            {

                                TP.Parallel.ForEach(consortiumIndividual.AsParallel().Where(x => x.Emails.Where(y => y.EmailTypeId == 23).ToList().Any() == true), z =>
                                                                  names.Add($"{consortiumPartner.PersonIdentificationNumber}-{ consortiumPartner.FullName}")
                                );
                                
                                //foreach (var consortiumEmail in consortiumIndividual)
                                //{
                                //    var EmailType = consortiumEmail.Emails.Where(x => x.EmailTypeId == 23).ToList();

                                //    if (!EmailType.Any())
                                //    {
                                //        names.Add($"{consortiumPartner.PersonIdentificationNumber}-{ consortiumPartner.FullName}");
                                //    }
                                //}
                            }
                        }

                        if (!names.Any())
                        {
                            facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityConsortiumPartnerRiskList.ConceptId, entityConsortiumPartnerRiskList.EntityId), string.Join(",", names.Distinct()));
                            facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateConsortiatesElectronicBilling", ex);
            }
        }


        public void ValidateConsortiatesRiskList(Rules.Facade facade)
        {
            try
            {
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());
                int entityRiskId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_RISK).ToString());

                Func<SCREN.Concept, bool> entityPredicate = concept => concept != null && concept.ConceptName == "Rol a Validar" && concept.EntityId == entityId;
                SCREN.Concept entityRolToValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Listas de riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Tipo Lista de Riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityTypeRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Integrantes del consorcio" && concept.EntityId == entityId;
                SCREN.Concept entityConsortiumPartnerRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Beneficiarios en listas de riesgo" && concept.EntityId == entityRiskId;
                SCREN.Concept entityBeneficiariesRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "¿Validar Integrantes del consorcio?" && concept.EntityId == entityId;
                SCREN.Concept entityConsortiatesList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                bool consortium = facade.GetConcept<bool>(RuleConceptGeneral.DynamicConcept(entityConsortiatesList.ConceptId, entityConsortiatesList.EntityId));
                if (consortium == true)
                {
                    if (entityRolToValidate == null || entityRiskList == null || entityTypeRiskList == null)
                    { return; }

                    string documentNumber = string.Empty;
                    int documentType = 0;
                    string fullName = string.Empty;
                    int associationType = 0;
                    CompanyExtended associationTypeBeneficiary = new CompanyExtended();
                    int individual = 0;
                    List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                    int rolToValidate = facade.GetConcept<int>(RuleConceptGeneral.DynamicConcept(entityRolToValidate.ConceptId, entityRolToValidate.EntityId));

                    switch (rolToValidate)
                    {
                        case 1: //Tomador 
                            fullName = facade.GetConcept<string>(RuleConceptGeneral.NameHolder);
                            documentNumber = facade.GetConcept<string>(RuleConceptGeneral.DocumentoNumberHolder);
                            documentType = facade.GetConcept<int>(RuleConceptGeneral.TypeOfHolderDocument);
                            associationType = facade.GetConcept<int>(RuleConceptGeneral.AssociationType);
                            individual = facade.GetConcept<int>(RuleConceptGeneral.IndividualNumberHolder);
                            break;

                        case 2: //Asegurado
                            fullName = facade.GetConcept<string>(RuleConceptRisk.NameInsured);
                            documentNumber = facade.GetConcept<string>(RuleConceptRisk.InsuredDocumentNumber);
                            documentType = facade.GetConcept<int>(RuleConceptRisk.TypeOfInsuredDocument);
                            associationType = facade.GetConcept<int>(RuleConceptRisk.InsuredAssociationType);
                            individual = facade.GetConcept<int>(RuleConceptRisk.IndividualInsured);
                            break;

                        case 3: //Beneficiario
                            beneficiaries = facade.GetConcept<List<CompanyBeneficiary>>(RuleConceptRisk.Beneficiaries);
                            break;

                        case 4://Afianzado
                            fullName = facade.GetConcept<string>(RuleConceptRisk.InsuredNameOfTheBond);
                            documentNumber = facade.GetConcept<string>(RuleConceptRisk.InsuredDocumentNumberOfTheBond);
                            documentType = facade.GetConcept<int>(RuleConceptRisk.TypeOfTheBondDocument);
                            associationType = facade.GetConcept<int>(RuleConceptRisk.ContractorAssociationType);
                            individual = facade.GetConcept<int>(RuleConceptRisk.IndividualOfTheBond);
                            break;

                        default:
                            return;
                    }

                    Func<string, int, string, List<ListRiskMatchDTO>> delegateFunc = (document, typeDocument, name) =>
                    {
                        if (string.IsNullOrEmpty(document) || string.IsNullOrEmpty(name) || documentType == 0)
                        {
                            return new List<ListRiskMatchDTO>();
                        }

                        if (typeDocument == 2 && document.Length > 9)
                        {
                            document = document.Substring(0, 9);
                        }

                        int riskListType = facade.GetConcept<int>(RuleConceptGeneral.DynamicConcept(entityTypeRiskList.ConceptId));
                        return DelegateService.coreUniqueListRiskPersonService.ValidateListRiskPerson(document, name, riskListType);
                    };
                    if (rolToValidate != 3 && associationType != 1 && associationType > 0)
                    {
                        List<ConsorciatedDTO> consortiums = new List<ConsorciatedDTO>();
                        consortiums = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(individual);
                        if (consortiums != null && consortiums.Count > 0)
                        {
                            List<string> names = new List<string>();
                            List<string> listNames = new List<string>();
                            foreach (ConsorciatedDTO consortiumPartner in consortiums)
                            {
                                List<ListRiskMatchDTO> riskList = delegateFunc(consortiumPartner.PersonIdentificationNumber, (int)consortiumPartner.DocumentType, consortiumPartner.FullName);

                                if (riskList.Any())
                                {
                                    names.Add($"{consortiumPartner.PersonIdentificationNumber}-{ consortiumPartner.FullName}");
                                    listNames.AddRange(riskList.Select(x => x.listType));
                                }
                            }

                            if (names.Any() && listNames.Any())
                            {
                                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityRiskList.ConceptId, entityRiskList.EntityId), string.Join(",", listNames.Distinct()));
                                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityConsortiumPartnerRiskList.ConceptId, entityConsortiumPartnerRiskList.EntityId), string.Join(",", names.Distinct()));
                                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                            }
                        }
                    }
                    else
                    {
                        List<string> beneficiariesNames = new List<string>();
                        List<string> listRiskNames = new List<string>();
                        List<string> beneficiariesNamesInt = new List<string>();
                        List<ConsorciatedDTO> consortiumsBeneficiary = new List<ConsorciatedDTO>();

                        if (beneficiaries != null && beneficiaries.Count > 0)
                        {
                            foreach (CompanyBeneficiary beneficiary in beneficiaries)
                            {
                                documentNumber = beneficiary.IdentificationDocument.Number;
                                documentType = beneficiary.IdentificationDocument.DocumentType.Id;
                                fullName = beneficiary.Name;
                                individual = beneficiary.IndividualId;
                                if (beneficiary.CustomerType == ENUM.CustomerType.Individual)
                                {
                                    if (beneficiary.IndividualType == ENUM.IndividualType.Company)
                                    {
                                        if (beneficiary.AssociationType?.Id == 0 || beneficiary.AssociationType == null)
                                        {
                                            associationTypeBeneficiary = DelegateService.uniquePersonServiceCore.GetCoCompanyByIndividualId(individual);
                                            associationType = associationTypeBeneficiary.AssociationType.Id;
                                        }
                                        else
                                        {
                                            associationType = beneficiary.AssociationType.Id;
                                        }

                                        if (associationType != 1 && associationType > 0)
                                        {
                                            consortiumsBeneficiary = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(individual);
                                            if (consortiumsBeneficiary != null)
                                            {
                                                foreach (ConsorciatedDTO partnerBeneficiary in consortiumsBeneficiary)
                                                {
                                                    List<ListRiskMatchDTO> riskList = delegateFunc(partnerBeneficiary.PersonIdentificationNumber, (int)partnerBeneficiary.DocumentType, partnerBeneficiary.FullName);

                                                    if (riskList.Any())
                                                    {
                                                        beneficiariesNames.Add(fullName);
                                                        beneficiariesNamesInt.Add($"{partnerBeneficiary.CompanyIdentifationNumber}-{partnerBeneficiary.FullName}");
                                                        listRiskNames.AddRange(riskList.Select(x => x.listType));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (beneficiariesNames.Any() && listRiskNames.Any())
                            {
                                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityRiskList.ConceptId, entityRiskList.EntityId), string.Join(",", listRiskNames.Distinct()));
                                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityBeneficiariesRiskList.ConceptId, entityBeneficiariesRiskList.EntityId), string.Join(",", beneficiariesNames.Distinct()));
                                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityConsortiumPartnerRiskList.ConceptId, entityConsortiumPartnerRiskList.EntityId), string.Join(",", beneficiariesNamesInt.Distinct()));
                                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateConsortiumRiskList", ex);
            }
        }

        public void ValidateSarlaft(Rules.Facade facade)
        {
            try
            {
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());
                int entityRiskId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_RISK).ToString());

                Func<SCREN.Concept, bool> entityPredicate = concept => concept != null && concept.ConceptName == "Rol a Validar" && concept.EntityId == entityId;
                SCREN.Concept entityRolToValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Beneficiarios con Sarlaft" && concept.EntityId == entityRiskId;
                SCREN.Concept entityBeneficiariesSarlaft = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                int individualId = 0;
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                int rolToValidate = facade.GetConcept<int>(RuleConceptGeneral.DynamicConcept(entityRolToValidate.ConceptId, entityRolToValidate.EntityId));

                switch (rolToValidate)
                {
                    case 1: //Tomador 
                        individualId = facade.GetConcept<int>(RuleConceptGeneral.IndividualNumberHolder);
                        break;

                    case 2: //Asegurado
                        individualId = facade.GetConcept<int>(RuleConceptRisk.IndividualInsured);
                        break;

                    case 3: //Beneficiario
                        beneficiaries = facade.GetConcept<List<CompanyBeneficiary>>(RuleConceptRisk.Beneficiaries);
                        break;

                    case 4://Afianzado
                        individualId = facade.GetConcept<int>(RuleConceptRisk.IndividualOfTheBond);
                        break;

                    default:
                        return;
                }

                Func<int, bool> delegateFunc = idIndividual =>
                {
                    if (idIndividual == 0)
                    { return false; }

                    List<SAR.SarlaftExonerationtDTO> exoneration = DelegateService.SarlaftApplicationServices.GetSarlaftExoneration(idIndividual);
                    if (exoneration.Count > 0 && exoneration[0].IsExonerated)
                    { return false; }

                    List<SAR.SarlaftDTO> result = DelegateService.SarlaftApplicationServices.GetSarlaft(idIndividual);
                    if (!result.Any())
                    { return true; }

                    SAR.SarlaftDTO objSarlaft = result.OrderByDescending(x => x.FillingDate).FirstOrDefault();
                    if (objSarlaft == null)
                    { return false; }

                    DateTime fillingDate = (DateTime)objSarlaft.FillingDate;
                    if (DateTime.Now.Subtract(fillingDate).Days > 365)
                    { return true; }

                    Parameter parameter = DelegateService.commonService.GetParameterByDescription("Vencimiento Previo Sarlaft");
                    return DateTime.Now.Subtract(fillingDate).Days > 365 - parameter.NumberParameter || objSarlaft.PendingEvent;
                };

                if (rolToValidate != 3)
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, delegateFunc(individualId));
                }
                else
                {
                    List<string> beneficiariesNames = new List<string>();
                    foreach (CompanyBeneficiary beneficiary in beneficiaries)
                    {
                        if (delegateFunc(beneficiary.IndividualId))
                        {
                            beneficiariesNames.Add(beneficiary.Name);
                        }
                    }

                    if (beneficiariesNames.Any())
                    {
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityBeneficiariesSarlaft.ConceptId, entityBeneficiariesSarlaft.EntityId), string.Join(",", beneficiariesNames));
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateSarlaft", ex);
            }
        }


        public void ValidateConsortiatesSarlaft(Rules.Facade facade)
        {
            try
            {
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());
                int entityRiskId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_RISK).ToString());

                Func<SCREN.Concept, bool> entityPredicate = concept => concept != null && concept.ConceptName == "Rol a Validar" && concept.EntityId == entityId;
                SCREN.Concept entityRolToValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Beneficiarios con Sarlaft" && concept.EntityId == entityRiskId;
                SCREN.Concept entityBeneficiariesSarlaft = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Integrantes del consorcio" && concept.EntityId == entityId;
                SCREN.Concept entityConsortiumPartnerRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "¿Validar Integrantes del consorcio?" && concept.EntityId == entityId;
                SCREN.Concept entityConsortiatesList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                bool consortium = facade.GetConcept<bool>(RuleConceptGeneral.DynamicConcept(entityConsortiatesList.ConceptId, entityConsortiatesList.EntityId));
                if (consortium == true)
                {
                    int individualId = 0;
                    int associationType = 0;
                    CompanyExtended associationTypeBeneficiary = new CompanyExtended();
                    List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                    int rolToValidate = facade.GetConcept<int>(RuleConceptGeneral.DynamicConcept(entityRolToValidate.ConceptId, entityRolToValidate.EntityId));

                    switch (rolToValidate)
                    {
                        case 1: //Tomador 
                            individualId = facade.GetConcept<int>(RuleConceptGeneral.IndividualNumberHolder);
                            associationType = facade.GetConcept<int>(RuleConceptGeneral.AssociationType);
                            break;

                        case 2: //Asegurado
                            individualId = facade.GetConcept<int>(RuleConceptRisk.IndividualInsured);
                            associationType = facade.GetConcept<int>(RuleConceptRisk.InsuredAssociationType);
                            break;

                        case 3: //Beneficiario
                            beneficiaries = facade.GetConcept<List<CompanyBeneficiary>>(RuleConceptRisk.Beneficiaries);
                            break;

                        case 4://Afianzado
                            individualId = facade.GetConcept<int>(RuleConceptRisk.IndividualOfTheBond);
                            associationType = facade.GetConcept<int>(RuleConceptRisk.InsuredAssociationType);
                            break;

                        default:
                            return;
                    }

                    Func<int, bool> delegateFunc = idIndividual =>
                    {
                        if (idIndividual == 0)
                        { return false; }

                        List<SAR.SarlaftExonerationtDTO> exoneration = DelegateService.SarlaftApplicationServices.GetSarlaftExoneration(idIndividual);
                        if (exoneration.Count > 0 && exoneration[0].IsExonerated)
                        { return false; }

                        List<SAR.SarlaftDTO> result = DelegateService.SarlaftApplicationServices.GetSarlaft(idIndividual);
                        if (!result.Any())
                        { return true; }

                        SAR.SarlaftDTO objSarlaft = result.OrderByDescending(x => x.FillingDate).FirstOrDefault();
                        if (objSarlaft == null)
                        { return false; }

                        DateTime fillingDate = (DateTime)objSarlaft.FillingDate;
                        if (DateTime.Now.Subtract(fillingDate).Days > 365)
                        { return true; }

                        Parameter parameter = DelegateService.commonService.GetParameterByDescription("Vencimiento Previo Sarlaft");
                        return DateTime.Now.Subtract(fillingDate).Days > 365 - parameter.NumberParameter || objSarlaft.PendingEvent;

                    };

                    if (rolToValidate != 3 && associationType != 1 && associationType > 0)
                    {
                        List<string> names = new List<string>();
                        List<ConsorciatedDTO> consortiums = new List<ConsorciatedDTO>();
                        consortiums = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(individualId);
                        if (consortiums != null && consortiums.Count > 0)
                        {
                            TP.Parallel.ForEach(consortiums.AsParallel().Where(x => delegateFunc(x.IndividualId)), z =>
                                   names.Add($"{z.CompanyIdentifationNumber}-{z.FullName}")
                                 );

                            //foreach (ConsorciatedDTO partner in consortiums)
                            //{
                            //    if (delegateFunc(partner.IndividualId))
                            //    {
                            //        names.Add($"{partner.CompanyIdentifationNumber}-{partner.FullName}");
                            //    }
                            //}
                            if (names.Any())
                            {
                                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityConsortiumPartnerRiskList.ConceptId, entityConsortiumPartnerRiskList.EntityId), string.Join(",", names.Distinct()));
                                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                            }
                        }
                    }
                    else
                    {
                        List<string> beneficiariesNames = new List<string>();
                        List<string> beneficiariesNamesInt = new List<string>();
                        if (beneficiaries != null && beneficiaries.Count > 0)
                        {
                            foreach (CompanyBeneficiary beneficiary in beneficiaries)
                            {
                                if (beneficiary.CustomerType == ENUM.CustomerType.Individual)
                                {
                                    if (beneficiary.IndividualType == ENUM.IndividualType.Company)
                                    {
                                        if (beneficiary.AssociationType?.Id == 0 || beneficiary.AssociationType == null)
                                        {
                                            associationTypeBeneficiary = DelegateService.uniquePersonServiceCore.GetCoCompanyByIndividualId(beneficiary.IndividualId);
                                            associationType = associationTypeBeneficiary.AssociationType.Id;
                                        }
                                        else
                                        {
                                            associationType = beneficiary.AssociationType.Id;
                                        }

                                        if (associationType != 1 && associationType != 0)
                                        {
                                            List<ConsorciatedDTO> consortiums = new List<ConsorciatedDTO>();
                                            consortiums = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(beneficiary.IndividualId);
                                            if (consortiums != null && consortiums.Count > 0)
                                            {
                                                TP.Parallel.ForEach(consortiums.AsParallel().Where(x => delegateFunc(x.IndividualId)), z =>
                                                beneficiariesNamesInt.Add($"{z.CompanyIdentifationNumber}-{z.FullName}"));
                                                TP.Parallel.ForEach(consortiums.AsParallel().Where(x => delegateFunc(x.IndividualId)), z =>
                                                beneficiariesNames.Add(beneficiary.Name));


                                                //foreach (ConsorciatedDTO partner in consortiums)
                                                //{
                                                //    if (delegateFunc(partner.IndividualId))
                                                //    {
                                                //        beneficiariesNames.Add(beneficiary.Name);
                                                //        beneficiariesNamesInt.Add($"{partner.CompanyIdentifationNumber}-{partner.FullName}");
                                                //    }
                                                //}
                                            }
                                        }
                                    }
                                }
                            }
                            if (beneficiariesNames.Any() && beneficiariesNamesInt.Any())
                            {
                                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityConsortiumPartnerRiskList.ConceptId, entityConsortiumPartnerRiskList.EntityId), string.Join(",", beneficiariesNamesInt.Distinct()));
                                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityBeneficiariesSarlaft.ConceptId, entityBeneficiariesSarlaft.EntityId), string.Join(",", beneficiariesNames.Distinct()));
                                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                            }
                        }
                    }
                }
            }


            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateSarlaft", ex);
            }
        }

        /// <summary>
        /// Funcion de politicas que valida la cartera pendiente por poliza (EMISION)
        /// </summary>
        /// <param name="facade"></param>
        public void ValidatePendingPortfolioByPolicy(Rules.Facade facade)
        {
            int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());

            int branchId = facade.GetConcept<int>(CompanyRuleConceptGeneral.BranchCode);
            int prefixId = facade.GetConcept<int>(CompanyRuleConceptGeneral.PrefixCode);
            string documentNumber = facade.GetConcept<string>(CompanyRuleConceptGeneral.DocumentNumber);

            Func<SCREN.Concept, bool> entityPredicate = concept => concept != null && concept.ConceptName == "Días de Cartera a validar" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentDaysValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Valor de Cartera a validar" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentValueValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Valor de Cartera" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentValue = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Días de Cartera" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentDays = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);


            if (entityPaymentDaysValidate == null || entityPaymentValueValidate == null || branchId == 0 || prefixId == 0 || string.IsNullOrEmpty(documentNumber))
            { return; }

            int paymentDaysValidate = facade.GetConcept<int>(CompanyRuleConceptGeneral.DynamicConcept(entityPaymentDaysValidate.ConceptId, entityPaymentDaysValidate.EntityId));
            decimal paymentValueValidate = facade.GetConcept<decimal>(CompanyRuleConceptGeneral.DynamicConcept(entityPaymentValueValidate.ConceptId, entityPaymentValueValidate.EntityId));

            PortfolioPolicy portfolioPolicy = DelegateService.underwritingServiceCore.GetPortfolioPolicyByPolicy(branchId, prefixId, documentNumber);

            if (portfolioPolicy.PortfolioDays > paymentDaysValidate || portfolioPolicy.PendingValue > double.Parse(paymentValueValidate.ToString()))
            {
                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityPaymentValue.ConceptId, entityPaymentValue.EntityId), portfolioPolicy.PendingValue);
                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityPaymentDays.ConceptId, entityPaymentDays.EntityId), portfolioPolicy.PortfolioDays);
                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
            }
        }

        /// <summary>
        /// Funcion de politicas que valida el recaudo por poliza (EMISION)
        /// </summary>
        /// <param name="facade"></param>
        public void ValidateCollectedValuePortfolioByPolicy(Rules.Facade facade)
        {
            int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());

            int branchId = facade.GetConcept<int>(CompanyRuleConceptGeneral.BranchCode);
            int prefixId = facade.GetConcept<int>(CompanyRuleConceptGeneral.PrefixCode);
            string documentNumber = facade.GetConcept<string>(CompanyRuleConceptGeneral.DocumentNumber);

            Func<SCREN.Concept, bool> entityPredicate = concept => concept != null && concept.ConceptName == "Valor Recaudado a validar" && concept.EntityId == entityId;
            SCREN.Concept entityCollectedValueValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Valor Recaudado" && concept.EntityId == entityId;
            SCREN.Concept entityCollectedValue = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            if (entityCollectedValueValidate == null || entityCollectedValue == null || branchId == 0 || prefixId == 0 || string.IsNullOrEmpty(documentNumber))
            { return; }

            decimal? paymentValueValidate = facade.GetConcept<decimal?>(CompanyRuleConceptGeneral.DynamicConcept(entityCollectedValueValidate.ConceptId, entityCollectedValueValidate.EntityId));

            if (paymentValueValidate.HasValue)
            {
                PortfolioPolicy portfolioPolicy = DelegateService.underwritingServiceCore.GetPortfolioPolicyByPolicy(branchId, prefixId, documentNumber);
                if (portfolioPolicy.CollectedValue > double.Parse(paymentValueValidate.ToString()))
                {
                    facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityCollectedValue.ConceptId, entityCollectedValue.EntityId), portfolioPolicy.CollectedValue);
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }
            }
        }

        /// <summary>
        /// Funcion de politicas que valida la cartera pendiente para una persona por rol (EMISION)
        /// </summary>
        /// <param name="facade"></param>
        public void ValidatePendingPortfolioByRol(Rules.Facade facade)
        {
            int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());

            Func<SCREN.Concept, bool> entityPredicate = concept => concept != null && concept.ConceptName == "Rol a Validar" && concept.EntityId == entityId;
            SCREN.Concept entityRolToValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Días de Cartera a validar" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentDaysValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Valor de Cartera a validar" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentValueValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Valor de Cartera" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentValue = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Días de Cartera" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentDays = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "¿Validar Integrantes del consorcio?" && concept.EntityId == entityId;
            SCREN.Concept entityValidateAssociation = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Integrantes del consorcio" && concept.EntityId == entityId;
            SCREN.Concept entityAssociation = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            if (entityRolToValidate == null || entityPaymentDaysValidate == null || entityPaymentValueValidate == null || entityPaymentValue == null || entityPaymentDays == null || entityValidateAssociation == null || entityAssociation == null)
            { return; }

            string documentNumber = string.Empty;
            int documentType = 0;
            int individual = 0;
            int associationType = 0;
            string associationPeople = string.Empty;
            List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
            List<PortfolioPolicy> portfolioPolicies = new List<PortfolioPolicy>();

            int rolToValidate = facade.GetConcept<int>(RuleConceptGeneral.DynamicConcept(entityRolToValidate.ConceptId, entityRolToValidate.EntityId));
            int? paymentDaysValidate = facade.GetConcept<int?>(RuleConceptGeneral.DynamicConcept(entityPaymentDaysValidate.ConceptId, entityPaymentDaysValidate.EntityId));
            decimal? paymentValueValidate = facade.GetConcept<decimal?>(RuleConceptGeneral.DynamicConcept(entityPaymentValueValidate.ConceptId, entityPaymentValueValidate.EntityId));
            bool validateAssociation = facade.GetConcept<bool>(RuleConceptGeneral.DynamicConcept(entityValidateAssociation.ConceptId, entityValidateAssociation.EntityId));

            switch (rolToValidate)
            {
                case 1: //Tomador 
                    documentNumber = facade.GetConcept<string>(RuleConceptGeneral.DocumentoNumberHolder);
                    documentType = facade.GetConcept<int>(RuleConceptGeneral.TypeOfHolderDocument);
                    associationType = facade.GetConcept<int>(RuleConceptGeneral.AssociationType);
                    individual = facade.GetConcept<int>(RuleConceptGeneral.IndividualNumberHolder);
                    break;

                case 2: //Asegurado
                    documentNumber = facade.GetConcept<string>(RuleConceptRisk.InsuredDocumentNumber);
                    documentType = facade.GetConcept<int>(RuleConceptRisk.TypeOfInsuredDocument);
                    associationType = facade.GetConcept<int>(RuleConceptRisk.InsuredAssociationType);
                    individual = facade.GetConcept<int>(RuleConceptRisk.IndividualInsured);
                    break;

                case 3: //Beneficiario
                    beneficiaries = facade.GetConcept<List<CompanyBeneficiary>>(RuleConceptRisk.Beneficiaries);
                    break;

                case 4://Afianzado
                    documentNumber = facade.GetConcept<string>(RuleConceptRisk.InsuredDocumentNumberOfTheBond);
                    documentType = facade.GetConcept<int>(RuleConceptRisk.TypeOfTheBondDocument);
                    associationType = facade.GetConcept<int>(RuleConceptRisk.ContractorAssociationType);
                    individual = facade.GetConcept<int>(RuleConceptRisk.IndividualOfTheBond);
                    break;

                default:
                    return;
            }

            if (documentNumber == null || documentType == 0 || individual == 0)
            { return; }

            List<string> namesConsortiums = new List<string>();
            List<string> paymentValue = new List<string>();
            List<string> paymentDays = new List<string>();

            if (rolToValidate != 3)//Tomador, Asegurado o Afianzado
            {
                if (!validateAssociation) //Individual
                {
                    portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = documentNumber, DocumentType = new DocumentType { Id = documentType } });
                    portfolioPolicies = DelegateService.underwritingServiceCore.GetPortfolioPolicyByPerson(portfolioPolicies);

                    if ((paymentDaysValidate.HasValue && portfolioPolicies.First().PortfolioDays > paymentDaysValidate) || (paymentValueValidate.HasValue && portfolioPolicies.First().PendingValue > double.Parse(paymentValueValidate.ToString())))
                    {
                        paymentValue.Add(portfolioPolicies.First().PendingValue.ToString());
                        paymentDays.Add(portfolioPolicies.First().PortfolioDays.ToString());
                    }
                }
                else if (validateAssociation && associationType != 1 && associationType > 0) //Consorciados del Tomador, Asegurado o Afianzado
                {
                    List<ConsorciatedDTO> consortiums = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(individual);
                    if (consortiums != null)
                    {
                        consortiums.ForEach(x => portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = x.CompanyIdentifationNumber, DocumentType = new DocumentType { Id = x.DocumentType.Value } }));
                        //foreach (ConsorciatedDTO consortium in consortiums)
                        //{
                        //    portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = consortium.CompanyIdentifationNumber, DocumentType = new DocumentType { Id = consortium.DocumentType.Value } });
                        //}
                        portfolioPolicies = DelegateService.underwritingServiceCore.GetPortfolioPolicyByPerson(portfolioPolicies);

                        foreach (PortfolioPolicy portfolioPolicy in portfolioPolicies)
                        {
                            if ((paymentDaysValidate.HasValue && portfolioPolicy.PortfolioDays > paymentDaysValidate) || (paymentValueValidate.HasValue && portfolioPolicy.PendingValue > double.Parse(paymentValueValidate.ToString())))
                            {
                                ConsorciatedDTO consorciated = consortiums.Where(x => x.CompanyIdentifationNumber == portfolioPolicy.DocumentNumber || x.PersonIdentificationNumber == portfolioPolicy.DocumentNumber).FirstOrDefault();
                                if (consorciated != null)
                                {
                                    namesConsortiums.Add(consorciated.FullName);
                                    paymentValue.Add(portfolioPolicy.PendingValue.ToString());
                                    paymentDays.Add(portfolioPolicy.PortfolioDays.ToString());
                                }
                            }
                        }
                    }
                }
            }
            else if (rolToValidate == 3) //Beneficiarios
            {
                List<dynamic> keysConsociates = new List<dynamic>();

                foreach (CompanyBeneficiary beneficiary in beneficiaries)
                {
                    if (!validateAssociation) //Individual
                    {
                        portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = beneficiary.IdentificationDocument.Number, DocumentType = new DocumentType { Id = beneficiary.IdentificationDocument.DocumentType.Id } });
                    }
                    else if (validateAssociation && beneficiary.AssociationType.Id != 1 && beneficiary.AssociationType.Id > 0) //Consorciados de los beneficiarios
                    {
                        List<ConsorciatedDTO> consortiums = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(beneficiary.IndividualId);
                        if (consortiums != null)
                        {
                            foreach (ConsorciatedDTO consortium in consortiums)
                            {
                                keysConsociates.Add(new { Consortium = consortium, Beneficiary = beneficiary });
                                portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = consortium.CompanyIdentifationNumber, DocumentType = new DocumentType { Id = consortium.DocumentType.Value } });
                            }
                        }
                    }
                }

                portfolioPolicies = DelegateService.underwritingServiceCore.GetPortfolioPolicyByPerson(portfolioPolicies);

                if (!validateAssociation) //Individual
                {
                    foreach (PortfolioPolicy portfolioPolicy in portfolioPolicies)
                    {
                        if ((paymentDaysValidate.HasValue && portfolioPolicy.PortfolioDays > paymentDaysValidate) || (paymentValueValidate.HasValue && portfolioPolicy.PendingValue > double.Parse(paymentValueValidate.ToString())))
                        {
                            CompanyBeneficiary beneficiary = beneficiaries.Where(x => x.IdentificationDocument.Number == portfolioPolicy.DocumentNumber).FirstOrDefault();

                            paymentValue.Add(beneficiary.IdentificationDocument.Number + "-" + beneficiary.Name + ":" + portfolioPolicy.PendingValue);
                            paymentDays.Add(beneficiary.IdentificationDocument.Number + "-" + beneficiary.Name + ":" + portfolioPolicy.PortfolioDays);
                        }
                    }
                }
                else if (validateAssociation) //Consorciados
                {
                    foreach (PortfolioPolicy portfolioPolicy in portfolioPolicies)
                    {
                        if ((paymentDaysValidate.HasValue && portfolioPolicy.PortfolioDays > paymentDaysValidate) || (paymentValueValidate.HasValue && portfolioPolicy.PendingValue > double.Parse(paymentValueValidate.ToString())))
                        {
                            dynamic keyPairValue = keysConsociates.First(x => x.Consortium.CompanyIdentifationNumber == portfolioPolicy.DocumentNumber && x.Consortium.DocumentType.Value == portfolioPolicy.DocumentType.Id);

                            CompanyBeneficiary beneficiary = keyPairValue.Beneficiary;
                            ConsorciatedDTO consortium = keyPairValue.Consortium;

                            paymentValue.Add($"{consortium.CompanyIdentifationNumber}-{consortium.FullName}({beneficiary.Name}):{portfolioPolicy.PendingValue}");
                            paymentDays.Add($"{consortium.CompanyIdentifationNumber}-{consortium.FullName}({beneficiary.Name}):{portfolioPolicy.PortfolioDays}");
                        }
                    }
                }
            }

            if (paymentValue.Any() && paymentDays.Any())
            {
                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityPaymentValue.ConceptId, entityPaymentValue.EntityId), string.Join(", ", paymentValue));
                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityPaymentDays.ConceptId, entityPaymentDays.EntityId), string.Join(", ", paymentDays));
                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityAssociation.ConceptId, entityAssociation.EntityId), string.Join(", ", namesConsortiums));
                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
            }
        }


        /// <summary>
        /// Funcion de politicas que valida la cartera pendiente para una persona (SINIESTROS)
        /// </summary>
        /// <param name="facade"></param>
        public void ValidateClaimIndividualWallet(Rules.Facade facade)
        {
            int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_ESTIMATION).ToString());

            Func<SCREN.Concept, bool> entityPredicate = concept => concept != null && concept.ConceptName == "Dias de Cartera a validar" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentDaysValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Valor de Cartera a validar" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentValueValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Valor de Cartera" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentValue = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Dias de Cartera" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentDays = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "¿Validar Integrantes del consorcio?" && concept.EntityId == entityId;
            SCREN.Concept entityValidateAssociation = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Integrantes del consorcio" && concept.EntityId == entityId;
            SCREN.Concept entityAssociation = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            if (entityPaymentDaysValidate == null || entityPaymentValueValidate == null || entityPaymentValue == null || entityPaymentDays == null || entityValidateAssociation == null || entityAssociation == null)
            { return; }

            List<PortfolioPolicy> portfolioPolicies = new List<PortfolioPolicy>();

            int? paymentDaysValidate = facade.GetConcept<int?>(RuleConceptEstimation.DynamicConcept(entityPaymentDaysValidate.ConceptId, entityPaymentDaysValidate.EntityId));
            decimal? paymentValueValidate = facade.GetConcept<decimal?>(RuleConceptEstimation.DynamicConcept(entityPaymentValueValidate.ConceptId, entityPaymentValueValidate.EntityId));
            bool validateAssociation = facade.GetConcept<bool>(RuleConceptEstimation.DynamicConcept(entityValidateAssociation.ConceptId, entityValidateAssociation.EntityId));

            string documentNumber = facade.GetConcept<string>(RuleConceptEstimation.AffectedDocumentNumber);
            int documentType = facade.GetConcept<int>(RuleConceptEstimation.AffectedDocumentType);
            int individual = facade.GetConcept<int>(RuleConceptEstimation.AffectedId);


            if (documentNumber == null || documentType == 0 || individual == 0)
            { return; }

            List<string> namesConsortiums = new List<string>();
            List<string> paymentValue = new List<string>();
            List<string> paymentDays = new List<string>();

            if (!validateAssociation)
            {
                portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = documentNumber, DocumentType = new DocumentType { Id = documentType } });
                portfolioPolicies = DelegateService.underwritingServiceCore.GetPortfolioPolicyByPerson(portfolioPolicies);

                if ((paymentDaysValidate.HasValue && portfolioPolicies.First().PortfolioDays > paymentDaysValidate) || (paymentValueValidate.HasValue && portfolioPolicies.First().PendingValue > double.Parse(paymentValueValidate.ToString())))
                {
                    paymentValue.Add(portfolioPolicies.First().PendingValue.ToString());
                    paymentDays.Add(portfolioPolicies.First().PortfolioDays.ToString());
                }
            }
            else
            {
                List<ConsorciatedDTO> consortiums = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(individual);
                if (consortiums != null)
                {

                    TP.Parallel.ForEach(consortiums.AsParallel(), z =>
                        portfolioPolicies.Add(new PortfolioPolicy()
                        {
                            DocumentNumber = z.CompanyIdentifationNumber,
                            DocumentType = new DocumentType { Id = z.DocumentType.Value }
                        }));

                    //foreach (ConsorciatedDTO consortium in consortiums)
                    //{
                    //    portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = consortium.CompanyIdentifationNumber, DocumentType = new DocumentType { Id = consortium.DocumentType.Value } });
                    //}

                    portfolioPolicies = DelegateService.underwritingServiceCore.GetPortfolioPolicyByPerson(portfolioPolicies);

                    foreach (PortfolioPolicy portfolioPolicy in portfolioPolicies)
                    {
                        if ((paymentDaysValidate.HasValue && portfolioPolicy.PortfolioDays > paymentDaysValidate) || (paymentValueValidate.HasValue && portfolioPolicy.PendingValue > double.Parse(paymentValueValidate.ToString())))
                        {
                            ConsorciatedDTO consorciated = consortiums.Where(x => x.CompanyIdentifationNumber == portfolioPolicy.DocumentNumber || x.PersonIdentificationNumber == portfolioPolicy.DocumentNumber).FirstOrDefault();
                            if (consorciated != null)
                            {
                                namesConsortiums.Add(consorciated.FullName);
                                paymentValue.Add(portfolioPolicy.PendingValue.ToString());
                                paymentDays.Add(portfolioPolicy.PortfolioDays.ToString());
                            }
                        }
                    }
                }
            }

            if (paymentValue.Any() && paymentDays.Any())
            {
                facade.SetConcept(RuleConceptEstimation.DynamicConcept(entityPaymentValue.ConceptId, entityPaymentValue.EntityId), string.Join(", ", paymentValue));
                facade.SetConcept(RuleConceptEstimation.DynamicConcept(entityPaymentDays.ConceptId, entityPaymentDays.EntityId), string.Join(", ", paymentDays));
                facade.SetConcept(RuleConceptEstimation.DynamicConcept(entityAssociation.ConceptId, entityAssociation.EntityId), string.Join(", ", namesConsortiums));
                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
            }
        }


        /// <summary>
        /// Funcion de politicas que valida la cartera pendiente para una persona (SINIESTROS - SOLICUTUD DE PAGO)
        /// </summary>
        /// <param name="facade"></param>
        public void ValidatePaymentIndividualWallet(Rules.Facade facade)
        {
            int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_PAYMENT_REQUEST).ToString());

            Func<SCREN.Concept, bool> entityPredicate = concept => concept != null && concept.ConceptName == "Días de Cartera a validar" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentDaysValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Valor de Cartera a validar" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentValueValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Valor de Cartera" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentValue = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Días de Cartera" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentDays = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "¿Validar Integrantes del consorcio?" && concept.EntityId == entityId;
            SCREN.Concept entityValidateAssociation = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Integrantes del consorcio" && concept.EntityId == entityId;
            SCREN.Concept entityAssociation = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            if (entityPaymentDaysValidate == null || entityPaymentValueValidate == null || entityPaymentValue == null || entityPaymentDays == null || entityValidateAssociation == null || entityAssociation == null)
            { return; }

            List<PortfolioPolicy> portfolioPolicies = new List<PortfolioPolicy>();

            int? paymentDaysValidate = facade.GetConcept<int?>(RuleConceptPaymentRequest.DynamicConcept(entityPaymentDaysValidate.ConceptId, entityPaymentDaysValidate.EntityId));
            decimal? paymentValueValidate = facade.GetConcept<decimal?>(RuleConceptPaymentRequest.DynamicConcept(entityPaymentValueValidate.ConceptId, entityPaymentValueValidate.EntityId));
            bool validateAssociation = facade.GetConcept<bool>(RuleConceptPaymentRequest.DynamicConcept(entityValidateAssociation.ConceptId, entityValidateAssociation.EntityId));

            string documentNumber = facade.GetConcept<string>(RuleConceptPaymentRequest.PaymentIndividualDocumentNumber);
            int documentType = facade.GetConcept<int>(RuleConceptPaymentRequest.PaymentIndividualDocumentType);
            int individual = facade.GetConcept<int>(RuleConceptPaymentRequest.PaymentIndividualId);


            if (documentNumber == null || documentType == 0 || individual == 0)
            { return; }

            List<string> namesConsortiums = new List<string>();
            List<string> paymentValue = new List<string>();
            List<string> paymentDays = new List<string>();

            if (!validateAssociation)
            {
                portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = documentNumber, DocumentType = new DocumentType { Id = documentType } });
                portfolioPolicies = DelegateService.underwritingServiceCore.GetPortfolioPolicyByPerson(portfolioPolicies);

                if ((paymentDaysValidate.HasValue && portfolioPolicies.First().PortfolioDays > paymentDaysValidate) || (paymentValueValidate.HasValue && portfolioPolicies.First().PendingValue > double.Parse(paymentValueValidate.ToString())))
                {
                    paymentValue.Add(portfolioPolicies.First().PendingValue.ToString());
                    paymentDays.Add(portfolioPolicies.First().PortfolioDays.ToString());
                }
            }
            else
            {
                List<ConsorciatedDTO> consortiums = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(individual);
                if (consortiums != null)
                {
                    DocumentType DocumentType = new DocumentType();
                    consortiums.ForEach(x => portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = x.CompanyIdentifationNumber, DocumentType = new DocumentType { Id = x.DocumentType.Value } }));
                    //foreach (ConsorciatedDTO consortium in consortiums)
                    //{
                    //    portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = consortium.CompanyIdentifationNumber, DocumentType = new DocumentType { Id = consortium.DocumentType.Value } });
                    //}

                    portfolioPolicies = DelegateService.underwritingServiceCore.GetPortfolioPolicyByPerson(portfolioPolicies);

                    //portfolioPolicies.Select(x => paymentDaysValidate.HasValue && x.PortfolioDays > paymentDaysValidate || (paymentValueValidate.HasValue && x.PendingValue > double.Parse(paymentValueValidate.ToString())));
                    //ConsorciatedDTO consorciated = consortiums.Where(x => x.CompanyIdentifationNumber == portfolioPolicy.DocumentNumber || x.PersonIdentificationNumber == portfolioPolicy.DocumentNumber).FirstOrDefault();
                    //if (consorciated != null)
                    //{
                    //    namesConsortiums.Add(consorciated.FullName);
                    //    paymentValue.Add(portfolioPolicies.Select(x => x.PendingValue).ToString());
                    //    paymentDays.Add(portfolioPolicies.Select(x => x.PortfolioDays).ToString());
                    //}
                    foreach (PortfolioPolicy portfolioPolicy in portfolioPolicies)
                    {
                        if ((paymentDaysValidate.HasValue && portfolioPolicy.PortfolioDays > paymentDaysValidate) || (paymentValueValidate.HasValue && portfolioPolicy.PendingValue > double.Parse(paymentValueValidate.ToString())))
                        {
                            ConsorciatedDTO consorciated = consortiums.Where(x => x.CompanyIdentifationNumber == portfolioPolicy.DocumentNumber || x.PersonIdentificationNumber == portfolioPolicy.DocumentNumber).FirstOrDefault();
                            if (consorciated != null)
                            {
                                namesConsortiums.Add(consorciated.FullName);
                                paymentValue.Add(portfolioPolicy.PendingValue.ToString());
                                paymentDays.Add(portfolioPolicy.PortfolioDays.ToString());
                            }
                        }
                    }
                }
            }

            if (paymentValue.Any() && paymentDays.Any())
            {
                facade.SetConcept(RuleConceptPaymentRequest.DynamicConcept(entityPaymentValue.ConceptId, entityPaymentValue.EntityId), string.Join(", ", paymentValue));
                facade.SetConcept(RuleConceptPaymentRequest.DynamicConcept(entityPaymentDays.ConceptId, entityPaymentDays.EntityId), string.Join(", ", paymentDays));
                facade.SetConcept(RuleConceptPaymentRequest.DynamicConcept(entityAssociation.ConceptId, entityAssociation.EntityId), string.Join(", ", namesConsortiums));
                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
            }
        }


        /// <summary>
        /// Funcion de politicas que valida la cartera pendiente para una persona (SINIESTROS - SOLICUTUD DE COBRO)
        /// </summary>
        /// <param name="facade"></param>
        public void ValidateChargeIndividualWallet(Rules.Facade facade)
        {
            int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_CHARGE_REQUEST).ToString());

            Func<SCREN.Concept, bool> entityPredicate = concept => concept != null && concept.ConceptName == "Días de Cartera a validar" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentDaysValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Valor de Cartera a validar" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentValueValidate = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Valor de Cartera" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentValue = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Días de Cartera" && concept.EntityId == entityId;
            SCREN.Concept entityPaymentDays = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "¿Validar Integrantes del consorcio?" && concept.EntityId == entityId;
            SCREN.Concept entityValidateAssociation = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Integrantes del consorcio" && concept.EntityId == entityId;
            SCREN.Concept entityAssociation = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            if (entityPaymentDaysValidate == null || entityPaymentValueValidate == null || entityPaymentValue == null || entityPaymentDays == null || entityValidateAssociation == null || entityAssociation == null)
            { return; }

            List<PortfolioPolicy> portfolioPolicies = new List<PortfolioPolicy>();

            int? paymentDaysValidate = facade.GetConcept<int?>(RuleConceptChargeRequest.DynamicConcept(entityPaymentDaysValidate.ConceptId, entityPaymentDaysValidate.EntityId));
            decimal? paymentValueValidate = facade.GetConcept<decimal?>(RuleConceptChargeRequest.DynamicConcept(entityPaymentValueValidate.ConceptId, entityPaymentValueValidate.EntityId));
            bool validateAssociation = facade.GetConcept<bool>(RuleConceptChargeRequest.DynamicConcept(entityValidateAssociation.ConceptId, entityValidateAssociation.EntityId));

            string documentNumber = facade.GetConcept<string>(RuleConceptChargeRequest.ChargeIndividualDocumentNumber);
            int documentType = facade.GetConcept<int>(RuleConceptChargeRequest.ChargeIndividualDocumentType);
            int individual = facade.GetConcept<int>(RuleConceptChargeRequest.ChargeIndividualId);


            if (documentNumber == null || documentType == 0 || individual == 0)
            { return; }

            List<string> namesConsortiums = new List<string>();
            List<string> paymentValue = new List<string>();
            List<string> paymentDays = new List<string>();

            if (!validateAssociation)
            {
                portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = documentNumber, DocumentType = new DocumentType { Id = documentType } });
                portfolioPolicies = DelegateService.underwritingServiceCore.GetPortfolioPolicyByPerson(portfolioPolicies);

                if ((paymentDaysValidate.HasValue && portfolioPolicies.First().PortfolioDays > paymentDaysValidate) || (paymentValueValidate.HasValue && portfolioPolicies.First().PendingValue > double.Parse(paymentValueValidate.ToString())))
                {
                    paymentValue.Add(portfolioPolicies.First().PendingValue.ToString());
                    paymentDays.Add(portfolioPolicies.First().PortfolioDays.ToString());
                }
            }
            else
            {
                List<ConsorciatedDTO> consortiums = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(individual);
                if (consortiums != null)
                {
                    consortiums.ForEach(x => portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = x.CompanyIdentifationNumber, DocumentType = new DocumentType { Id = x.DocumentType.Value } }));

                    //foreach (ConsorciatedDTO consortium in consortiums)
                    //{
                    //    portfolioPolicies.Add(new PortfolioPolicy() { DocumentNumber = consortium.CompanyIdentifationNumber, DocumentType = new DocumentType { Id = consortium.DocumentType.Value } });
                    //}

                    portfolioPolicies = DelegateService.underwritingServiceCore.GetPortfolioPolicyByPerson(portfolioPolicies);

                    foreach (PortfolioPolicy portfolioPolicy in portfolioPolicies)
                    {
                        if ((paymentDaysValidate.HasValue && portfolioPolicy.PortfolioDays > paymentDaysValidate) || (paymentValueValidate.HasValue && portfolioPolicy.PendingValue > double.Parse(paymentValueValidate.ToString())))
                        {
                            ConsorciatedDTO consorciated = consortiums.Where(x => x.CompanyIdentifationNumber == portfolioPolicy.DocumentNumber || x.PersonIdentificationNumber == portfolioPolicy.DocumentNumber).FirstOrDefault();
                            if (consorciated != null)
                            {
                                namesConsortiums.Add(consorciated.FullName);
                                paymentValue.Add(portfolioPolicy.PendingValue.ToString());
                                paymentDays.Add(portfolioPolicy.PortfolioDays.ToString());
                            }
                        }
                    }
                }
            }

            if (paymentValue.Any() && paymentDays.Any())
            {
                facade.SetConcept(RuleConceptChargeRequest.DynamicConcept(entityPaymentValue.ConceptId, entityPaymentValue.EntityId), string.Join(", ", paymentValue));
                facade.SetConcept(RuleConceptChargeRequest.DynamicConcept(entityPaymentDays.ConceptId, entityPaymentDays.EntityId), string.Join(", ", paymentDays));
                facade.SetConcept(RuleConceptChargeRequest.DynamicConcept(entityAssociation.ConceptId, entityAssociation.EntityId), string.Join(", ", namesConsortiums));
                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
            }
        }
    }
}
