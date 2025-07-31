using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Sureties.SuretyServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.Sureties.SuretyServices.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using System;
using Sistran.Core.Application.RulesScriptsServices.Models;
using UND = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Sureties.SuretyServices.EEProvider.Assembler;
using Sistran.Core.Application.Sureties.SuretyServices.EEProvider.Models;

namespace Sistran.Core.Application.Sureties.SuretyServices.EEProvider.DAOs
{
    public class ContactDAO
    {
        public List<Contract> GetSuretiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            List<Contract> Contracts = new List<Contract>();

            switch (moduleType)
            {
                case ModuleType.Emission:
                    return Contracts;
                case ModuleType.Claim:
                    return GetSuretiesByEndorsementIdModuleType(endorsementId);
                default:
                    return Contracts;
            }
        }

        private List<Contract> GetSuretiesByEndorsementIdModuleType(int endorsementId)
        {
            List<Contract> contracts = new List<Contract>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            ClaimRiskSuretyView view = new ClaimRiskSuretyView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskSuretyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                List<ISSEN.Risk> entityRisks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.Policy> entityPolicies = view.Policies.Cast<ISSEN.Policy>().ToList();
                List<ISSEN.RiskSurety> entityRiskSureties = view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                foreach (ISSEN.Risk entityRisk in entityRisks)
                {
                    ISSEN.EndorsementRisk entityEndorsementRisk = entityEndorsementRisks.First(x => x.RiskId == entityRisk.RiskId);
                    ISSEN.Policy entityPolicy = entityPolicies.First(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                    ISSEN.RiskSurety entityRiskSurety = entityRiskSureties.FirstOrDefault(x => x.RiskId == entityRisk.RiskId);

                    ContractMapper contractDto = new ContractMapper();
                    contractDto.entityRisk = entityRisk;
                    contractDto.entityEndorsementRisk = entityEndorsementRisk;
                    contractDto.entityRiskSurety = entityRiskSurety;
                    contractDto.entityPolicy = entityPolicy;

                    Contract contract = ModelAssembler.CreateContract(contractDto);

                    if (contract != null)
                    {
                        ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                        filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                        filterSumAssured.Equal();
                        filterSumAssured.Constant(endorsementId);
                        SumAssuredView assuredView = new SumAssuredView();
                        ViewBuilder builderAssured = new ViewBuilder("SumAssuredView");
                        builderAssured.Filter = filterSumAssured.GetPredicate();
                        DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                        decimal insuredAmount = 0;

                        foreach (ISSEN.RiskCoverage entityRiskCoverage in assuredView.RiskCoverages)
                        {
                            insuredAmount += entityRiskCoverage.LimitAmount;
                        }

                        contract.Risk.AmountInsured = insuredAmount;

                        if (contract != null)
                        {
                            IssuanceInsured contractor = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(contract.Contractor.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                            if (contractor != null)
                            {
                                contract.Contractor.Name = contractor.IndividualType == IndividualType.Person ? (
                                        contractor.Surname + " " + (string.IsNullOrEmpty(contractor.SecondSurname) ? "" : contractor.SecondSurname + " ") + contractor.Name
                                        ) : contractor.Name;

                                contract.Contractor.IdentificationDocument = contractor.IdentificationDocument;

                                contracts.Add(contract);
                            }
                        }
                    }

                }
            }

            return contracts;
        }


        public List<Contract> GetRisksSuretyByInsuredId(int insuredId)
        {
            List<Contract> contracts = new List<Contract>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);            

            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);            

            ClaimRiskSuretyView view = new ClaimRiskSuretyView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskSuretyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                List<ISSEN.Risk> entityRisks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.Policy> entityPolicies = view.Policies.Cast<ISSEN.Policy>().ToList();
                List<ISSEN.RiskSurety> entityRiskSureties = view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                foreach (ISSEN.Risk entityRisk in entityRisks)
                {
                    ISSEN.EndorsementRisk entityEndorsementRisk = entityEndorsementRisks.First(x => x.RiskId == entityRisk.RiskId);
                    ISSEN.Policy entityPolicy = entityPolicies.First(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                    ISSEN.RiskSurety entityRiskSurety = entityRiskSureties.FirstOrDefault(x => x.RiskId == entityRisk.RiskId);

                    ContractMapper contractDto = new ContractMapper();
                    contractDto.entityRisk = entityRisk;
                    contractDto.entityEndorsementRisk = entityEndorsementRisk;
                    contractDto.entityRiskSurety = entityRiskSurety;
                    contractDto.entityPolicy = entityPolicy;
                    Contract contract = ModelAssembler.CreateContract(contractDto);

                    if (contract != null)
                    {
                        IssuanceInsured contractor = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(contract.Contractor.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                        if (contractor != null)
                        {
                            contract.Contractor.Name = contractor.IndividualType == IndividualType.Person ? (
                                    contractor.Surname + " " + (string.IsNullOrEmpty(contractor.SecondSurname) ? "" : contractor.SecondSurname + " ") + contractor.Name
                                    ) : contractor.Name;

                            contract.Contractor.IdentificationDocument = contractor.IdentificationDocument;

                            contracts.Add(contract);
                        }
                    }
                }
            }

            return contracts;
        }


        public List<Contract> GetRisksSuretyBySuretyId(int suretyId)
        {
            List<Contract> contracts = new List<Contract>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskSurety.Properties.IndividualId, typeof(ISSEN.RiskSurety).Name);
            filter.Equal();
            filter.Constant(suretyId);

            ClaimRiskSuretyView view = new ClaimRiskSuretyView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskSuretyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                List<ISSEN.Risk> entityRisks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.Policy> entityPolicies = view.Policies.Cast<ISSEN.Policy>().ToList();
                List<ISSEN.RiskSurety> entityRiskSureties = view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                foreach (ISSEN.Risk entityRisk in entityRisks)
                {
                    ISSEN.EndorsementRisk entityEndorsementRisk = entityEndorsementRisks.First(x => x.RiskId == entityRisk.RiskId);
                    ISSEN.Policy entityPolicy = entityPolicies.First(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                    ISSEN.RiskSurety entityRiskSurety = entityRiskSureties.FirstOrDefault(x => x.RiskId == entityRisk.RiskId);

                    
                    ContractMapper contractDto = new ContractMapper();
                    contractDto.entityRisk = entityRisk;
                    contractDto.entityEndorsementRisk = entityEndorsementRisk;
                    contractDto.entityRiskSurety = entityRiskSurety;
                    contractDto.entityPolicy = entityPolicy;
                    Contract contract = ModelAssembler.CreateContract(contractDto);

                    if (contract != null)
                    {
                        IssuanceInsured contractor = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(contract.Contractor.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                        if (contractor != null)
                        {
                            contract.Contractor.Name = contractor.IndividualType == IndividualType.Person ? (
                                    contractor.Surname + " " + (string.IsNullOrEmpty(contractor.SecondSurname) ? "" : contractor.SecondSurname + " ") + contractor.Name
                                    ) : contractor.Name;

                            contract.Contractor.IdentificationDocument = contractor.IdentificationDocument;

                            contracts.Add(contract);
                        }
                    }
                }
            }

            return contracts;
        }

        public Contract GetSuretyByRiskIdModuleType(int riskId, ModuleType moduleType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.RiskId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(riskId);

            ClaimRiskSuretyView riskSuretyView = new ClaimRiskSuretyView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskSuretyView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, riskSuretyView);

            if (riskSuretyView.Risks.Count > 0)
            {
                ISSEN.Risk entityRisk = riskSuretyView.Risks.Cast<ISSEN.Risk>().FirstOrDefault();
                ISSEN.EndorsementRisk entityEndorsementRisk = riskSuretyView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().FirstOrDefault(x => x.RiskId == entityRisk.RiskId);
                ISSEN.Policy entityPolicy = riskSuretyView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                ISSEN.RiskSurety entityRiskSurety = riskSuretyView.RiskSureties.Cast<ISSEN.RiskSurety>().FirstOrDefault(x => x.RiskId == entityRisk.RiskId);

                ContractMapper contractDto = new ContractMapper();
                contractDto.entityRisk = entityRisk;
                contractDto.entityEndorsementRisk = entityEndorsementRisk;
                contractDto.entityRiskSurety = entityRiskSurety;
                contractDto.entityPolicy = entityPolicy;
                Contract contract = ModelAssembler.CreateContract(contractDto);

                if (contract.Contractor != null)
                {
                    IssuanceInsured contractor = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(contract.Contractor.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                    if (contractor != null)
                    {
                        contract.Contractor.Name = contractor.IndividualType == IndividualType.Person ? (
                                contractor.Surname + " " + (string.IsNullOrEmpty(contractor.SecondSurname) ? "" : contractor.SecondSurname + " ") + contractor.Name
                                ) : contractor.Name;

                        contract.Contractor.IdentificationDocument = contractor.IdentificationDocument;
                    }
                    else
                    {
                        return null;
                    }
                }

                return contract;
            }
            return null;
        }

        public List<Contract> GetRisksBySurety(string description)
        {
            List<Contract> contracts = new List<Contract>();

            List<IssuanceInsured> insureds = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, InsuredSearchType.DocumentNumber, CustomerType.Individual);

            if (insureds.Count > 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.RiskSurety.Properties.IndividualId, typeof(ISSEN.RiskSurety).Name);
                filter.In();
                filter.ListValue();
                insureds.ForEach(x => filter.Constant(x.IndividualId));
                filter.EndList();

                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(true);

                ClaimRiskSuretyView view = new ClaimRiskSuretyView();
                ViewBuilder builder = new ViewBuilder("ClaimRiskSuretyView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.Risks.Count > 0)
                {
                    List<ISSEN.Risk> entityRisks = view.Risks.Cast<ISSEN.Risk>().ToList();
                    List<ISSEN.Policy> entityPolicies = view.Policies.Cast<ISSEN.Policy>().ToList();
                    List<ISSEN.RiskSurety> entityRiskSureties = view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
                    List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                    foreach (ISSEN.Risk entityRisk in entityRisks)
                    {
                        ISSEN.EndorsementRisk entityEndorsementRisk = entityEndorsementRisks.First(x => x.RiskId == entityRisk.RiskId);
                        ISSEN.Policy entityPolicy = entityPolicies.First(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                        ISSEN.RiskSurety entityRiskSurety = entityRiskSureties.FirstOrDefault(x => x.RiskId == entityRisk.RiskId);

                        ContractMapper contractDto = new ContractMapper();
                        contractDto.entityRisk = entityRisk;
                        contractDto.entityEndorsementRisk = entityEndorsementRisk;
                        contractDto.entityRiskSurety = entityRiskSurety;
                        contractDto.entityPolicy = entityPolicy;

                        
                        Contract contract = ModelAssembler.CreateContract(contractDto);

                        if (contract != null)
                        {
                            IssuanceInsured contractor = insureds.FirstOrDefault(x => x.IndividualId == contract.Contractor.IndividualId);

                            if (contractor != null)
                            {
                                contract.Contractor.Name = contractor.IndividualType == IndividualType.Person ? (
                                        contractor.Surname + " " + (string.IsNullOrEmpty(contractor.SecondSurname) ? "" : contractor.SecondSurname + " ") + contractor.Name
                                        ) : contractor.Name;

                                contract.Contractor.IdentificationDocument = contractor.IdentificationDocument;

                                contracts.Add(contract);
                            }
                        }
                    }
                }
            }

            return contracts;
        }
    }
}
