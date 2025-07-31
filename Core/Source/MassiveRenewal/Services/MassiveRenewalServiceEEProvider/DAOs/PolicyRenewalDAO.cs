using Newtonsoft.Json;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Collections.Generic;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using EnumsUnCo = Sistran.Core.Application.UnderwritingServices.Enums;


namespace Sistran.Core.Application.MassiveRenewalServices.EEProvider.DAOs
{
    class PolicyRenewalDAO
    {
        /// <summary>
        /// Obtener Pólizas Por Fecha De Vencimiento
        /// </summary>
        /// <param name="policy">Parametros De Busqueda</param>
        /// <returns>Lista De Pólizas</returns>
        public List<Policy> GetPoliciesByDueDate(Policy policy)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Issuance.Entities.Policy.Properties.PrefixCode, typeof(Issuance.Entities.Policy).Name).Equal().Constant(policy.Prefix.Id);
            filter.And();
            filter.Property(Issuance.Entities.Endorsement.Properties.CurrentTo, typeof(Issuance.Entities.Endorsement).Name).GreaterEqual().Constant(policy.CurrentFrom);
            filter.And();
            filter.Property(Issuance.Entities.Endorsement.Properties.CurrentTo, typeof(Issuance.Entities.Endorsement).Name).LessEqual().Constant(policy.CurrentTo);
            filter.And();
            filter.Property(Product.Entities.Product.Properties.IsCollective, typeof(Product.Entities.Product).Name).Equal().Constant(false);
            filter.And();
            filter.Property(Issuance.Entities.Policy.Properties.NextPolicyId, typeof(Issuance.Entities.Policy).Name).IsNull();
            if (policy.Product != null && policy.Product.Id > 0)
            {
                filter.And();
                filter.Property(Product.Entities.Product.Properties.ProductId, typeof(Product.Entities.Product).Name).Equal().Constant(policy.Product.Id);
            }
            if (policy.Agencies != null && policy.Agencies.Count > 0 && policy.Agencies[0].Agent != null && policy.Agencies[0].Agent.IndividualId > 0 && policy.Agencies[0].Id > 0)
            {
                filter.And();
                filter.Property(Issuance.Entities.PolicyAgent.Properties.IndividualId, typeof(Issuance.Entities.PolicyAgent).Name).Equal().Constant(policy.Agencies[0].Agent.IndividualId);
                filter.And();
                filter.Property(Issuance.Entities.PolicyAgent.Properties.AgentAgencyId, typeof(Issuance.Entities.PolicyAgent).Name).Equal().Constant(policy.Agencies[0].Id);
            }

            if (policy.Branch != null && policy.Branch.Id > 0)
            {
                filter.And();
                filter.Property(Issuance.Entities.Policy.Properties.BranchCode, typeof(Issuance.Entities.Policy).Name).Equal().Constant(policy.Branch.Id);
            }                     

            if (policy.Holder != null && policy.Holder.IndividualId > 0)
            {
                filter.And();
                filter.Property(Issuance.Entities.Policy.Properties.PolicyholderId, typeof(Issuance.Entities.Policy).Name).Equal().Constant(policy.Holder.IndividualId);
            }

            if (policy.Request != null && policy.Request.Id > 0)
            {
                filter.And();
                filter.Property(Issuance.Entities.CoPolicy.Properties.RequestId, typeof(Issuance.Entities.CoPolicy).Name).Equal().Constant(policy.Request.Id);
            }

            List<UnderwritingServices.Models.Policy> policies = new List<UnderwritingServices.Models.Policy>();

            PolicyRenewalView policyRenewalView = new PolicyRenewalView();
            ViewBuilder builder = new ViewBuilder("PolicyRenewalView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, policyRenewalView);

            if (policyRenewalView.Policies.Count > 0)
            {
                filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name).In().ListValue();

                foreach (ISSEN.Policy entityPolicy in policyRenewalView.Policies)
                {
                    List<Issuance.Entities.GroupEndorsement> groupEndorsements = policyRenewalView.GroupEndorsements.Cast<Issuance.Entities.GroupEndorsement>().Where(x => x.PolicyId == entityPolicy.PolicyId).ToList();
                    List<Issuance.Entities.GroupEndorsement> groupEndorsementsCancelled = groupEndorsements.Where(x => x.RefEndorsementId.HasValue).ToList();

                    groupEndorsements = groupEndorsements.Where(x => !groupEndorsementsCancelled.Any(y => y.EndorsementId == x.EndorsementId || y.RefEndorsementId == x.EndorsementId)).ToList();

                    if (groupEndorsements.Count > 0)
                    {
                    if (groupEndorsements.Last().EndoTypeCode != (int)EnumsUnCo.EndorsementType.Cancellation)
                    {
                        Issuance.Entities.Endorsement entityEndorsement = policyRenewalView.Endorsements.Cast<Issuance.Entities.Endorsement>().First(x => x.EndorsementId == groupEndorsements.Last().EndorsementId);
                        Common.Entities.Branch entityBranch = policyRenewalView.Branches.Cast<Common.Entities.Branch>().First(x => x.BranchCode == entityPolicy.BranchCode);
                        Product.Entities.Product entityProduct = policyRenewalView.Products.Cast<Product.Entities.Product>().First(x => x.ProductId == entityPolicy.ProductId);
                        Temporary.Entities.TempSubscription tempSubscription = policyRenewalView.TempSubscriptions.Cast<Temporary.Entities.TempSubscription>().FirstOrDefault(x => x.EndorsementId == entityEndorsement.EndorsementId);

                            Policy searchPolicy = new Policy
                            {
                            UserId = entityEndorsement.UserId,
                            DocumentNumber = entityPolicy.DocumentNumber,
                            Endorsement = new UnderwritingServices.Models.Endorsement
                            {
                                Id = entityEndorsement.EndorsementId,
                                PolicyId = entityEndorsement.PolicyId,
                                CurrentFrom = entityEndorsement.CurrentFrom,
                                CurrentTo = entityEndorsement.CurrentTo.Value
                            },
                            Branch = new Branch
                            {
                                Id = entityBranch.BranchCode,
                                Description = entityBranch.Description
                            },
                            Product = new ProductServices.Models.Product
                            {
                                Id = entityProduct.ProductId,
                                Description = entityProduct.Description
                            },
                            Prefix = new Prefix
                            {
                                Id = entityPolicy.PrefixCode
                            },
                            Holder = DelegateService.underwritingServiceCore.GetHoldersByDescriptionInsuredSearchTypeCustomerType(entityPolicy.PolicyholderId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First(),
                            Agencies = new List<IssuanceAgency>(),
                            Summary = new Summary()
                        };

                        ISSEN.PolicyAgent entityPolicyAgent = policyRenewalView.PolicyAgents.Cast<ISSEN.PolicyAgent>().Where(x => x.EndorsementId == entityEndorsement.EndorsementId && x.IsPrimary).First();
                        searchPolicy.Agencies.Add(DelegateService.underwritingServiceCore.GetAgencyByAgentIdAgentAgencyId(entityPolicyAgent.IndividualId, entityPolicyAgent.AgentAgencyId));
                        searchPolicy.Agencies[0].IsPrincipal = true; 

                        if (tempSubscription != null)
                        {
                            searchPolicy.TemporalType = (EnumsUnCo.TemporalType)tempSubscription.TemporalTypeCode;
                            searchPolicy.Id = tempSubscription.OperationId.GetValueOrDefault();
                            searchPolicy.Endorsement.TemporalId = tempSubscription.TempId;
                            searchPolicy.Endorsement.EndorsementType = (EnumsUnCo.EndorsementType)tempSubscription.EndorsementTypeCode;
                        }

                            policies.Add(searchPolicy);
                            filter.Constant(searchPolicy.Endorsement.Id);
                        }
                    }
                }

                filter.EndList();

                RiskRenewalView riskRenewalView = new RiskRenewalView();
                builder = new ViewBuilder("RiskRenewalView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, riskRenewalView);

                foreach (UnderwritingServices.Models.Policy searchPolicy in policies)
                {
                    List<ISSEN.EndorsementRisk> endorsementRisks = riskRenewalView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().Where(x => x.EndorsementId == searchPolicy.Endorsement.Id).ToList();
                    List<Issuance.Entities.Risk> risks = riskRenewalView.Risks.Cast<Issuance.Entities.Risk>().Where(x => endorsementRisks.Any(y => y.RiskId == x.RiskId)).ToList();

                    if (risks.Count > 0)
                    {
                        searchPolicy.Summary.CoveredRiskType = (CoveredRiskType)risks[0].CoveredRiskTypeCode;

                        switch (searchPolicy.Summary.CoveredRiskType)
                        {
                            case CoveredRiskType.Vehicle:
                                searchPolicy.Summary.Description = string.Join(",", riskRenewalView.RiskVehicles.Cast<ISSEN.RiskVehicle>().Where(x => x.RiskId == risks[0].RiskId).Select(y => y.LicensePlate).ToArray());
                                break;
                            case CoveredRiskType.Location:
                                searchPolicy.Summary.Description = string.Join(",", riskRenewalView.RiskLocations.Cast<ISSEN.RiskLocation>().Where(x => x.RiskId == risks[0].RiskId).Select(y => y.Street).ToArray());
                                break;
                            case CoveredRiskType.Surety:
                                break;
                            case CoveredRiskType.Transport:
                                break;
                            case CoveredRiskType.Aircraft:
                                break;
                        }
                    }
                }
            }

            return policies;
        }

        /// <summary>
        /// Obtener Temporal Por Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Temporal</returns>
        public UnderwritingServices.Models.Policy GetTemporalPolicyById(int id)
        {
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(id);

            UnderwritingServices.Models.Policy policy = JsonConvert.DeserializeObject<UnderwritingServices.Models.Policy>(pendingOperation.Operation);
            policy.Id = pendingOperation.Id;

            List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(policy.Id);

            foreach (PendingOperation item in pendingOperations)
            {
                UnderwritingServices.Models.Risk risk = new UnderwritingServices.Models.Risk();
                risk = JsonConvert.DeserializeObject<UnderwritingServices.Models.Risk>(item.Operation);
                risk.Id = item.Id;

                if (policy.Summary != null)
                {
                    policy.Summary.Description = risk.Description + ", ";
                }
            }

            if (policy.Summary != null)
            {
                policy.Summary.Description = policy.Summary.Description.Remove(policy.Summary.Description.Length - 2);
            }
            else
            {
                policy.Summary = new Summary();
            }

            return policy;
        }

        /// <summary>
        /// Obtener Póliza Por Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Póliza</returns>
        public UnderwritingServices.Models.Policy GetPolicyById(int id)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Issuance.Entities.Policy.Properties.PolicyId, typeof(Issuance.Entities.Policy).Name).Equal().Constant(id);

            PolicyRenewalView policyRenewalView = new PolicyRenewalView();
            ViewBuilder builder = new ViewBuilder("PolicyRenewalView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, policyRenewalView);

            ISSEN.Policy entityPolicy = policyRenewalView.Policies.Cast<ISSEN.Policy>().First();
            Issuance.Entities.Endorsement entityEndorsement = policyRenewalView.Endorsements.Cast<Issuance.Entities.Endorsement>().First();
            Common.Entities.Branch entityBranch = policyRenewalView.Branches.Cast<Common.Entities.Branch>().First(x => x.BranchCode == entityPolicy.BranchCode);
            Product.Entities.Product entityProduct = policyRenewalView.Products.Cast<Product.Entities.Product>().First(x => x.ProductId == entityPolicy.ProductId);

            UnderwritingServices.Models.Policy policy = new UnderwritingServices.Models.Policy
            {
                DocumentNumber = entityPolicy.DocumentNumber,
                Endorsement = new UnderwritingServices.Models.Endorsement
                {
                    Id = entityEndorsement.EndorsementId,
                    PolicyId = entityEndorsement.PolicyId,
                    CurrentFrom = entityEndorsement.CurrentFrom,
                    CurrentTo = entityEndorsement.CurrentTo.Value
                },
                Branch = new Branch
                {
                    Id = entityBranch.BranchCode,
                    Description = entityBranch.Description
                },
                Product = new ProductServices.Models.Product
                {
                    Id = entityProduct.ProductId,
                    Description = entityProduct.Description
                },
                Prefix = new Prefix
                {
                    Id = entityPolicy.PrefixCode
                },
                Holder = DelegateService.underwritingServiceCore.GetHoldersByDescriptionInsuredSearchTypeCustomerType(entityPolicy.PolicyholderId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First(),
                Agencies = new List<IssuanceAgency>(),
                Summary = new Summary()
            };

            ISSEN.PolicyAgent entityPolicyAgent = policyRenewalView.PolicyAgents.Cast<ISSEN.PolicyAgent>().Where(x => x.EndorsementId == entityEndorsement.EndorsementId && x.IsPrimary).First();
            policy.Agencies.Add(DelegateService.underwritingServiceCore.GetAgencyByAgentIdAgentAgencyId(entityPolicyAgent.IndividualId, entityPolicyAgent.AgentAgencyId));

            filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name).Equal().Constant(policy.Endorsement.Id);

            RiskRenewalView riskRenewalView = new RiskRenewalView();
            builder = new ViewBuilder("RiskRenewalView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, riskRenewalView);

            if (riskRenewalView.Risks.Count > 0)
            {
                policy.Summary.Risks = new List<UnderwritingServices.Models.Risk>();

                foreach (Issuance.Entities.Risk entityRisk in riskRenewalView.Risks)
                {
                    policy.Summary.Risks.Add(new UnderwritingServices.Models.Risk
                    {
                        RiskId = entityRisk.RiskId
                    });
                }

                policy.Summary.CoveredRiskType = (CoveredRiskType)riskRenewalView.Risks.Cast<Issuance.Entities.Risk>().First().CoveredRiskTypeCode;

                switch (policy.Summary.CoveredRiskType)
                {
                    case CoveredRiskType.Vehicle:
                        policy.Summary.Description = string.Join(",", riskRenewalView.RiskVehicles.Cast<ISSEN.RiskVehicle>().Select(y => y.LicensePlate).ToArray());
                        break;
                    case CoveredRiskType.Location:
                        policy.Summary.Description = string.Join(",", riskRenewalView.RiskLocations.Cast<ISSEN.RiskLocation>().Select(y => y.Street).ToArray());
                        break;
                    case CoveredRiskType.Surety:
                        break;
                    case CoveredRiskType.Transport:
                        break;
                    case CoveredRiskType.Aircraft:
                        break;
                }
            }

            return policy;
        }

        /// <summary>
        /// Obtener Póliza Por Id De Póliza Anterior
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Póliza</returns>
        public UnderwritingServices.Models.Policy GetPolicyByPreviewPolicyId(int id)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Policy.Properties.PrevPolicyId, typeof(ISSEN.Policy).Name).Equal().Constant(id);

            PolicyRenewalView policyRenewalView = new PolicyRenewalView();
            ViewBuilder builder = new ViewBuilder("PolicyRenewalView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, policyRenewalView);

            ISSEN.Policy entityPolicy = policyRenewalView.Policies.Cast<ISSEN.Policy>().First();
            ISSEN.Endorsement entityEndorsement = policyRenewalView.Endorsements.Cast<ISSEN.Endorsement>().First();
            Common.Entities.Branch entityBranch = policyRenewalView.Branches.Cast<Common.Entities.Branch>().First(x => x.BranchCode == entityPolicy.BranchCode);
            PRODEN.Product entityProduct = policyRenewalView.Products.Cast<PRODEN.Product>().First(x => x.ProductId == entityPolicy.ProductId);

            UnderwritingServices.Models.Policy policy = new UnderwritingServices.Models.Policy
            {
                DocumentNumber = entityPolicy.DocumentNumber,
                Endorsement = new UnderwritingServices.Models.Endorsement
                {
                    Id = entityEndorsement.EndorsementId,
                    Number = entityEndorsement.DocumentNum,
                    PolicyId = entityEndorsement.PolicyId,
                    CurrentFrom = entityEndorsement.CurrentFrom,
                    CurrentTo = entityEndorsement.CurrentTo.Value
                },
                Branch = new Branch
                {
                    Id = entityBranch.BranchCode,
                    Description = entityBranch.Description
                },
                Product = new ProductServices.Models.Product
                {
                    Id = entityProduct.ProductId,
                    Description = entityProduct.Description
                },
                Prefix = new Prefix
                {
                    Id = entityPolicy.PrefixCode
                },
                Holder = DelegateService.underwritingServiceCore.GetHoldersByDescriptionInsuredSearchTypeCustomerType(entityPolicy.PolicyholderId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First(),
                Summary = GetSummary(entityEndorsement.EndorsementId),
                Agencies = new List<IssuanceAgency>()
            };

            ISSEN.PolicyAgent entityPolicyAgent = policyRenewalView.PolicyAgents.Cast<ISSEN.PolicyAgent>().Where(x => x.EndorsementId == entityEndorsement.EndorsementId && x.IsPrimary).First();
            policy.Agencies.Add(DelegateService.underwritingServiceCore.GetAgencyByAgentIdAgentAgencyId(entityPolicyAgent.IndividualId, entityPolicyAgent.AgentAgencyId));

            filter = new ObjectCriteriaBuilder();

            filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name).Equal().Constant(policy.Endorsement.Id);

            RiskRenewalView riskRenewalView = new RiskRenewalView();
            builder = new ViewBuilder("RiskRenewalView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, riskRenewalView);

            policy.Summary.Risks = new List<UnderwritingServices.Models.Risk>();

            foreach (Issuance.Entities.Risk entityRisk in riskRenewalView.Risks)
            {
                policy.Summary.Risks.Add(new UnderwritingServices.Models.Risk
                {
                    RiskId = entityRisk.RiskId
                });
            }

            policy.Summary.CoveredRiskType = (CoveredRiskType)riskRenewalView.Risks.Cast<Issuance.Entities.Risk>().First().CoveredRiskTypeCode;

            switch (policy.Summary.CoveredRiskType)
            {
                case CoveredRiskType.Vehicle:
                    policy.Summary.Description = string.Join(",", riskRenewalView.RiskVehicles.Cast<ISSEN.RiskVehicle>().Select(y => y.LicensePlate).ToArray());
                    break;
                case CoveredRiskType.Location:
                    policy.Summary.Description = string.Join(",", riskRenewalView.RiskLocations.Cast<ISSEN.RiskLocation>().Select(y => y.Street).ToArray());
                    break;
                case CoveredRiskType.Surety:
                    break;
                case CoveredRiskType.Transport:
                    break;
                case CoveredRiskType.Aircraft:
                    break;
            }

            return policy;
        }

        /// <summary>
        /// Calcular Componentes Póliza
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Resumen</returns>
        public Summary GetSummary(int endorsementId)
        {
            Summary summary = new Summary();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name).In().ListValue();
            filter.Constant(EnumsUnCo.ComponentType.Premium).Constant(EnumsUnCo.ComponentType.Expenses).Constant(EnumsUnCo.ComponentType.Taxes).EndList();

            BusinessCollection entityComponents = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Component), filter.GetPredicate()));

            filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.PayerComp.Properties.EndorsementId, typeof(ISSEN.PayerComp).Name).Equal().Constant(endorsementId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.PayerComp), filter.GetPredicate()));
            List<ISSEN.PayerComp> entityPayerComponents = businessCollection.Cast<ISSEN.PayerComp>().ToList();

            foreach (QUOEN.Component entityComponent in entityComponents)
            {
                switch ((EnumsUnCo.ComponentType)entityComponent.ComponentTypeCode)
                {
                    case EnumsUnCo.ComponentType.Premium:
                        summary.AmountInsured += entityPayerComponents.Where(x => x.ComponentCode == entityComponent.ComponentCode).Sum(y => y.CalcBaseAmount);
                        summary.Premium += entityPayerComponents.Where(x => x.ComponentCode == entityComponent.ComponentCode).Sum(y => y.ComponentAmount);
                        break;
                    case EnumsUnCo.ComponentType.Expenses:
                        summary.Expenses += entityPayerComponents.Where(x => x.ComponentCode == entityComponent.ComponentCode).Sum(y => y.ComponentAmount);
                        break;
                    case EnumsUnCo.ComponentType.Taxes:
                        summary.Taxes += entityPayerComponents.Where(x => x.ComponentCode == entityComponent.ComponentCode).Sum(y => y.ComponentAmount);
                        break;
                }
            }

            summary.FullPremium = summary.Premium + summary.Expenses + summary.Taxes;

            return summary;
        }
    }
}