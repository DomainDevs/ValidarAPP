using AutoMapper;
using Sistran.Core.Application.CollectiveServices.EEProvider.Helper;
using Sistran.Core.Application.CollectiveServices.EEProvider.Resources;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using System;
using MSVEN = Sistran.Core.Application.Massive.Entities;

namespace Sistran.Core.Application.CollectiveServices.EEProvider.Assemblers
{
    public static class EntityAssembler
    {
        /// <summary>
        /// Creates the collective emission row.
        /// </summary>
        /// <param name="collectiveEmissionRow">The collective emission row.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Riesgo Vacio</exception>
        //public static MSVEN.CollectiveEmissionRow CreateCollectiveEmissionRow(CollectiveEmissionRow collectiveEmissionRow)
        //{
        //    if (collectiveEmissionRow == null)
        //    {
        //        throw new ArgumentNullException(Errors.ErrorCreateCollectiveEmissionRow);
        //    }
        //    else
        //    {
        //        var immaper = CreateMapCollectiveEmissionRow();
        //        return immaper.Map<CollectiveEmissionRow, MSVEN.CollectiveEmissionRow>(collectiveEmissionRow);
        //    }
        //}

        /// <summary>
        /// Creates the collective emission.
        /// </summary>
        /// <param name="collectiveEmission">The collective emission.</param>
        /// <returns></returns>
        public static MSVEN.CollectiveEmission CreateCollectiveEmission(CollectiveEmission collectiveEmission)
        {
            if (collectiveEmission == null)
            {
                throw new ArgumentNullException(Errors.ErrorCreateCollectiveEmissionRow);
            }
            else
            {
                var immaper = CreateMapCollectiveEmission();
                return immaper.Map<CollectiveEmission, MSVEN.CollectiveEmission>(collectiveEmission);
            }
        }

        #region Autommaper
        /// <summary>
        /// Creates the map collective emission row.
        /// </summary>
        /// <returns></returns>
        //public static IMapper CreateMapCollectiveEmissionRow()
        //{
        //    var config = MapperCache.GetMapper<CollectiveEmissionRow, MSVEN.CollectiveEmissionRow>(cfg =>
        //    {
        //        cfg.CreateMap<CollectiveEmissionRow, MSVEN.CollectiveEmissionRow>()
        //        .ForMember(x => x.RiskDescription, opts => opts.MapFrom(x => ValidateData.ValidateNull<string>("Riskscription", x.Risk.Description)))
        //        .ForMember(x => x.RiskId, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("RisksId", x.Risk.RiskId)))
        //        .ForMember(x => x.StatusId, opts => opts.MapFrom(x => ValidateData.ValidateNull<int?>("RisksId", (int?)x.Risk.Status)))
        //        .ForMember(x => x.MassiveLoadId, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("MassiveLoadId", x.Risk.MassiveLoadId)))
        //        .ForMember(x => x.HasError, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("HasError", x.Risk.HasError)))
        //        .ForMember(x => x.Id, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("Id", x.Risk.Id)))
        //        .ForMember(x => x.Observations, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("Observations", x.Risk.Observations)))
        //        .ForMember(x => x.Premium, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("Premium", x.Risk.Premium)))
        //        .ForMember(x => x.RiskDescription, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("RiskDescription", x.Risk.RiskDescription)))
        //        .ForMember(x => x.RowNumber, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("RowNumber", x.Risk.RowNumber)))
        //        .ForMember(x => x.SerializedRow, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("SerializedRow", x.Risk.SerializedRow)))
        //        .ForMember(x => x.HasEvents, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("HasEvents", x.Risk.HasEvents)))
        //        ;
        //    });
        //    return config;
        //}

        public static MSVEN.CollectiveEmissionRow CreateCollectiveEmissionRow(CollectiveEmissionRow collectiveEmissionRow)
       {
           return new MSVEN.CollectiveEmissionRow()
           {
               MassiveLoadId = collectiveEmissionRow.MassiveLoadId,
               HasError = collectiveEmissionRow.HasError,
               Id = collectiveEmissionRow.Id,
               Observations = collectiveEmissionRow.Observations,
               Premium = collectiveEmissionRow.Premium,
               RiskDescription = collectiveEmissionRow.Risk == null ? "" : collectiveEmissionRow.Risk.Description,
               RiskId = collectiveEmissionRow.Risk == null ? 0 : collectiveEmissionRow.Risk.RiskId,
               RowNumber = collectiveEmissionRow.RowNumber,
               SerializedRow = collectiveEmissionRow.SerializedRow,
               StatusId = (int)collectiveEmissionRow.Status,
               HasEvents = collectiveEmissionRow.HasEvents
           };
       }

        /// <summary>
        /// Creates the map collective emission.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapCollectiveEmission()
        {
            var config = MapperCache.GetMapper<CollectiveEmission, MSVEN.CollectiveEmission>(cfg =>
            {
                cfg.CreateMap<CollectiveEmission, MSVEN.CollectiveEmission>()
                .ForMember(x => x.AgencyId, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("Agency", x.Agency.Id)))
                .ForMember(x => x.AgentId, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("Agent", x.Agency.Agent.IndividualId)))
                .ForMember(x => x.BranchId, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("Branch", x.Branch.Id)))
                .ForMember(x => x.MassiveLoadId, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("Branch", x.Id)))
                .ForMember(x => x.PrefixId, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("Prefix", x.Prefix.Id)))
                .ForMember(x => x.TempId, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("TemporalId", x.TemporalId)))
                .ForMember(x => x.ProductId, opts => opts.MapFrom(x => ValidateData.ValidateNull<int>("Product", x.Product.Id)))
                .ForMember(x => x.CoveredRiskTypeCode, opts => opts.MapFrom(x => ValidateData.ValidateNull<int?>("CoveredRiskType", (int?)x.CoveredRiskType)))
                .ForMember(x => x.IsAutomatic, opts => opts.MapFrom(x => ValidateData.ValidateNull<bool>("IsAutomatic", x.IsAutomatic)))
                .ForMember(x => x.Commiss, opts => opts.MapFrom(x => ValidateData.ValidateNull<decimal?>("Commiss", x.Commiss)))
                .ForMember(x => x.Premium, opts => opts.MapFrom(x => ValidateData.ValidateNull<decimal?>("Premium", x.Premium)))
                .ForMember(x => x.DocumentNumber, opts => opts.MapFrom(x => ValidateData.ValidateNull<decimal?>("DocumentNumber", x.DocumentNumber)))
                .ForMember(x => x.HasEvents, opts => opts.MapFrom(x => ValidateData.ValidateNull<bool>("HasEvents", x.HasEvents)))
                .ForMember(x => x.EndorsementNumber, opts => opts.MapFrom(x => ValidateData.ValidateNull<int?>("EndorsementNumber", x.EndorsementNumber)))
                .ForMember(x => x.EndorsementId, opts => opts.MapFrom(x => ValidateData.ValidateNull<int?>("EndorsementId", x.EndorsementId)))
                ;
            });
            return config;
        }

        /*public static MSVEN.CollectiveEmission CreateCollectiveEmission(CollectiveEmission collectiveEmission)
        {
            return new MSVEN.CollectiveEmission
            {
                AgencyId = collectiveEmission.Agency.Id,
                AgentId = collectiveEmission.Agency.Agent.IndividualId,
                BranchId = collectiveEmission.Branch.Id,
                IsAutomatic = collectiveEmission.IsAutomatic,
                MassiveLoadId = collectiveEmission.Id,
                PrefixId = collectiveEmission.Prefix.Id,
                TempId = collectiveEmission.TemporalId,
                ProductId = collectiveEmission.Product.Id,
                Commiss = collectiveEmission.Commiss,
                Premium = collectiveEmission.Premium,
                DocumentNumber = collectiveEmission.DocumentNumber,
                HasEvents = collectiveEmission.HasEvents,
                EndorsementNumber = collectiveEmission.EndorsementNumber,                
                CoveredRiskTypeCode = collectiveEmission.CoveredRiskType == null ? (int?)null : (int)collectiveEmission.CoveredRiskType,
                EndorsementId = collectiveEmission.EndorsementId
            };
        }*/

        #endregion Autommaper



    }
}