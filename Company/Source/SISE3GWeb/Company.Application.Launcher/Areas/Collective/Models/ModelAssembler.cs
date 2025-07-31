namespace Sistran.Core.Framework.UIF.Web.Areas.Collective.Models
{
    using AutoMapper;
    using Sistran.Company.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.CollectiveServices.Models;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.ProductServices.Models;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.UniqueUserServices.Models;
    using Sistran.Core.Application.Utilities.Cache;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using Sistran.Core.Services.UtilitiesServices.Models;
    public class ModelAssembler
    {
        public static CollectiveEmission CreateCollectiveEmission(CollectiveModelView collectiveViewModel)
        {
            CollectiveEmission collectiveEmission = new CollectiveEmission
            {
                LoadType = new Application.MassiveServices.Models.LoadType
                {
                    Id = collectiveViewModel.LoadTypeId,
                },
                Agency = new Application.UnderwritingServices.Models.IssuanceAgency
                {
                    Id = collectiveViewModel.AgencyId,
                    Agent = new Application.UnderwritingServices.Models.IssuanceAgent
                    {
                        IndividualId = collectiveViewModel.AgentId
                    }
                },
                Prefix = new Prefix
                {
                    Id = collectiveViewModel.PrefixId
                },
                Branch = new Branch
                {
                    Id = collectiveViewModel.BranchId
                },
                Description = collectiveViewModel.LoadName,

                File = new File()
                {
                    Name = collectiveViewModel.FileName
                },
                Product = new Product
                { Id = collectiveViewModel.ProductId },
                Id = collectiveViewModel.Id,
                IsAutomatic = collectiveViewModel.IsAutomatic,
                User = new User { UserId = SessionHelper.GetUserId() }
            };
            int temp = 0;
            if (int.TryParse(collectiveViewModel.TempId, out temp))
            {
                collectiveEmission.TemporalId = temp;
            }
            return collectiveEmission;
        }
        #region AutoMapper
        #region Collective
        public static IMapper CreateMappClauses()
        {
            var config = MapperCache.GetMapper<Clause, CompanyClause>(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();

            });
            return config;
        }
        public static IMapper CreateMappRiskActivity()
        {
            var config = MapperCache.GetMapper<RiskActivity, CompanyRiskActivity>(cfg =>
            {
                cfg.CreateMap<RiskActivity, CompanyRiskActivity>();

            });
            return config;
        }

        #endregion
        #endregion
    }
}