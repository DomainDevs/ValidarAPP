using AutoMapper;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Integration.CommonServices.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Integration.CommonServices.EEProvider.Assemblers
{
    internal static class ModelAssembler
    {
        internal static IMapper CreateMapParameter()
        {
            var config = MapperCache.GetMapper<ParameterDTO, Parameter>(cfg =>
            {
                cfg.CreateMap<ParameterDTO, Parameter>();
            });
            return config;
        }

        internal static Parameter ToModel(this ParameterDTO parameterDTO)
        {
            var config = CreateMapParameter();
            return config.Map<ParameterDTO, Parameter>(parameterDTO);
        }

        internal static IEnumerable<Parameter> ToModels(this IEnumerable<ParameterDTO> parameterDTOs)
        {
            return parameterDTOs.Select(ToModel);
        }
    }
}
