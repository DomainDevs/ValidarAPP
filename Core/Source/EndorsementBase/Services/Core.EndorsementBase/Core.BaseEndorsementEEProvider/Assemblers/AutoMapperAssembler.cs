using AutoMapper;
using Sistran.Core.Application.BaseEndorsementService.DTOs;
using Sistran.Core.Application.Utilities.Cache;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
namespace Sistran.Core.Application.BaseEndorsementService.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region  Clause
        public static IMapper CreateMapModificationType()
        {
            IMapper config = MapperCache.GetMapper<PARAMEN.EndorsementModificationType, EndorsementTypeDTO>(cfg =>
            {
                cfg.CreateMap<PARAMEN.EndorsementModificationType, EndorsementTypeDTO>();
            });
            return config;
        }
        #endregion
    }
}
