using System.Collections.Generic;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
/*using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Cache;*/
using Sistran.Core.Framework.DAF;
using COLLEN = Sistran.Core.Application.Collective.Entities;
using MSVEN = Sistran.Core.Application.Massive.Entities;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UtilitiesServices.Models;
using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using COMMON=Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.CollectiveServices.EEProvider.Assemblers
{
    public static class ModelAssembler
    {
        public static List<CollectiveEmissionRow> CreateCollectiveEmissionRow(BusinessCollection businessCollection)
        {
            List<CollectiveEmissionRow> collectiveEmissionRows = new List<CollectiveEmissionRow>();

            foreach (MSVEN.CollectiveEmissionRow entityCollectiveEmissionRow in businessCollection)
            {
                collectiveEmissionRows.Add(ModelAssembler.CreateCollectiveEmissionRow(entityCollectiveEmissionRow));
            }
            return collectiveEmissionRows;
        }

        public static List<CollectiveEmissionRow> CreateCollectiveEmissionRows(BusinessCollection businessCollection)
        {
            List<CollectiveEmissionRow> collectiveEmissionRows = new List<CollectiveEmissionRow>();

            foreach (MSVEN.CollectiveEmissionRow collectiveEmissionRow in businessCollection)
            {
                collectiveEmissionRows.Add(ModelAssembler.CreateCollectiveEmissionRow(collectiveEmissionRow));
            }

            return collectiveEmissionRows;
        }

        public static CollectiveEmissionRow CreateCollectiveEmissionRow(MSVEN.CollectiveEmissionRow entityCollectiveEmissionRow)
        {
            return new CollectiveEmissionRow()
            {
                MassiveLoadId = entityCollectiveEmissionRow.MassiveLoadId,
                HasError = entityCollectiveEmissionRow.HasError,
                Id = entityCollectiveEmissionRow.Id,
                Observations = entityCollectiveEmissionRow.Observations,
                Premium = entityCollectiveEmissionRow.Premium,
                Risk = new Risk
                {
                    Description = entityCollectiveEmissionRow.RiskDescription,
                    RiskId = entityCollectiveEmissionRow.RiskId.GetValueOrDefault(),
                    Id = entityCollectiveEmissionRow.RiskId.GetValueOrDefault(),
                    Number = entityCollectiveEmissionRow.RowNumber
                },
                RowNumber = entityCollectiveEmissionRow.RowNumber,
                SerializedRow = entityCollectiveEmissionRow.SerializedRow,
                Status = (CollectiveLoadProcessStatus)entityCollectiveEmissionRow.StatusId,
                HasEvents = entityCollectiveEmissionRow.HasEvents
            };
        }
        public static List<CollectiveEmission> CreateCollectiveEmissions(BusinessCollection businessCollection)
        {
            List<CollectiveEmission> collectiveEmissions = new List<CollectiveEmission>();

            foreach (MSVEN.CollectiveEmission entityCollectiveEmission in businessCollection)
            {
                collectiveEmissions.Add(ModelAssembler.CreateCollectiveEmission(entityCollectiveEmission));
            }

            return collectiveEmissions;
        }

        public static CollectiveEmission CreateCollectiveEmission(MSVEN.CollectiveEmission entityCollectiveEmission)
        {
            return new CollectiveEmission()
            {
                Id = entityCollectiveEmission.MassiveLoadId,
                Agency = new IssuanceAgency
                {
                    Id = entityCollectiveEmission.AgencyId,
                    Agent = new IssuanceAgent
                    {
                        IndividualId = entityCollectiveEmission.AgentId
                    }
                },
                Branch = new Branch
                {
                    Id = entityCollectiveEmission.BranchId
                },
                Prefix = new Prefix
                {
                    Id = entityCollectiveEmission.PrefixId
                },
                Product = new ProductServices.Models.Product
                {
                    Id = entityCollectiveEmission.ProductId
                },
                TemporalId = entityCollectiveEmission.TempId,
                IsAutomatic = entityCollectiveEmission.IsAutomatic,
                Commiss = entityCollectiveEmission.Commiss,
                DocumentNumber = entityCollectiveEmission.DocumentNumber,
                Premium = entityCollectiveEmission.Premium,
                HasEvents = entityCollectiveEmission.HasEvents,
                EndorsementNumber = entityCollectiveEmission.EndorsementNumber,
                CoveredRiskType = (CoveredRiskType?)entityCollectiveEmission.CoveredRiskTypeCode,
                EndorsementId = entityCollectiveEmission.EndorsementId
            };
        }

        public static CollectiveEmission CreateCollectiveEmission(MSVEN.CollectiveEmission entityEmission, MSVEN.MassiveLoad entityLoad)
        {
            CollectiveEmission collectiveEmission = CreateCollectiveEmission(entityEmission);
            if (entityLoad != null)
            {
                collectiveEmission.Description = entityLoad.Description;
                collectiveEmission.ErrorDescription = entityLoad.ErrorDescription;
                collectiveEmission.File = new Services.UtilitiesServices.Models.File() { Name = entityLoad.FileName };
                collectiveEmission.LoadType = new MassiveServices.Models.LoadType
                {
                    Id = entityLoad.LoadTypeId.Value
                };
                collectiveEmission.HasError = entityLoad.HasError;
                collectiveEmission.Status = (MassiveLoadStatus)entityLoad.StatusId;
                collectiveEmission.User = new User() { UserId = entityLoad.UserId };
                collectiveEmission.TotalRows = entityLoad.TotalRows.GetValueOrDefault();
            }
            return collectiveEmission;
        }

        #region Autommaper

        public static IMapper CreateMapField()
        {
            var config = MapperCache.GetMapper<COMMON.Field, COMMON.Field>(cfg =>
            {
                cfg.CreateMap<COMMON.Field, COMMON.Field>();
            });
            return config;
        }
        #region colectiveEmisison
        public static IMapper CreateCollectiveEmission()
        {
            var config = MapperCache.GetMapper<MSVEN.CollectiveEmission, CollectiveEmission> (cfg =>
            {
                cfg.CreateMap<MSVEN.CollectiveEmission, CollectiveEmission>()
                 .ForMember(x => x.Agency, opts => opts.MapFrom(x => new IssuanceAgency
                 {
                     Id = x.AgencyId,
                     Agent = new IssuanceAgent
                     {
                         IndividualId = x.AgentId
                     }
                 }))
                .ForMember(x => x.Branch, opts => opts.MapFrom(x => new Branch
                {
                    Id = x.BranchId
                }))
                .ForMember(x => x.Id, opts => opts.MapFrom(x => x.MassiveLoadId))
                .ForMember(x => x.Prefix, opts => opts.MapFrom(x => new Prefix
                {
                    Id = x.PrefixId
                }))
                .ForMember(x => x.TemporalId, opts => opts.MapFrom(x => x.TempId))
                .ForMember(x => x.Product, opts => opts.MapFrom(x => new ProductServices.Models.Product
                {
                    Id = x.ProductId
                }))
                .ForMember(x => x.CoveredRiskType, opts => opts.MapFrom(x => (CoveredRiskType?)x.CoveredRiskTypeCode));
            });
            return config;
        }

        #endregion colectiveEmisison
        #region CollectiveEmissionRow
        public static IMapper CreateCollectiveEmissionRow()
        {
            var config = MapperCache.GetMapper<MSVEN.CollectiveEmissionRow, CollectiveEmissionRow>(cfg =>
            {
                cfg.CreateMap<MSVEN.CollectiveEmissionRow, CollectiveEmissionRow>()
                .ForMember(x => x.Risk, opts => opts.MapFrom(x => new Risk
                {
                    Description = x.RiskDescription,
                    RiskId = x.RiskId.GetValueOrDefault(),
                    Id = x.RiskId.GetValueOrDefault()                   
                }))
                .ForMember(x => x.Status, opts => opts.MapFrom(x => (CollectiveLoadProcessStatus)x.StatusId));
            });
            return config;

        }
        #endregion CollectiveEmissionRow

        #endregion Autommaper
    }
}