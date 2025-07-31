using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.Sureties.SuretyServices.EEProvider
{
    public class SuretiesRuleEngineCompatibilityServiceEEProvider
    {
        /// <summary>
        /// Validar Cupo Operativo
        /// </summary>
        /// <param name="facade"></param>
        public void ValidateOperationQuota(Rules.Facade facade)
        {
            bool eventresult = false;
            int temporaltype = facade.GetConcept<int>(RuleConceptGeneral.TemporalTypeCode);
            if (temporaltype != (int)TemporalType.Quotation)
            {
                bool isconsortium = facade.GetConcept<bool>(RuleConceptRisk.IsConsortium);
                int lineBusinessSurety = facade.GetConcept<int>(RuleConceptGeneral.PrefixCode);
                DateTime issueDate = facade.GetConcept<DateTime>(RuleConceptGeneral.IssueDate);

                // Si es consorcio
                if (isconsortium)
                {
                    #region Si es consorcio
                    int contractorinsuredcode = facade.GetConcept<int>(RuleConceptRisk.ContractorInsuredCode);
                    List<Consortium> consortiums = DelegateService.uniquePersonServiceCore.GetConsortiumByInsurendId(contractorinsuredcode);
                    if (consortiums != null)
                    {
                        foreach (Consortium c in consortiums)
                        {
                            if (c != null)
                            {
                                List<Amount> amounts = DelegateService.uniquePersonServiceCore.GetAvailableAmountByIndividualId(c.IndividualId, lineBusinessSurety, issueDate);

                                if (amounts != null)
                                {
                                    if (amounts[0].Value == 0)
                                    {
                                        eventresult = true;
                                    }
                                }
                                else
                                {
                                    eventresult = true;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Si no es consorcio
                    int individualId = facade.GetConcept<int>(RuleConceptRisk.IndividualId);
                    decimal operatingPile = facade.GetConcept<decimal>(RuleConceptRisk.OperatingPile);

                    List<Amount> amounts = DelegateService.uniquePersonServiceCore.GetAvailableAmountByIndividualId(individualId, lineBusinessSurety, issueDate);

                    List<Coverage> coverages = facade.GetConcept<List<Coverage>>(RuleConceptRisk.Coverages);

                    decimal sumSubLimitAmount = coverages.Sum(x => x.EndorsementSublimitAmount);

                    decimal sumTotal = sumSubLimitAmount + operatingPile;

                    if (amounts != null)
                    {
                        if (amounts[0].Value == 0)
                        {
                            eventresult = true;
                        }
                        else
                        {
                            if (sumTotal > amounts[0].Value)
                            {
                                eventresult = true;
                            }
                        }
                    }
                    else
                    {
                        eventresult = true;
                    }
                    #endregion
                }

                //Se valida si se debe ejecutar o no el evento
                if (eventresult)
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, eventresult);
                }
            }
        }
    }
}
