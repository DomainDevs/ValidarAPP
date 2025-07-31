using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.EEProvider.Views;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.EEProvider.Assemblers;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.Models;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using ISSEN =Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.UnderwritingServices.Models;
using AutoMapper;
using TM=System.Threading.Tasks;
using System.Collections.Concurrent;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.EEProvider.Resources;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.Transports.Transport.BusinessServices.EEProvider.Assemblers;
using TP = Sistran.Core.Application.Utilities.Utility;


namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.EEProvider.Business
{
    public class BusinessCreditNote
    {
        BaseBusinessCia provider;
        public BusinessCreditNote()
        {
            provider = new BaseBusinessCia();
        }
        public List<CompanyEndorsementType> GetAvaibleEndorsementsByPolicyId(int policyId )
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name);
            filter.IsNotNull();
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementType.Properties.HasQuotation, typeof(ISSEN.EndorsementType).Name);
            filter.Equal();
            filter.Constant(1);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.In();
            filter.ListValue();
            filter.Constant((int)RiskStatusType.Excluded);
            filter.Constant((int)RiskStatusType.Cancelled);
            filter.EndList();

            EndorsmentTransportsView view = new EndorsmentTransportsView();
            ViewBuilder builder = new ViewBuilder("EndorsmentTransportsView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.EndorsementTypes.Count > 0)
            {
                List<ISSEN.EndorsementType> entityEndorsementTypes = view.EndorsementTypes.Cast<ISSEN.EndorsementType>().ToList();

                List<ISSEN.Endorsement> entityEndorsements = view.Endorsements.Cast<ISSEN.Endorsement>().ToList();
                
                ////List<ISSEN.EndorsementRisk> entityEndorsementRisks =  ((view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList()!=null)
                //            ?   view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList()
                //                :  new List<ISSEN.EndorsementRisk>());

                //List<ISSEN.RiskCoverage> entityEndorsementRiskCoverages = view.EndorsementRiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
              
                foreach (var Endorsements in entityEndorsementTypes)
                {
                    var list = entityEndorsements.Where(x => x.EndoTypeCode.Equals(Endorsements.EndoTypeCode)).ToList();
                    Endorsements.Description = Endorsements.Description + " (" + list.Count() + ")";

                }

                return EntityAssembler.CreateEndorsementTypes(entityEndorsementTypes, entityEndorsements /*entityEndorsementRisks, entityEndorsementRiskCoverages*/);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyPolicy">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        internal CompanyPolicy CreateEndorsementCreditNote(CompanyPolicy companyPolicy, CompanyRisk risk, CompanyCoverage coverage, decimal? premiumToReturn)
        {
            try
            {
                if (companyPolicy == null)
                {
                    throw new ArgumentException("Poliza Vacia");
                }
                List<CompanyRisk> risks = new List<CompanyRisk>();
                CompanyPolicy policy;
                List<CompanyTransport> companyTransports = new List<CompanyTransport>();
                PendingOperation pendingOperation = new PendingOperation();

                if (companyPolicy?.Endorsement?.TemporalId > 0)
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.PolicyId, false);
                    if (policy != null)
                    {

                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.CreditNoteEndorsementType = companyPolicy.Endorsement.CreditNoteEndorsementType;
                        policy.Endorsement.RiskId = companyPolicy.Endorsement.RiskId;
                        policy.Endorsement.CoverageId = companyPolicy.Endorsement.CoverageId;
                        policy.Endorsement.PremiumToReturn = premiumToReturn;
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text?.TextBody,
                            Observations = companyPolicy.Endorsement.Text?.Observations
                        };

                        if (policy.Id != 0)
                        {
                            companyTransports = DelegateService.transportService.GetCompanyTransportsByTemporalId(policy.Id);
                            if (companyTransports == null || companyTransports.Count == 0)
                                companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                        }
                        else
                        {
                            companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                        }

                        //companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                        if (companyTransports != null && companyTransports.Any())
                        {
                            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                            companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            risks = CreateCreditNote(companyTransports, risk, coverage, premiumToReturn);
                        }
                        else
                        {
                            companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyTransports != null && companyTransports.Any())
                            {
                                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                                companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                risks = CreateCreditNote(companyTransports, risk, coverage, premiumToReturn);
                            }
                            else
                            {
                                throw new Exception("Error Obteniendo riesgos");
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Temporal poliza no encontrado");
                    }
                }
                else
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicy.Endorsement.Id);
                    if (policy != null)
                    {

                        policy.UserId = companyPolicy.UserId;
                        policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                        if (policy.Endorsement == null)
                        {
                            policy.Endorsement = new CompanyEndorsement();
                        }
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text.TextBody,
                            Observations = companyPolicy.Endorsement.Text.Observations
                        };

                        policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.EndorsementType = EndorsementType.CreditNoteEndorsement;
                        policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                        policy.TemporalType = TemporalType.Endorsement;
                        policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                        policy.Endorsement.CreditNoteEndorsementType = companyPolicy.Endorsement.CreditNoteEndorsementType;
                        policy.Endorsement.RiskId = companyPolicy.Endorsement.RiskId;
                        policy.Endorsement.CoverageId = companyPolicy.Endorsement.CoverageId;
                        policy.Endorsement.PremiumToReturn = premiumToReturn;
                        
                        var imapper = ModelAssembler.CreateMapCompanyClause();
                        policy.Clauses = imapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                        policy.Summary = new CompanySummary
                        {
                            RiskCount = 0
                        };

                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        if (policy != null)
                        {
                            companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyTransports != null && companyTransports.Any())
                            {
                                companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                // jhgomez
                                // Se usa para forzar la inicialización de las coberturas
                                companyTransports.AsParallel().ForAll(x => x.Risk.Premium = 0 );
                                risks = CreateCreditNote(companyTransports,risk,coverage,premiumToReturn);
                            }
                            else
                            {
                                throw new Exception("Error Obteniendo riesgos");
                            }
                        }
                        else
                        {
                            throw new Exception("Error Creando temporal");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Poliza no encontrada");
                    }
                }
                if (policy.InfringementPolicies != null && policy.InfringementPolicies.Count() > 0)
                {
                    risks.AsParallel().ForAll(x => policy.InfringementPolicies.AddRange(x.InfringementPolicies));
                }
                if (risks != null && risks.Count != 0)
                {
                    policy.Summary = risks.First().Policy.Summary;
                    policy.Summary.AmountInsured = risks.First().AmountInsured;
                }
                return policy;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error  al Crear Endoso de Nota CreditoS");
            }
        }

        //internal CompanyPolicy CreateEndorsementCreditNote(CompanyPolicy companyPolicy, List<CompanyRisk> companyRisks)
        //{
        //    try
        //    {
        //        if (companyPolicy == null)
        //        {
        //            throw new ArgumentException("Poliza Vacia");
        //        }
        //        List<CompanyRisk> risks = new List<CompanyRisk>();
        //        CompanyPolicy policy;
        //        List<CompanyTransport> companyTransports = new List<CompanyTransport>();
        //        PendingOperation pendingOperation = new PendingOperation();

        //        if (companyPolicy?.Endorsement?.TemporalId > 0)
        //        {
        //            policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.TemporalId, false);
        //            if (policy != null)
        //            {

        //                policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
        //                policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
        //                policy.Endorsement.CreditNoteEndorsementType = companyPolicy.Endorsement.CreditNoteEndorsementType;
        //                policy.Endorsement.RiskId = companyPolicy.Endorsement.RiskId;
        //                policy.Endorsement.CoverageId = companyPolicy.Endorsement.CoverageId;
        //                policy.Endorsement.PremiumToReturn = companyPolicy.Endorsement.PremiumToReturn;
        //                policy.Endorsement.Text = new CompanyText
        //                {
        //                    TextBody = companyPolicy.Endorsement.Text?.TextBody,
        //                    Observations = companyPolicy.Endorsement.Text?.Observations
        //                };

        //                if (policy.Id != 0)
        //                {
        //                    companyTransports = DelegateService.transportService.GetCompanyTransportsByTemporalId(policy.Id);
        //                    if (companyTransports == null || companyTransports.Count == 0)
        //                        companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
        //                }
        //                else
        //                {
        //                    companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
        //                }

        //                //companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
        //                if (companyTransports != null && companyTransports.Any())
        //                {
        //                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
        //                    companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
        //                    risks = CreateCreditNote(companyTransports, risk, coverage, premiumToReturn);
        //                }
        //                else
        //                {
        //                    companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
        //                    if (companyTransports != null && companyTransports.Any())
        //                    {
        //                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
        //                        companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
        //                        risks = CreateCreditNote(companyTransports, risk, coverage, premiumToReturn);
        //                    }
        //                    else
        //                    {
        //                        throw new Exception("Error Obteniendo riesgos");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                throw new ArgumentException("Temporal poliza no encontrado");
        //            }
        //        }
        //        else
        //        {
        //            policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicy.Endorsement.Id);
        //            if (policy != null)
        //            {
        //                //policy.UserId = BusinessContext.Current.UserId;
        //                policy.UserId = policy.UserId;
        //                policy.CurrentFrom = policy.CurrentTo;
        //                policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
        //                policy.Endorsement.CreditNoteEndorsementType = companyPolicy.Endorsement.CreditNoteEndorsementType;
        //                policy.Endorsement.RiskId = companyPolicy.Endorsement.RiskId;
        //                policy.Endorsement.CoverageId = companyPolicy.Endorsement.CoverageId;
        //                policy.Endorsement.PremiumToReturn = companyPolicy.Endorsement.PremiumToReturn;

        //                policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
        //                if (policy.Endorsement == null)
        //                {
        //                    policy.Endorsement = new CompanyEndorsement();
        //                }
        //                policy.Endorsement.Text = new CompanyText
        //                {
        //                    TextBody = companyPolicy.Endorsement.Text.TextBody,
        //                    Observations = companyPolicy.Endorsement.Text.Observations
        //                };

        //                policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
        //                policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
        //                policy.Endorsement.EndorsementType = EndorsementType.CreditNoteEndorsement;
        //                policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
        //                policy.TemporalType = TemporalType.Endorsement;
        //                policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);

        //                var imapper = ModelAssembler.CreateMapCompanyClause();
        //                policy.Clauses = imapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

        //                policy.Summary = new CompanySummary
        //                {
        //                    RiskCount = 0
        //                };

        //                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
        //                if (policy != null)
        //                {
        //                    companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
        //                    if (companyTransports != null && companyTransports.Any())
        //                    {
        //                        companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
        //                        risks = CreateCreditNote(companyTransports, risk, coverage, premiumToReturn);
        //                    }
        //                    else
        //                    {
        //                        throw new Exception("Error Obteniendo riesgos");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception("Error Creando temporal");
        //                }
        //            }
        //            else
        //            {
        //                throw new ArgumentException("Poliza no encontrada");
        //            }
        //        }
        //        if (policy.InfringementPolicies != null && policy.InfringementPolicies.Count() > 0)
        //        {
        //            risks.AsParallel().ForAll(x => policy.InfringementPolicies.AddRange(x.InfringementPolicies));
        //        }
        //        if (risks != null && risks.Count != 0)
        //        {
        //            policy.Summary = risks.First().Policy.Summary;
        //            policy.Summary.AmountInsured = risks.First().AmountInsured;
        //        }
        //        return policy;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException("Error  al Crear Endoso de Nota CreditoS");
        //    }
        //}



        public CompanyTransport GetCompanyTransportTemporalByRiskId(int riskId)

        {
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(riskId);

            if (pendingOperation != null)
            {
                CompanyTransport companyTransport = JsonConvert.DeserializeObject<CompanyTransport>(pendingOperation.Operation);
                companyTransport.Risk.Id = pendingOperation.Id;
                companyTransport.Risk.IsPersisted = true;

                return companyTransport;
            }
            else
            {
                return null;
            }
        }

        public List<CompanyTransport> GetCompanyTransportsByTemporalId(int temporalId)
        {
            List<CompanyTransport> companyTransports = new List<CompanyTransport>();
            List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);

            foreach (PendingOperation pendingOperation in pendingOperations)
            {
                CompanyTransport companyTransport = JsonConvert.DeserializeObject<CompanyTransport>(pendingOperation.Operation);
                companyTransport.Risk.RiskId = pendingOperation.Id;
                companyTransport.Risk.IsPersisted = true;
                companyTransports.Add(companyTransport);
            }

            return companyTransports;
        }

        private List<CompanyRisk> CreateCreditNote(List<CompanyTransport> companyTransports, CompanyRisk riskI, CompanyCoverage coverage, decimal? premiumToReturn)
        {
            try
            {
                if (companyTransports != null && companyTransports.Any())
                {
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    PendingOperation pendingOperation = new PendingOperation();

                    if ((bool)companyTransports.First()?.Risk?.Policy.Product.IsCollective)
                    {
                        if (companyTransports.First().Risk.Policy.Endorsement.TemporalId > 0)
                        {
                            companyTransports = DelegateService.transportService.GetCompanyTransportsByTemporalId(companyTransports.First().Risk.Policy.Endorsement.TemporalId);
                            TP.Parallel.ForEach(companyTransports, item =>
                            {
                                item.Risk.IsPersisted = true;
                                item.Risk.Status = RiskStatusType.Modified;
                                var risk = GetDataCreditNote(item, CoverageStatusType.NotModified);
                                risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
                                risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                                risks.Add(risk.Risk);
                            });

                            var transportPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                            transportPolicy = DelegateService.underwritingService.CreatePolicyTemporal(transportPolicy, false);

                            return risks;
                        }
                        else
                        {
                            companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyTransports.First().Risk.Policy.Endorsement.PolicyId);
                            TP.Parallel.ForEach(companyTransports, item =>
                            {
                                item.Risk.IsPersisted = true;
                                var risk = GetDataCreditNote(item, CoverageStatusType.NotModified);
                                risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
                                risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                                risks.Add(risk.Risk);
                            });

                            var transportPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                            transportPolicy = DelegateService.underwritingService.CreatePolicyTemporal(transportPolicy, false);

                            return risks;
                        }
                    }
                    else
                    {
                        TP.Parallel.ForEach(companyTransports, item =>
                        {
                            item.Risk.IsPersisted = true;
                            CompanyTransport risk = GetDataCreditNote(item, CoverageStatusType.NotModified);
                            item.Risk.OriginalStatus = RiskStatusType.Modified;
                            item.Risk.Status = RiskStatusType.Modified;
                            risk.Risk = CalculateReturnedPremium(risk.Risk, riskI, coverage, premiumToReturn);
                            risk.Risk.Premium = risk.Risk.Coverages.Sum(x => x.PremiumAmount);
                            risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        });

                        var transportPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                        /*Actualiza el Pending Operation de la Poliza*/
                        transportPolicy = DelegateService.underwritingService.CreatePolicyTemporal(transportPolicy, false);
                        return risks;
                    }
                }
                else
                {
                    throw new Exception("No existen Transportes");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //private List<CompanyRisk> CreateCreditNote(List<CompanyTransport> companyTransports, CompanyRisk riskI, CompanyCoverage coverage, decimal? premiumToReturn)
        //{
        //    try
        //    {
        //        if (companyTransports != null && companyTransports.Any())
        //        {
        //            List<CompanyRisk> risks = new List<CompanyRisk>();
        //            PendingOperation pendingOperation = new PendingOperation();

        //            if ((bool)companyTransports.First()?.Risk?.Policy.Product.IsCollective)
        //            {
        //                if (companyTransports.First().Risk.Policy.Endorsement.TemporalId > 0)
        //                {
        //                    companyTransports = DelegateService.transportService.GetCompanyTransportsByTemporalId(companyTransports.First().Risk.Policy.Endorsement.TemporalId);
        //                    Parallel.ForEach(companyTransports, item =>
        //                    {
        //                        item.Risk.IsPersisted = true;
        //                        var risk = GetDataCreditNote(item);
        //                        risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
        //                        risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
        //                        risks.Add(risk.Risk);
        //                    });

        //                    var transportPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

        //                    transportPolicy = DelegateService.underwritingService.CreatePolicyTemporal(transportPolicy, false);

        //                    return risks;
        //                }
        //                else
        //                {
        //                    companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyTransports.First().Risk.Policy.Endorsement.PolicyId);
        //                    Parallel.ForEach(companyTransports, item =>
        //                    {
        //                        item.Risk.IsPersisted = true;
        //                        var risk = GetDataCreditNote(item);
        //                        risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
        //                        risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
        //                        risks.Add(risk.Risk);
        //                    });

        //                    var transportPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

        //                    transportPolicy = DelegateService.underwritingService.CreatePolicyTemporal(transportPolicy, false);

        //                    return risks;
        //                }
        //            }
        //            else
        //            {
        //                Parallel.ForEach(companyTransports, item =>
        //                {
        //                    item.Risk.IsPersisted = true;
        //                    CompanyTransport risk = GetDataCreditNote(item);
        //                    risk.Risk = CalculateReturnedPremium(risk.Risk, riskI, coverage, premiumToReturn);
        //                    risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
        //                    risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
        //                    risks.Add(risk.Risk);
        //                });

        //                var transportPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

        //                /*Actualiza el Pending Operation de la Poliza*/
        //                transportPolicy = DelegateService.underwritingService.CreatePolicyTemporal(transportPolicy, false);
        //                return risks;
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("No existen Transportes");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        private CompanyRisk CalculateReturnedPremium(CompanyRisk risk, CompanyRisk riskI, CompanyCoverage coverage, decimal? premiumToReturn)
        {
            if(premiumToReturn == null)
            {
                throw new Exception("Valor de prima a devolver invalido");
            }
            try
            {
                if (risk.RiskId == riskI.Id)
                {
                    foreach (var cover in risk.Coverages)
                    {
                        if (cover.Id == coverage.Id)
                        {
                            cover.PremiumAmount = (decimal)premiumToReturn;
                            cover.CalculationType = Core.Services.UtilitiesServices.Enums.CalculationType.Direct;
                            cover.CoverStatus = CoverageStatusType.Modified;
                            cover.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Modified);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en calculo de nueva prima para cobertura.");
            }
            return risk;
        }

        private CompanyTransport GetDataCreditNote(CompanyTransport risk, CoverageStatusType coverageStatusType)
        {
            if (risk.Risk?.Beneficiaries?[0].IdentificationDocument == null)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                ConcurrentBag<string> error = new ConcurrentBag<string>();
                if (risk.Risk.Beneficiaries != null)
                {
                    risk.Risk.Beneficiaries.AsParallel().ForAll(
                        item =>
                        {
                            var beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(
                                                item.IndividualId.ToString(), 
                                                InsuredSearchType.IndividualId)
                                            .FirstOrDefault();
                            if (beneficiary != null)
                            {
                                item.IdentificationDocument = beneficiary.IdentificationDocument;
                                item.Name = beneficiary.Name;
                            }
                            else
                            {
                                error.Add(Errors.ErrorBeneficiaryNotFound);
                            }
                        }
                        );
                    if (error.Any())
                    {
                        throw new Exception(string.Join(",", error));
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorBeneficiaryNotFound);
                }
            }


            if (risk != null && risk.Risk.Premium == 0)
            {
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
                if (coverages != null && coverages.Count > 0)
                {
                    //Se dejan  las que vienen el endoso Anterior Definicion Taila
                    List<CompanyCoverage> coveragesAll = new List<CompanyCoverage>(); ;
                    var coveragesData = risk.Risk.Coverages;
                    coveragesData.AsParallel().ForAll(item =>
                    {
                        var coverageLocal = coverages.First(z => z.Id == item.Id);
                        item.RuleSetId = coverageLocal.RuleSetId;
                        item.PosRuleSetId = coverageLocal.PosRuleSetId;
                        item.ScriptId = coverageLocal.ScriptId;
                        item.InsuredObject = coverageLocal.InsuredObject;
                        item.OriginalLimitAmount = item.LimitAmount;
                        item.OriginalSubLimitAmount = item.SubLimitAmount;
                        item.CoverageOriginalStatus = item.CoverStatus;
                        item.CoverStatus = coverageStatusType;
                        item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverageStatusType);
                        item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                        item.CurrentTo = risk.Risk.Policy.CurrentTo;
                        item.AccumulatedPremiumAmount = 0;
                        item.FlatRatePorcentage = 0;
                        item.PremiumAmount = 0;
                    });
                    risk.Risk.Coverages = coveragesData;
                    risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
                }
                else
                {
                    throw new Exception(Errors.ErrorCoverages);
                }
            }

            return risk;
        }
        
        
    }
}
