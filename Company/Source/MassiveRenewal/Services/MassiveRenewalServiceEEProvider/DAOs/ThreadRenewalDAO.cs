using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.MassiveRenewalServices.EEProvider.DAOs;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Threading.Tasks;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Application.ProductServices.Models;

namespace Sistran.Company.Application.MassiveRenewalServices.EEProvider.DAOs
{
    public class ThreadRenewalDAO
    {        
        public void InicializeRenewal(int userId, int processId, List<CompanyPolicy> policies)
        {
            AsynchronousProcess asynchronousProcess = new AsynchronousProcess();
            asynchronousProcess.Id = processId;
            asynchronousProcess.Active = true;
            asynchronousProcess.StatusId = (int)ProcessRenewalStatus.Temporals;

            DelegateService.utilitiesService.UpdateAsynchronousProcess(asynchronousProcess);

            try
            {
                ParallelHelper.ForEach(policies, (policy) =>
                {
                    policy.UserId = userId;
                    ExecuteRenewalProcess(policy, processId);
                });
            }
            catch (BusinessException ex)
            {
                asynchronousProcess.EndDate = DateTime.Now;
                asynchronousProcess.HasError = true;
                asynchronousProcess.ErrorDescription = ex.ToString();
                asynchronousProcess.Active = false;
                asynchronousProcess.StatusId = (int)ProcessRenewalStatus.Finalized;

                DelegateService.utilitiesService.UpdateAsynchronousProcess(asynchronousProcess);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        public void ExecuteRenewalProcess(CompanyPolicy policy, int processId)
        {
            MassiveRenewalRow massiveRenewalRow = new MassiveRenewalRow();
            massiveRenewalRow.Id = processId;
            //Validar
           // massiveRenewalRow.Risk.Policy = policy;

            MassiveRenewalDAO massiveRenewalDAO = new MassiveRenewalDAO();
            //renewalProcess = renewalProcessDAO.CreateRenewalProcess(renewalProcess);

            try
            {
                policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);

                switch (policy.Product.CoveredRisk.SubCoveredRiskType)
                {
                    case SubCoveredRiskType.JudicialSurety:
                        break;
                    case SubCoveredRiskType.Liability:
                        CreateTemporalLiability(policy, massiveRenewalRow);
                        break;
                    case SubCoveredRiskType.Property:
                        CreateTemporalProperty(policy, massiveRenewalRow);
                        break;
                    case SubCoveredRiskType.Surety:
                        break;
                    case SubCoveredRiskType.ThirdPartyLiability:
                        CreateTemporalThirdPartyLiability(policy, massiveRenewalRow);
                        break;
                    case SubCoveredRiskType.Vehicle:
                        //Validar mapeo
                        //CreateTemporalVehicle(policy, massiveRenewalRow);
                        break;
                }
            }
            catch (Exception ex)
            {
                massiveRenewalRow.HasError = true;
                massiveRenewalRow.Observations = ex.ToString();
                //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);

                throw new Exception(ex.ToString());
            }
        }
        
        private void CreateTemporalProperty(CompanyPolicy policy, MassiveRenewalRow massiveRenewalRow)
        {
            MassiveRenewal massiveRenewalDAO = new MassiveRenewal();
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policy.Endorsement.PolicyId, policy.Endorsement.Id);
            List<CompanyPropertyRisk> companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(policy.Endorsement.PolicyId);

            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Validation;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);

            companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentTo;
            companyPolicy.CurrentTo = companyPolicy.CurrentFrom.AddYears(1);
            companyPolicy.Endorsement.EndorsementType = EndorsementType.Renewal;
            companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
            companyPolicy.TemporalType = TemporalType.Endorsement;
            companyPolicy.UserId = policy.UserId;
            companyPolicy.Branch = policy.Branch;
            
            companyPolicy.Agencies.First(x => x.IsPrincipal).FullName = policy.Agencies.First().FullName;

            int index = companyPolicy.Agencies.FindIndex(x => x.IsPrincipal);
            ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(companyPolicy.Agencies[index].Agent.IndividualId, companyPolicy.Agencies[index].Id, companyPolicy.Product.Id);

            foreach (IssuanceAgency agency in companyPolicy.Agencies)
            {
                foreach (IssuanceCommission commission in agency.Commissions)
                {
                    commission.Percentage = productAgencyCommiss.CommissPercentage;
                    commission.PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault();
                }
            }
            companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);

            companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);

            //Valida conversión implícita
            //companyPolicy.Clauses = DelegateService.underwritingServiceCore.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, companyPolicy.Prefix.Id).Where(x => x.IsMandatory).ToList();

           // Mapper.CreateMap(companyPolicy.GetType(), policy.GetType());
          //  Mapper.Map(companyPolicy, policy);

            PendingOperation pendingOperation = new PendingOperation();

            pendingOperation.UserId = policy.UserId;
            pendingOperation.OperationName = "Temporal";
            pendingOperation.Operation = JsonConvert.SerializeObject(policy);

            pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
            companyPolicy.Id = pendingOperation.Id;

            foreach (CompanyPropertyRisk property in companyPropertyRisks)
            {
                //Validar modelo
                //property.Status = RiskStatusType.Original;

                List<CompanyInsuredObject> insuredObjects = property.Risk.Coverages.GroupBy(i => i.InsuredObject.Id, (key, group) => group.First().InsuredObject).ToList();

                foreach (CompanyInsuredObject insuredObject in insuredObjects)
                {
                    //Validar property.GroupCoverage.Id
                    //List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObject.Id, property.GroupCoverage.Id, companyPolicy.Product.Id);
                    List<CompanyCoverage> coverages = new List<CompanyCoverage>();

                    foreach (CompanyCoverage coverage in property.Risk.Coverages)
                    {
                        coverage.CurrentFrom = companyPolicy.CurrentFrom;
                        coverage.CurrentTo = companyPolicy.CurrentTo;
                        coverage.CoverStatus = CoverageStatusType.Original;
                        coverage.Description = coverages.FirstOrDefault(x => x.Id == coverage.Id).Description;
                        coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                        coverage.AccumulatedPremiumAmount = 0;
                        coverage.FlatRatePorcentage = 0;
                        coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                        coverage.InsuredObject.Amount = insuredObjects.FirstOrDefault(u => u.Id == insuredObject.Id).Amount;
                        coverage.RuleSetId = coverages.First(x => x.Id == coverage.Id).RuleSetId;
                        coverage.PosRuleSetId = coverages.First(x => x.Id == coverage.Id).PosRuleSetId;
                        coverage.IsVisible = coverages.FirstOrDefault(u => u.Id == coverage.Id).IsVisible;
                        coverage.IsMandatory = coverages.FirstOrDefault(u => u.Id == coverage.Id).IsMandatory;
                        coverage.IsSelected = coverages.FirstOrDefault(u => u.Id == coverage.Id).IsSelected;
                        coverage.MainCoverageId = coverages.FirstOrDefault(u => u.Id == coverage.Id).MainCoverageId;
                    }
                }
                
                pendingOperation = new PendingOperation();

                pendingOperation.UserId = policy.UserId;
                pendingOperation.ParentId = companyPolicy.Id;
                pendingOperation.Operation = JsonConvert.SerializeObject(property);

                pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
                //Validar
                //property.Id = pendingOperation.Id;
            }

            massiveRenewalRow.Risk.Policy.Id = companyPolicy.Id;
            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Tariff;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);

            //Validar
           // companyPropertyRisks = DelegateService.propertyService.QuotateProperties(companyPropertyRisks, false, true);

            List<Risk> risks = new List<Risk>();

            foreach (CompanyPropertyRisk property in companyPropertyRisks)
            {
                //risks.Add(property);
            }

            //Validar converstión de tipos
            //companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponents(policy, risks,true);
            //companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(policy, risks);

            //companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policy);

            //companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(companyPolicy, risks);

          /*  Mapper.CreateMap(companyPolicy.GetType(), policy.GetType());
            Mapper.Map(companyPolicy, policy);
*/
            pendingOperation = new PendingOperation();
            pendingOperation.Id = companyPolicy.Id;
            pendingOperation.UserId = companyPolicy.UserId;
            pendingOperation.OperationName = "Temporal";
            pendingOperation.Operation = JsonConvert.SerializeObject(policy);

            pendingOperation = DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);

            foreach (CompanyPropertyRisk property in companyPropertyRisks)
            {
                pendingOperation = new PendingOperation();
                //Validar
                //pendingOperation.Id = property.Id;
                pendingOperation.UserId = policy.UserId;
                pendingOperation.ParentId = companyPolicy.Id;
                pendingOperation.Operation = JsonConvert.SerializeObject(property);

                pendingOperation = DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
            }

            //pendiente ejecucion de eventos
            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Events;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);

            DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, true);

            foreach (CompanyPropertyRisk property in companyPropertyRisks)
            {
                DelegateService.propertyService.CreatePropertyTemporal(property, true);
            }

            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Finalized;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);
        }

        private void CreateTemporalLiability(CompanyPolicy policy, MassiveRenewalRow massiveRenewalRow)
        {
            MassiveRenewalDAO renewalProcessDAO = new MassiveRenewalDAO();
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policy.Endorsement.PolicyId, policy.Endorsement.Id);
            List<CompanyLiabilityRisk> liabilityPolicy = null;//DelegateService.liabilityService.GetLiabilityPolicyByPolicyIdEndorsementId(policy.Endorsement.PolicyId, policy.Endorsement.Id);

            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Validation;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);

            companyPolicy.CurrentFrom = policy.Endorsement.CurrentTo;
            companyPolicy.CurrentTo = policy.CurrentFrom.AddYears(1);
            companyPolicy.Endorsement.EndorsementType = EndorsementType.Renewal;
            companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
            companyPolicy.TemporalType = TemporalType.Endorsement;
            companyPolicy.UserId = policy.UserId;
            companyPolicy.Branch = policy.Branch;
            companyPolicy.Agencies.First(x => x.IsPrincipal).FullName = policy.Agencies.First().FullName;

            int index = companyPolicy.Agencies.FindIndex(x => x.IsPrincipal);
            ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(companyPolicy.Agencies[index].Agent.IndividualId, companyPolicy.Agencies[index].Id, companyPolicy.Product.Id);

            foreach (IssuanceAgency agency in companyPolicy.Agencies)
            {
                foreach (IssuanceCommission commission in agency.Commissions)
                {
                    commission.Percentage = productAgencyCommiss.CommissPercentage;
                    commission.PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault();
                }
            }
            companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);

            companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);

            //Validar conversión de tipos
            //companyPolicy.Clauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, companyPolicy.Prefix.Id).Where(x => x.IsMandatory).ToList();

            PendingOperation pendingOperation = new PendingOperation();
            pendingOperation.UserId = companyPolicy.UserId;
            pendingOperation.OperationName = "Temporal";
            pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);

            pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
            companyPolicy.Id = pendingOperation.Id;

            foreach (CompanyLiabilityRisk liability in liabilityPolicy)
            {
                //Validar modelo
                //liability.Status = RiskStatusType.Original;

                //Validar propiedad coverage
                //List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, liability.GroupCoverage.Id, companyPolicy.Prefix.Id);
                List<CompanyCoverage> coverages = new List<CompanyCoverage>();

                foreach (CompanyCoverage coverage in liability.Risk.Coverages)
                {
                    coverage.CurrentFrom = companyPolicy.CurrentFrom;
                    coverage.CurrentTo = companyPolicy.CurrentTo;
                    coverage.CoverStatus = CoverageStatusType.Original;
                    coverage.Description = coverages.FirstOrDefault(x => x.Id == coverage.Id).Description;
                    coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                    coverage.AccumulatedPremiumAmount = 0;
                    coverage.FlatRatePorcentage = 0;
                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                    coverage.RuleSetId = coverages.First(x => x.Id == coverage.Id).RuleSetId;
                    coverage.PosRuleSetId = coverages.First(x => x.Id == coverage.Id).PosRuleSetId;
                    coverage.IsVisible = coverages.FirstOrDefault(u => u.Id == coverage.Id).IsVisible;
                    coverage.IsMandatory = coverages.FirstOrDefault(u => u.Id == coverage.Id).IsMandatory;
                    coverage.IsSelected = coverages.FirstOrDefault(u => u.Id == coverage.Id).IsSelected;
                }

                pendingOperation = new PendingOperation();

                pendingOperation.UserId = companyPolicy.UserId;
                pendingOperation.ParentId = companyPolicy.Id;
                pendingOperation.Operation = JsonConvert.SerializeObject(liability);

                pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
                //Validar modelo
                //liability.Id = pendingOperation.Id;
            }

            massiveRenewalRow.Risk.Policy.Id = companyPolicy.Id;
            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Tariff;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);

            //Validar
            //liabilityPolicy = DelegateService.liabilityService.QuotationLiabilityRisk(liabilityPolicy, false, true);

            List<Risk> risks = new List<Risk>();

            foreach (CompanyLiabilityRisk liability in liabilityPolicy)
            {
                //risks.Add(liability);
            }

            //Validar covenrsión de tipos
            //companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponents(policy, risks,true);
            //companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(policy, risks);

            //companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policy);

            //companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(companyPolicy, risks);

            pendingOperation = new PendingOperation();
            pendingOperation.Id = companyPolicy.Id;
            pendingOperation.UserId = companyPolicy.UserId;
            pendingOperation.OperationName = "Temporal";
            pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);

            pendingOperation = DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);

            foreach (CompanyLiabilityRisk liability in liabilityPolicy)
            {
                pendingOperation = new PendingOperation();
                pendingOperation.Id = companyPolicy.Id;
                pendingOperation.UserId = companyPolicy.UserId;
                pendingOperation.ParentId = companyPolicy.Id;
                pendingOperation.Operation = JsonConvert.SerializeObject(liabilityPolicy);

                pendingOperation = DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
            }

            //pendiente ejecucion de eventos
            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Events;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);

            DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, true);

            foreach (CompanyLiabilityRisk liability in liabilityPolicy)
            {
                DelegateService.liabilityService.CreateLiabilityTemporal(liability, true);
            }

            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Finalized;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);
        }

        private void CreateTemporalVehicle(Policy policy, MassiveRenewalRow massiveRenewalRow)
        {
            MassiveRenewalDAO massiveRenewalDAO = new MassiveRenewalDAO();

            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policy.Endorsement.PolicyId, policy.Endorsement.Id);
            

            List<CompanyVehicle> companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(policy.Endorsement.PolicyId);

            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Validation;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);

            companyPolicy.IsPersisted = true;
            companyPolicy.CurrentFrom = policy.Endorsement.CurrentTo;
            companyPolicy.CurrentTo = companyPolicy.CurrentFrom.AddYears(1);
            companyPolicy.Endorsement.EndorsementType = EndorsementType.Renewal;
            companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
            companyPolicy.TemporalType = TemporalType.Endorsement;
            companyPolicy.UserId = policy.UserId;
            //Validar conversión de tipos
            //companyPolicy.Branch = policy.Branch;
            companyPolicy.Agencies.First(x => x.IsPrincipal).FullName = policy.Agencies.First().FullName;

            int index = companyPolicy.Agencies.FindIndex(x => x.IsPrincipal);
            ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(companyPolicy.Agencies[index].Agent.IndividualId, companyPolicy.Agencies[index].Id, companyPolicy.Product.Id);

            foreach (IssuanceAgency agency in companyPolicy.Agencies)
            {
                foreach (IssuanceCommission commission in agency.Commissions)
                {
                    commission.Percentage = productAgencyCommiss.CommissPercentage;
                    commission.PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault();
                }
            }

            companyPolicy.ExchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, companyPolicy.ExchangeRate.Currency.Id);

            companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);

            companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);

            //Validar conversión de tipos
            //companyPolicy.Clauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, companyPolicy.Prefix.Id).Where(x => x.IsMandatory).ToList();

            foreach (CompanyVehicle companyVehicle in companyVehicles)
            {
                //Validar propiedades no definidas
                //companyVehicle.IsPersisted = true;
                //companyVehicle.Status = RiskStatusType.Original;
                companyVehicle.StandardVehiclePrice = companyVehicle.Price;

                //Validar propiedad companyVehicle.GroupCoverage
                //List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, companyVehicle.GroupCoverage.Id, companyPolicy.Prefix.Id);
                List<CompanyCoverage> coverages = new List<CompanyCoverage>();

                companyVehicle.Risk.Coverages = coverages;

                foreach (CompanyCoverage coverage in companyVehicle.Risk.Coverages)
                {
                    coverage.CurrentFrom = companyPolicy.CurrentFrom;
                    coverage.CurrentTo = companyPolicy.CurrentTo;
                    coverage.CoverStatus = CoverageStatusType.Original;
                    coverage.Description = coverages.FirstOrDefault(x => x.Id == coverage.Id).Description;
                    coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                    coverage.AccumulatedPremiumAmount = 0;
                    coverage.FlatRatePorcentage = 0;
                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                    coverage.RuleSetId = coverages.First(x => x.Id == coverage.Id).RuleSetId;
                    coverage.PosRuleSetId = coverages.First(x => x.Id == coverage.Id).PosRuleSetId;
                }
            }

            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Tariff;
            //Validar
            //companyVehicles[0].CompanyPolicy = companyPolicy;

            //Validar 
            //companyVehicles = DelegateService.vehicleService.QuotateVehicles(companyVehicles, true, true);
            List<Risk> risks = new List<Risk>();
            foreach (CompanyVehicle companyVehicle in companyVehicles)
            {
                //risks.Add(companyVehicle);
            }

            //Validar
            //companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponents(companyPolicy, risks,true);
            //companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(companyPolicy, risks);

            //companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(companyPolicy);

            //companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(companyPolicy, risks);
            
            PendingOperation pendingOperation = new PendingOperation();
            
            pendingOperation.UserId = policy.UserId;
            pendingOperation.OperationName = "Temporal";
            pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);

            pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
            companyPolicy.Id = pendingOperation.Id;
            
            foreach (CompanyVehicle companyVehicle in companyVehicles)
            {
                pendingOperation = new PendingOperation();
                //Validar  modelo
                //pendingOperation.Id = companyVehicle.Id;
                pendingOperation.UserId = policy.UserId;
                pendingOperation.ParentId = companyPolicy.Id;
                pendingOperation.Operation = JsonConvert.SerializeObject(companyVehicle);

                pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
                //Validar modelo
                //companyVehicle.Id = pendingOperation.Id;
            }

            //pendiente ejecucion de eventos
            massiveRenewalRow.Risk.Policy.Id = companyPolicy.Id;
            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Events;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);

            DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, true);

            foreach (CompanyVehicle vehicle in companyVehicles)
            {
                //Validar
                //vehicle.CompanyPolicy = companyPolicy;
                DelegateService.vehicleService.CreateVehicleTemporal(vehicle,true);
            }

            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Finalized;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);
        }

        private void CreateTemporalThirdPartyLiability(CompanyPolicy policy, MassiveRenewalRow massiveRenewalRow)
        {
            MassiveRenewalDAO massiveRenewalDAO = new MassiveRenewalDAO();
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policy.Endorsement.PolicyId, policy.Endorsement.Id);
            //se comentarearon las dos siguientes lineas temporalmente
            //List<CompanyTplRisk> companyTplRisks = DelegateService.thirdPartyLiabilityService.GetCompanyThirdPartyLiabilitiesByPolicyId(policy.Endorsement.PolicyId);
            List<CompanyTplRisk> companyTplRisks = new List<CompanyTplRisk>();

            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Validation;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);

            companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentTo;
            companyPolicy.CurrentTo = companyPolicy.CurrentFrom.AddYears(1);
            companyPolicy.Endorsement.EndorsementType = EndorsementType.Renewal;
            companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
            companyPolicy.TemporalType = TemporalType.Endorsement;
            companyPolicy.UserId = policy.UserId;
            companyPolicy.Branch = policy.Branch;
            companyPolicy.Agencies.First(x => x.IsPrincipal).FullName = policy.Agencies.First().FullName;

            int index = companyPolicy.Agencies.FindIndex(x => x.IsPrincipal);
            ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(companyPolicy.Agencies[index].Agent.IndividualId, companyPolicy.Agencies[index].Id, companyPolicy.Product.Id);

            foreach (IssuanceAgency agency in companyPolicy.Agencies)
            {
                foreach (IssuanceCommission commission in agency.Commissions)
                {
                    commission.Percentage = productAgencyCommiss.CommissPercentage;
                    commission.PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault();
                }
            }
            companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);

            companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);

            //Validar conversión de tipos
            //companyPolicy.Clauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, companyPolicy.Prefix.Id).Where(x => x.IsMandatory).ToList();

            PendingOperation pendingOperation = new PendingOperation();
            

            pendingOperation.UserId = companyPolicy.UserId;
            pendingOperation.OperationName = "Temporal";
            pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);

            pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
            companyPolicy.Id = pendingOperation.Id;

            foreach (CompanyTplRisk companyTplRisk in companyTplRisks)
            {
                //Validar modelo
                //companyTplRisk.Status = RiskStatusType.Original;

                //Validar propiedad companyTplRisk.GroupCoverage
                //List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, companyTplRisk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                List<CompanyCoverage> coverages = new List<CompanyCoverage>();

                foreach (CompanyCoverage coverage in companyTplRisk.Risk.Coverages)
                {
                    coverage.CurrentFrom = companyPolicy.CurrentFrom;
                    coverage.CurrentTo = companyPolicy.CurrentTo;
                    coverage.CoverStatus = CoverageStatusType.Original;
                    coverage.Description = coverages.FirstOrDefault(x => x.Id == coverage.Id).Description;
                    coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                    coverage.AccumulatedPremiumAmount = 0;
                    coverage.FlatRatePorcentage = 0;
                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                    coverage.RuleSetId = coverages.First(x => x.Id == coverage.Id).RuleSetId;
                    coverage.PosRuleSetId = coverages.First(x => x.Id == coverage.Id).PosRuleSetId;
                }

                pendingOperation = new PendingOperation();

                pendingOperation.UserId = companyPolicy.UserId;
                pendingOperation.ParentId = companyPolicy.Id;
                pendingOperation.Operation = JsonConvert.SerializeObject(companyTplRisk);

                pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
                //Validar modelo
                //companyTplRisk.Id = pendingOperation.Id;
            }

            massiveRenewalRow.Risk.Policy.Id = companyPolicy.Id;
            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Tariff;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);

            //Validar
            //companyTplRisks[0].CompanyPolicy = companyPolicy;
            //companyTplRisks = DelegateService.thirdPartyLiabilityService.QuotateThirdPartyLiabilities(companyTplRisks, true, true);

            List<Risk> risks = new List<Risk>();

            foreach (CompanyTplRisk companyTplRisk in companyTplRisks)
            {
                //risks.Add(companyTplRisk);
            }

            //Validar
            //companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponents(companyPolicy, risks,true);
            //companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(companyPolicy, risks);

            //companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(companyPolicy);

            //companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(companyPolicy, risks);

            pendingOperation = new PendingOperation();
            pendingOperation.Id = companyPolicy.Id;
            pendingOperation.UserId = companyPolicy.UserId;
            pendingOperation.OperationName = "Temporal";
            pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);

            pendingOperation = DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);

            foreach (CompanyTplRisk companyTplRisk in companyTplRisks)
            {
                pendingOperation = new PendingOperation();
                //Validar modelo
                //pendingOperation.Id = companyTplRisk.Id;
                pendingOperation.UserId = companyPolicy.UserId;
                pendingOperation.ParentId = companyPolicy.Id;
                pendingOperation.Operation = JsonConvert.SerializeObject(companyTplRisk);

                pendingOperation = DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
            }

            //pendiente ejecucion de eventos
            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Events;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);
            DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, true);

            foreach (CompanyTplRisk companyTplRisk in companyTplRisks)
            {
                //Validar
                //companyTplRisk.CompanyPolicy = companyPolicy;
                //DelegateService.thirdPartyLiabilityService.CreateThirdPartyLiabilityTemporal(companyTplRisk, true);
            }

            massiveRenewalRow.Status = Core.Application.MassiveUnderwritingServices.Enums.MassiveLoadProcessStatus.Finalized;
            //renewalProcess = renewalProcessDAO.UpdateRenewalProcess(renewalProcess);
        }
    }
}