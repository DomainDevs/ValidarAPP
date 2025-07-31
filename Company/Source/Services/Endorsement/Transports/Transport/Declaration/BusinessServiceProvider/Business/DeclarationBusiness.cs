using AutoMapper;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.Transports.Endorsement.Declaration.BusinessServices.EEProvider.Resources;
using Sistran.Company.Application.Transports.Endorsement.Declaration.BusinessServices.EEProvider.Assemblers;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.Transports.TransportBusinessService.Models;
using Sistran.Co.Application.Data;
using System.Data;
using Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Business;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.BusinessServices.EEProvider.Business
{
    public class DeclarationBusiness
    {
        BaseBusinessCia provider;

        public DeclarationBusiness()
        {
            provider = new BaseBusinessCia();
        }
        /// <summary>
        /// creates endorsement
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        public CompanyPolicy CreateEndorsementDeclaration(CompanyPolicy companyPolicy, Dictionary<string, object> formValues)
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
                CompanyCoverage coverage = new CompanyCoverage();
                coverage.DeclaredAmount = (decimal)companyPolicy.Endorsement.DeclaredValue;
                PendingOperation pendingOperation = new PendingOperation();
                if (companyPolicy?.Endorsement?.TemporalId > 0)
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.TemporalId, false);
                    if (policy != null)
                    {
                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.CurrentTo);
                        policy.CurrentFrom= Convert.ToDateTime(companyPolicy.CurrentFrom);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.DeclaredValue = companyPolicy.Endorsement.DeclaredValue;
                        policy.Endorsement.RiskId = companyPolicy.Endorsement.RiskId;
                        policy.Endorsement.InsuredObjectId = companyPolicy.Endorsement.InsuredObjectId;
                        policy.Endorsement.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                        policy.Endorsement.CurrentTo = companyPolicy.Endorsement.CurrentTo;
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text?.TextBody,
                            Observations = companyPolicy.Endorsement.Text?.Observations
                        };
                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;
                        if (policy.Id != 0)
                        {
                            companyTransports = DelegateService.transportServices.GetCompanyTransportsByTemporalId(policy.Id);
                            if (companyTransports == null || companyTransports.Count == 0)
                                companyTransports = DelegateService.transportServices.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                        }
                        else
                        {
                            companyTransports = DelegateService.transportServices.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                        }
                        if (companyTransports != null && companyTransports.Any())
                        {
                            //CompanyTransport tempRisk = companyTransports.Where(x => x.Risk.RiskId == int.Parse(formValues["RiskId"].ToString())).FirstOrDefault();
                            CompanyRisk tempRisk = policy.Summary.Risks.Where(x => x.Policy.Endorsement.RiskId == int.Parse(formValues["RiskId"].ToString())).FirstOrDefault();
                            if (tempRisk != null)
                            {
                                if (tempRisk.Policy.Endorsement.InsuredObjectId == int.Parse(formValues["InsuredObjectId"].ToString()))
                                {
                                    policy.Summary.Risks.Where(x => x.Policy.Endorsement.RiskId == int.Parse(formValues["RiskId"].ToString())).FirstOrDefault().Policy.Endorsement = policy.Endorsement;
                                }
                                else
                                {
                                    tempRisk.Policy = new CompanyPolicy { Endorsement = companyPolicy.Endorsement };
                                    policy.Summary.Risks.Add(tempRisk);
                                }
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
                            risks = CreateDeclaration(companyTransports, companyPolicy);
                            //risks = CreateDeclaration(companyTransports.Where(x=>x.Risk.RiskId == (int)formValues["RiskId"]).ToList(), companyPolicy);
                        }
                        else
                        {
                            throw new Exception("Error Obteniendo transportes");
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
                        policy.CurrentFrom = companyPolicy.CurrentFrom;
                        policy.CurrentTo = companyPolicy.CurrentTo;
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.EndorsementType = EndorsementType.DeclarationEndorsement;
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
                        //if (policy.PolicyType.IsFloating)
                        //{
                        CompanyRisk tRisk = new CompanyRisk();
                        tRisk.Policy = new CompanyPolicy { Endorsement = companyPolicy.Endorsement };
                        policy.Summary.Risks = new List<CompanyRisk>() { tRisk };
                        //}
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        if (policy != null)
                        {
                            companyTransports = DelegateService.transportServices.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyTransports != null && companyTransports.Any())
                            {
                                companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                if (policy.Summary.Risks.Count > 0)
                                {
                                    companyPolicy.Summary = policy.Summary;
                                }
                                risks = CreateDeclaration(companyTransports, companyPolicy);
                                //risks = CreateDeclaration(companyTransports.Where(x => x.Risk.RiskId == (int)formValues["RiskId"]).ToList(), companyPolicy);
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
                    policy.Summary = risks.Where(x=>x.RiskId == (int)formValues["RiskId"]).FirstOrDefault().Policy.Summary;
                    //policy.Summary = risks.First().Policy.Summary;
                    //policy.Summary.AmountInsured = risks.First().AmountInsured;
                    policy.Summary.AmountInsured = risks.Where(x => x.RiskId == (int)formValues["RiskId"]).FirstOrDefault().AmountInsured;

                }
                //if (policy.PolicyType.IsFloating)
                //{
                //    Task.Run(() => { StoreDeclarationDetailPolicyPeriod(ModelAssembler.CreateCompanyEndorsementDetail(policy, companyTransports.First())); });
                //}
                return policy;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error  al Crear Endoso de Declaración" + ex);
            }
        }
        /// <summary>
        /// Create Declaration
        /// </summary>
        /// <param name="companyTransports"></param>
        /// <returns></returns>
        private List<CompanyRisk> CreateDeclaration(List<CompanyTransport> companyTransports, CompanyPolicy policy)
        {
            if (companyTransports != null && companyTransports.Any())
            {
                List<CompanyRisk> risks = new List<CompanyRisk>();
                PendingOperation pendingOperation = new PendingOperation();

                if ((bool)companyTransports.First()?.Risk?.Policy.Product.IsCollective)
                {
                    if (companyTransports.First().Risk.Policy.Endorsement.TemporalId > 0)
                    {
                        companyTransports = DelegateService.transportServices.GetCompanyTransportsByTemporalId(companyTransports.First().Risk.Policy.Endorsement.TemporalId);
                        foreach (var item in companyTransports)
                        {
                            item.Risk.Policy.CurrentFrom = policy.CurrentFrom;
                            item.Risk.Policy.CurrentTo = policy.CurrentTo;
                            item.Risk.IsPersisted = true;
                            var risk = GetDataDeclaration(item, policy);
                            risk = DelegateService.transportServices.CreateCompanyTransportTemporal(risk);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        }
                        var transportPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);
                        transportPolicy = DelegateService.underwritingService.CreatePolicyTemporal(transportPolicy, false);
                        return risks;
                    }
                    else
                    {
                        companyTransports = DelegateService.transportServices.GetCompanyTransportsByPolicyId(companyTransports.First().Risk.Policy.Endorsement.PolicyId);
                        TP.Parallel.ForEach(companyTransports, item =>
                        {
                            item.Risk.Policy.CurrentFrom = policy.CurrentFrom;
                            item.Risk.Policy.CurrentTo = policy.CurrentTo;
                            item.Risk.IsPersisted = true;
                            var risk = GetDataDeclaration(item, policy);
                            risk = DelegateService.transportServices.CreateCompanyTransportTemporal(risk);
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
                    foreach (var item in companyTransports)
                    {
                        item.Risk.Policy.CurrentFrom = policy.Endorsement.CurrentFrom;
                        item.Risk.Policy.CurrentTo = policy.Endorsement.CurrentTo;
                        item.Risk.IsPersisted = true;
                        var risk = GetDataDeclaration(item, policy);
                        item.Risk.OriginalStatus = RiskStatusType.Modified;
                        item.Risk.Status = RiskStatusType.Modified;
                        risk = DelegateService.transportServices.CreateCompanyTransportTemporal(risk);
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
        /// Get Data Declaration
        /// </summary>
        /// <param name="risk"></param>
        /// <returns></returns>
        private CompanyTransport GetDataDeclaration(CompanyTransport risk, CompanyPolicy policy)
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
                    item.CurrentFromOriginal = risk.Risk.Policy.CurrentFrom;
                    item.CurrentToOriginal = risk.Risk.Policy.CurrentTo;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = 0;
                    item.SubLineBusiness = coverageLocal.SubLineBusiness;
                    item.IsSelected = coverageLocal.IsSelected;
                    item.IsMandatory = coverageLocal.IsMandatory;
                    item.IsVisible = coverageLocal.IsVisible;
                    item.Rate =  item.Rate;
                    item.DepositPremiumPercent = item.DepositPremiumPercent;
                    item.PremiumAmount = item.PremiumAmount;
                    if (item.InsuredObject.Id == risk.Risk.Policy.Endorsement.InsuredObjectId)
                    {
                        item.DeclaredAmount = (decimal)risk.Risk.Policy.Endorsement.DeclaredValue;
                        item.LimitAmount = (decimal)risk.Risk.Policy.Endorsement.DeclaredValue;
                    }
                    else
                    {
                        item.DeclaredAmount = 0;
                        item.LimitAmount = 0;
                    }
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }
            risk = DelegateService.transportServices.QuotateCompanyTransport(risk, false, false);
            return risk;
        }

        //public void StoreDeclarationDetailPolicyPeriod(CompanyEndorsementDetail detail)
        //{
        //    Dictionary<string, object> result = new Dictionary<string, object>();
        //    try
        //    {
        //        int version;
        //        CompanyEndorsementPeriod period = GetDetailPolicy(detail.PolicyId, out version);
        //        List<CompanyEndorsementDetail> details = GetDetailEndorsements(detail.PolicyId, version);
        //        int count = details.Where(x => x.EndorsementType == (int)EndorsementType.DeclarationEndorsement && x.RiskNum == detail.RiskNum && x.InsuredObjectId == detail.InsuredObjectId && x.PolicyId == period.PolicyId && x.Version == period.Version).Count();
        //        if (count < period.TotalDeclarations)
        //        {
        //            detail.Version = period.Version;
        //            detail.EndorsementDate = DateTime.Now;
        //            ExecSaveTempEndorsementDetail(detail);

        //        }
        //        else
        //        {
        //            result.Add("Message", string.Format("Los endosos de Declaracion para el riesgo y el objeto del seguro ya se realizaron"));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}

        //public CompanyEndorsementPeriod GetDetailPolicy(decimal policyId, out int version)
        //{
        //    try
        //    {
        //        version = 0;
        //        NameValue[] parameters = new NameValue[1];
        //        CompanyEndorsementPeriod companyEndorsementPeriod = new CompanyEndorsementPeriod();
        //        DatatableToList dataTable = new DatatableToList();
        //        parameters[0] = new NameValue("@POLICY_ID", policyId);
        //        DataSet result;
        //        using (DynamicDataAccess pdb = new DynamicDataAccess())
        //        {
        //            result = pdb.ExecuteSPDataSet("ISS.GET_ENDORSEMENT_COUNT_PERIOD", parameters);
        //        }
        //        if (result != null && result.Tables[0].Rows.Count > 0)
        //        {
        //            companyEndorsementPeriod = dataTable.ConvertTo<CompanyEndorsementPeriod>(result.Tables[0]).FirstOrDefault();
        //            version = companyEndorsementPeriod.Version;
        //        }
        //        return companyEndorsementPeriod;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public List<CompanyEndorsementDetail> GetDetailEndorsements(decimal policyId, int version)
        //{
        //    try
        //    {
        //        NameValue[] parameters = new NameValue[2];
        //        List<CompanyEndorsementDetail> companyEndorsementPeriod = new List<CompanyEndorsementDetail>();
        //        DatatableToList dataTable = new DatatableToList();
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

        //public CompanyEndorsementDetail ExecSaveTempEndorsementDetail(CompanyEndorsementDetail saveData)
        //{
        //    try
        //    {
        //        NameValue[] parameters = new NameValue[8];
        //        CompanyEndorsementDetail resultEndorsementDetail = new CompanyEndorsementDetail();

        //        parameters[0] = new NameValue("@POLICY_ID", resultEndorsementDetail.PolicyId);
        //        parameters[1] = new NameValue("@ENDORSEMENT_TYPE", resultEndorsementDetail.EndorsementType);
        //        parameters[2] = new NameValue("@RISK_ID", resultEndorsementDetail.RiskNum);
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
