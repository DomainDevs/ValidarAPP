using AutoMapper;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessService.EEProvider.Resources;
using Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessServices.EEProvider;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using CoreEnum = Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessService.EEProvider.View;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessServices.EEProvider.Assemblers;
using Sistran.Company.Application.Transports.TransportBusinessService.Models;
using Sistran.Co.Application.Data;
using System.Data;
using TRBB = Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Business;
using Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Business;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessService.EEProvider.Business
{
    public class AdjustmentBusiness
    {

        BaseBusinessCia provider;


        public AdjustmentBusiness()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyPolicy">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        public CompanyPolicy CreateEndorsementAdjustment(CompanyPolicy companyPolicy, Dictionary<string, object> formValues)
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
                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.TemporalId, false);
                    if (policy != null)
                    {
                        policy.CurrentFrom = Convert.ToDateTime(companyPolicy.Endorsement.CurrentFrom);
                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text?.TextBody,
                            Observations = companyPolicy.Endorsement.Text?.Observations
                        };
                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;
                        companyTransports = DelegateService.transportService.GetCompanyTransportsByTemporalId(policy.Id);
                        //companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                        if (companyTransports == null || companyTransports.Count == 0)
                        {
                            companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                        }
                        if (companyTransports != null && companyTransports.Any())
                        {
                            int riskNum = companyTransports.Where(x => x.Risk.RiskId == (int)formValues["RiskId"]).Select(n => n.Risk.Number).FirstOrDefault();
                            CompanyRisk tempRisk = policy.Summary.Risks.Where(x => x.Policy.Endorsement.RiskId == riskNum || x.Policy.Endorsement.RiskId == (int)formValues["RiskId"]).FirstOrDefault();
                            if (tempRisk != null)
                            {
                                policy.Summary.Risks.Where(x => x.Policy.Endorsement.RiskId == riskNum || x.Policy.Endorsement.RiskId == (int)formValues["RiskId"]).FirstOrDefault().Policy.Endorsement = policy.Endorsement;
                            }
                            else
                            {
                                CompanyRisk tRisk = new CompanyRisk();
                                tRisk.Policy = new CompanyPolicy { Endorsement = companyPolicy.Endorsement };
                                policy.Summary.Risks.Add(tRisk);
                            }
                            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                            companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            if (policy.Summary.Risks.Count > 0)
                            {
                                companyPolicy.Summary = policy.Summary;
                            }
                            risks = CreateAdjustment(companyTransports, policy);
                        }
                        else
                        {
                            throw new Exception("Error Obteniendo transportes");
                            //companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                            //if (companyTransports != null && companyTransports.Any())
                            //{
                            //    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                            //    companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            //    risks = CreateAdjustment(companyTransports, policy);
                            //}
                            //else
                            //{
                            //    throw new Exception("Error Obteniendo transportes");
                            //}
                        }

                    }
                    else
                    {
                        throw new ArgumentException("Temporal poliza encontrado");
                    }

                }
                else
                {

                    policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicy.Endorsement.Id);
                    policy.PolicyType.IsFloating = DelegateService.commonService.GetPolicyTypesByPrefixIdById(policy.Product.Prefix.Id, policy.PolicyType.Id).IsFloating;
                    if (policy != null)
                    {
                        policy.UserId = policy.UserId;
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
                        policy.Endorsement.EndorsementType = EndorsementType.AdjustmentEndorsement;
                        policy.Endorsement.EndorsementDays = companyPolicy.Endorsement.EndorsementDays;
                        policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                        policy.TemporalType = TemporalType.Endorsement;
                        policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                        policy.Endorsement.DeclaredValue = companyPolicy.Endorsement.DeclaredValue;
                        policy.Endorsement.RiskId = companyPolicy.Endorsement.RiskId;
                        policy.Endorsement.InsuredObjectId = companyPolicy.Endorsement.InsuredObjectId;
                        policy.Endorsement.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                        policy.Endorsement.CurrentTo = companyPolicy.Endorsement.CurrentTo;
                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;

                        var imapper = ModelAssembler.CreateMapCompanyClause();
                        policy.Clauses = imapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                        policy.Summary = new CompanySummary
                        {
                            RiskCount = 0
                        };
                        //CompanyRisk tRisk = new CompanyRisk();
                        //tRisk.Policy = new CompanyPolicy { Endorsement = companyPolicy.Endorsement };
                        //policy.Summary.Risks = new List<CompanyRisk>() { tRisk };
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        if (policy != null)
                        {
                            companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyTransports != null && companyTransports.Any())
                            {
                                CompanyTransport currentRisk = companyTransports.Where(x => x.Risk.RiskId == (int)formValues["RiskId"]).FirstOrDefault();
                                CompanyInsuredObject insuredObject = currentRisk.InsuredObjects.Where(x => x.Id == (int)formValues["InsuredObjectId"]).FirstOrDefault();
                                companyPolicy.Endorsement.InsuredObjectId = insuredObject.Id;
                                companyPolicy.Endorsement.RiskId = currentRisk.Risk.Number;
                                companyPolicy.Endorsement.DeclaredValue = currentRisk.Risk.AmountInsured;
                                CompanyRisk tRisk = new CompanyRisk();
                                tRisk.Policy = new CompanyPolicy { Endorsement = companyPolicy.Endorsement };
                                policy.Summary.Risks = new List<CompanyRisk>() { tRisk };
                                companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);

                                //if (policy.Summary.Risks.Count > 0)
                                //{

                                //    companyPolicy.Summary = policy.Summary;
                                //}
                                risks = CreateAdjustment(companyTransports, policy);
                            }
                            else
                            {
                                throw new Exception("Error Obteniendo transportes");
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
                    //policy.Summary = risks.First().Policy.Summary;
                    //policy.Summary.AmountInsured = risks.First().AmountInsured;
                    policy.Summary = risks.Where(x => x.RiskId == (int)formValues["RiskId"]).FirstOrDefault().Policy.Summary;
                    policy.Summary.AmountInsured = risks.Where(x => x.RiskId == (int)formValues["RiskId"]).FirstOrDefault().AmountInsured;
                    CompanyCoverage coverage = risks.Where(x => x.RiskId == (int)formValues["RiskId"]).FirstOrDefault().Coverages.Where(y => y.IsPrimary).FirstOrDefault();
                    policy.Summary.Premium = (decimal)GetPremiumByRiskByInsuredObject(policy.Endorsement.PolicyId, risks.Where(x => x.RiskId == (int)formValues["RiskId"]).FirstOrDefault().Number, (int)formValues["InsuredObjectId"], coverage);

                }

                if (policy.Endorsement.TicketDate == null || policy.Endorsement.TicketNumber == null)
                {
                    policy.Endorsement.TicketDate = (DateTime)formValues["TicketDate"];
                    policy.Endorsement.TicketNumber = (int)formValues["TicketNumber"];
                }
                return policy;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al Crear Endoso de Ajuste");
            }
        }

        public decimal GetPremiumByRiskByInsuredObject(decimal policyId, decimal riskId, decimal insuredObject, CompanyCoverage coverage)
        {

            TransportBusiness transportBusiness = new TransportBusiness();
            CompanyEndorsementPeriod period = transportBusiness.GetEndorsementPeriodByPolicyId(policyId);
            List<CompanyEndorsementDetail> details = transportBusiness.GetEndorsementDetailsListByPolicyId(policyId, period.Version);
            details = details.Where(x => x.RiskNum == riskId && x.InsuredObjectId == insuredObject && x.EndorsementType == (int)EndorsementType.DeclarationEndorsement).ToList();
            decimal sumDeclaration = (decimal)details.Sum(x => x.PremiumAmount);
            if (coverage.DepositPremiumPercent > 0)
            {
                return sumDeclaration - (decimal)(coverage.SubLimitAmount * (coverage.DepositPremiumPercent/100)* (coverage.Rate/100));
            }
            return 0;

        }

        /// <summary>
        /// Creates Extension.
        /// </summary>
        /// <param name="companyTransports">The company endorsement.</param>
        /// <returns></returns>
        private List<CompanyRisk> CreateAdjustment(List<CompanyTransport> companyTransports, CompanyPolicy policy)
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
                        //Parallel.ForEach(companyTransports, item =>
                        //{
                        //    item.Risk.IsPersisted = true;
                        //    item.Risk.Status = RiskStatusType.Modified;
                        //    var risk = GetDataAdjustment(item);
                        //    risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
                        //    risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                        //    risks.Add(risk.Risk);
                        //});
                        foreach (var item in companyTransports)
                        {
                            item.Risk.IsPersisted = true;
                            item.Risk.Status = RiskStatusType.Modified;
                            var risk = GetDataAdjustment(item);
                            risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        }

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
                            var risk = GetDataAdjustment(item);
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
                    //Parallel.ForEach(companyTransports, item =>
                    //{
                    //    item.Risk.IsPersisted = true;
                    //    var risk = GetDataAdjustment(item);
                    //    item.Risk.OriginalStatus = RiskStatusType.Modified;
                    //    item.Risk.Status = RiskStatusType.Modified;
                    //    risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
                    //    risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                    //    risks.Add(risk.Risk);
                    //});
                    foreach (var item in companyTransports)
                    {
                        item.Risk.IsPersisted = true;
                        var risk = GetDataAdjustment(item);
                        item.Risk.OriginalStatus = RiskStatusType.Modified;
                        item.Risk.Status = RiskStatusType.Modified;
                        risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);
                        risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                        risks.Add(risk.Risk);
                    }

                    var transportPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);
                    if (policy.Summary.Risks.Count > 0)
                    {
                        transportPolicy.Summary.Risks = policy.Summary.Risks;
                    }
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

        /// <summary>
        /// Gets the data extension
        /// </summary>
        /// <param name="risk">The company endorsement.</param>
        /// <returns></returns>
        private CompanyTransport GetDataAdjustment(CompanyTransport risk)
        {
            if (risk.Risk.Beneficiaries.Count > 0)
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
                                var beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
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
            }
            else
            {
                throw new Exception(Errors.ErrorGetBeneficiary);
            }

            List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
            if (coverages != null && coverages.Count > 0)
            {
                coverages = coverages.Where(c => (risk.Risk.Coverages.Any(x => x.Id == c.Id))).ToList();
                var ciaCoverages = risk.Risk.Coverages.Where(x => coverages.Select(z => z.Id).Contains(x.Id)).ToList();
                ciaCoverages.AsParallel().ForAll(item =>
                {
                    var coverageLocal = coverages.FirstOrDefault(u => u.Id == item.Id);
                    item.CoverStatus = item.CoverageOriginalStatus;
                    item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                    item.CurrentTo = risk.Risk.Policy.CurrentTo;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = 0;
                    item.SubLineBusiness = coverageLocal.SubLineBusiness;
                    item.IsSelected = coverageLocal.IsSelected;
                    item.IsMandatory = coverageLocal.IsMandatory;
                    item.IsVisible = coverageLocal.IsVisible;
                    //item.DeclaredAmount=
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);

                //companyCoverage = DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyTransport.Risk.Policy.Endorsement.PolicyId, companyTransport.Risk.RiskId);
                //companyCoverage.PremiumAmount = (companyCoverage.PremiumAmount * companyCoverage.DepositPremiumPercent) / 100;
            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }
            //List<CompanyRisk> temRisk = new List<CompanyRisk>();
            //if (risk.Risk.Policy.Summary?.Risks?.Count > 0 )
            //{
            //    temRisk = risk.Risk.Policy.Summary?.Risks;
            //}
            risk = DelegateService.transportService.QuotateCompanyTransport(risk, false, false);

            return risk;
        }
        /// <summary>
        /// lista los endosos de declaracion y obtiene el valor declarado por endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        public List<CompanyCoverage> GetCoverageByEndorsementIdPolicyIdriskId(int policyId, int riskId)
        {
            List<CompanyCoverage> coverages = new List<CompanyCoverage>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(policyId);
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(riskId);
                filter.And();
                filter.Property(ISSEN.EndorsementType.Properties.EndoTypeCode, typeof(ISSEN.EndorsementType).Name);
                filter.In();
                filter.ListValue();
                filter.Constant((int)EndorsementType.DeclarationEndorsement);
                filter.EndList();
                DeclarationCoverageView view = new DeclarationCoverageView();
                ViewBuilder builder = new ViewBuilder("DeclarationCoverageView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ISSEN.RiskCoverage> riskCoverages = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                List<ISSEN.Endorsement> Endorsements = view.Endorsements.Cast<ISSEN.Endorsement>().ToList();
                List<ISSEN.EndorsementRiskCoverage> entityEndorsementRiskCoverage = view.EndoRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
                if (riskCoverages.Count > 0)
                {

                    coverages = EntityAssembler.Createcoverage(riskCoverages, Endorsements, entityEndorsementRiskCoverage);
                }
                else
                { }

            }
            catch (Exception ex)
            {


            }


            return coverages;
        }

        public void SaveAdjustDetailPolicyPeriod(CompanyEndorsementDetail detail)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                int version;
                CompanyEndorsementPeriod period = GetDetailPolicy(detail.PolicyId, out version);
                List<CompanyEndorsementDetail> details = GetDetailEndorsements(detail.PolicyId, version);
                int count = details.Where(x => x.EndorsementType == (int)EndorsementType.AdjustmentEndorsement && x.RiskNum == detail.RiskNum && x.InsuredObjectId == detail.InsuredObjectId && x.PolicyId == period.PolicyId && x.Version == period.Version).Count();
                decimal totalDeclarations = (decimal)details.Where(x => x.EndorsementType == (int)EndorsementType.DeclarationEndorsement && x.PolicyId == period.PolicyId && x.Version == period.Version).Sum(n => n.DeclarationValue);
                if (count < period.TotalAdjustment)
                {

                }
                else
                {
                    result.Add("Message", string.Format("Los endosos de Ajuste para el riesgo y el objeto del seguro ya se realizaron"));
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public CompanyEndorsementPeriod GetDetailPolicy(decimal policyId, out int version)
        {
            try
            {
                version = 0;
                NameValue[] parameters = new NameValue[1];
                CompanyEndorsementPeriod companyEndorsementPeriod = new CompanyEndorsementPeriod();
                TRBB.DatatableToList dataTable = new TRBB.DatatableToList();
                parameters[0] = new NameValue("@POLICY_ID", policyId);
                DataSet result;
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataSet("ISS.GET_ENDORSEMENT_COUNT_PERIOD", parameters);
                }
                if (result != null && result.Tables[0].Rows.Count > 0)
                {
                    companyEndorsementPeriod = dataTable.ConvertTo<CompanyEndorsementPeriod>(result.Tables[0]).FirstOrDefault();
                    version = companyEndorsementPeriod.Version;
                }
                return companyEndorsementPeriod;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<CompanyEndorsementDetail> GetDetailEndorsements(decimal policyId, int version)
        {
            try
            {
                NameValue[] parameters = new NameValue[2];
                List<CompanyEndorsementDetail> companyEndorsementPeriod = new List<CompanyEndorsementDetail>();
                DatatableToList dataTable = new DatatableToList();
                int rowcount = 0;
                parameters[0] = new NameValue("@POLICY_ID", policyId);
                parameters[1] = new NameValue("@VERSION", version);
                DataSet result;
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataSet("ISS.GET_ENDORSEMENT_COUNT_DETAIL", parameters);
                }
                if (result != null && result.Tables[0].Rows.Count > 0)
                {
                    companyEndorsementPeriod = dataTable.ConvertTo<CompanyEndorsementDetail>(result.Tables[0]);
                }
                return companyEndorsementPeriod;
            }
            catch (Exception)
            {

                throw;
            }

        }



        //
        //public List<CompanyEndorsementDetail> GetDetailEndorsements(decimal policyId, int version)
        //{
        //    try
        //    {
        //        NameValue[] parameters = new NameValue[2];
        //        List<CompanyEndorsementDetail> companyEndorsementPeriod = new List<CompanyEndorsementDetail>();
        //        TRBB.DatatableToList dataTable = new TRBB.DatatableToList();
        //        int rowcount = 0;
        //        parameters[0] = new NameValue("@POLICY_ID", policyId);
        //        parameters[1] = new NameValue("@VERSION", version);
        //        DataSet result;
        //        using (DynamicDataAccess pdb = new DynamicDataAccess())
        //        {
        //            result = pdb.ExecuteSPDataSet("ISS.GET_ENDORSEMENT_COUNT_DETAIL", parameters);
        //        }
        //        if (result != null && result.Tables[0].Rows.Count > 0)
        //        {
        //            companyEndorsementPeriod = dataTable.ConvertTo<CompanyEndorsementDetail>(result.Tables[0]);
        //        }
        //        return companyEndorsementPeriod;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        //public CompanyEndorsementDetail ExecSaveEndorsementDetail(CompanyEndorsementDetail saveData)
        //{
        //    try
        //    {
        //        NameValue[] parameters = new NameValue[8];
        //        CompanyEndorsementDetail resultEndorsementDetail = new CompanyEndorsementDetail();
        //        //decimal monthsVigency = GetMonthsByVigency(companyEndorsementPeriod.CurrentFrom, companyEndorsementPeriod.CurrentTo);
        //        //companyEndorsementPeriod.DeclarationPeriod = GetMothsByDeclarationPeriod(companyEndorsementPeriod.DeclarationPeriod);
        //        //companyEndorsementPeriod.AdjustPeriod = GetMothsByAdjustmentPeriod(companyEndorsementPeriod.AdjustPeriod);
        //        //companyEndorsementPeriod.TotalAdjust = (int)Math.Floor(monthsVigency / companyEndorsementPeriod.AdjustPeriod);
        //        //companyEndorsementPeriod.TotalDeclaration = (int)Math.Ceiling(monthsVigency / companyEndorsementPeriod.DeclarationPeriod);
        //        //if (monthsVigency < 12 && companyEndorsementPeriod.TotalAdjust == 0)
        //        //{
        //        //    companyEndorsementPeriod.TotalAdjust = 1;
        //        //}
        //        //int rowcount = 0;
        //        parameters[0] = new NameValue("@POLICY_ID", resultEndorsementDetail.PolicyId);
        //        parameters[1] = new NameValue("@ENDORSEMENT_TYPE", resultEndorsementDetail.EndorsementType);
        //        parameters[2] = new NameValue("@RISK_ID", resultEndorsementDetail.RiskId);
        //        parameters[3] = new NameValue("@INSURED_OBJECT_ID", resultEndorsementDetail.InsuredObjectId);
        //        parameters[4] = new NameValue("@VERSION", resultEndorsementDetail.Version);
        //        parameters[5] = new NameValue("@ENDORSEMENT_DATE", resultEndorsementDetail.EndorsementDate);
        //        parameters[6] = new NameValue("@DECLARATION_VALUE", resultEndorsementDetail.DeclarationValue);
        //        parameters[6] = new NameValue("@PREMIUM_AMOUNT", resultEndorsementDetail.PremiumAmmount);
        //        parameters[6] = new NameValue("@DEDUCTIBLE_AMOUNT", resultEndorsementDetail.DeductibleAmmount);
        //        parameters[6] = new NameValue("@TAXES", resultEndorsementDetail.Taxes);
        //        parameters[7] = new NameValue("@SURCHANGE", resultEndorsementDetail.Surchanges);
        //        parameters[7] = new NameValue("@EXPENSES", resultEndorsementDetail.Expenses);
        //        DataSet result;
        //        using (DynamicDataAccess pdb = new DynamicDataAccess())
        //        {
        //            result = pdb.ExecuteSPDataSet("ISS.SAVE_ENDORSEMENT_COUNT_DETAIL", parameters);
        //        }
        //        if (result != null)
        //        {
        //            return resultEndorsementDetail;
        //            //resultEndorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementPeriod>(result.Tables[0]).FirstOrDefault();
        //        }
        //        return new CompanyEndorsementDetail();
        //    }
        //    catch (Exception ex)
        //    {

        //        //EventLog.WriteEntry("SaveCompanyEndorsementPeriod", String.Format("Error Persistiendo Datos de la poliza en ISS.ENDORSEMENT_COUNT_PERIOD DETALLES {0} : {1}", ex.Message, JsonConvert.SerializeObject(companyEndorsementPeriod)));
        //        //if (tryAgain)
        //        //{
        //        //    tryAgain = false;
        //        //    SaveCompanyEndorsementPeriod(companyEndorsementPeriod);

        //        //}
        //        throw new Exception(String.Format("Error Persistiendo Datos de la poliza en ISS.ENDORSEMENT_COUNT_PERIOD DETALLES {0}", ex.Message));
        //    }
        //}

    }
}
