using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.SuretyReversionService.EEProvider.DAOs;
using Sistran.Company.Application.SuretyReversionService.EEProvider.Resources;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVSE = Sistran.Company.Application.Sureties.SuretyServices.EEProvider;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Core.Integration.OperationQuotaServices.Enums;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Concurrent;

namespace Sistran.Company.Application.SuretyReversionService.EEProvider.Business
{
    public class SuretyReversionBusinessCia
    {
        BaseBusinessCia provider;

        public SuretyReversionBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Crea un nuevo temporal
        /// </summary>
        /// <param name="reversionViewModel"></param>
        /// <returns> Identificador del temporal creado </returns>
        public CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement, bool clearPolicies)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErroEndorsementNotFound);
            }
            CompanyPolicy companyPolicy = new CompanyPolicy();

            if (companyEndorsement.TemporalId != 0)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
            }
            else
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
            }

            if (companyPolicy != null)
            {
                companyPolicy.Endorsement = new CompanyEndorsement
                {
                    Id = companyEndorsement.Id,
                    PolicyId = companyEndorsement.PolicyId,
                    EndorsementReasonId = companyEndorsement.EndorsementReasonId,
                    EndorsementType = EndorsementType.LastEndorsementCancellation,
                    Text = new CompanyText
                    {
                        TextBody = companyEndorsement.Text.TextBody,
                        Observations = companyEndorsement.Text.Observations
                    }
                };

                companyPolicy.Id = companyEndorsement.TemporalId;
                companyPolicy.TemporalType = TemporalType.Endorsement;
                companyPolicy.TemporalTypeDescription = Errors.ResourceManager.GetString(EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement));
                companyPolicy.UserId = companyEndorsement.UserId;
                companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);
                companyPolicy.TicketNumber = companyEndorsement.TicketNumber;
                companyPolicy.TicketDate = companyEndorsement.TicketDate;
                companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                companyPolicy.IssueDate = Convert.ToDateTime(companyPolicy.IssueDate.ToString("dd-MM-yyyy"));
                companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                ReversionDAOCia reversionDAOCia = new ReversionDAOCia();
                CompanyPolicy company = new CompanyPolicy();
                companyPolicy.InfringementPolicies = !clearPolicies ? reversionDAOCia.ValidateAuthorizationPolicies(companyPolicy) : new List<PoliciesAut>();

                if (companyPolicy.InfringementPolicies == null || companyPolicy.InfringementPolicies.Count == 0)
                {
                    company = reversionDAOCia.CreateEndorsementReversion(companyPolicy, clearPolicies);
                    List<CompanyEndorsement> listEndorsements = DelegateService.underwritingService.GetCiaEndorsementsByFilterPolicy(companyPolicy.Branch.Id, companyPolicy.Prefix.Id, companyPolicy.DocumentNumber, false);

                    //Endoso que se esta reversando
                    CompanyEndorsement endorsementToReverse = listEndorsements.First(x => x.Id == companyEndorsement.Id);
                    List<CompanyContract> companyContracts = DelegateService.suretyService.GetCompanySuretyByEndorsementId(endorsementToReverse.Id);
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(endorsementToReverse.Id);
                    companyPolicy.Endorsement = listEndorsements.First(x => x.Number == endorsementToReverse.Number + 1);
                    companyContracts.ForEachParallel(x => x.Risk.Policy = companyPolicy);

                    //Se valida si es un endoso Cambio de* y se busca su respectiva cancelacion/reversion
                    EndorsementType? endorsementTypeToReverse = endorsementToReverse.EndorsementType;
                    if (endorsementTypeToReverse == EndorsementType.ChangeAgentEndorsement ||
                        endorsementTypeToReverse == EndorsementType.ChangeCoinsuranceEndorsement ||
                        endorsementTypeToReverse == EndorsementType.ChangeConsolidationEndorsement ||
                        endorsementTypeToReverse == EndorsementType.ChangePolicyHolderEndorsement ||
                        endorsementTypeToReverse == EndorsementType.ChangeTermEndorsement)
                    {
                        CompanyEndorsement cancelationEndorsement = listEndorsements.First(x => x.Number == endorsementToReverse.Number - 1);
                        List<CompanyContract> companyContractsTmp = DelegateService.suretyService.GetCompanySuretyByEndorsementId(cancelationEndorsement.Id);
                        var companyPolicyTmp = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(cancelationEndorsement.Id);
                        companyPolicyTmp.Endorsement = listEndorsements.First(x => x.Number == endorsementToReverse.Number + 2);
                        companyContractsTmp.ForEachParallel(x => x.Risk.Policy = companyPolicyTmp);

                        companyContracts.AddRange(companyContractsTmp);
                    }


                    if (companyContracts != null)
                    {
                        object lockObject = new object();

                        Parallel.For(0, companyContracts.Count, ParallelHelper.DebugParallelFor(), id =>
                        {
                            try
                            {
                                var suretyRisk = companyContracts[id];
                                var companyPolicyTmp = suretyRisk.Risk.Policy;

                                var operatingQuotaEvents = new List<OperatingQuotaEventDTO>();

                                foreach (CompanyCoverage item in companyContracts[id].Risk.Coverages)
                                {
                                    OperatingQuotaEventDTO operatingQuotaEvent = new OperatingQuotaEventDTO();

                                    operatingQuotaEvent.ApplyEndorsement = new ApplyEndorsementDTO();
                                    operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLI_LAST_CANCELLATION_ENDORSEMENT;
                                    operatingQuotaEvent.IssueDate = companyPolicyTmp.IssueDate;
                                    operatingQuotaEvent.IdentificationId = suretyRisk.Contractor.IndividualId;
                                    operatingQuotaEvent.LineBusinessID = companyPolicyTmp.Prefix.Id;
                                    operatingQuotaEvent.Policy_Init_Date = companyPolicyTmp.CurrentFrom;
                                    operatingQuotaEvent.Policy_End_Date = companyPolicyTmp.CurrentTo;
                                    operatingQuotaEvent.Cov_Init_Date = item.CurrentFrom;
                                    operatingQuotaEvent.Cov_End_Date = item.CurrentTo.Value;
                                    operatingQuotaEvent.ApplyEndorsement.IndividualId = suretyRisk.Contractor.IndividualId;
                                    operatingQuotaEvent.ApplyEndorsement.CurrencyType = companyPolicyTmp.ExchangeRate.Currency.Id;
                                    operatingQuotaEvent.ApplyEndorsement.CurrencyTypeDesc = companyPolicyTmp.ExchangeRate.Currency.Description;
                                    operatingQuotaEvent.ApplyEndorsement.CoverageId = item.Id;
                                    operatingQuotaEvent.ApplyEndorsement.IsSeriousOffer = item.IsSeriousOffer;
                                    operatingQuotaEvent.ApplyEndorsement.Endorsement = companyPolicyTmp.Endorsement.Id;
                                    operatingQuotaEvent.ApplyEndorsement.PolicyID = companyPolicyTmp.Endorsement.PolicyId;
                                    operatingQuotaEvent.ApplyEndorsement.EndorsementType = (int)EndorsementType.LastEndorsementCancellation;
                                    operatingQuotaEvent.ApplyEndorsement.ParticipationPercentage = 100;
                                    operatingQuotaEvent.ApplyEndorsement.AmountCoverage = item.EndorsementLimitAmount * -1;
                                    if (item.PremiumAmount == 0 && companyPolicyTmp.Endorsement.EndorsementType != EndorsementType.Cancellation)
                                    {
                                        operatingQuotaEvent.ApplyEndorsement.AmountCoverage = 0;
                                    }
                                    operatingQuotaEvents.Add(operatingQuotaEvent);
                                }

                                lock (lockObject)
                                {
                                    DelegateService.OperationQuotaIntegrationService.InsertApplyEndorsementOperatingQuotaEvent(operatingQuotaEvents);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        });
                    }
                }
                else
                {
                    company = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                }
                return company;
            }
            else
            {
                throw new Exception(Errors.ErroEndorsementNotFound);
            }
        }
    }
}