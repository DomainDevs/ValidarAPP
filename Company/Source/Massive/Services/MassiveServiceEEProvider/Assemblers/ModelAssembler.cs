using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMENCOM = Sistran.Company.Application.Common.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using REQEN = Sistran.Company.Application.Request.Entities;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Company.Application.MassiveServices.EEProvider.Entities.View;
using Sistran.Core.Application.MassiveServices.Models;
using MSVEN = Sistran.Core.Application.Massive.Entities;
using UUSMOD = Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Company.Application.MassiveServices.EEProvider.Assemblers
{
    public static class ModelAssembler
    {

        #region CompanyRequest
        /// <summary>
        /// Crea un Model.CompanyRequest a partir de un Entity.CompanyRequest 
        /// </summary>
        /// <param name="coRequest">Entity.coRequest</param>
        /// <returns></returns>
        public static CompanyRequest CreateCoRequest(REQEN.CoRequest coRequest)
        {
            return new CompanyRequest
            {
                Id = coRequest.RequestId,
                Branch = new Branch {
                    Id = coRequest.BranchCode
                },
                //Description = coRequest.Description,
                Prefix = new Prefix
                {
                    Id = coRequest.PrefixCode
                },
                RequestDate = coRequest.RequestDate
            };
        }

        /// <summary>
        /// Crea un Model.CompanyRequest a partir de un Entity.CompanyRequest y de unas listas de  businessCollection         
        /// </summary>
        /// <param name="coRequestEntity">Entity.coRequest</param>        
        /// <param name="collectionEndorsement">lista de  Entity.CoRequestEndorsement</param>
        /// <param name="collectionEndoAgn">lista de  Entity.CoRequestEndorsementAgent</param>
        /// <param name="collectionAgent">lista de  Entity.CoRequestAgent</param>
        ///<param name="businessCollectionAgent">lista de  Entity.Agent </param>
        ///<param name="businessCollectionBillingGroup">lista de  Entity.BillingGroup</param>
        /// <returns></returns>
        public static CompanyRequest CreateCoRequest(REQEN.CoRequest coRequestEntity, BusinessCollection collectionEndorsement, BusinessCollection collectionEndoAgn, BusinessCollection collectionAgent, BusinessCollection businessCollectionAgent, BusinessCollection businessCollectionBillingGroup)
        {
            BillingGroup billingroup = new BillingGroup();
            foreach (ISSEN.BillingGroup itembillingGroup in businessCollectionBillingGroup)
            {
                if (coRequestEntity.BillingGroupCode == itembillingGroup.BillingGroupCode)
                {
                    billingroup.Id = itembillingGroup.BillingGroupCode;
                    billingroup.Description = itembillingGroup.Description;
                }
            }

            CompanyRequest coRequest = new CompanyRequest
            {
                Id = coRequestEntity.RequestId,
                Branch = new Branch
                {
                    Id = coRequestEntity.BranchCode
                },
                Description = coRequestEntity.Description,
                Prefix = new Prefix
                {
                    Id = coRequestEntity.PrefixCode
                },
                RequestDate = coRequestEntity.RequestDate,
                BillingGroup = billingroup,
                BusinessType = (BusinessType)coRequestEntity.BusinessTypeCode
            };

            List<CompanyRequestEndorsement> coRequestEndorsements = new List<CompanyRequestEndorsement>();
            foreach (REQEN.CoRequestEndorsement field in collectionEndorsement)
            {
                if (field.RequestId == coRequestEntity.RequestId)
                {
                    CompanyRequestEndorsement coRequestEndorsement = new CompanyRequestEndorsement();

                    int productId = field.ProductId != null ? (int)field.ProductId : 0;
                    coRequestEndorsement.Product = new Product
                    {
                        Id = productId
                    };
                    coRequestEndorsement.Id = field.RequestEndorsementId;                    
                    coRequestEndorsement.EndorsementType = (EndorsementType)field.EndoTypeCode;
                    coRequestEndorsement.DocumentNumber = field.DocumentNum;
                    coRequestEndorsement.EndorsementDate = field.EndorsementDate;
                    coRequestEndorsement.Holder = new Holder
                    {
                        IndividualId = field.PolicyHolderId
                    };
                    coRequestEndorsement.CurrentFrom = field.CurrentFrom;
                    coRequestEndorsement.CurrentTo = field.CurrentTo;
                    coRequestEndorsement.MonthPayerDay = field.MonthlyPayDay != null ? (Int16)field.MonthlyPayDay : (Int16)0;
                    coRequestEndorsement.IssueExpensesAmount = field.IssueExpensesAmount;
                    coRequestEndorsement.UserId = field.UserId;
                    coRequestEndorsement.IsOpenEffect = field.IsOpenEffect;
                    coRequestEndorsement.Annotations = field.Annotations;
                    int prefixCode = field.PrefixCode != null ? (int)field.PrefixCode : 0;
                    coRequestEndorsement.Prefix = new Prefix
                    {
                        Id = prefixCode
                    };
                    int policyType = field.PolicyTypeCode != null ? (int)field.PolicyTypeCode : 0;
                    coRequestEndorsement.PolicyType = new PolicyType
                    {
                        Id = policyType
                    };
                    coRequestEndorsement.GiftAmount = field.AmtGift;
                    coRequestEndorsement.PaymentPlan = new PaymentPlan
                    {
                        Id = field.PaymentScheduleId
                    };

                    coRequestEndorsement.Agencies = new List<IssuanceAgency>();

                    List<UPEN.Agent> AgentList = businessCollectionAgent.Cast<UPEN.Agent>().ToList();
                    foreach (REQEN.CoRequestEndorsementAgent endoAgent in collectionEndoAgn)
                    {
                        if (coRequestEndorsement.Id == endoAgent.RequestEndorsementId)
                        {
                            foreach (REQEN.CoRequestAgent agent in collectionAgent)
                            {
                                if (endoAgent.RequestAgentId == agent.RequestAgentId)
                                {
                                    UPEN.Agent Agent = AgentList.Where(c => c.IndividualId == agent.IndividualId).SingleOrDefault();
                                    IssuanceAgency agency = new IssuanceAgency
                                    {
                                        Id = agent.AgentAgencyId,
                                        Agent = new IssuanceAgent
                                        {
                                            IndividualId = agent.IndividualId,
                                            FullName = Agent.CheckPayableTo,

                                        },
                                        AgentType = new IssuanceAgentType
                                        {
                                            Id = Agent.AgentTypeCode
                                        },
                                        IsPrincipal = agent.IsPrimary,
                                        Participation = agent.ParticipationPercentage
                                        
                                    };

                                    coRequestEndorsement.Agencies.Add(agency);
                                }
                            }
                        }
                    }
                    
                    coRequestEndorsements.Add(coRequestEndorsement);
                }
            }

            coRequest.CompanyRequestEndorsements = coRequestEndorsements;
            return coRequest;
        }

        /// <summary>
        /// Crea una lista de Model.CompanyRequest a partir de unas listas de  businessCollection 
        /// </summary>
        /// <param name="businessCollection">lista de  Entity.CompanyRequest </param>
        /// <param name="businessCollectionEndo">lista de  Entity.CoRequestEndorsement</param>
        /// <param name="businessCollectionEndoAgn">lista de  Entity.CoRequestEndorsementAgent</param>
        /// <param name="businessCollectionAgn">lista de  Entity.CoRequestAgent</param>
        /// <param name="businessCollectionAgent">lista de  Entity.Agent </param>
        /// <param name="businessCollectionBillingGroup">lista de  Entity.BillingGroup</param>
        /// <returns></returns>
        public static List<CompanyRequest> CreateCoRequests(BusinessCollection businessCollection, BusinessCollection businessCollectionEndo, BusinessCollection businessCollectionEndoAgn, BusinessCollection businessCollectionAgn, BusinessCollection businessCollectionAgent, BusinessCollection businessCollectionBillingGroup)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<CompanyRequest> coRequest = new List<CompanyRequest>();

            foreach (REQEN.CoRequest field in businessCollection)
            {
                coRequest.Add(ModelAssembler.CreateCoRequest(field, businessCollectionEndo, businessCollectionEndoAgn, businessCollectionAgn, businessCollectionAgent, businessCollectionBillingGroup));
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.MassiveServices.EEProvider.Assemblers.CreateCoRequests");

            return coRequest;
        }

        #endregion


        public static List<CompanyRequest> CreateCoRequests(BusinessCollection businessCollection)
        {
            List<CompanyRequest> coRequests = new List<CompanyRequest>();

            foreach (REQEN.CoRequest entity in businessCollection)
            {
                coRequests.Add(ModelAssembler.CreateCoRequest(entity));
            }

            return coRequests;
        }
        //#region CoRequestCoinsuranceAccepted
        ///// <summary>
        ///// Crea un Model.CompanyRequest a partir de un Entity.CompanyRequest 
        ///// </summary>
        ///// <param name="coRequest">Entity.coRequest</param>
        ///// <returns></returns>
        public static CompanyIssuanceCoInsuranceCompany CreateCoRequestCoinsuranceAccepted(Core.Application.Request.Entities.CoRequestCoinsuranceAccepted coRequestCoinsuranceAccepted)
        {
            return new CompanyIssuanceCoInsuranceCompany
            {
                Id = coRequestCoinsuranceAccepted.InsuranceCompanyId,
                ParticipationPercentage = coRequestCoinsuranceAccepted.PartCiaPercentage,
                ParticipationPercentageOwn = coRequestCoinsuranceAccepted.PartMainPercentage,
                PolicyNumber = coRequestCoinsuranceAccepted.PolicyNumMain,
                EndorsementNumber = coRequestCoinsuranceAccepted.AnnexNumMain,
                ExpensesPercentage = coRequestCoinsuranceAccepted.ExpensesPercentage
            };
        }

        #region CoRequestCoinsuranceAssigned
        /// <summary>
        /// Crea un Model.CompanyRequest a partir de un Entity.CompanyRequest 
        /// </summary>
        /// <param name="coRequest">Entity.coRequest</param>
        /// <returns></returns>
        public static List<CompanyIssuanceCoInsuranceCompany> CreateCoRequestCoinsuranceAssigned(CoRequestCoinsuranceAssignedView coRequestCoinsuranceAssignedView)
        {
            string Description = String.Empty;
            List<CompanyIssuanceCoInsuranceCompany> coRequests = new List<CompanyIssuanceCoInsuranceCompany>();
            foreach (Core.Application.Request.Entities.CoRequestCoinsuranceAssigned entity in coRequestCoinsuranceAssignedView.CoRequestCoinsuranceAssigned)
            {
                Description = coRequestCoinsuranceAssignedView.CoInsuranceCompany.Cast<COMMEN.CoInsuranceCompany>().Where(x => x.InsuranceCompanyId == entity.InsuranceCompanyId).FirstOrDefault().Description;
                coRequests.Add(ModelAssembler.CreateCoRequestCoinsuranceAssigned(entity, Description));
            }

            return coRequests;
        }
        /// <summary>
        /// Crea un Model.CompanyRequest a partir de un Entity.CompanyRequest 
        /// </summary>
        /// <param name="coRequest">Entity.coRequest</param>
        /// <returns></returns>
        public static CompanyIssuanceCoInsuranceCompany CreateCoRequestCoinsuranceAssigned(Core.Application.Request.Entities.CoRequestCoinsuranceAssigned coRequestCoinsuranceAssigned, string Description)
        {
            return new CompanyIssuanceCoInsuranceCompany
            {
                Id = coRequestCoinsuranceAssigned.InsuranceCompanyId,
                ParticipationPercentage = coRequestCoinsuranceAssigned.PartCiaPercentage,
                ExpensesPercentage = coRequestCoinsuranceAssigned.ExpensesPercentage,
                Description = Description
            };
        }
        #endregion

        #region Product

        internal static List<Product> CreateProducts(BusinessCollection businessCollection)
        {
            List<Product> products = new List<Product>();

            foreach (PRODEN.Product entityProduct in businessCollection)
            {
                products.Add(CreateProduct(entityProduct));
            }

            return products;
        }

        internal static Product CreateProduct(PRODEN.Product entityProduct)
        {
            return new Product
            {
                Id = entityProduct.ProductId,
                Description = entityProduct.Description
            };
        }

        #endregion


        #region Prefix

        public static Prefix CreatePrefix(COMMEN.Prefix entityPrefix)
        {
            return new Prefix
            {
                Id = entityPrefix.PrefixCode,
                Description = entityPrefix.Description,
                SmallDescription = entityPrefix.SmallDescription,
                TinyDescription = entityPrefix.TinyDescription
            };
        }

        public static List<Prefix> CreatePrefixes(BusinessCollection businessCollection)
        {
            List<Prefix> prefixes = new List<Prefix>();

            foreach (COMMEN.Prefix entity in businessCollection)
            {
                prefixes.Add(ModelAssembler.CreatePrefix(entity));
            }

            return prefixes;
        }

        #endregion

        

        internal static List<MassiveLoad> CreateMassiveLoads(BusinessCollection businessCollection)
        {
            List<MassiveLoad> massiveLoads = new List<MassiveLoad>();

            foreach (MSVEN.MassiveLoad entityMassiveLoad in businessCollection)
            {
                massiveLoads.Add(ModelAssembler.CreateMassiveLoad(entityMassiveLoad));
            }

            return massiveLoads;
        }

        internal static MassiveLoad CreateMassiveLoad(MSVEN.MassiveLoad entityMassiveLoad)
        {
            MassiveLoad massiveLoad = new MassiveLoad
            {
                Id = entityMassiveLoad.Id,
                LoadType = new LoadType()
                {
                    Id = entityMassiveLoad.LoadTypeId.Value
                },
                File = new Core.Services.UtilitiesServices.Models.File()
                {
                    Name = entityMassiveLoad.FileName
                },
                User = new UUSMOD.User
                {
                    UserId = entityMassiveLoad.UserId
                },
                Description = entityMassiveLoad.Description,
                Status = (MassiveLoadStatus)entityMassiveLoad.StatusId,
                HasError = entityMassiveLoad.HasError,
                ErrorDescription = entityMassiveLoad.ErrorDescription,
                TotalRows = entityMassiveLoad.TotalRows.GetValueOrDefault()
            };

            if (massiveLoad.ErrorDescription != null)
            {
                string[] errorMessages = massiveLoad.ErrorDescription.Split('(');
                massiveLoad.ErrorDescription = errorMessages[0];
            }

            return massiveLoad;
        }


        public static CompanyAcceptCoInsuranceAgent MapCompanyAcceptCoInsuranceAgentFromAgency(IssuanceAgency issuanceAgency)
        {
            CompanyAcceptCoInsuranceAgent companyAcceptCoInsuranceAgent = new CompanyAcceptCoInsuranceAgent()
            {

                Agent = new CompanyPolicyAgent()
                {
                    IndividualId = issuanceAgency.Agent.IndividualId,
                    Id = issuanceAgency.Id,
                    FullName = issuanceAgency.Agent.FullName
                },
                ParticipationPercentage = issuanceAgency.Participation

            };
            return companyAcceptCoInsuranceAgent;
        }

        public static IMapper CreateMapCompanyClause()
        {
            var config = MapperCache.GetMapper<Clause, CompanyClause>(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });
            return config;
        }

    }
}