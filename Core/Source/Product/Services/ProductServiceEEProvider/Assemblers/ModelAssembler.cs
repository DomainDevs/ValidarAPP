using AutoMapper;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using PROMDL = Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Framework.DAF;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Core.Application.ProductServices.EEProvider.Assemblers
{
    using Model = Models;
    using PersonEnt = UniquePerson.Entities;
    using PRODEN = Product.Entities;
    public class ModelAssembler
    {
        #region productos
        public static Model.ProductFinancialPlan CreateProductFinancialPlan(PRODEN.ProductFinancialPlan productFinancialPlan)
        {
            return new Model.ProductFinancialPlan
            {
                Id = productFinancialPlan.FinancialPlanId,
                ProductId = productFinancialPlan.ProductId,
                IsDefault = productFinancialPlan.IsDefault
            };
        }

        public static List<Model.ProductFinancialPlan> CreateProductFinancialPlans(BusinessCollection collectionProductFinancialPlan)
        {
            List<Model.ProductFinancialPlan> productFinancialPlans = new List<Model.ProductFinancialPlan>();

            foreach (PRODEN.ProductFinancialPlan item in collectionProductFinancialPlan)
            {
                productFinancialPlans.Add(ModelAssembler.CreateProductFinancialPlan(item));
            }

            return productFinancialPlans;
        }

        #region ProductAgent

        public static List<Model.ProductAgent> ProductAgents(BusinessCollection businessCollection)
        {
            ConcurrentBag<Model.ProductAgent> productAgents = new ConcurrentBag<Model.ProductAgent>();
            TP.Parallel.ForEach(businessCollection.Cast<PRODEN.ProductAgent>().ToList(), item =>
            {
                productAgents.Add(ModelAssembler.ProductAgent(item));
            });

            return productAgents.ToList();
        }

        public static Model.ProductAgent ProductAgent(PRODEN.ProductAgent productAgent)
        {
            return new Model.ProductAgent
            {
                IndividualId = productAgent.IndividualId,
                ProductId = productAgent.ProductId
            };
        }

        #endregion

        #region ProductAgencyCommissions

        public static List<Model.ProductAgencyCommiss> ProductAgencyCommissions(BusinessCollection businessCollection)
        {
            ConcurrentBag<Model.ProductAgencyCommiss> productAgencyCommissions = new ConcurrentBag<Model.ProductAgencyCommiss>();

            TP.Parallel.ForEach(businessCollection.Cast<PRODEN.ProductAgencyCommiss>().ToList(), item =>
            {
                productAgencyCommissions.Add(ModelAssembler.ProductAgencyCommission(item));
            });
            return productAgencyCommissions.ToList();
        }
        public static List<Model.ProductAgencyCommiss> ProductAgencyCommissions(List<PRODEN.ProductAgencyCommiss> businessCollection)
        {
            ConcurrentBag<Model.ProductAgencyCommiss> productAgencyCommissions = new ConcurrentBag<Model.ProductAgencyCommiss>();

            TP.Parallel.ForEach(businessCollection, item =>
            {
                productAgencyCommissions.Add(ModelAssembler.ProductAgencyCommission(item));
            });
            return productAgencyCommissions.ToList();
        }

        public static Model.ProductAgencyCommiss ProductAgencyCommission(PRODEN.ProductAgencyCommiss productAgencyCommission)
        {
            return new Model.ProductAgencyCommiss
            {
                IndividualId = productAgencyCommission.IndividualId,
                ProductId = productAgencyCommission.ProductId,
                CommissPercentage = productAgencyCommission.StCommissPercentage,
                AdditionalCommissionPercentage = productAgencyCommission.AdditCommissPercentage,
                AgencyId = productAgencyCommission.AgentAgencyId,

            };
        }

        #endregion

        #region Agency

        public static PROMDL.ProductAgency CreateAgency(PersonEnt.AgentAgency agency)
        {
            return new PROMDL.ProductAgency
            {
                Id = agency.AgentAgencyId,
                Code = agency.AgentCode,
                FullName = agency.Description,
                DateDeclined = agency.DeclinedDate,
                //Branch = new CommonModel.Branch()
                //{
                //    Id = agency.BranchCode
                //},
                Agent = new PROMDL.ProductAgent
                {
                    IndividualId = agency.IndividualId
                },
                //AgentType = new PersonModel.AgentType
                //{
                //    Id = agency.AgentTypeCode
                //},
                //AgentDeclinedType = new PersonModel.AgentDeclinedType
                //{
                //    Id = agency.AgentDeclinedTypeCode
                //}
            };
        }

        public static List<PROMDL.ProductAgency> CreateAgencies(BusinessCollection businessCollection)
        {
            List<PROMDL.ProductAgency> agencies = new List<PROMDL.ProductAgency>();
            foreach (PersonEnt.AgentAgency field in businessCollection)
            {
                agencies.Add(ModelAssembler.CreateAgency(field));

            }
            return agencies;
        }

        #endregion
        #region Agent

        /// <summary>
        /// Creates the agents.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<PROMDL.ProductAgent> CreateAgents(BusinessCollection businessCollection)
        {
            //Mapper.CreateMap<PersonEnt.Agent, PersonModel.Agent>();
            //return Mapper.Map<List<PersonEnt.Agent>, List<PersonModel.Agent>>(businessCollection.Cast<PersonEnt.Agent>().ToList());
            ConcurrentBag<PROMDL.ProductAgent> agents = new ConcurrentBag<PROMDL.ProductAgent>();
            TP.Parallel.ForEach(businessCollection.Cast<PersonEnt.Agent>().ToList(), field =>
            {
                agents.Add(ModelAssembler.CreateAgent(field));
            });

            return agents.ToList();
        }
        #endregion
        /// <summary>
        /// Creates the agent.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <returns></returns>
        public static PROMDL.ProductAgent CreateAgent(PersonEnt.Agent agent)
        {
            return new PROMDL.ProductAgent
            {
                IndividualId = agent.IndividualId,
                AgentType = new PROMDL.ProductAgentType() { Id = agent.AgentTypeCode },
                FullName = agent.CheckPayableTo,
                //AgentDeclinedType = new PersonModel.AgentDeclinedType() { Id = agent.AgentDeclinedTypeCode },
                //DateCurrent = agent.EnteredDate,
                //DateDeclined = agent.DeclinedDate,
                //Annotations = agent.Annotations,
                //DateModification = agent.ModifyDate
            };
        }

        #endregion
        #region Currency

        private static Currency CreateCurrency(COMMEN.Currency entityCurrency)
        {
            return new Currency
            {
                Id = entityCurrency.CurrencyCode,
                Description = entityCurrency.Description,
                SmallDescription = entityCurrency.SmallDescription,
                TinyDescription = entityCurrency.TinyDescription
            };
        }

        public static List<Currency> CreateCurrencies(BusinessCollection businessCollection)
        {
            var currencies = businessCollection.Cast<COMMEN.Currency>().ToList();
            var imaper = CreateMapCurrencie();
            return imaper.Map<List<COMMEN.Currency>, List<Currency>>(currencies);
          
        }
        private static PROMDL.ProductCurrency CreateProductCurrency(PRODEN.ProductCurrency entityCurrency)
        {
            return new PROMDL.ProductCurrency
            {
                Id = entityCurrency.CurrencyCode,
                DecimalQuantity = entityCurrency.DecimalQuantity,
                ProductId = entityCurrency.ProductId
            };
        }

        public static List<PROMDL.ProductCurrency> CreateProductCurrencies(BusinessCollection businessCollection)
        {
            var currencies = businessCollection.Cast<PRODEN.ProductCurrency>().ToList();
            var imaper = CreateMapProductCurrencie();
            return imaper.Map<List<PRODEN.ProductCurrency>, List<PROMDL.ProductCurrency>>(currencies);

        }

        #endregion

        #region CoveredRisk
        public static List<PROMDL.CoveredRisk> CreateCoveredRisks(BusinessCollection businessCollection)
        {
            List<PROMDL.CoveredRisk> coveredRisks = new List<PROMDL.CoveredRisk>();

            foreach (PRODEN.ProductCoverRiskType field in businessCollection)
            {
                coveredRisks.Add(ModelAssembler.CreateCoveredRisk(field));
            }

            return coveredRisks;
        }

        public static PROMDL.CoveredRisk CreateCoveredRisk(PRODEN.ProductCoverRiskType productCoverRiskType)
        {
            return new PROMDL.CoveredRisk
            {
                MaxRiskQuantity = productCoverRiskType.MaxRiskQuantity,
                RuleSetId = productCoverRiskType.RuleSetId,
                PreRuleSetId = productCoverRiskType.PreRuleSetId,
                CoveredRiskType = (CoveredRiskType)productCoverRiskType.CoveredRiskTypeCode,
                ScriptId = productCoverRiskType.ScriptId
            };
        }

        #endregion
        #region Auttomaper
        public static IMapper CreateMapCurrencie()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<COMMEN.Currency, Currency>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CurrencyCode));
            });
            return config.CreateMapper();
        }
        public static IMapper CreateMapProductCurrencie()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PRODEN.ProductCurrency, PROMDL.ProductCurrency>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CurrencyCode))
               .ForMember(dest => dest.DecimalQuantity, opt => opt.MapFrom(src => src.DecimalQuantity));
            });
            return config.CreateMapper();
        }
        #endregion Auttomaper


    }
}
