using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using Rules = Sistran.Core.Framework.Rules;
using PER = Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using ENTQUO = Sistran.Company.Application.Quotation.Entities;
using SCREN = Sistran.Core.Application.Script.Entities;

namespace Sistran.Company.Application.Sureties.SuretyServices.EEProvider
{
    using Sistran.Core.Application.ClaimServices.DTOs.Claims;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.Utilities.Cache;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Helper;
    using Sistran.Core.Application.Utilities.Utility;
    using TP = Sistran.Core.Application.Utilities.Utility;
    using System.Collections.Concurrent;
    using Utilities.RulesEngine;
    using Sistran.Company.Application.Sureties.SuretyServices.Models;

    public class SuretyRuleEngineCompatibilityServiceEEProvider
    {
        /// <summary>
        /// Validar Cupo Operativo
        /// </summary>
        /// <param name="facade"></param>
        public void ValidateOperationQuota(Rules.Facade facade)
        {
            TemporalType temporaltype = facade.GetConcept<TemporalType>(RuleConceptGeneral.TemporalTypeCode);

            if (temporaltype != TemporalType.Quotation)
            {
                bool isConsortium = facade.GetConcept<bool>(RuleConceptRisk.IsConsortium);
                int prefixId = facade.GetConcept<int>(RuleConceptGeneral.PrefixCode);
                int contractorId = facade.GetConcept<int>(RuleConceptRisk.ContractorId);
                int currencyId = facade.GetConcept<int>(RuleConceptGeneral.CurrencyCode);

                if (!isConsortium)
                {
                    List<CompanyCoverage> coverages = facade.GetConcept<List<CompanyCoverage>>(RuleConceptRisk.Coverages);

                    if (coverages != null)
                    {
                        decimal sumSubLimitAmount = coverages.Sum(x => x.EndorsementSublimitAmount);
                        decimal operatingPile = facade.GetConcept<decimal>(RuleConceptRisk.OperatingPile);

                        PrimaryKey primaryKey = UPEN.OperatingQuota.CreatePrimaryKey(contractorId, prefixId, currencyId);

                        UPEN.OperatingQuota entityOperatingQuota = (UPEN.OperatingQuota)DataFacadeManager.GetObject(primaryKey);

                        decimal limitPileAmount = operatingPile + sumSubLimitAmount;

                        if (entityOperatingQuota != null && (entityOperatingQuota.OperatingQuotaAmount < limitPileAmount || entityOperatingQuota.CurrentTo < DateTime.Now))
                        {
                            facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Validar Cupo Operativo Excedido
        /// politica: CUPO EXCEDIDO INTEGRANTES ASOCIACIÓN
        /// </summary>
        /// <param name="facade"></param>
        public void ValidateOperationQuotaExceeded(Rules.Facade facade)
        {
            int temporaltype = facade.GetConcept<int>(RuleConceptGeneral.TemporalTypeCode);
            if (temporaltype != (int)TemporalType.Quotation)
            {
                bool isConsortium = facade.GetConcept<bool>(RuleConceptRisk.IsConsortium);
                int prefixId = facade.GetConcept<int>(RuleConceptGeneral.PrefixCode);
                int contractorId = facade.GetConcept<int>(RuleConceptRisk.ContractorId);
                int currencyId = facade.GetConcept<int>(RuleConceptGeneral.CurrencyCode);
                decimal pileAmount = facade.GetConcept<decimal>(RuleConceptRisk.OperatingPile);

                if (isConsortium)
                {
                    Func<SCREN.Concept, bool> entityPredicate;
                    int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL_PERSON).ToString());

                    entityPredicate = concept => concept != null && concept.ConceptName == "Asociados con cupo excedido" && concept.EntityId == entityId;
                    SCREN.Concept entityDocumentConcept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                    int insuredId = facade.GetConcept<int>(RuleConceptRisk.ContractorInsuredCode);
                    List<Consortium> consortiums = DelegateService.uniquePersonService.GetConsortiumByInsurendId(insuredId);
                    List<string> consortiumsName = new List<string>();

                    if (consortiums != null)
                    {
                        List<CompanyCoverage> coverages = facade.GetConcept<List<CompanyCoverage>>(RuleConceptRisk.Coverages);
                        if (coverages != null)
                        {
                            decimal sumSubLimitAmount = coverages.Sum(x => x.EndorsementSublimitAmount);
                            SuretyServiceEEProvider suretyServiceEEProvider = new SuretyServiceEEProvider();
                            TP.Parallel.For(0, consortiums.Count, x =>
                            {
                                Consortium consortium = consortiums.ElementAt(x);
                                List<Amount> amounts = TP.Task.Run(() => suretyServiceEEProvider.GetAvailableAmountByIndividualId(consortium.IndividualId, prefixId, DateTime.Now)).GetAwaiter().GetResult();
                                if (amounts[0].Value == 0)
                                {
                                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                                    consortiumsName.Add(consortium.FullName);
                                }
                                else
                                {
                                    decimal available = amounts[0].Value - amounts[1].Value;
                                    decimal percentageAmount = (sumSubLimitAmount * consortium.ParticipationRate) / 100;

                                    if (percentageAmount > available)
                                    {
                                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                                        consortiumsName.Add(consortium.FullName);
                                    }
                                }
                            });
                            facade.SetConcept(CompanyRuleConceptGeneral.DynamicConcept(entityDocumentConcept.ConceptId), string.Join(", ", consortiumsName.ToList()));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validar Cobertura postcontractual        
        /// </summary>
        /// <param name="facade"></param>
        public void ValidateCoveragePostContractual(Rules.Facade facade)
        {
            List<CompanyCoverage> coverages = facade.GetConcept<List<CompanyCoverage>>(RuleConceptRisk.Coverages);
            if (coverages != null)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ENTQUO.CiaCoverage.Properties.IsPost, typeof(PER.Insured).Name);
                filter.Equal();
                filter.Constant(1);

                var result = DataFacadeManager.GetObjects(typeof(ENTQUO.CiaCoverage), filter.GetPredicate()).Select(m => (ENTQUO.CiaCoverage)m);
                var res = coverages.Where(n => result.Where(m => m.CoverageId == n.Id).Count() > 0).Count() > 0;
                if (res && facade.GetConcept<DateTime>(CompanyRuleConceptRisk.ContractDate) == null)
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
            }
        }
    }
}